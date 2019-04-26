using Icon.Dashboard.CommonDatabaseAccess;
using Icon.Dashboard.Mvc.Services;
using Icon.Dashboard.Mvc.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Icon.Dashboard.Mvc.Helpers
{
    /// <summary>
    /// ActionFilterAttribute implementation to allow common place to set ViewBag values for
    ///  global menu options
    /// </summary>
    public sealed class MenuOptionsFilter : ActionFilterAttribute
    {
        public override void OnResultExecuting(ResultExecutingContext filterContext)
        {
            base.OnResultExecuting(filterContext);
            
            var controllerName = filterContext.RequestContext.RouteData.Values["Controller"].ToString();
            var url = filterContext.RequestContext.HttpContext.Request.Url;
            var viewBag = filterContext.Controller.ViewBag;

            var allMessageTypes = IconApiControllerMessageType.GetAll();
            viewBag.MenuOptionsForApiJobs = GetMenuOptionsForApiJobs(url, controllerName, allMessageTypes);

            var iconDbService = filterContext.HttpContext.Items["iconLoggingDataService"];
            if (null != iconDbService && iconDbService is IIconDatabaseServiceWrapper)
            {
                var allApps = (iconDbService as IIconDatabaseServiceWrapper).GetApps();
                viewBag.MenuOptionsForIconAppLogs = GetMenuOptionsForAppLogs(url, controllerName, allApps);
            }
            var mammothDbService = filterContext.HttpContext.Items["mammothLoggingDataService"];
            if (null != mammothDbService && mammothDbService is IMammothDatabaseServiceWrapper)
            {
                var allApps = (mammothDbService as IMammothDatabaseServiceWrapper).GetApps();
                viewBag.MenuOptionsForMammothAppLogs = GetMenuOptionsForAppLogs(url, controllerName, allApps);
            }
        }

        public static Dictionary<IconLoggedAppViewModel, bool> GetMenuOptionsForAppLogs(
            Uri currentUri,
            string controllerName,
            IEnumerable<IconLoggedAppViewModel> knownApps)
        {
            var appLogsMenuOptions = new Dictionary<IconLoggedAppViewModel, bool>();
            bool isActive;
            var requestIdParameter = Utils.GetIdParameterFronUrl(currentUri);

            foreach (var eachKnownApp in knownApps)
            {
                isActive = ((controllerName == "IconLogs" || controllerName == "MammothLogs")
                    && !String.IsNullOrWhiteSpace(requestIdParameter)
                    && requestIdParameter == eachKnownApp.AppName);
                
                appLogsMenuOptions.Add(eachKnownApp, isActive);
            }

            return appLogsMenuOptions;
        }

        public static Dictionary<string, bool> GetMenuOptionsForApiJobs(
            Uri currentUri,
            string controllerName,
            IEnumerable<string> knownMessageTypes)
        {
            var apiJobsMenuOptions = new Dictionary<string, bool>();
            bool isActive;
            var requestIdParameter = Utils.GetIdParameterFronUrl(currentUri);

            var messageTypesToExclude = new List<string>()
            {
                 IconApiControllerMessageType.Undefined,
                 IconApiControllerMessageType.DepartmentSale,
                 IconApiControllerMessageType.NotUsed,
                 IconApiControllerMessageType.CCHTaxUpdate,
                 IconApiControllerMessageType.eWIC,
                 IconApiControllerMessageType.InforNewItem
            };

            foreach (var eachMessageType in knownMessageTypes.Except(messageTypesToExclude))
            {
                isActive = (controllerName == "ApiJobs"
                    && !String.IsNullOrWhiteSpace(requestIdParameter)
                    && requestIdParameter == eachMessageType);

                if (eachMessageType != IconApiControllerMessageType.Undefined && eachMessageType != IconApiControllerMessageType.NotUsed)
                {
                    apiJobsMenuOptions.Add(eachMessageType, isActive);
                }
            }

            return apiJobsMenuOptions;
        }
    }
}