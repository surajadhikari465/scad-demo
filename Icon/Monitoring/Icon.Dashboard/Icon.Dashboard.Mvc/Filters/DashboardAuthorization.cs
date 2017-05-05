using Icon.Dashboard.Mvc.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Web;
using System.Web.Mvc;

namespace Icon.Dashboard.Mvc.Filters
{
    /// <summary>
    /// Custom Authorization Attribute for determining whether users belong to the 
    ///  expected security groups which have rights to use the Dashboard
    /// </summary>
    public sealed class DashboardAuthorization : AuthorizeAttribute
    {
        public UserRoleEnum RequiredRole { get; set; }
        public const string IrmaApplicationsRoleName = "IRMA.Applications";
        public const string IrmaDevelopersRoleName = "IRMA.Developers";
        public const string NotAuthorizedViewName = "~/Views/Shared/NotAuthorized.cshtml";
        public const string ReadOnlyAuthorizedViewName = "~/Views/Shared/ReadOnlyAuthorized.cshtml";

        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            var baseAuth = base.AuthorizeCore(httpContext);
            return baseAuth && IsAuthorized(httpContext.User, RequiredRole);
        }

        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            if (!filterContext.HttpContext.User.Identity.IsAuthenticated)
            {
                // user is not authenticated
                base.HandleUnauthorizedRequest(filterContext);
            }
            else if (!IsAuthorized(filterContext.HttpContext.User, RequiredRole))
            {
                var viewName = (RequiredRole == UserRoleEnum.IrmaDeveloper)
                    ? ReadOnlyAuthorizedViewName 
                    : NotAuthorizedViewName;
                // The user is not in any of the listed roles show the unauthorized view
                filterContext.Result = new ViewResult
                {
                    ViewName = viewName
                };
            }
            else
            {
                // just in case...
                base.HandleUnauthorizedRequest(filterContext);
            };
        }

        internal static bool IsAuthorized(IPrincipal user, UserRoleEnum requiredRole)
        {
            var userRole = GetUserRole(user);
            return ((userRole &= requiredRole) == requiredRole);
        }

        internal static UserRoleEnum GetUserRole(IPrincipal user)
        {
            var role = UserRoleEnum.Unauthorized;

            if (user != null)
            {
                role = (user.IsInRole(IrmaApplicationsRoleName) ? UserRoleEnum.IrmaApplications : UserRoleEnum.Unauthorized)
                       | (user.IsInRole(IrmaDevelopersRoleName) ? UserRoleEnum.IrmaDeveloper : UserRoleEnum.Unauthorized);
            }
            return role;
        }
    }
}