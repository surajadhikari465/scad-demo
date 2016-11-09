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
    public class SetViewBagMenuOptionsFilter : ActionFilterAttribute
    {
        public override void OnResultExecuting(ResultExecutingContext filterContext)
        {
            base.OnResultExecuting(filterContext);
            
            var controllerName = filterContext.RequestContext.RouteData.Values["Controller"].ToString();
            var url = filterContext.RequestContext.HttpContext.Request.Url;
            var viewBag = filterContext.Controller.ViewBag;

            viewBag.EnvironmentOptions = new EnvironmentSwitcher().GetServersForEnvironments();

            viewBag.DataFlowSystemOptions = Utils.GetDataFlowSystemSelections();

            var allMessageTypes = IconApiControllerMessageType.GetAll();
            viewBag.MenuOptionsForApiJobs = GetMenuOptionsForApiJobs(url, controllerName, allMessageTypes);

            var loggingDataService = filterContext.HttpContext.Items["loggingDataService"];
            if (null != loggingDataService && loggingDataService is IIconDatabaseServiceWrapper)
            {
                var allApps = (loggingDataService as IIconDatabaseServiceWrapper).GetApps();
                viewBag.MenuOptionsForAppLogs = GetMenuOptionsForAppLogs(url, controllerName, allApps);
            }
        }

        public Dictionary<IconAppViewModel, bool> GetMenuOptionsForAppLogs(
            Uri currentUri,
            string controllerName,
            IEnumerable<IconAppViewModel> knownApps)
        {
            Dictionary<IconAppViewModel, bool> appLogsMenuOptions = new Dictionary<IconAppViewModel, bool>();
            bool isActive;
            var requestIdParameter = Utils.GetIdParameterFronUrl(currentUri);

            foreach (var eachKnownApp in knownApps)
            {
                isActive = (controllerName == "Logs"
                    && !String.IsNullOrWhiteSpace(requestIdParameter)
                    && requestIdParameter == eachKnownApp.AppName);
                
                appLogsMenuOptions.Add(eachKnownApp, isActive);
            }

            return appLogsMenuOptions;
        }

        public Dictionary<string, bool> GetMenuOptionsForApiJobs(
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