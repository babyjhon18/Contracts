using Contracts.Model;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Contracts.ViewModels
{
    public class TermsOfPaymentsViewModel
    {
        DataBaseContext context;
        public TermsOfPaymentsViewModel(DataBaseContext db)
        {
            context = db;
        }

        public Object GetTerms(int termID = 0)
        {
            if (termID != 0)
                return context.TermsOfPayment.Where(t => t.id == termID).FirstOrDefault();
            return new { TermsOfPayment = context.TermsOfPayment };
        }
    }
}
