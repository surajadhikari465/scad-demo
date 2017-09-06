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
            return ConstructController(base.dataFilePath, base.dataServiceWrapper, base.loggingServiceWrapper, base.serverUtility);
        }

        protected HomeController ConstructController(string pathToXmlDataFile = null,
            IDataFileServiceWrapper dataServiceWrapper = null,
            IIconDatabaseServiceWrapper loggingServiceWrapper = null,
            HttpServerUtilityBase serverUtility = null)
        {
            var controller = new HomeController(pathToXmlDataFile, dataServiceWrapper, loggingServiceWrapper, serverUtility);
            base.SetupMockHttpContext(controller);
            return controller;
        }

    }
}
