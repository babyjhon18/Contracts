using Contracts.Model;
using Contracts.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;

namespace Contracts.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BlackListCompaniesController : BaseController 
    {
       public BlackListCompaniesController(DataBaseContext context, IConfiguration Configuration) :
            base(context, Configuration)
       {
       }

       [HttpGet]
       public Object Get(int companyId = 0) 
       {
            return Status(new BlackListCompaniesViewModel(db).GetBlackList(companyId));
       }

       [HttpPost]
       public Object Post([FromBody] Object dataItem, int companyId)
       {
           return Status(new BlackListCompaniesViewModel(db).CreateBlackListClient(dataItem, companyId));
       }

       [HttpDelete]
       public void Delete(int companyId)
       {
           Status(new BlackListCompaniesViewModel(db).DeleteClient(new BlackListClient() { Id = companyId }));
       }
    }
}
