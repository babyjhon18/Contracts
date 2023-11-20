using Contracts.Model;
using Contracts.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;

namespace Contracts.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TermsOfPaymentController : BaseController
    {
        public TermsOfPaymentController(DataBaseContext context, IConfiguration Configuration):
            base(context, Configuration)
        {
        }

        [HttpGet]
        public Object Get(int termID = 0)
        {
            return Status(new TermsOfPaymentsViewModel(db).GetTerms(termID));
        }
    }
}
