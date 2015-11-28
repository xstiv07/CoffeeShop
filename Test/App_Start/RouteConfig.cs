// --------------------------------------------------------------------------------------------------------------------
// <copyright file="RouteConfig.cs" company="">
//   Copyright © 2015 
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace App.Test
{
    using System.Web.Routing;

    using App.Test.Routing;

    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.Add("Default", new DefaultRoute());
        }
    }
}
