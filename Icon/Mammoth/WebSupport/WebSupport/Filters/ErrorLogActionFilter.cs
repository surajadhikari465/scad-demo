using Icon.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebSupport.Filters
{
    public class ErrorLogActionFilterAttribute : ActionFilterAttribute
    {
        private NLogLoggerSingleton logger;

        public ErrorLogActionFilterAttribute() : base()
        {
            this.logger = new NLogLoggerSingleton(typeof(NLogLoggerSingleton));
        }

        public override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            if (filterContext.Exception != null)
                logger.Error(filterContext.Exception.ToString());

            base.OnActionExecuted(filterContext);
        }
    }
}