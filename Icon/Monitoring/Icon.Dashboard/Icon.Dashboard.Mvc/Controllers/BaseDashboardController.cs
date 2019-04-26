using Icon.Dashboard.MammothDatabaseAccess;
using Icon.Dashboard.Mvc.Helpers;
using Icon.Dashboard.Mvc.Services;
using System;
using System.Collections.Generic;
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
        public BaseDashboardController() : this(null, null) { }

        public BaseDashboardController(
           IIconDatabaseServiceWrapper iconDbService = null,
           IMammothDatabaseServiceWrapper mammothDbService = null)
        {
            IconDatabaseService = iconDbService ?? new IconDatabaseServiceWrapper();
            MammothDatabaseService = mammothDbService ?? new MammothDatabaseServiceWrapper();
        }

        protected IIconDatabaseServiceWrapper IconDatabaseService { get; private set; }
        protected IMammothDatabaseServiceWrapper MammothDatabaseService { get; private set; }

    }
}