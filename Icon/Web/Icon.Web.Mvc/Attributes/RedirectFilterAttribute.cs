using System.Web.Mvc;
using System.Web.Routing;

public class RedirectFilterAttribute : ActionFilterAttribute
{
    public override void OnActionExecuting(ActionExecutingContext filterContext)
    {
        filterContext.Result = new RedirectToRouteResult(
                           new RouteValueDictionary
                           {
                            { "controller", "Home" },
                            { "action", "Index" }
                           });

        base.OnActionExecuting(filterContext);
    }
}