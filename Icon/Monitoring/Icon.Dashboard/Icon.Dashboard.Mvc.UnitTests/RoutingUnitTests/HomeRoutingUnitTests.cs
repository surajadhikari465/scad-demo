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
    public class HomeRoutingUnitTests : _MvcRoutingUnitTestBase
    {
        string controller = "Home";

        [TestMethod]
        public void RegisterRoutes_Home_Index_NoParam_GetsHomeIndexRoute()
        {
            // Arrange
            string action = "Index";
            var routes = base.SetMockPathAndRegisterRoutes($"~/{controller}/{action}");
            // Act
            RouteData routeData = routes.GetRouteData(moqContext.Object);
            // Assert
            AssertExpectedRouteControllerAndAction(routeData:routeData, controller:"Home", action:"Index");
        }

        [TestMethod]
        public void RegisterRoutes_Home_NoAction_NoParam_ShouldRouteToDefaultHome()
        {
            // Arrange
            var routes = base.SetMockPathAndRegisterRoutes($"~/{controller}");
            // Act
            RouteData routeData = routes.GetRouteData(moqContext.Object);
            // Assert
            AssertExpectedRouteControllerAndAction(routeData: routeData, controller: "Home", action: "Index");
        }

        [TestMethod]
        public void RegisterRoutes_Home_Index_WithParam_GetsHomeIndexRoute()
        {
            // Arrange
            string action = "Index";
            string environment = "Tst0";
            var routes = base.SetMockPathAndRegisterRoutes($"~/{controller}/{action}/{environment}");
            // Act
            RouteData routeData = routes.GetRouteData(moqContext.Object);
            // Assert
            AssertExpectedRouteControllerAndAction(routeData: routeData, controller: "Home", action: "Index");
        }

        [TestMethod]
        public void RegisterRoutes_Home_Index_WithParam_GetsHomeIndexRoute_IgnoresParam()
        {
            // Arrange
            string action = "Index";
            string environment = "Perf";
            var routes = base.SetMockPathAndRegisterRoutes($"~/{controller}/{action}/{environment}");
            // Act
            RouteData routeData = routes.GetRouteData(moqContext.Object);
            // Assert
            var routingDataThatShouldNotBeThere = new List<string>() { environment };
            AssertUndesirableRouteDataIsAbsent(routeData, routingDataThatShouldNotBeThere);
        }

        [TestMethod]
        public void RegisterRoutes_Home_Edit_WithParams_GetsHomeEditRoute()
        {
            // Arrange
            string action = "Edit";
            string appServerParam = "aServer";
            string serviceNameParam = "myApplication";
            var routes = base.SetMockPathAndRegisterRoutes($"~/{controller}/{action}/{appServerParam}/{serviceNameParam}");
            // Act
            RouteData routeData = routes.GetRouteData(moqContext.Object);
            // Assert
            AssertExpectedRouteControllerAndAction(routeData: routeData, controller: "Home", action: action);
        }

        [TestMethod]
        public void RegisterRoutes_Home_Edit_WithParams_HasExectedQueryValues()
        {
            // Arrange
            string action = "Edit";
            string appServerParam = "aServer";
            string serviceNameParam = "myApplication";
            var routes = base.SetMockPathAndRegisterRoutes($"~/{controller}/{action}/{appServerParam}/{serviceNameParam}");
            // Act
            RouteData routeData = routes.GetRouteData(moqContext.Object);
            // Assert
            var expectedValues = new Dictionary<string, object>()
            {
                {"appServer", appServerParam },
                {"serviceName", serviceNameParam } 
            };
            AssertDesiredRouteDataIsPresent(routeData, expectedValues);
        }

        [TestMethod]
        public void RegisterRoutes_Home_Edit_NoParams_ShouldRouteToHomeEdit()
        {
            // Arrange
            // will route to Home/Edit then Redirect to Home/Index
            string action = "Edit";
            var routes = base.SetMockPathAndRegisterRoutes($"~/{controller}/{action}");
            // Act
            RouteData routeData = routes.GetRouteData(moqContext.Object);
            // Assert
            AssertExpectedRouteControllerAndAction(routeData: routeData, controller: "Home", action: "Edit");
        }

        [TestMethod]
        public void RegisterRoutes_Home_SetAlternateEnvironment_NoParam_ShouldRouteToDefaultHome()
        {
            // Arrange
            string action = "SetAlternateEnvironment";
            var routes = base.SetMockPathAndRegisterRoutes($"~/{controller}/{action}");
            // Act
            RouteData routeData = routes.GetRouteData(moqContext.Object);
            // Assert
            AssertExpectedRouteControllerAndAction(routeData: routeData, controller: "Home", action: "Index");
        }

        [TestMethod]
        public void RegisterRoutes_Home_SetAlternateEnvironment_WithParam_GetsExpectedRoute()
        {
            // Arrange
            string action = "SetAlternateEnvironment";
            string environment = "Perf";
            var routes = base.SetMockPathAndRegisterRoutes($"~/{controller}/{action}/{environment}");
            // Act
            RouteData routeData = routes.GetRouteData(moqContext.Object);
            // Assert
            AssertExpectedRouteControllerAndAction(
                routeData: routeData, controller: "Home", action: "SetAlternateEnvironment");
        }

        [TestMethod]
        public void RegisterRoutes_Home_SetAlternateEnvironment_WithParam_GetsExpectedRoute_WithParamValue()
        {
            // Arrange
            string action = "SetAlternateEnvironment";
            string environment = "Perf";
            var routes = base.SetMockPathAndRegisterRoutes($"~/{controller}/{action}/{environment}");
            // Act
            RouteData routeData = routes.GetRouteData(moqContext.Object);
            // Assert
            var expectedValues = new Dictionary<string, object>()
            {
                {"environment", environment }
            };
            AssertDesiredRouteDataIsPresent(routeData, expectedValues);
        }

        [TestMethod]
        public void RegisterRoutes_Home_Custom_NoParam_ShouldRouteToDefaultHome()
        {
            // Arrange
            string action = "Custom";
            var routes = base.SetMockPathAndRegisterRoutes($"~/{controller}/{action}");
            // Act
            RouteData routeData = routes.GetRouteData(moqContext.Object);
            // Assert
            AssertExpectedRouteControllerAndAction(routeData: routeData, controller: "Home", action: "Custom");
        }

        [TestMethod]
        public void RegisterRoutes_Home_Custom_WithParam_ShouldRouteToDefaultHome()
        {
            // Arrange
            string action = "Custom";
            string environment = "Tst1";
            var routes = base.SetMockPathAndRegisterRoutes($"~/{controller}/{action}/{environment}");
            // Act
            RouteData routeData = routes.GetRouteData(moqContext.Object);
            // Assert
            AssertExpectedRouteControllerAndAction(routeData:routeData, controller:"Home", action: "Index");
        }

        [TestMethod]
        public void RegisterRoutes_Home_Custom_WithParam_GetsHomeCustomRoute_IgnoresParam()
        {
            // Arrange
            string action = "Custom";
            string environment = "Tst1";
            var routes = base.SetMockPathAndRegisterRoutes($"~/{controller}/{action}/{environment}");
            // Act
            RouteData routeData = routes.GetRouteData(moqContext.Object);
            // Assert
            var routingDataThatShouldNotBeThere = new List<string>() { environment };
            AssertUndesirableRouteDataIsAbsent(routeData, routingDataThatShouldNotBeThere);
        }
    }
}