using Contracts.Model;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Contracts.ViewModels
{
    public class PaymentsViewModel
    {
        DataBaseContext context;
        public PaymentsViewModel(DataBaseContext db)
        {
            context = db;
        }

        public Object GetPayments(int paymentId)
        {
            if (paymentId != 0)
                return new { Payments = context.Payments.Where(p => p.Id == paymentId) };
            else
                return new { Payments = context.Payments };
        }

        public KeyValuePair<bool, int> CreatePayment(Object dataItem, int paymentId)
        {
            try
            {
                Payments payment = JsonConvert.DeserializeObject<Payments>(dataItem.ToString());
                if (paymentId != 0 && payment.Id != 0)
                {
                    return UpdatePayment(payment);
                }
                else if ((paymentId != 0 && payment.Id == 0) || (paymentId == 0 && payment.Id != 0))
                {
                    return new KeyValuePair<bool, int>(false, 0);
                }
                else
                {
                    context.Payments.Add(payment);
                    var contract = context.Contracts.Where(c => c.id == payment.FK_ContractId).FirstOrDefault();
                    if(contract.IsPaid == false && contract.NotForWorkPlan == true)
                    {
                        contract.IsPaid = true;
                        contract.ReadyMark = true;
                        context.Entry(contract).State = EntityState.Modified;
                    }
                    context.SaveChanges();
                    int createdPaymentId = context.Payments.Max(pid => pid.Id);
                    return new KeyValuePair<bool, int>(true, createdPaymentId);
                }
            }
            catch
            {
                return new KeyValuePair<bool, int>(false, 0);
            }
        }

        public KeyValuePair<bool, int> UpdatePayment(Payments dataItem)
        {
            try
            {
                context.Entry(dataItem).State = EntityState.Modified;
                context.SaveChanges();
                    int editedPaymentId = context.Payments.Where(p => p.Id == dataItem.Id).FirstOrDefault().Id;
                    return new KeyValuePair<bool, int>(true, editedPaymentId);
            }
            catch
            {
                return new KeyValuePair<bool, int>(false, 0); 
            }
        }

        public bool DeletePayment(int paymentId)
        {
            try
            {
                context.Entry(context.Payments.Where(p => p.Id == paymentId).FirstOrDefault()).State = EntityState.Deleted;
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
