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
            routes.AppendTrailingSlash = true;
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
                name: "IconLogsWithId",
                url: "IconLogs/{action}/{appName}",
                defaults: new
                {
                    controller = "IconLogs",
                    action = "Index",
                    appName = UrlParameter.Optional
                }
            );

            routes.MapRoute(
                name: "MammothLogsWithId",
                url: "MammothLogs/{action}/{appName}",
                defaults: new
                {
                    controller = "MammothLogs",
                    action = "Index",
                    appName = UrlParameter.Optional
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
                url: "{Home}/{action}/{server}/{application}",
                defaults: new
                {
                    controller = "Home",
                    server = UrlParameter.Optional,
                    application = UrlParameter.Optional
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
