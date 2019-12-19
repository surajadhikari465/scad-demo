using Icon.Web.Mvc.Attributes;
using System.Web.Mvc;

namespace Icon.Web
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
            filters.Add(new ReadAccessAuthorizeAttribute());
        }
    }
}
