using Contracts.Model;
using Contracts.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System;
using Microsoft.Extensions.Configuration;

namespace Contracts.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class DictionaryController : BaseController
    {
        public DictionaryController(DataBaseContext context, IConfiguration Configuration) :
            base(context, Configuration)
        {
        }

        [HttpGet]
        public Object GetTechnicalConditions(int tcId = 0)
        {
            return Status(new TechnicalConditionsViewModel(db).GetTechnicalConditions(tcId));
        }

        [HttpPost]
        public Object CreateTechnicalConditions([FromBody] Object dataItem, int tcId = 0)
        {
            return Status(new TechnicalConditionsViewModel(db).CreateTechnicalConditions(dataItem, tcId));
        }

        [HttpDelete]
        public void DeleteTechnicalConditions(int tcId)
        {
            Status(new TechnicalConditionsViewModel(db).DeleteTechnicalConditions(tcId));
        }

        [HttpGet]
        public Object IndResponsiblePerson()
        {
            return Status(new DictionaryViewModel(db, Configuration).GetIndelResponsiblePerson());
        }

        [HttpPost]
        public Object CreateIndelRP([FromBody] Object dataItem, int RPID = 0)
        {
            return Status(new DictionaryViewModel(db, Configuration).CreateIndelResponsiblePerson(dataItem, RPID));
        }

        [HttpDelete]
        public void DeleteRP(int RPID)
        {
            Status(new DictionaryViewModel(db, Configuration).DeleteRP(RPID));
        }

        [HttpPost]
        public Object CreateBank([FromBody] Object dataItem, int BID = 0)
        {
            return Status(new ClientVewModel(db).CreateNewBank(dataItem, BID));
        }

        [HttpDelete]
        public void DeleteBank(int BID)
        {
            Status(new ClientVewModel(db).DeleteBank(BID));
        }

        [HttpGet]
        public IActionResult GetTemplate()
        {
            FileExt file = new DictionaryViewModel(db, Configuration).Template();
            return File(file.Bytes, file.FileExtention, file.FileName);
        }

        [HttpPost]
        public Object SendEmail([FromBody] Object dataItem)
        {
            return Status(new DictionaryViewModel(db, Configuration).EmailSending(dataItem));
        }
    }
}
