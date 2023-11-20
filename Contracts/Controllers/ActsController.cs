using Contracts.Model;
using Contracts.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace Contracts.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ActsController : BaseController
    {
        public ActsController(DataBaseContext context, IConfiguration Configuration) :
            base(context, Configuration)
        {
        }

        [HttpGet]
        public Object Get(int actId = 0)
        {
            return Status(new ActsViewModel(db).GetActs(actId));
        }

        [HttpPost]
        public Object Post([FromBody] Object dataItem, int actId)
        {
            return Status(new ActsViewModel(db).CreateAct(dataItem, actId));
        }

        [HttpDelete]
        public void Delete(int actId)
        {
            Status(new ActsViewModel(db).DeleteAct(actId));
        }
    }
}
