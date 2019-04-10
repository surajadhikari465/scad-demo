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
    public class EsbSwitchRoutingUnitTests : _MvcRoutingUnitTestBase
    {
        string controller = "Esb";

        [TestMethod]
        public void MvcRouting_EsbIndex_GetsExpectedRoute()
        {
            // Arrange
            string action = "Index";
            SetupRequestUrl($"~/{controller}/{action}");
            var routes = RegisterRoutesForTest();
            // Act
            RouteData routeData = routes.GetRouteData(moqContext.Object);
            // Assert
            //really we'd like it to interpret the 2nd segment as the action and assume the optional id is missing, like this
            AssertExpectedRouteControllerAndAction(routeData, controller, action);
            ////but it is interpreting the 2nd segment as the id and assuming the action was omitted (defaults to Index)
            //AssertExpectedRouteData(routeData, controller, "Index", action);
        }

        [TestMethod]
        public void MvcRouting_EsbNoAction_GetsExpectedDefaultRoute()
        {
            // Arrange
            string action = "";
            SetupRequestUrl($"~/{controller}/{action}");
            var routes = RegisterRoutesForTest();
            // Act
            RouteData routeData = routes.GetRouteData(moqContext.Object);
            // Assert
            AssertExpectedRouteControllerAndAction(routeData, controller, "Index");
        }

        [TestMethod]
        public void MvcRouting_EsbDetailsWithName_GetsExpectedRoute()
        {
            // Arrange
            string action = "Details";
            string id = "hey";
            SetupRequestUrl($"~/{controller}/{action}/{id}");
            var routes = RegisterRoutesForTest();
            // Act
            RouteData routeData = routes.GetRouteData(moqContext.Object);
            // Assert
            AssertExpectedRouteControllerAndAction(routeData, controller, action);
        }

        [TestMethod]
        public void MvcRouting_EsbEditWithName_GetsExpectedRoute()
        {
            // Arrange
            string action = "Edit";
            string id = "test";
            SetupRequestUrl($"~/{controller}/{action}/{id}");
            var routes = RegisterRoutesForTest();
            // Act
            RouteData routeData = routes.GetRouteData(moqContext.Object);
            // Assert
            AssertExpectedRouteControllerAndAction(routeData, controller, action);
        }

        [TestMethod]
        public void MvcRouting_EsbCreateNoParameter_xxx()
        {
            // Arrange
            string action = "Create";
            SetupRequestUrl($"~/{controller}/{action}");
            var routes = RegisterRoutesForTest();
            // Act
            RouteData routeData = routes.GetRouteData(moqContext.Object);
            // Assert
            AssertExpectedRouteControllerAndAction(routeData, controller, action);
        }

        [TestMethod]
        public void MvcRouting_EsbCreateWithParameter_GetsExpectedRoute()
        {
            // Arrange
            string action = "Edit";
            string id = "test";
            SetupRequestUrl($"~/{controller}/{action}/{id}");
            var routes = RegisterRoutesForTest();
            // Act
            RouteData routeData = routes.GetRouteData(moqContext.Object);
            // Assert
            AssertExpectedRouteControllerAndAction(routeData, controller, action);
        }
    }    
}