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
    public class ResponsibleController : BaseController
    {
        public ResponsibleController(DataBaseContext context):
            base(context)
        {
        }
        
        [HttpGet]
        public Object ResponsiblePerson(int clientId)
        {
            return Status(new ResponsibleViewModel(db).GetPersons(clientId));
        }

        [HttpPost]
        public Object CreateResponsiblePerson([FromBody] Object dataItem)
        {
            return Status(new ResponsibleViewModel(db).CreatePerson(dataItem));
        }
    }
}
