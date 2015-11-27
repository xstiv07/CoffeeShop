using DataFramework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Xml.Serialization;

namespace Restbucks
{
    public class BaseController : ApiController
    {
        protected CoffeeShopDBEntities db;

        public BaseController()
        {
            db = new CoffeeShopDBEntities();
        }
    }
}