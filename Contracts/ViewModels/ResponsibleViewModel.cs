using Contracts.Model;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Contracts.ViewModels
{
    public class ResponsibleViewModel
    {
        DataBaseContext context;
        public ResponsibleViewModel(DataBaseContext db)
        {
            context = db;
        }
        public Object GetPersons(int clientId)
        {
            return ResponsiblePersonsList(clientId);
        }

        public Object CreatePerson(Object dataItem, int responsiblePersonId)
        {
            try
            {
                ResponsiblePersons responsiblePerson = JsonConvert.DeserializeObject<ResponsiblePersons>(dataItem.ToString());
                if (responsiblePersonId != 0 && responsiblePerson.id != 0)
                {
                    return UpdateResponsiblePerson(responsiblePerson);
                }
                else if ((responsiblePersonId != 0 && responsiblePerson.id == 0) || (responsiblePersonId == 0 && responsiblePerson.id != 0))
                {
                    return new KeyValuePair<bool, int>(false, 0);
                }
                else
                {
                    context.ResponsiblePersons.Add(responsiblePerson);
                    context.SaveChanges();
                    int createdPaymentId = context.ResponsiblePersons.Max(rp => rp.id);
                    return ResponsiblePersonsList(responsiblePerson.FK_ClientId);
                }
            }
            catch
            {
                return new KeyValuePair<bool, int>(false, 0);
            }
        }

        public Object UpdateResponsiblePerson(ResponsiblePersons dataItem)
        {
            try
            {
                context.Entry(dataItem).State = EntityState.Modified;
                context.SaveChanges();
                return ResponsiblePersonsList(dataItem.FK_ClientId);
            }
            catch
            {
                return new KeyValuePair<bool, int>(false, 0);
            }
        }

        private Object ResponsiblePersonsList(int clientId)
        {
            var responsiblePersons = new List<Object>();
            foreach (var person in context.ResponsiblePersons.Where(p => p.FK_ClientId == clientId))
                responsiblePersons.Add(person);
            return responsiblePersons;
        }

        public bool DeleteResponsiblePerson(int rId)
        {
            try
            {
                context.Entry(context.ResponsiblePersons.Where(rp => rp.id == rId).FirstOrDefault()).State = EntityState.Deleted;
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
