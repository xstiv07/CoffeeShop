using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using System.Web.Http.Cors;

namespace Restbucks
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            //enabling cors globally - no need to annotate each attribute
            var corsAttr = new EnableCorsAttribute("http://localhost:8080", "*", "*");
            config.EnableCors(corsAttr);

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );

            config.Formatters.XmlFormatter.UseXmlSerializer = true;
        }
    }
}
