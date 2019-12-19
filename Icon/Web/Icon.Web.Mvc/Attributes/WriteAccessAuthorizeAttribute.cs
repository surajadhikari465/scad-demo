using System;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Configuration;

namespace Icon.Web.Mvc.Attributes
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
    public class WriteAccessAuthorizeAttribute : AuthorizeAttribute
    {
        public bool IsJsonResult { get; set; }

        public bool SetStatusCode { get; set; }

        public bool GlobalDataTeamException { get; set; }

        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            bool grantAccess = false ;

            if(httpContext.Request.IsAuthenticated)
            {
                grantAccess = ConfigurationManager.AppSettings["WriteAccess"].Split(',').Any(x => httpContext.User.IsInRole(x.Trim()));
            }

            return grantAccess ? base.AuthorizeCore(httpContext) : false;
        }

        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            if(!filterContext.HttpContext.User.Identity.IsAuthenticated)
            {
                // The user is not authenticated
                base.HandleUnauthorizedRequest(filterContext);
            }
            else if(!this.Roles.Split(',').Any(filterContext.HttpContext.User.IsInRole))
            {
                if(this.SetStatusCode)
                {
                    filterContext.HttpContext.Response.StatusCode = (int)HttpStatusCode.Forbidden;
                }

                // The user is not in any of the listed roles => show the unauthorized view
                if(IsJsonResult)
                {
                    filterContext.Result = new JsonResult
                    {
                        Data = new
                        {
                            Success = false,
                            Error = "Access denied. You do not have sufficient permissions to perform this operation.",
                            Message = "Access denied. You do not have sufficient permissions to perform this operation."
                        }
                    };
                }
                else
                {
                    filterContext.Result = new ViewResult
                    {
                        ViewName = "~/Views/Shared/AccessDenied.cshtml"
                    };
                }
            }
            else
            {
                base.HandleUnauthorizedRequest(filterContext);
            }
        }
    }
}