using Icon.Dashboard.Mvc.Controllers;
using Icon.Dashboard.Mvc.Services;
using Icon.Dashboard.Mvc.ViewModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Icon.Dashboard.Mvc.UnitTests.ControllerTests.HomeControllerUnitTests
{
    [TestClass]
    public class _HomeControllerUnitTestBase : _MvcControllerUnitTestBase
    {
        public _HomeControllerUnitTestBase() : base() { }

        protected HomeController ConstructController()
        {
            return ConstructController(
                base.dashboardEnvironmentManager,
                base.iconDbServiceWrapper,
                base.mammothDbServiceWrapper,
                base.esbEnvironmentManager,
                base.wmiServiceWrapper);
        }

        protected HomeController ConstructController(
            IDashboardEnvironmentManager dashboardEnvironmentManager = null,
            IIconDatabaseServiceWrapper iconDbLoggingWrapper = null,
            IMammothDatabaseServiceWrapper mammothLoggingWrapper = null,
            IEsbEnvironmentManager esbEnvironmentMgmtSvc = null,
            IRemoteWmiServiceWrapper remoteWmiServiceWrapper = null)
        {
            var controller = new HomeController(
                dashboardEnvironmentManager,
                iconDbLoggingWrapper,
                mammothDbServiceWrapper,
                esbEnvironmentMgmtSvc,
                remoteWmiServiceWrapper);
            base.SetupMockHttpContext(controller);
            return controller;
        }
    }
}
