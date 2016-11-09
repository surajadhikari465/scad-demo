using System.Linq;
using Icon.Common;
using System.Security.Principal;
using System.Web;

namespace Icon.Web.Mvc.Utility
{
    public static class ViewHelpers
    {
        public static bool DisplayAdminLink(HttpRequestBase request, IPrincipal user)
        {
            if (request.IsAuthenticated)
            {
                var adminRoles = AppSettingsAccessor.GetStringSetting("AdminAccess").Split(',');

                if (adminRoles.Contains("AllRoles"))
                    return true;

                bool grantAccess = adminRoles.Any(r => user.IsInRole(r));

                return grantAccess;
            }

            return false;
        }
    }
}