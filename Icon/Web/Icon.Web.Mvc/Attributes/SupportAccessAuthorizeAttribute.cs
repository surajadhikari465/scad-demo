using System;
using System.Linq;
using System.Configuration;
using System.Web;
using System.Web.Mvc;

namespace Icon.Web.Mvc.Attributes
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
    public class SupportAccessAuthorizeAttribute : AuthorizeAttribute
    {
        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            if(httpContext.Request.IsAuthenticated)
            {
                if(ConfigurationManager.AppSettings["AdminAccess"].Split(',').Any(r => httpContext.User.IsInRole(r.Trim())))
                {
                    return base.AuthorizeCore(httpContext);
                }
            }
            return false;
        }
    }
}