using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OutOfStock.Controllers
{
    public class HomeController : Controller
    {

        public ActionResult Index()
        {
            OutOfStock.MvcApplication.oosLog.Trace("Enter");
            OutOfStock.MvcApplication.oosLog.Warn("Prevented access with insufficient permissions");
            ActionResult result = View();
            OutOfStock.MvcApplication.oosLog.Trace("Exit");
            return result;
        }

    }
}
