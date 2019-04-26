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
        public void MvcRouting_IconLogsIndexNoId_GetsLogsIndexRoute()
        {
            // Arrange
            string action = "Index";
            SetupRequestUrl($"~/{controller}/{action}");
            var routes = RegisterRoutesForTest();
            // Act
            RouteData routeData = routes.GetRouteData(moqContext.Object);
            // Assert
            AssertExpectedRouteControllerAndAction(routeData: routeData, controller: "IconLogs", action: "Index");
        }

        [TestMethod]
        public void MvcRouting_IconLogsIndexNoId_IdIsOptional()
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
        public void MvcRouting_IconLogsNoActionNoId_DefaultsToIndexRoute()
        {
            // Arrange
            SetupRequestUrl($"~/{controller}");
            var routes = RegisterRoutesForTest();
            // Act
            RouteData routeData = routes.GetRouteData(moqContext.Object);
            // Assert
            AssertExpectedRouteControllerAndAction(routeData, "IconLogs", "Index");
        }

        [TestMethod]
        public void MvcRouting_IconLogsNoActionNoId_IdIsOptional()
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
        public void MvcRouting_IconLogsIndexWithId_GetsExpectedRoute()
        {
            // Arrange
            string action = "Index";
            string appName = "Global Controller";
            SetupRequestUrl($"~/{controller}/{action}/{appName}");
            var routes = RegisterRoutesForTest();
            // Act
            RouteData routeData = routes.GetRouteData(moqContext.Object);
            // Assert
            AssertExpectedRouteControllerAndAction(routeData, "IconLogs", "Index");
        }

        [TestMethod]
        public void MvcRouting_IconLogsIndexWithId_UsesIdValue()
        {
            // Arrange
            string action = "Index";
            string appName = "Global Controller";
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