using Icon.Dashboard.Mvc.Filters;
using Icon.Dashboard.Mvc.Helpers;
using Icon.Dashboard.Mvc.Models;
using Icon.Dashboard.Mvc.Services;
using Icon.Dashboard.Mvc.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Icon.Dashboard.Mvc.Controllers
{
    public class EsbController : BaseDashboardController
    {
        public EsbController() : this(null, null, null, null, null) { }

        public EsbController(
            HttpServerUtilityBase serverUtility = null,
            IIconDatabaseServiceWrapper iconDbService = null,
            string pathToXmlDataFile = null,
            IDataFileServiceWrapper dataServiceWrapper = null,
            IMammothDatabaseServiceWrapper mammothDbService = null)
            : base(serverUtility, iconDbService, pathToXmlDataFile, dataServiceWrapper, mammothDbService) { }

        #region GET

        [HttpGet]
        [DashboardAuthorization(RequiredRole = UserAuthorizationLevelEnum.ReadOnly)]
        public ActionResult Index()
        {
            //enable filter to use the logging service for building menus
            HttpContext.Items["loggingDataService"] = IconDatabaseService;
            ViewBag.Action = nameof(Index);
            ViewBag.Title = "Configure ESB Apps";
            var dataFileWebServerPath = Utils.GetPathForDataFile(ServerUtility, DataFileName);
            var viewData = DashboardDataFileService.GetEsbEnvironments(dataFileWebServerPath);
            return View(viewData);
        }

        [HttpGet]
        [DashboardAuthorization(RequiredRole = UserAuthorizationLevelEnum.ReadOnly)]
        public ActionResult Details(string name)
        {
            //enable filter to use the logging service for building menus
            HttpContext.Items["loggingDataService"] = IconDatabaseService;

            if (String.IsNullOrWhiteSpace(name))
            {
                return RedirectToAction("Index");
            }
            var dataFileWebServerPath = Utils.GetPathForDataFile(ServerUtility, DataFileName);
            var esbEnvironment = DashboardDataFileService.GetEsbEnvironment(dataFileWebServerPath, name);
            ViewBag.Action = "Details";
            ViewBag.Title = String.Format("View ESB Environment \"{0}\" Configuration", esbEnvironment.Name);
            return View(esbEnvironment);
        }

        [HttpGet]
        [DashboardAuthorization(RequiredRole = UserAuthorizationLevelEnum.ReadOnly)]
        public ActionResult Edit(string name)
        {
            //enable filter to use the logging service for building menus
            HttpContext.Items["loggingDataService"] = IconDatabaseService;

            if (String.IsNullOrWhiteSpace(name))
            {
                return RedirectToAction("Index");
            }
            var dataFileWebServerPath = Utils.GetPathForDataFile(ServerUtility, DataFileName);
            var esbEnvironment = DashboardDataFileService.GetEsbEnvironment(dataFileWebServerPath, name);

            ViewBag.Title = String.Format("Edit ESB Environment \"{0}\" Configuration", esbEnvironment.Name);
            return View(esbEnvironment);
        }

        [HttpGet]
        [DashboardAuthorization(RequiredRole = UserAuthorizationLevelEnum.ReadOnly)]
        public ActionResult Create()
        {
            //enable filter to use the logging service for building menus
            HttpContext.Items["loggingDataService"] = IconDatabaseService;

            var viewModel = new EsbEnvironmentViewModel();
            ViewBag.Title = "Create New ESB Environment Configuration";
            return View(viewModel);
        }

        #endregion

        #region POST
        [HttpPost]
        [DashboardAuthorization(RequiredRole = UserAuthorizationLevelEnum.EditingPrivileges)]
        public ActionResult Index(IEnumerable<EsbEnvironmentViewModel> esbEnvironments)
        {
            var dataFileWebServerPath = Utils.GetPathForDataFile(ServerUtility, DataFileName);

            var updated = DashboardDataFileService.ReconfigureEsbApps(dataFileWebServerPath, esbEnvironments);
            
            return RedirectToAction(nameof(Index), "Esb");
        }

        [HttpPost]
        [DashboardAuthorization(RequiredRole = UserAuthorizationLevelEnum.EditingPrivileges)]
        public ActionResult Edit(EsbEnvironmentViewModel model)
        {
            if (ModelState.IsValid)
            {
                var dataFileWebServerPath = Utils.GetPathForDataFile(ServerUtility, DataFileName);
                DashboardDataFileService.UpdateEsbEnvironment(dataFileWebServerPath, model);
                return RedirectToAction("Details", "Esb", new { name = model.Name });
            }

            return View("Edit", model);
        }

        [HttpPost]
        [DashboardAuthorization(RequiredRole = UserAuthorizationLevelEnum.EditingPrivileges)]
        public ActionResult Create(EsbEnvironmentViewModel model)
        {
            if (ModelState.IsValid)
            {
                var dataFileWebServerPath = Utils.GetPathForDataFile(ServerUtility, DataFileName);
                DashboardDataFileService.AddEsbEnvironment(dataFileWebServerPath, model);
                return RedirectToAction("Index");
            }

            return View("Create", model);
        }

        [HttpPost]
        [DashboardAuthorization(RequiredRole = UserAuthorizationLevelEnum.EditingPrivileges)]
        public ActionResult Delete(string name)
        {
            var dataFileWebServerPath = Utils.GetPathForDataFile(ServerUtility, DataFileName);
            var esbEnvironment = DashboardDataFileService.GetEsbEnvironment(dataFileWebServerPath, name);
            if (esbEnvironment != null)
            {
                DashboardDataFileService.DeleteEsbEnvironment(dataFileWebServerPath, esbEnvironment);
            }
            return RedirectToAction("Index");
        }
        #endregion
    }
}