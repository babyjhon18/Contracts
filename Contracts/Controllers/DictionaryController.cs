using Contracts.Model;
using Contracts.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;

namespace Contracts.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class DictionaryController : BaseController
    {
        public DictionaryController(DataBaseContext context) :
            base(context)
        {
        }

        [HttpGet]
        public Object IndResponsiblePerson()
        {
            return Status(new DictionaryViewModel().DictionaryData.IndelResponsiblePersons);
        }
    }
}
