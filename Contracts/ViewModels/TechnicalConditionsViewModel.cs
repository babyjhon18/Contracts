using Contracts.Model;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Collections.Generic;
using System;
using System.Linq;

namespace Contracts.ViewModels
{
    public class TechnicalConditionsViewModel
    {
        DataBaseContext context;
        public TechnicalConditionsViewModel(DataBaseContext db)
        {
            context = db;
        }

        public Object GetTechnicalConditions(int tcId)
        {
            if (tcId != 0)
                return context.TechnicalConditions.Where(tc => tc.id == tcId).FirstOrDefault();
            else
                return new { TechnicalConditions = context.TechnicalConditions };
        }

        public KeyValuePair<bool, int> CreateTechnicalConditions(Object dataItem, int tcId)
        {
            try
            {
                TechnicalConditions technicalConditions = JsonConvert.DeserializeObject<TechnicalConditions>(dataItem.ToString());
                if (tcId != 0 && technicalConditions.id != 0)
                {
                    return UpdateTechnicalConditions(technicalConditions);
                }
                else if ((tcId != 0 && technicalConditions.id == 0) || (tcId == 0 && technicalConditions.id != 0))
                {
                    return new KeyValuePair<bool, int>(false, 0);
                }
                else
                {
                    context.TechnicalConditions.Add(technicalConditions);
                    context.SaveChanges();
                    var createdTcId = context.TechnicalConditions.Max(tcId => technicalConditions.id);
                    return new KeyValuePair<bool, int>(true, createdTcId);
                }
            }
            catch
            {
                return new KeyValuePair<bool, int>(false, 0);
            }
        }

        public KeyValuePair<bool, int> UpdateTechnicalConditions(TechnicalConditions technicalConditions)
        {
            try
            {
                context.Entry(technicalConditions).State = EntityState.Modified;
                context.SaveChanges();
                return new KeyValuePair<bool, int>(true, technicalConditions.id);
            }
            catch
            {
                return new KeyValuePair<bool, int>(false, 0);
            }
        }

        public bool DeleteTechnicalConditions(int tcId)
        {
            try
            {
                context.Entry(context.TechnicalConditions.Where(tc => tc.id == tcId).FirstOrDefault()).State = EntityState.Deleted;
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

