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
        public BaseDashboardController() : this (null, null) {}

        public BaseDashboardController(
            HttpServerUtilityBase serverUtility = null,
            IIconDatabaseServiceWrapper loggingServiceWrapper = null )
        {
            IconDatabaseService = loggingServiceWrapper ?? new IconDatabaseServiceWrapper();
            _serverUtility = serverUtility;
        }

        public BaseDashboardController(
             HttpServerUtilityBase serverUtility = null,
             IIconDatabaseServiceWrapper loggingServiceWrapper = null,
             string pathToXmlDataFile = null,
             IDataFileServiceWrapper dataServiceWrapper = null)
        {
            _serverUtility = serverUtility;
            DataFileName = pathToXmlDataFile ?? Utils.DataFileName;
            ViewBag.DataFilePath = DataFileName;
            DashboardDataFileService = dataServiceWrapper ?? new DataFileServiceWrapper();
            IconDatabaseService = loggingServiceWrapper ?? new IconDatabaseServiceWrapper();
        }

        private HttpServerUtilityBase _serverUtility;

        protected IIconDatabaseServiceWrapper IconDatabaseService { get; private set; }

        protected HttpServerUtilityBase ServerUtility
        {
            get
            {
                return _serverUtility ?? Server;
            }
        }
        protected IDataFileServiceWrapper DashboardDataFileService { get; private set; }

        protected string DataFileName { get; set; }
    }
}