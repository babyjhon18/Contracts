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
    public class ContractsController : BaseController
    {
        public ContractsController(DataBaseContext context) :
            base(context)
        {
        }

        [HttpGet]
        public Object Get(bool isReadyForAssemble = false)
        {
            return Status(new ContractsViewModel(db).GetContracts(isReadyForAssemble));
        } 

        [HttpPost]
        public Object Post([FromBody] Object dataItem, int contractID = 0)
        {
            return Status(new ContractsViewModel(db).CreateContract(dataItem, contractID));
        }
    }
}
