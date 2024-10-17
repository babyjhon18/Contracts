using Contracts.Model;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Contracts.ViewModels
{
    public class ActsViewModel
    {
        DataBaseContext context;
        public ActsViewModel(DataBaseContext db)
        {
            context = db;
        }

        public Object GetActs(int actId)
        {
            if (actId != 0)
                return context.Acts.Where(aI => aI.id == actId).FirstOrDefault();
            else
                return new { Acts = context.Acts };
        }

        public KeyValuePair<bool, int> CreateAct(Object dataItem, int actId)
        {
            try
            {
                Acts act = JsonConvert.DeserializeObject<Acts>(dataItem.ToString());
                if (actId != 0 && act.id != 0)
                {
                    return UpdateAct(act);
                }
                else if ((actId != 0 && act.id == 0) || (actId == 0 && act.id != 0))
                {
                    return new KeyValuePair<bool, int>(false, 0);
                }
                else
                {
                    context.Acts.Add(act);
                    var contract = context.Contracts.Where(c => c.id == act.FK_ContractId).FirstOrDefault();
                    if (contract.IsPaid == false && contract.NotForWorkPlan == true)
                    {
                        contract.IsPaid = true;
                        contract.ReadyMark = true;
                    }
                    context.SaveChanges();
                    contract.LastActDate = act.ActDate;
                    if (GetActsByContract(contract))
                        contract.ReadyMark = true;
                    context.Entry(contract).State = EntityState.Modified;
                    context.SaveChanges();
                    var createdActId = context.Acts.Max(aid => aid.id);
                    return new KeyValuePair<bool, int>(true, createdActId);
                }
            }
            catch
            {
                return new KeyValuePair<bool, int>(false, 0);
            }
        }

        private bool GetActsByContract(Model.Contracts contracts)
        {
            var actsList = context.Acts.Where(a => a.FK_ContractId == contracts.id);
            double actsSum = 0;
            foreach (var act in actsList)
            {
                actsSum += act.ActPayment;
            }
            if (contracts.Amount <= actsSum)
                return true;
            return false;
        }

        public KeyValuePair<bool, int> UpdateAct(Acts act)
        {
            try
            {
                var contract = context.Contracts.Where(c => c.id == act.FK_ContractId).FirstOrDefault();
                contract.LastActDate = act.ActDate;
                context.Entry(act).State = EntityState.Modified;
                context.SaveChanges();
                if (GetActsByContract(contract))
                    contract.ReadyMark = true;
                context.Entry(contract).State = EntityState.Modified;
                context.SaveChanges();
                return new KeyValuePair<bool, int>(true, act.id);
            }
            catch
            {
                return new KeyValuePair<bool, int>(false, 0);
            }
        }

        public bool DeleteAct(int actId)
        {
            try
            {
                context.Entry(context.Acts.Where(a => a.id == actId).FirstOrDefault()).State = EntityState.Deleted;
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
