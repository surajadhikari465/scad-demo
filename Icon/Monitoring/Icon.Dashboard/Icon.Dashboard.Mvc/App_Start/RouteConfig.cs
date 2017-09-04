using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace Icon.Dashboard.Mvc
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "ApiJobsWithId",
                url: "ApiJobs/{action}/{id}",
                defaults: new
                {
                    controller = "ApiJobs",
                    action = "Index",
                    id = UrlParameter.Optional
                }
            );

            routes.MapRoute(
                name: "LogsWithId",
                url: "Logs/{action}/{id}",
                defaults: new
                {
                    controller = "Logs",
                    action = "Index",
                    id = UrlParameter.Optional
                }
            );

            routes.MapRoute(
                name: "EsbSwitchWithName",
                url: "Esb/{action}/{name}",
                defaults: new
                {
                    controller = "Esb",
                    action = "Index",
                    name = UrlParameter.Optional
                }
            );

            routes.MapRoute(
                name: "HomeActionWithParameters",
                url: "{Home}/{action}/{application}/{server}",
                defaults: new
                {
                    controller = "Home",
                    application = UrlParameter.Optional,
                    server = UrlParameter.Optional
                }
            );

            routes.MapRoute(
                name: "DefaultToHomeIndex",
                url: "{controller}/{action}",
                defaults: new
                {
                    controller = "Home",
                    action = "Index"
                }
            );
        }
    }
}
