using System.Security.Principal;
using Icon.Dashboard.Mvc.Enums;

namespace Icon.Dashboard.Mvc.Filters
{
    public interface IDashboardAuthorizer
    {
        UserAuthorizationLevelEnum RequiredRole { get; set; }

        bool IsAuthorized(IPrincipal user, UserAuthorizationLevelEnum requiredAuthLevel);
    }
}