using Icon.Dashboard.Mvc.Controllers;
using Icon.Dashboard.Mvc.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Icon.Dashboard.Mvc.UnitTests.ControllerTests.HomeControllerUnitTests
{
    public class _HomeControllerUnitTestBase : _MvcControllerUnitTestBase
    {
        public _HomeControllerUnitTestBase() : base() { }

        protected HomeController ConstructController()
        {
            return ConstructController(
                base.iconDbServiceWrapper,
                base.mammothDbServiceWrapper,
                base.dashboardEnvironmentManager,
                base.esbEnvironmentManager,
                base.wmiServiceWrapper);
        }

        protected HomeController ConstructController(
            IIconDatabaseServiceWrapper iconDbLoggingWrapper = null,
            IMammothDatabaseServiceWrapper mammothLoggingWrapper = null,
            IDashboardEnvironmentManager dashboardEnvironmentManager = null,
            IEsbEnvironmentManager esbEnvironmentMgmtSvc = null,
            IRemoteWmiServiceWrapper remoteWmiServiceWrapper = null)
        {
            var controller = new HomeController(
                iconDbLoggingWrapper,
                mammothDbServiceWrapper,
                dashboardEnvironmentManager,
                esbEnvironmentMgmtSvc,
                remoteWmiServiceWrapper);
            base.SetupMockHttpContext(controller);
            return controller;
        }

    }
}
