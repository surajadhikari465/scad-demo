using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ApplicationDashboard.ViewModels;
using System.IO;
using ApplicationDashboard.Core;

namespace ApplicationDashboard.Controllers
{
    public class HomeController : Controller
    {
        public ApplicationContext ApplicationContext { get; private set; }

        public HomeController()
        {
            ApplicationContext = new ApplicationContext();
        }

        public IActionResult Index()
        {
            return View(ApplicationContext.Applications.Select(a => new ApplicationViewModel(a)));
        }

        public IActionResult Details(int id)
        {
            var application = ApplicationContext.GetApplication(id);

            return PartialView("_Details", new ApplicationViewModel(application));
        }

        public IActionResult Error()
        {
            return View();
        }
    }
}
