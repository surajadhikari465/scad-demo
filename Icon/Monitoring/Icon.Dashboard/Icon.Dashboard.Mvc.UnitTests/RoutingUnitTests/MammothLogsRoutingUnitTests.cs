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
        public void MvcRouting_MammothLogsIndexNoId_GetsLogsIndexRoute()
        {
            // Arrange
            string action = "Index";
            SetupRequestUrl($"~/{controller}/{action}");
            var routes = RegisterRoutesForTest();
            // Act
            RouteData routeData = routes.GetRouteData(moqContext.Object);
            // Assert
            AssertExpectedRouteControllerAndAction(routeData: routeData, controller: "MammothLogs", action: "Index");
        }

        [TestMethod]
        public void MvcRouting_MammothLogsIndexNoId_IdIsOptional()
        {
            // Arrange
            string action = "Index";
            SetupRequestUrl($"~/{controller}/{action}");
            var routes = RegisterRoutesForTest();
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
        public void MvcRouting_MammothLogsNoActionNoId_DefaultsToIndexRoute()
        {
            // Arrange
            SetupRequestUrl($"~/{controller}");
            var routes = RegisterRoutesForTest();
            // Act
            RouteData routeData = routes.GetRouteData(moqContext.Object);
            // Assert
            AssertExpectedRouteControllerAndAction(routeData, "MammothLogs", "Index");
        }

        [TestMethod]
        public void MvcRouting_MammothLogsNoActionNoId_IdIsOptional()
        {
            // Arrange
            SetupRequestUrl($"~/{controller}");
            var routes = RegisterRoutesForTest();
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
        public void MvcRouting_MammothLogsIndexWithId_GetsExpectedRoute()
        {
            // Arrange
            string action = "Index";
            string appName = "Mammoth Price Controller";
            SetupRequestUrl($"~/{controller}/{action}/{appName}");
            var routes = RegisterRoutesForTest();
            // Act
            RouteData routeData = routes.GetRouteData(moqContext.Object);
            // Assert
            AssertExpectedRouteControllerAndAction(routeData, "MammothLogs", "Index");
        }

        [TestMethod]
        public void MvcRouting_MammothLogsIndexWithId_UsesIdValue()
        {
            // Arrange
            string action = "Index";
            string appName = "Mammoth Price Controller";
            SetupRequestUrl($"~/{controller}/{action}/{appName}");
            var routes = RegisterRoutesForTest();
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