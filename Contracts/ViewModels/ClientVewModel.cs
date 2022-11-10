using Contracts.JSONViewModels;
using Contracts.Model;
using Microsoft.EntityFrameworkCore;
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
            if (singleClientData.id != 0)
                return context.Clients.Where(c => c.id == singleClientData.id).FirstOrDefault();
            if (singleClientData.name != "" && singleClientData.name != null)
            {
                return context.Clients.Where(c => c.name.Replace(" ", "") ==
                    singleClientData.name.Replace(" ", "")).FirstOrDefault();
            }
            return new { Clients = context.Clients };
        }

        public KeyValuePair<bool, int> CreateClient(Object dataItem, int clientId)
        {
            try
            {
                Clients client = JsonConvert.DeserializeObject<Clients>(dataItem.ToString());
                if (clientId != 0 && client.id != 0)
                {
                    return UpdateClient(client);
                }
                else if ((clientId != 0 && client.id == 0) || (clientId == 0 && client.id != 0))
                {
                    return new KeyValuePair<bool, int>(false, 0);
                }
                else
                {
                    context.Clients.Add(client);
                    context.SaveChanges();
                    var createdActId = context.Acts.Max(clid => clid.id);
                    return new KeyValuePair<bool, int>(true, createdActId);
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
    }
}
