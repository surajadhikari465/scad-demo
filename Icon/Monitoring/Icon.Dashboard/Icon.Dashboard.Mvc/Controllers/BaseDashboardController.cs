using Icon.Dashboard.MammothDatabaseAccess;
using Icon.Dashboard.Mvc.Helpers;
using Icon.Dashboard.Mvc.Models;
using Icon.Dashboard.Mvc.Services;
using Icon.Dashboard.Mvc.ViewModels;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Icon.Dashboard.Mvc.Controllers
{
    /// <summary>
    /// Base class for dashboard cotnrollers
    /// </summary>
    public abstract class BaseDashboardController : Controller
    {
        public BaseDashboardController() : this(null, null, null) { }

        public BaseDashboardController(
            IDashboardEnvironmentManager environmentManager = null,
            IIconDatabaseServiceWrapper iconDbService = null,
            IMammothDatabaseServiceWrapper mammothDbService = null)
        {
            EnvironmentManager = environmentManager ?? new DashboardEnvironmentManager();
            IconDatabaseService = iconDbService ?? new IconDatabaseServiceWrapper();
            MammothDatabaseService = mammothDbService ?? new MammothDatabaseServiceWrapper();
        }

        protected IIconDatabaseServiceWrapper IconDatabaseService { get; private set; }
        protected IMammothDatabaseServiceWrapper MammothDatabaseService { get; private set; }
        protected IDashboardEnvironmentManager EnvironmentManager { get; private set; } 

    }
}