using Contracts.Model;
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

        public Object CreatePerson(Object dataItem)
        {
            ResponsiblePersons persons = JsonConvert.DeserializeObject<ResponsiblePersons>(dataItem.ToString());
            context.ResponsiblePersons.Add(persons);
            context.SaveChanges();
            return ResponsiblePersonsList(persons.FK_ClientId);
        }

        private Object ResponsiblePersonsList(int clientId)
        {
            var responsiblePersons = new List<Object>();
            foreach (var person in context.ResponsiblePersons.Where(p => p.FK_ClientId == clientId))
                responsiblePersons.Add(person);
            return responsiblePersons;
        }
    }
}
