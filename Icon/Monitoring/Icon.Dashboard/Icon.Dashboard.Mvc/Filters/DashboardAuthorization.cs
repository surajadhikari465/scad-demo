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
        public UserAuthorizationLevelEnum RequiredRole { get; set; }
        internal static string ReadOnlyGroups = "IRMA.Applications";
        internal static string PrivilegedGroups = "IRMA.Developers";
        internal const string AppSettingForReadOnlyGroups = "securityGroupForReadOnly";
        internal const string AppSettingForPrivilegedGroups = "securityGroupForEditing";
        internal const string NotAuthorizedViewName = "~/Views/Shared/NotAuthorized.cshtml";
        internal const string ReadOnlyAuthorizedViewName = "~/Views/Shared/ReadOnlyAuthorized.cshtml";

        public DashboardAuthorization() : base()
        {
            ReadOnlyGroups = ConfigurationManager.AppSettings[AppSettingForReadOnlyGroups];
            PrivilegedGroups = ConfigurationManager.AppSettings[AppSettingForPrivilegedGroups];
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
                var viewName = (RequiredRole == UserAuthorizationLevelEnum.EditingPrivileges)
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

        internal static bool IsAuthorized(IPrincipal user, UserAuthorizationLevelEnum requiredAuthLevel)
        {
            var userAuthLevel = GetAuthorizationLevel(user);
            return ((userAuthLevel &= requiredAuthLevel) == requiredAuthLevel);
        }

        internal static UserAuthorizationLevelEnum GetAuthorizationLevel(IPrincipal user)
        {
            if (user != null)
            {
                if (!string.IsNullOrWhiteSpace(PrivilegedGroups))
                {
                    foreach (var privilegedRole in PrivilegedGroups.Split(','))
                    {
                        if (user.IsInRole(privilegedRole)) return UserAuthorizationLevelEnum.EditingPrivileges;
                    }
                }
                if (!string.IsNullOrWhiteSpace(ReadOnlyGroups))
                {
                    foreach (var readOnlyRole in ReadOnlyGroups.Split(','))
                    {
                        if (user.IsInRole(readOnlyRole)) return UserAuthorizationLevelEnum.ReadOnly;
                    }
                }
            }
            return UserAuthorizationLevelEnum.None;
        }
    }
}