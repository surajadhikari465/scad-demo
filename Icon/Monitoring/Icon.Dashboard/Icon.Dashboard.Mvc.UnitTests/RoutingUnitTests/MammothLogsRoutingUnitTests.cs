using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace Icon.Dashboard.Mvc.UnitTests.RoutingUnitTests
{

    [TestClass]
    public class MammothLogsRoutingUnitTests : _MvcRoutingUnitTestBase
    {
        string controller = "MammothLogs";

        [TestMethod]
        public void RegisterRoutes_MammothLogs_Index_NoParam_GetsLogsIndexRoute()
        {
            // Arrange
            string action = "Index";
            var routes = base.SetMockPathAndRegisterRoutes($"~/{controller}/{action}");
            // Act
            RouteData routeData = routes.GetRouteData(moqContext.Object);
            // Assert
            AssertExpectedRouteControllerAndAction(routeData: routeData, controller: "MammothLogs", action: "Index");
        }

        [TestMethod]
        public void RegisterRoutes_MammothLogs_Index_NoParam_ParamIsOptional()
        {
            // Arrange
            string action = "Index";
            var routes = base.SetMockPathAndRegisterRoutes($"~/{controller}/{action}");
            // Act
            RouteData routeData = routes.GetRouteData(moqContext.Object);
            // Assert
            var expectedValues = new Dictionary<string, object>()
            {
                {"appName", UrlParameter.Optional }
            };
            AssertDesiredRouteDataIsPresent(routeData, expectedValues);
        }

        [TestMethod]
        public void RegisterRoutes_MammothLogs_NoAction_NoParam_DefaultsToIndexRoute()
        {
            // Arrange
            var routes = base.SetMockPathAndRegisterRoutes($"~/{controller}");
            // Act
            RouteData routeData = routes.GetRouteData(moqContext.Object);
            // Assert
            AssertExpectedRouteControllerAndAction(routeData, "MammothLogs", "Index");
        }

        [TestMethod]
        public void RegisterRoutes_MammothLogs_NoAction_NoParam_IdIsOptional()
        {
            // Arrange
            var routes = base.SetMockPathAndRegisterRoutes($"~/{controller}");
            // Act
            RouteData routeData = routes.GetRouteData(moqContext.Object);
            // Assert
            var expectedValues = new Dictionary<string, object>()
            {
                {"appName", UrlParameter.Optional }
            };
            AssertDesiredRouteDataIsPresent(routeData, expectedValues);
        }

        [TestMethod]
        public void RegisterRoutes_MammothLogs_Index_WithParam_GetsExpectedRoute()
        {
            // Arrange
            string action = "Index";
            string appName = "Mammoth Price Controller";
            var routes = base.SetMockPathAndRegisterRoutes($"~/{controller}/{action}/{appName}");
            // Act
            RouteData routeData = routes.GetRouteData(moqContext.Object);
            // Assert
            AssertExpectedRouteControllerAndAction(routeData, "MammothLogs", "Index");
        }

        [TestMethod]
        public void RegisterRoutes_MammothLogs_Index_WithParam_UsesParamValue()
        {
            // Arrange
            string action = "Index";
            string appName = "Mammoth Price Controller";
            var routes = base.SetMockPathAndRegisterRoutes($"~/{controller}/{action}/{appName}");
            // Act
            RouteData routeData = routes.GetRouteData(moqContext.Object);
            // Assert
            var expectedValues = new Dictionary<string, object>()
            {
                {"appName", appName }
            };
            AssertDesiredRouteDataIsPresent(routeData, expectedValues);
        }
    }
}