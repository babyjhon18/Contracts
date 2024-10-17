using Contracts.JSONViewModels;
using Contracts.Model;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using MailKit.Security;
using MimeKit;
using Microsoft.Extensions.Configuration;
using EasyDox;
using Spire.Doc;

namespace Contracts.ViewModels
{
    public class DictionaryViewModel
    {
        DataBaseContext context;
        IConfiguration _Configuration;
        public DictionaryViewModel(DataBaseContext db, IConfiguration Configuraiton)
        {
            context = db;
            _Configuration = Configuraiton.GetSection("EmailConfiguration");
        }

        //Ответственные лица ЗАО Инделко
        public Object GetIndelResponsiblePerson()
        {
            return new { IndelResponsiblePersons = context.RPIndel };
        }

        public KeyValuePair<bool, int> CreateIndelResponsiblePerson(Object dataItem, int RPID)
        {
            try
            {
                RPIndel RPIndel = JsonConvert.DeserializeObject<RPIndel>(dataItem.ToString());
                if (RPID != 0 && RPIndel.id != 0)
                {
                    return UpdateRPIndel(RPIndel);
                }
                else if ((RPID != 0 && RPIndel.id == 0) || (RPID == 0 && RPIndel.id != 0))
                {
                    return new KeyValuePair<bool, int>(false, 0);
                }
                else
                {
                    context.RPIndel.Add(RPIndel);
                    context.SaveChanges();
                    var createdRPIndelId = context.RPIndel.Max(rid => rid.id);
                    return new KeyValuePair<bool, int>(true, createdRPIndelId);
                }
            }
            catch
            {
                return new KeyValuePair<bool, int>(false, 0);
            }
        }

        public KeyValuePair<bool, int> UpdateRPIndel(RPIndel RPIndel)
        {
            try
            {
                context.Entry(RPIndel).State = EntityState.Modified;
                context.SaveChanges();
                return new KeyValuePair<bool, int>(true, RPIndel.id);
            }
            catch
            {
                return new KeyValuePair<bool, int>(false, 0);
            }
        }

        public bool DeleteRP(int RPID)
        {
            try
            {
                context.Entry(context.RPIndel.Where(rp => rp.id == RPID).FirstOrDefault()).State = EntityState.Deleted;
                context.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public FileExt Template()
        {
            FileExt file = new FileExt();
            file.FileExtention = "application/msword";
            string localFilePath = AppDomain.CurrentDomain.BaseDirectory + "App_Data\\ContractTemplate" + ".dotm";
            file.FileName = "Шаблон договора.dotm";
            file.Bytes = System.IO.File.ReadAllBytes(localFilePath);
            return file;
        }

        public const string AttachmentName = "Текст письма по договору № ";
        public bool EmailSending(Object dataItem)
        {
            try
            {
                JSONNotificationModel Notification = JsonConvert.DeserializeObject<JSONNotificationModel>(dataItem.ToString());
                Clients currentClient = context.Clients.Where(c => c.id == Notification.ClientId).FirstOrDefault();
                Model.Contracts currentContract = context.Contracts
                    .Where(c => c.id == Notification.ContractId).FirstOrDefault();
                var engine = new Engine();
                Document document = new Document();
                var fieldValues = new Dictionary<string, string>();
                context.Notifications.Add(new Notifications { NotificationDate = DateTime.UtcNow });
                context.SaveChanges();
                var notificationNumber = context.Notifications.Max(n => n.id).ToString();
                var ContractNumber = currentContract.ContractNumber.Replace("/", "_");
                fieldValues = new Dictionary<string, string>
                {
                    {
                        //"Сегодняшняя дата"
                        "CurrentDate", DateTime.UtcNow.ToString("D")
                    },
                    {
                        //"Текущий номер уведомления"
                        "CurrentNotifNumber", notificationNumber
                    },
                    {
                        //"Наименование клиента сокращенно"
                        "ClientShortName",
                        currentClient.FullName != null &&
                        currentClient.FullName != "" ?
                        currentClient.FullName : ""
                    },
                    {
                        //"Адрес клиента"
                        "ClientAddress", currentClient.LegalAddress
                    },
                    {
                        //"Email адрес клиента"
                        "ClientEmail", 
                        currentClient.Email != null && 
                        currentClient.Email != "" ? 
                        currentClient.Email : ""
                    },
                    {
                    //"Номер договора"
                        "ContractNumber", currentContract.ContractNumber
                    },
                    {
                        //"Дата ЗАКЛЮЧЕНИЯ договора"
                        "ContractDate", currentContract.ContractDate.ToString("d")
                    },
                };
                var url = AppDomain.CurrentDomain.BaseDirectory + "App_Data\\";
                var filename = AttachmentName + ContractNumber;
                //Merge fields
                engine.Merge(url + "ReadyMessage.docx", fieldValues, url + filename + ".docx");
                //Convert Word to PDF
                document.LoadFromFile(url + filename + ".docx");
                document.SaveToFile(url + AttachmentName + ContractNumber + ".pdf", FileFormat.PDF);
                //SendEmail
                using var emailMessage = new MimeMessage();
                var builder = new BodyBuilder();
                builder.TextBody = "ЗАО «ИнДелКо» информирует о готовности Вашего оборудования.";
                if(currentClient.Email != null && currentClient.Email != "")
                {
                    emailMessage.From.Add(new MailboxAddress("ЗАО «ИнДелКо»", _Configuration.GetValue("IndelEmailAddress", "")));
                    //_Configuration.GetValue("DeputyAccountantEmail", "")
                    emailMessage.To.Add(new MailboxAddress(currentClient.name, currentClient.Email));
                    builder.Attachments.Add(url + AttachmentName + currentContract.ContractNumber.Replace("/", "_") + ".pdf");
                }
                else
                {
                    emailMessage.From.Add(new MailboxAddress("ЗАО «ИнДелКо»", _Configuration.GetValue("IndelEmailAddress", "")));
                    emailMessage.To.Add(new MailboxAddress("Galina", _Configuration.GetValue("DeputyAccountantEmail", "")));
                    //emailMessage.To.Add(new MailboxAddress(currentClient.FullName, currentClient.Email));
                    builder.Attachments.Add(url + AttachmentName + currentContract.ContractNumber.Replace("/", "_") + ".docx");
                }
                emailMessage.Subject = "Готовность оборудования";
                emailMessage.Body = builder.ToMessageBody();

                using (var client = new MailKit.Net.Smtp.SmtpClient())
                {
                    client.Connect(_Configuration.GetValue("smtp", ""), Convert.ToInt32(_Configuration.GetValue("port", "")), SecureSocketOptions.StartTls);
                    client.Authenticate(_Configuration.GetValue("IndelEmailAddress", ""), _Configuration.GetValue("password", ""));
                    client.Send(emailMessage);
                    client.Disconnect(true);
                }
                return true;
            }
            catch
            {
                return false;
            }
            
        }
    }
}
