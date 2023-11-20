using Contracts.JSONViewModels;
using Contracts.Model;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Contracts.ViewModels
{
    public class BlackListCompaniesViewModel
    {
        DataBaseContext context;
        public BlackListCompaniesViewModel(DataBaseContext db)
        {
            context = db;
        }

        public Object GetBlackList(int CompanyID = 0)
        {
            if (CompanyID != 0)
                return context.BlackListCompanies.Where(blCom => blCom.Id == CompanyID).FirstOrDefault();
            else
                return new { BlackListCompanies = context.BlackListCompanies }; 
        }

        public KeyValuePair<bool, int> CreateBlackListClient (Object dataItem, int clientId)
        {
            try
            {
                BlackListClient blackListClient = JsonConvert.DeserializeObject<BlackListClient>(dataItem.ToString());
                if (clientId != 0 && blackListClient.Id != 0)
                {
                    return UpdateClient(blackListClient);
                }
                else if ((clientId != 0 && blackListClient.Id == 0) || (clientId == 0 && blackListClient.Id != 0))
                {
                    return new KeyValuePair<bool, int>(false, 0);
                }
                else
                {
                    context.BlackListCompanies.Add(blackListClient);
                    context.SaveChanges();
                    var createdActId = context.BlackListCompanies.Max(clid => clid.Id);
                    return new KeyValuePair<bool, int>(true, createdActId);
                }
            }
            catch
            {
                return new KeyValuePair<bool, int>(false, 0);
            }
        }

        public KeyValuePair<bool, int> UpdateClient(BlackListClient blackListClient)
        {
            try
            {
                context.Entry(blackListClient).State = EntityState.Modified;
                context.SaveChanges();
                return new KeyValuePair<bool, int>(true, blackListClient.Id);
            }
            catch
            {
                return new KeyValuePair<bool, int>(false, 0);
            }
        }

        public bool DeleteClient(BlackListClient blackListClient)
        {
            try
            {
                context.Entry(context.BlackListCompanies.Where(p => p.Id == blackListClient.Id).FirstOrDefault()).State = EntityState.Deleted;
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
