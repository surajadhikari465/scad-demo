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
                url: "ApiJobs/{action}/{jobType}",
                defaults: new
                {
                    controller = "ApiJobs",
                    action = "Index",
                    jobType = UrlParameter.Optional
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
                 name: "HomeSetAltEnvironmentWithParameter",
                 url: "Home/SetAlternateEnvironment/{environment}",
                 defaults: new
                 {
                     controller = "Home",
                     action = "SetAlternateEnvironment"
                 }
             );

            routes.MapRoute(
                 name: "HomeEditWithParameters",
                 url: "Home/Edit/{appServer}/{serviceName}",
                 defaults: new
                 {
                     controller = "Home",
                     action = "Edit",
                     appServer = UrlParameter.Optional,
                     serviceName = UrlParameter.Optional
                 }
             );

            routes.MapRoute(
                name: "HomeCustom",
                url: "Home/Custom",
                defaults: new
                {
                    controller = "Home",
                    action = "Custom"
                }
            );

            routes.MapRoute(
                name: "HomeIndex",
                url: "Home/Index",
                defaults: new
                {
                    controller = "Home",
                    action = "Index"
                }
            );

            routes.MapRoute(
                name: "CatchUnexpected",
                url: "{*.*}",
                defaults: new
                {
                    controller = "Home",
                    action = "Index"
                }
            );
        }
    }
}
