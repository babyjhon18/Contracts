using Contracts.JSONViewModels;
using Contracts.Model;
using Microsoft.EntityFrameworkCore;
using Morpher.WebService.V2;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Contracts.ViewModels
{
    public class ClientVewModel
    {
        DataBaseContext context;
        public ClientVewModel(DataBaseContext db)
        {
            context = db;
        }

        public Object GetClients(Object client = null)
        {
            var singleClientData = JsonConvert.DeserializeObject<JSONSingleClientModel>(client.ToString());
            var clientData = new Object();
            if (singleClientData.id != 0)
            {

                var Client = context.Clients.Where(c => c.id == singleClientData.id).FirstOrDefault();
                clientData = new
                {
                    Client = Client,
                    BankInfo = context.Banks.Where(b => b.FK_ClientId == Client.id)
                };
                return clientData;
            }
            if (singleClientData.name != "" && singleClientData.name != null)
            {
                var Client = context.Clients.Where(c => c.FullName.Replace(" ", "") ==
                        singleClientData.name.Replace(" ", "")).FirstOrDefault();
                clientData = new
                {
                    Client = Client,
                    BankInfo = context.Banks.Where(b => b.FK_ClientId == Client.id)
                };
                return clientData;
            }
            return new { Clients = context.Clients, Banks = context.Banks };
        }

        public KeyValuePair<bool, int> CreateClient(Object dataItem, int clientId)
        {
            try
            {
                Clients client = JsonConvert.DeserializeObject<Clients>(dataItem.ToString());
                if (clientId != 0 && client.id != 0)
                {
                    var ifClientExists = context.Clients.Where(c => c.FullName == client.FullName || c.name == client.name).FirstOrDefault();
                    if (ifClientExists.id == client.id)
                    {
                        ifClientExists.UNP = client.UNP;
                        ifClientExists.Phones = client.Phones;
                        ifClientExists.Email = client.Email;
                        ifClientExists.FullName = client.FullName;
                        ifClientExists.OKPO = client.OKPO;
                        ifClientExists.IsIndividualEnterprise = client.IsIndividualEnterprise;
                        ifClientExists.LegalAddress = client.LegalAddress;
                        ifClientExists.PostAddress = client.PostAddress;    
                        ifClientExists.StorageAddress = client.StorageAddress;  
                        ifClientExists.name = client.name;
                        return UpdateClient(ifClientExists);
                    }
                    else
                    {
                        return new KeyValuePair<bool, int>(false, 2);
                    }
                }
                else if ((clientId != 0 && client.id == 0) || (clientId == 0 && client.id != 0))
                {
                    return new KeyValuePair<bool, int>(false, 0);
                }
                else
                {
                    var ifClientExists = context.Clients.Where(c => c.FullName == client.FullName || c.name == client.name).FirstOrDefault();
                    if (ifClientExists == null)
                    {
                        context.Clients.Add(client);
                        context.SaveChanges();
                        var createdActId = context.Clients.Max(clid => clid.id);
                        return new KeyValuePair<bool, int>(true, createdActId);
                    }
                    else
                    {
                        return new KeyValuePair<bool, int>(false, 2);
                    }
                }
            }
            catch
            {
                return new KeyValuePair<bool, int>(false, 0);
            }
        }

        public KeyValuePair<bool, int> UpdateClient(Clients client)
        {
            try
            {
                context.Entry(client).State = EntityState.Modified;
                context.SaveChanges();
                return new KeyValuePair<bool, int>(true, client.id);
            }
            catch
            {
                return new KeyValuePair<bool, int>(false, 0);
            }
        }

        //Bank
        public KeyValuePair<bool, int> CreateNewBank(Object dataItem, int bankId)
        {
            try
            {
                Banks bank = JsonConvert.DeserializeObject<Banks>(dataItem.ToString());
                if (bankId != 0 && bank.Id != 0)
                {
                    return UpdateBank(bank);
                }
                else if ((bankId != 0 && bank.Id == 0) || (bankId == 0 && bank.Id != 0))
                {
                    return new KeyValuePair<bool, int>(false, 0);
                }
                else
                {
                    context.Banks.Add(bank);
                    context.SaveChanges();
                    var createdActId = context.Banks.Max(bid => bid.Id);
                    return new KeyValuePair<bool, int>(true, createdActId);
                }
            }
            catch
            {
                return new KeyValuePair<bool, int>(false, 0);
            }
        }


        public KeyValuePair<bool, int> UpdateBank(Banks bank)
        {
            try
            {
                context.Entry(bank).State = EntityState.Modified;
                context.SaveChanges();
                return new KeyValuePair<bool, int>(true, bank.Id);
            }
            catch
            {
                return new KeyValuePair<bool, int>(false, 0);
            }
        }

        public bool DeleteBank(int bankId)
        {
            try
            {
                context.Entry(context.Banks.Where(b => b.Id == bankId).FirstOrDefault()).State = EntityState.Deleted;
                context.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
