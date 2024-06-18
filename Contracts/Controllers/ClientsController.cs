using Contracts.Model;
using Contracts.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;

namespace Contracts.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ClientsController : BaseController
    {
        public ClientsController(DataBaseContext context, IConfiguration Configuration) :
            base(context, Configuration)
        {
        }
        
        [HttpGet]
        public Object Get([FromBody] Object client = null)
        {
            return Status(new ClientVewModel(db).GetClients(client));
        }

        [HttpPost]
        public Object Post([FromBody] Object dataItem, int clientId)
        {
            return Status(new ClientVewModel(db).CreateClient(dataItem, clientId)); 
        }
    }
}
