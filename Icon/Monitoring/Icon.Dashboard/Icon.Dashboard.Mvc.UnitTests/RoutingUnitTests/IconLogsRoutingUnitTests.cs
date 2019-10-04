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
    public class IconLogsRoutingUnitTests : _MvcRoutingUnitTestBase
    {
        string controller = "IconLogs";

        [TestMethod]
        public void RegisterRoutes_IconLogs_Index_NoParam_GetsLogsIndexRoute()
        {
            // Arrange
            string action = "Index";
            var routes = base.SetMockPathAndRegisterRoutes($"~/{controller}/{action}");
            // Act
            RouteData routeData = routes.GetRouteData(moqContext.Object);
            // Assert
            AssertExpectedRouteControllerAndAction(routeData: routeData, controller: "IconLogs", action: "Index");
        }

        [TestMethod]
        public void RegisterRoutes_IconLogs_Index_NoParam_ParamIsOptional()
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
        public void RegisterRoutes_IconLogs_NoAction_NoParam_DefaultsToIndexRoute()
        {
            // Arrange
            var routes = base.SetMockPathAndRegisterRoutes($"~/{controller}");
            // Act
            RouteData routeData = routes.GetRouteData(moqContext.Object);
            // Assert
            AssertExpectedRouteControllerAndAction(routeData, "IconLogs", "Index");
        }

        [TestMethod]
        public void RegisterRoutes_IconLogs_NoAction_NoParam_ParamIsOptional()
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
        public void RegisterRoutes_IconLogs_Index_WithParam_GetsExpectedRoute()
        {
            // Arrange
            string action = "Index";
            string appName = "Global Controller";
            var routes = base.SetMockPathAndRegisterRoutes($"~/{controller}/{action}/{appName}");
            // Act
            RouteData routeData = routes.GetRouteData(moqContext.Object);
            // Assert
            AssertExpectedRouteControllerAndAction(routeData, "IconLogs", "Index");
        }

        [TestMethod]
        public void RegisterRoutes_IconLogs_Index_WithParam_UsesParamValue()
        {
            // Arrange
            string action = "Index";
            string appName = "Global Controller";
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