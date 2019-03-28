using Icon.Dashboard.Mvc.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
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
        internal static string ReadOnlyGroupRole = "IRMA.Applications";
        internal static string PrivilegedGroupRole = "IRMA.Developers";
        internal const string AppSettingForReadOnlyGroup = "securityGroupForReadOnly";
        internal const string AppSettingForEditingGroup = "securityGroupForEditing";
        internal const string NotAuthorizedViewName = "~/Views/Shared/NotAuthorized.cshtml";
        internal const string ReadOnlyAuthorizedViewName = "~/Views/Shared/ReadOnlyAuthorized.cshtml";

        public DashboardAuthorization() : base()
        {
            ReadOnlyGroupRole = ConfigurationManager.AppSettings[AppSettingForReadOnlyGroup];
            PrivilegedGroupRole = ConfigurationManager.AppSettings[AppSettingForEditingGroup];
        }

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
                var viewName = (RequiredRole == UserRoleEnum.EditingPrivileges)
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
                role = (user.IsInRole(ReadOnlyGroupRole) ? UserRoleEnum.ReadOnly : UserRoleEnum.Unauthorized)
                       | (user.IsInRole(PrivilegedGroupRole) ? UserRoleEnum.EditingPrivileges : UserRoleEnum.Unauthorized);
            }
            return role;
        }
    }
}