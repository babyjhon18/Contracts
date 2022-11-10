using Contracts.Model;
using Contracts.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System;

namespace Contracts.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TermsOfPaymentController : BaseController
    {
        public TermsOfPaymentController(DataBaseContext context):
            base(context)
        {
        }

        [HttpGet]
        public Object Get(int termID = 0)
        {
            return Status(new TermsOfPaymentsViewModel(db).GetTerms(termID));
        }
    }
}
