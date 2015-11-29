// --------------------------------------------------------------------------------------------------------------------
// <copyright file="BundleConfig.cs" company="">
//   Copyright © 2015 
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace App.Test
{
    using System.Web;
    using System.Web.Optimization;

    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new StyleBundle("~/content/css/app").Include(
                "~/content/app.css",
                "~/content/bootstrap.min.css",
                "~/content/Site.css",
                "~/content/timer.css"
                ));

            bundles.Add(new ScriptBundle("~/js/jquery").Include("~/scripts/vendor/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/js/app").Include(
                "~/scripts/app.js",
                "~/scripts/vendor/angular-ui-router.js",
                "~/scripts/services.js",
                "~/scripts/controllers.js",
                "~/Scripts/bootstrap.min.js"
                ));
        }
    }
}
