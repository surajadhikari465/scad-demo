using System.Web;
using Icon.Dashboard.Mvc.Models;

namespace Icon.Dashboard.Mvc.Services
{
    public interface IEnvironmentCookieManager
    {
        string EnvironmentAppServersCookieName { get; set; }
        int EnvironmentCookieDurationHours { get; set; }
        string EnvironmentNameCookieName { get; set; }
        EnvironmentCookieModel GetEnvironmentCookieIfPresent(HttpRequestBase request);
        void SetEnvironmentCookies(HttpRequestBase request, HttpResponseBase response, EnvironmentCookieModel selectedEnvironment);
        void ClearEnvironmentCookies(HttpRequestBase request, HttpResponseBase response);
    }
}