using Icon.Dashboard.Mvc.Helpers;
using Icon.Dashboard.Mvc.Infrastructure;
using Icon.Dashboard.Mvc.Services;
using Icon.Dashboard.Mvc.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Icon.Dashboard.Mvc.Controllers
{
    public class EsbSwitchController : Controller
    {

        private const string defaultDataFile = "DashboardApplications.xml";

        public IDataFileServiceWrapper DashboardDataFileService { get; private set; }
        public IIconDatabaseServiceWrapper IconDatabaseDataAccess { get; private set; }

        public string XmlDataFile { get; set; }

        private HttpServerUtilityBase _serverUtility;
        public HttpServerUtilityBase ServerUtility
        {
            get
            {
                return _serverUtility ?? Server;
            }
        }

        public EsbSwitchController() : this(null, null, null, null)
        {
        }

        public EsbSwitchController(string dataFile = null,
            IDataFileServiceWrapper dataServiceWrapper = null,
            IIconDatabaseServiceWrapper loggingServiceWrapper = null,
            HttpServerUtilityBase serverUtility = null)
        {
            XmlDataFile = dataFile ?? defaultDataFile;
            DashboardDataFileService = dataServiceWrapper ?? new DataFileServiceWrapper();
            IconDatabaseDataAccess = loggingServiceWrapper ?? new IconDatabaseServiceWrapper();

            _serverUtility = serverUtility;
        }

        [HttpGet]
        [SetViewBagMenuOptionsFilter]
        public ActionResult Index()
        {
            //enable filter to use the logging service for building menus
            HttpContext.Items["loggingDataService"] = IconDatabaseDataAccess;

            ViewBag.Action = "Index";
            ViewBag.Title = $"ESB Environments";
            var esbEnvironments = DashboardDataFileService.GetEsbEnvironments(ServerUtility, XmlDataFile)
                .OrderBy(e => e.Name);
            //var currentEnvironment = DashboardDataFileService.GetCurrentEsbEnvironment(ServerUtility, XmlDataFile);
            //ViewBag.CurrentEsbEnvironment = currentEnvironment?.Name;
            return View(esbEnvironments);
        }

        [HttpGet]
        [SetViewBagMenuOptionsFilter]
        public ActionResult Details(string name)
        {
            //enable filter to use the logging service for building menus
            HttpContext.Items["loggingDataService"] = IconDatabaseDataAccess;

            if (String.IsNullOrWhiteSpace(name))
            {
                return RedirectToAction("Index");
            }
            var esbEnvironment = DashboardDataFileService.GetEsbEnvironment(ServerUtility, XmlDataFile, name);
            ViewBag.Action = "Details";
            ViewBag.Title = $"View ESB Environment \"{esbEnvironment?.Name}\" Configuration";
            return View(esbEnvironment);
        }

        [HttpGet]
        [SetViewBagMenuOptionsFilter]
        public ActionResult Edit(string name)
        {
            //enable filter to use the logging service for building menus
            HttpContext.Items["loggingDataService"] = IconDatabaseDataAccess;

            if (String.IsNullOrWhiteSpace(name))
            {
                return RedirectToAction("Index");
            }
            var esbEnvironment = DashboardDataFileService.GetEsbEnvironment(ServerUtility, XmlDataFile, name);
            var currentEnvironment = DashboardDataFileService.GetCurrentEsbEnvironment(ServerUtility, XmlDataFile);
            // populate ApplicationsList for each model. We need this to display in dropdown
            if (esbEnvironment.Applications != null && esbEnvironment.Applications.Count>0)
            {
                esbEnvironment.Applications[0].ApplicationsList = DashboardDataFileService.GetApplications(ServerUtility, XmlDataFile);
            }
            foreach (IconApplicationIdentifierViewModel app in esbEnvironment.Applications)
            {
                app.ApplicationsList = esbEnvironment.Applications[0].ApplicationsList;
            }
   
            ViewBag.Title = $"Edit ESB Environment \"{esbEnvironment?.Name}\" Configuration";
            return View(esbEnvironment);
        }

        [HttpPost]
        [SetViewBagMenuOptionsFilter]
        public ActionResult Edit(EsbEnvironmentViewModel model)
        {
            if (ModelState.IsValid)
            {
                //try {
                DashboardDataFileService.UpdateEsbEnvironment(ServerUtility, model, XmlDataFile);
                return RedirectToAction("Details", "EsbSwitch", new { name = model.Name });
            }
            
            return View("Edit", model);
        }

        [HttpGet]
        [SetViewBagMenuOptionsFilter]
        public ActionResult Create()
        {
            //enable filter to use the logging service for building menus
            HttpContext.Items["loggingDataService"] = IconDatabaseDataAccess;

            var viewModel = new EsbEnvironmentViewModel();
            ViewBag.Title = $"Create New ESB Environment Configuration";
            return View(viewModel);
        }

        [HttpPost]
        [SetViewBagMenuOptionsFilter]
        public ActionResult Create(EsbEnvironmentViewModel model)
        {
            if (ModelState.IsValid)
            {
                DashboardDataFileService.AddEsbEnvironment(ServerUtility, model, XmlDataFile);
                return RedirectToAction("Index");
            }

            return View("Create", model);
        }

        [HttpPost]
        public ActionResult Delete(string name)
        {
            var esbEnvironment = DashboardDataFileService.GetEsbEnvironment(ServerUtility, XmlDataFile, name);
            if (esbEnvironment != null)
            {
                DashboardDataFileService.DeleteEsbEnvironment(ServerUtility, esbEnvironment, XmlDataFile);
            }
            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult AddApplicationRow()
        {
            // pass all appplications to it
            IconApplicationIdentifierViewModel emptyViewModel;
            IEnumerable<DataFileAccess.Models.IApplication> ApplicationsList = DashboardDataFileService.GetApplications(ServerUtility, XmlDataFile);
            // check if application list is not null and set selected item as first item in the list
            if(ApplicationsList!= null && ApplicationsList.Count() > 0)
            {
                 emptyViewModel = new IconApplicationIdentifierViewModel(ApplicationsList.First().Name, "test");
            }
            else
            {
                emptyViewModel = new IconApplicationIdentifierViewModel("appName", "server");
            }
            emptyViewModel.ApplicationsList = ApplicationsList;

            return PartialView("EditorTemplates/IconApplicationIdentifierViewModel", emptyViewModel);
        }

        [HttpPost]
        public ActionResult SetEsbEnvironment(string name)
        {
            var esbEnvironment = DashboardDataFileService.GetEsbEnvironment(ServerUtility, XmlDataFile, name);
            var results = DashboardDataFileService.SetEsbEnvironment(ServerUtility, esbEnvironment, XmlDataFile);

            var failures = results.Where(r => r.Item1 == false).ToList();
            string reportMessage = failures != null && failures.Count > 0
                ? $"Not all aplications set for '{name}' ESB Envrionment.{Environment.NewLine}{String.Join(Environment.NewLine, failures.Select(f=>f.Item2))}"
                : $"Applications successfully set for ESB environment '{name}'.";

            return Json(new { message = reportMessage });
        }

        [HttpGet]
        public ActionResult GetCurrentEsbEnvironment()
        {
            var currentEnvironment = DashboardDataFileService.GetCurrentEsbEnvironment(ServerUtility, XmlDataFile);
            //ViewBag.CurrentEsbEnvironment = currentEnvironment?.Name;
            return Json(currentEnvironment?.Name, JsonRequestBehavior.AllowGet);
        }
    }
}