using Contracts.Model;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Contracts.Controllers
{
    public class BaseController : Controller
    {
        public DataBaseContext db;
        public BaseController(DataBaseContext context)
        {
            db = context;
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
