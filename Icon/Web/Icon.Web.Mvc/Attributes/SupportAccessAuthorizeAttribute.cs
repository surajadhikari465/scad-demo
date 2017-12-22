namespace Icon.Web.Mvc.Attributes
{
    using Icon.Common;
    using System;
    using System.Linq;
    using System.Net;
    using System.Web;
    using System.Web.Mvc;

    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
    public class SupportAccessAuthorizeAttribute : AuthorizeAttribute
    {
        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            if (httpContext.Request.IsAuthenticated)
            {
                var adminAccessRoles = AppSettingsAccessor.GetStringSetting("AdminAccess").Split(',');
                if (adminAccessRoles.Any(r => httpContext.User.IsInRole(r)))
                {
                    return base.AuthorizeCore(httpContext);
                }
            }
            return false;
        }
    }
}