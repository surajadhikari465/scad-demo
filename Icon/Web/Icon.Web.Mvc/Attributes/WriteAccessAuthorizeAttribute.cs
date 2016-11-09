namespace Icon.Web.Mvc.Attributes
{
    using Icon.Common;
    using System;
    using System.Linq;
    using System.Net;
    using System.Web;
    using System.Web.Mvc;

    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
    public class WriteAccessAuthorizeAttribute : AuthorizeAttribute
    {
        public bool IsJsonResult { get; set; }

        public bool SetStatusCode { get; set; }

        public bool GlobalDataTeamException { get; set; }

        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            bool grantAccess;

            bool inforDisableAccess = AppSettingsAccessor.GetStringSetting("InforDisableIconInterface") == "1" ? true : false;

            grantAccess = GlobalDataTeamException || !inforDisableAccess;

            if (grantAccess && httpContext.Request.IsAuthenticated)
            {
                var writeAccessRoles = AppSettingsAccessor.GetStringSetting("WriteAccess").Split(',');

                grantAccess = writeAccessRoles.Any(r => httpContext.User.IsInRole(r));
            }

            if (grantAccess)
            {
                return base.AuthorizeCore(httpContext);
            }
            else
            {
                return false;
            }
        }

        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            if (!filterContext.HttpContext.User.Identity.IsAuthenticated)
            {
                // The user is not authenticated
                base.HandleUnauthorizedRequest(filterContext);
            }
            else if (!this.Roles.Split(',').Any(filterContext.HttpContext.User.IsInRole))
            {
                if (this.SetStatusCode)
                {
                    filterContext.HttpContext.Response.StatusCode =
                        (int)HttpStatusCode.Forbidden;
                }

                // The user is not in any of the listed roles => 
                // show the unauthorized view
                if (IsJsonResult)
                {
                    filterContext.Result = new JsonResult
                    {
                        Data = new
                        {
                            Success = false,
                            Error = "Access denied. This is now being managed in Infor.",
                            Message = "Access denied. This is now being managed in Infor."
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