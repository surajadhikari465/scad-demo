using ApplicationMonitor.Core;
using ApplicationMonitor.Core.Models;
using ApplicationMonitor.Web.Constants;
using ApplicationMonitor.Web.Extensions;
using ApplicationMonitor.Web.Infrastructure;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace ApplicationMonitor.Web.Controllers
{
    public class ApiControllerController : Controller
    {
        public ActionResult Index()
        {
            var apiControllers = ApplicationManager.GetApplication("API Controller", Server.MapPath(@"~/App_Data/Applications.xml"))
                .ToApiControllerViewModels(Environments.Icon, Environments.Esb);

            return View(apiControllers);
        }
    }
}