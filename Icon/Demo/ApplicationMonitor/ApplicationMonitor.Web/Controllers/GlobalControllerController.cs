using ApplicationMonitor.Core;
using ApplicationMonitor.Web.Extensions;
using ApplicationMonitor.Web.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ApplicationMonitor.Web.Controllers
{
    public class GlobalControllerController : Controller
    {
        public ActionResult Index()
        {
            var controllers = ApplicationManager.GetApplication("Global Controller", Server.MapPath(@"~/App_Data/Applications.xml"))
                .ToGlobalControllerViewModels(Environments.Icon);

            return View(controllers);
        }
    }
}