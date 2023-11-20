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
    public class ResponsibleController : BaseController
    {
        public ResponsibleController(DataBaseContext context, IConfiguration Configuration) :
            base(context, Configuration)
        {
        }
        
        [HttpGet]
        public Object ResponsiblePerson(int clientId)
        {
            return Status(new ResponsibleViewModel(db).GetPersons(clientId));
        }

        [HttpPost]
        public Object CreateResponsiblePerson([FromBody] Object dataItem, int responsiblePersonId)
        {
            return Status(new ResponsibleViewModel(db).CreatePerson(dataItem, responsiblePersonId));
        }

        [HttpDelete]
        public void Delete(int responsiblePersonId)
        {
            Status(new ResponsibleViewModel(db).DeleteResponsiblePerson(responsiblePersonId));
        }
    }
}
