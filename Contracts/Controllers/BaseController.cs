using Contracts.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace Contracts.Controllers
{
    public class BaseController : Controller
    {
        public readonly IConfiguration Configuration;

        public DataBaseContext db;
        public BaseController(DataBaseContext context, IConfiguration configuration)
        {
            db = context;
            Configuration = configuration;
        }

        public object Status(dynamic dataItem)
        {
            try
            {
                if (dataItem.Key != null && dataItem.Value != null)
                {
                    var keyValue = (KeyValuePair<bool, int>)dataItem;
                    if (keyValue.Key)
                    {
                        Response.StatusCode = 200;
                        return keyValue.Value;
                    }
                    else if (!keyValue.Key && keyValue.Value == 2)
                    {
                        Response.StatusCode = 409;
                        return new EmptyResult();
                    }
                    else
                    {
                        Response.StatusCode = 304;
                        return new EmptyResult();
                    }
                    
                }
            }
            catch
            {
                if (dataItem is Boolean)
                {
                    if (Convert.ToBoolean(dataItem))
                    {
                        Response.StatusCode = 200;
                        return new EmptyResult();
                    }
                    else
                    {
                        Response.StatusCode = 304;
                        return new EmptyResult();
                    }
                }
                else if (dataItem == null)
                {
                    Response.StatusCode = 400;
                    return new EmptyResult();
                }
                else return dataItem;
            }
            return new EmptyResult();
        }
    }
}
