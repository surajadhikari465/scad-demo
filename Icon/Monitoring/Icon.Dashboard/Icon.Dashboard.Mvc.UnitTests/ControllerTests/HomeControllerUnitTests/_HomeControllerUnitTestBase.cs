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
            return ConstructController(base.serverUtility,
                base.iconDbServiceWrapper,
                base.dataFilePath,
                base.dataServiceWrapper,
                base.mammothDbServiceWrapper,
                base.wmiServiceWrapper);
        }

        protected HomeController ConstructController(
            HttpServerUtilityBase serverUtility = null,
            IIconDatabaseServiceWrapper loggingServiceWrapper = null,
            string pathToXmlDataFile = null,
            IDataFileServiceWrapper dataServiceWrapper = null,
            IMammothDatabaseServiceWrapper mammothLoggingWrapper = null,
            IRemoteWmiServiceWrapper remoteWmiServiceWrapper = null)
        {
            var controller = new HomeController(serverUtility,
                loggingServiceWrapper,
                pathToXmlDataFile,
                dataServiceWrapper,
                mammothDbServiceWrapper,
                remoteWmiServiceWrapper);
            base.SetupMockHttpContext(controller);
            return controller;
        }

    }
}
