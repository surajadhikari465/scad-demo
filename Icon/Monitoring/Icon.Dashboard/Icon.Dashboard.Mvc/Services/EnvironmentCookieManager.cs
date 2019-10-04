using Icon.Dashboard.Mvc.Enums;
using Icon.Dashboard.Mvc.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Icon.Dashboard.Mvc.Services
{
    public class EnvironmentCookieManager : IEnvironmentCookieManager
    {
        //public EnvironmentCookieManager() { }
        public EnvironmentCookieManager(
            int environmentCookieDurationHours,
            string environmentNameCookieName,
            string environmentAppServersCookieName)
        {
            SetCookieParameters(environmentCookieDurationHours, environmentNameCookieName, environmentAppServersCookieName);
        }

        public string EnvironmentNameCookieName { get; set; }
        public string EnvironmentAppServersCookieName { get; set; }
        public int EnvironmentCookieDurationHours { get; set; }

        internal void SetCookieParameters(
                int environmentCookieDurationHours,
                string environmentNameCookieName,
                string environmentAppServersCookieName)
        {
            this.EnvironmentCookieDurationHours = environmentCookieDurationHours;
            this.EnvironmentNameCookieName = environmentNameCookieName;
            this.EnvironmentAppServersCookieName = environmentAppServersCookieName;

        }

        public EnvironmentCookieModel GetEnvironmentCookieIfPresent(HttpRequestBase request)
        {
            // read environment name from cookie if it is stored there
            var environmentName = GetEnvironmentNameCookieValueOrNull(request);
            if (!string.IsNullOrWhiteSpace(environmentName))
            {
                Enum.TryParse(environmentName, out EnvironmentEnum environmentEnum);

                // if the environment name matches one of the standard environments,
                // the enum value will have been set to match by the TryParse().
                // otherwise this is a custom environment
                if (environmentEnum == EnvironmentEnum.Undefined)
                {
                    environmentEnum = EnvironmentEnum.Custom;
                }

                var cookieModel = new EnvironmentCookieModel(environmentName, environmentEnum);

                // read whether a list of app servers was stored in the cookie
                var appServers = GetAppServersCookieValueOrNull(request);
                if (appServers != null && appServers.Count > 0)
                {
                    foreach (var server in appServers)
                    {
                        cookieModel.AppServers.Add(server);
                    }
                }

                return cookieModel;
            }

            return null;
        }

        public void SetEnvironmentCookies(HttpRequestBase request,
            HttpResponseBase response,
            EnvironmentCookieModel selectedEnvironment)
        {
            // validate model before adding cookies
            if (selectedEnvironment != null
                && !string.IsNullOrWhiteSpace(selectedEnvironment.Name)
                && selectedEnvironment.EnvironmentEnum != EnvironmentEnum.Undefined)
            {
                // custom environments must always have app servers supplied
                // standard defined environments always use the default app servers for that environment
                // only set the name cookie for a non-custom env or a custom env with servers
                if (selectedEnvironment.EnvironmentEnum != EnvironmentEnum.Custom
                    || (selectedEnvironment.EnvironmentEnum == EnvironmentEnum.Custom
                    && selectedEnvironment.AppServers?.Count > 0))
                {
                    var environmentCookie = new HttpCookie(EnvironmentNameCookieName, selectedEnvironment.Name);
                    environmentCookie.Expires = DateTime.Now.AddHours(EnvironmentCookieDurationHours);
                    response.Cookies.Add(environmentCookie);

                    // only set the app servers cookie if the chosen environment is a "custom" environment definition different from the standard environments
                    if (selectedEnvironment.EnvironmentEnum == Enums.EnvironmentEnum.Custom
                        && selectedEnvironment.AppServers?.Count > 0)
                    {
                        // store the custom environment app servers in a cookie
                        var commaSeparatedListOfAppServers = string.Join(",", selectedEnvironment.AppServers);
                        var appServersCookie = new HttpCookie(EnvironmentAppServersCookieName, commaSeparatedListOfAppServers);
                        appServersCookie.Expires = DateTime.Now.AddHours(EnvironmentCookieDurationHours);
                        response.Cookies.Add(appServersCookie);
                    }
                }
            }
        }

        public void ClearEnvironmentCookies(HttpRequestBase request, HttpResponseBase response)
        {
            if (request.Cookies[EnvironmentNameCookieName] != null)
            {
                var environmentNameCookieForResponse = new HttpCookie(EnvironmentNameCookieName);
                environmentNameCookieForResponse.Expires = DateTime.Now.AddDays(-1);
                response.Cookies.Add(environmentNameCookieForResponse);
            }
            if (request.Cookies[EnvironmentAppServersCookieName] != null)
            {
                var appServersCookieForResponse = new HttpCookie(EnvironmentAppServersCookieName);
                appServersCookieForResponse.Expires = DateTime.Now.AddDays(-1);
                response.Cookies.Add(appServersCookieForResponse);
            }
        }

        internal string GetEnvironmentNameCookieValueOrNull(HttpRequestBase request)
        {
            if (request.Cookies[EnvironmentNameCookieName] != null)
            {
                var environmentNameCookie = request.Cookies[EnvironmentNameCookieName];
                if (environmentNameCookie != null
                    && !string.IsNullOrWhiteSpace(environmentNameCookie.Value))
                {
                    return environmentNameCookie.Value;
                }
            }
            return null;
        }

        internal List<string> GetAppServersCookieValueOrNull(HttpRequestBase request)
        {
            if (request.Cookies[EnvironmentAppServersCookieName] != null)
            {
                var appServersCookie = request.Cookies[EnvironmentAppServersCookieName];
                if (appServersCookie != null
                    && !string.IsNullOrWhiteSpace(appServersCookie.Value))
                {
                    return appServersCookie.Value.Split(',').ToList();
                }
            }
            return null;
        }
    } 
}