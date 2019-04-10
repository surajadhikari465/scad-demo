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
        public void MvcRouting_HomeIndexNoId_GetsHomeIndexRoute()
        {
            // Arrange
            string action = "Index";
            SetupRequestUrl($"~/{controller}/{action}");
            var routes = RegisterRoutesForTest();
            // Act
            RouteData routeData = routes.GetRouteData(moqContext.Object);
            // Assert
            AssertExpectedRouteControllerAndAction(routeData:routeData, controller:"Home", action:"Index");
        }

        [TestMethod]
        public void MvcRouting_HomeNoActionNoId_GetsHomeIndexRoute()
        {
            // Arrange
            SetupRequestUrl($"~/{controller}");
            var routes = RegisterRoutesForTest();
            // Act
            RouteData routeData = routes.GetRouteData(moqContext.Object);
            // Assert
            AssertExpectedRouteControllerAndAction(routeData: routeData, controller: "Home", action: "Index");
        }

        [TestMethod]
        public void MvcRouting_HomeIndexWithId_GetsHomeIndexRoute()
        {
            // Arrange
            string action = "Index";
            string id = "555";
            SetupRequestUrl($"~/{controller}/{action}/{id}");
            var routes = RegisterRoutesForTest();
            // Act
            RouteData routeData = routes.GetRouteData(moqContext.Object);
            // Assert
            AssertExpectedRouteControllerAndAction(routeData: routeData, controller: "Home", action: "Index");
        }

        [TestMethod]
        public void MvcRouting_HomeIndexWithId_GetsHomeIndexRoute_IgnoresId()
        {
            // Arrange
            string action = "Index";
            string id = "555";
            SetupRequestUrl($"~/{controller}/{action}/{id}");
            var routes = RegisterRoutesForTest();
            // Act
            RouteData routeData = routes.GetRouteData(moqContext.Object);
            // Assert
            AssertUndesirableRouteDataIsAbsent(routeData, new List<string>() { "id" });
        }

        //[TestMethod]
        //public void MvcRouting_HomeConfigureWithTwoParameters_GetsHomeConfigureRoute()
        //{
        //    // Arrange
        //    string action = "Configure";
        //    string param1 = "oopsy";
        //    string param2 = "daisy";
        //    SetupRequestUrl($"~/{controller}/{action}/{param1}/{param2}");
        //    var routes = RegisterRoutesForTest();
        //    // Act
        //    RouteData routeData = routes.GetRouteData(moqContext.Object);
        //    // Assert
        //    AssertExpectedRouteControllerAndAction(routeData: routeData, controller: "Home", action: "Configure");
        //}

        //[TestMethod]
        //public void MvcRouting_HomeConfigureWithTwoParameters_AssignsExtraParamsToQuery()
        //{
        //    // Arrange
        //    string action = "Configure";
        //    string param1 = "oopsy";
        //    string param2 = "daisy";
        //    SetupRequestUrl($"~/{controller}/{action}/{param1}/{param2}");
        //    var routes = RegisterRoutesForTest();
        //    // Act
        //    RouteData routeData = routes.GetRouteData(moqContext.Object);
        //    // Assert
        //    var expectedValues = new Dictionary<string, object>()
        //    {
        //        {"server", param1 },
        //        {"application", param2 } 
        //    };
        //    AssertDesiredRouteDataIsPresent(routeData, expectedValues);
        //}

        //[TestMethod]
        //public void MvcRouting_HomeDetailsithQueryParameters_GetsHomeDetailsRoute()
        //{
        //    // Arrange
        //    string action = "Details";
        //    string param1 = "aServer";
        //    string param2 = "myApplication";
        //    SetupRequestUrl($"~/{controller}/{action}/{param1}/{param2}");
        //    var routes = RegisterRoutesForTest();
        //    // Act
        //    RouteData routeData = routes.GetRouteData(moqContext.Object);
        //    // Assert
        //    AssertExpectedRouteControllerAndAction(routeData: routeData, controller: "Home", action: action);
        //}

        //[TestMethod]
        //public void MvcRouting_HomeDetailsWithQueryParameters_HasExectedQueryValues()
        //{
        //    // Arrange
        //    string action = "Details";
        //    string param1 = "aServer";
        //    string param2 = "myApplication";
        //    SetupRequestUrl($"~/{controller}/{action}/{param1}/{param2}");
        //    var routes = RegisterRoutesForTest();
        //    // Act
        //    RouteData routeData = routes.GetRouteData(moqContext.Object);
        //    // Assert
        //    var expectedValues = new Dictionary<string, object>()
        //    {
        //        {"server", param1 },
        //        {"application", param2 } 
        //    };
        //    AssertDesiredRouteDataIsPresent(routeData, expectedValues);
        //}

        [TestMethod]
        public void MvcRouting_HomeEditWithQueryParameters_GetsHomeEditRoute()
        {
            // Arrange
            string action = "Edit";
            string param1 = "aServer";
            string param2 = "myApplication";
            SetupRequestUrl($"~/{controller}/{action}/{param1}/{param2}");
            var routes = RegisterRoutesForTest();
            // Act
            RouteData routeData = routes.GetRouteData(moqContext.Object);
            // Assert
            AssertExpectedRouteControllerAndAction(routeData: routeData, controller: "Home", action: action);
        }

        [TestMethod]
        public void MvcRouting_EditWithQueryParameters_HasExectedQueryValues()
        {
            // Arrange
            string action = "Edit";
            string param1 = "aServer";
            string param2 = "myApplication";
            SetupRequestUrl($"~/{controller}/{action}/{param1}/{param2}");
            var routes = RegisterRoutesForTest();
            // Act
            RouteData routeData = routes.GetRouteData(moqContext.Object);
            // Assert
            var expectedValues = new Dictionary<string, object>()
            {
                {"server", param1 },
                {"application", param2 } 
            };
            AssertDesiredRouteDataIsPresent(routeData, expectedValues);
        }

        [TestMethod]
        public void MvcRouting_HomeCreateNoId_GetsHomeCreateRoute()
        {
            // Arrange
            string action = "Create";
            SetupRequestUrl($"~/{controller}/{action}");
            var routes = RegisterRoutesForTest();
            // Act
            RouteData routeData = routes.GetRouteData(moqContext.Object);
            // Assert
            AssertExpectedRouteControllerAndAction(routeData: routeData, controller: "Home", action: "Create");
        }

        [TestMethod]
        public void MvcRouting_HomeCreateWithId_GetsHomeCreateRoute_ShouldIgnoreId()
        {
            // Arrange
            string action = "Create";
            string id = "235";
            SetupRequestUrl($"~/{controller}/{action}/{id}");
            var routes = RegisterRoutesForTest();
            // Act
            RouteData routeData = routes.GetRouteData(moqContext.Object);
            // Assert
            var routingDataThatShouldNotBeThere = new List<string>() { "id" };
            AssertUndesirableRouteDataIsAbsent(routeData, routingDataThatShouldNotBeThere);
        }

        [TestMethod]
        public void MvcRouting_HomeTaskPartialWithQueryParameters_GetsHomeTaskPartialRoute()
        {
            // Arrange
            string action = "TaskPartial";
            string param1 = "aServer";
            string param2 = "myApplication";
            SetupRequestUrl($"~/{controller}/{action}/{param1}/{param2}");
            var routes = RegisterRoutesForTest();
            // Act
            RouteData routeData = routes.GetRouteData(moqContext.Object);
            // Assert
            AssertExpectedRouteControllerAndAction(routeData: routeData, controller: "Home", action: action );
        }

        [TestMethod]
        public void MvcRouting_HomeTaskPartialWithQueryParameters_HasExectedQueryValues()
        {
            // Arrange
            string action = "TaskPartial";
            string param1 = "aServer";
            string param2 = "myApplication";
            SetupRequestUrl($"~/{controller}/{action}/{param1}/{param2}");
            var routes = RegisterRoutesForTest();
            // Act
            RouteData routeData = routes.GetRouteData(moqContext.Object);
            // Assert
            var expectedValues = new Dictionary<string, object>()
            {
                {"server", param1 },
                {"application", param2 } 
            };
            AssertDesiredRouteDataIsPresent(routeData, expectedValues);
        }

        [TestMethod]
        public void MvcRouting_HomeIconApiServicePartialWithQueryParameters_GetsHomeTaskPartialRoute()
        {
            // Arrange
            string action = "IconApiServicePartial";
            string param1 = "aServer";
            string param2 = "myApplication";
            SetupRequestUrl($"~/{controller}/{action}/{param1}/{param2}");
            var routes = RegisterRoutesForTest();
            // Act
            RouteData routeData = routes.GetRouteData(moqContext.Object);
            // Assert
            AssertExpectedRouteControllerAndAction(routeData: routeData, controller: "Home", action: action);
        }

        [TestMethod]
        public void MvcRouting_HomeIconApiServicePartialWithQueryParameters_HasExectedQueryValues()
        {
            // Arrange
            string action = "IconApiServicePartial";
            string param1 = "aServer";
            string param2 = "myApplication";
            SetupRequestUrl($"~/{controller}/{action}/{param1}/{param2}");
            var routes = RegisterRoutesForTest();
            // Act
            RouteData routeData = routes.GetRouteData(moqContext.Object);
            // Assert
            var expectedValues = new Dictionary<string, object>()
            {
                {"server", param1 },
                {"application", param2 } 
            };
            AssertDesiredRouteDataIsPresent(routeData, expectedValues);
        }
    }
}