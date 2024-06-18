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
    public class ContractsController : BaseController
    {
        public ContractsController(DataBaseContext context, IConfiguration Configuration) :
            base(context, Configuration)
        {
        }

        [HttpGet]
        public Object Get(int contractsType = 0)
        {
            return Status(new ContractsViewModel(db).GetContracts(contractsType));
        } 

        [HttpPost]
        public Object Post([FromBody] Object dataItem, int contractID = 0)
        {
            return Status(new ContractsViewModel(db).CreateContract(dataItem, contractID));
        }

        [HttpDelete]
        public void Delete(int contractId)
        {
            Status(new ContractsViewModel(db).DeleteContract(contractId));
        }
    }
}
