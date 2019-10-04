using Icon.Dashboard.Mvc.Enums;
using Icon.Dashboard.Mvc.Helpers;
using Icon.Dashboard.Mvc.Models;
using Icon.Dashboard.Mvc.Services;
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
    public sealed class DashboardAuthorizer : AuthorizeAttribute, IDashboardAuthorizer
    {
        public UserAuthorizationLevelEnum RequiredRole { get; set; }
        public static List<string> ReadOnlyGroups { get; set; } 
        public static List<string> PrivilegedGroups { get; set; }
        internal const string NotAuthorizedViewName = Constants.MvcNames.Views.NotAuthorized;
        internal const string ReadOnlyAuthorizedViewName = Constants.MvcNames.Views.ReadOnlyAuthorized;

        public DashboardAuthorizer() : base()
        {
            ReadOnlyGroups = DashboardGlobals.ConfigData.SecurityGroupsWithReadRights;
            PrivilegedGroups = DashboardGlobals.ConfigData.SecurityGroupsWithEditRights;
        }

        public DashboardAuthorizer(List<string> readOnlyGroups, List<string> privilegedGroups) : base()
        {
            ReadOnlyGroups = readOnlyGroups;
            PrivilegedGroups = privilegedGroups;
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
                // The user is not in any of the listed roles show the unauthorized view
                filterContext.Result = new ViewResult
                {
                    ViewName = RequiredRole == UserAuthorizationLevelEnum.EditingPrivileges
                        ? ReadOnlyAuthorizedViewName
                        : NotAuthorizedViewName

                };
            }
            else
            {
                // just in case...
                base.HandleUnauthorizedRequest(filterContext);
            };
        }

        public bool IsAuthorized(IPrincipal user, UserAuthorizationLevelEnum requiredAuthLevel)
        {
            var userAuthLevel = GetAuthorizationLevel(user);
            return ((userAuthLevel &= requiredAuthLevel) == requiredAuthLevel);
        }

        internal UserAuthorizationLevelEnum GetAuthorizationLevel(IPrincipal user)
        {
            if (user != null)
            {
                if (PrivilegedGroups != null)
                {
                    foreach (var privilegedRole in PrivilegedGroups)
                    {
                        if (user.IsInRole(privilegedRole))
                        {
                            return UserAuthorizationLevelEnum.EditingPrivileges;
                        }
                    }
                }
                if (ReadOnlyGroups != null)
                {
                    foreach (var readOnlyRole in ReadOnlyGroups)
                    {
                        if (user.IsInRole(readOnlyRole))
                        {
                            return UserAuthorizationLevelEnum.ReadOnly;
                        }
                    }
                }
            }
            return UserAuthorizationLevelEnum.None;
        }
    }
}