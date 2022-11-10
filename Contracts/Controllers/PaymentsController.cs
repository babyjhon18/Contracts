using Contracts.Model;
using Contracts.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Contracts.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PaymentsController : BaseController
    {
        public PaymentsController(DataBaseContext context) :
            base(context)
        {
        }

        [HttpGet]
        public Object Get(int paymentId = 0)
        {
            return Status(new PaymentsViewModel(db).GetPayments(paymentId));
        }

        [HttpPost]
        public Object Post([FromBody] Object dataItem, int paymentId)
        {
            return Status(new PaymentsViewModel(db).CreatePayment(dataItem, paymentId));
        }

        [HttpDelete]
        public void Delete(int paymentId)
        {
            Status(new PaymentsViewModel(db).DeletePayment(paymentId));
        }
    }
}
