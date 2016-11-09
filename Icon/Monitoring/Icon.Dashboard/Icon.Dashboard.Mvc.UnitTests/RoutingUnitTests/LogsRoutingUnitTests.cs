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
    public class LogsRoutingUnitTests : _MvcRoutingUnitTestBase
    {
        string controller = "Logs";

        [TestMethod]
        public void MvcRouting_LogsIndexNoId_GetsLogsIndexRoute()
        {
            // Arrange
            string action = "Index";
            SetupRequestUrl($"~/{controller}/{action}");
            var routes = RegisterRoutesForTest();
            // Act
            RouteData routeData = routes.GetRouteData(moqContext.Object);
            // Assert
            AssertExpectedRouteControllerAndAction(routeData: routeData, controller: "Logs", action: "Index");
        }

        [TestMethod]
        public void MvcRouting_LogsIndexNoId_IdIsOptional()
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
                {"id", UrlParameter.Optional }
            };
            AssertDesiredRouteDataIsPresent(routeData, expectedValues);
        }

        [TestMethod]
        public void MvcRouting_LogsNoActionNoId_DefaultsToIndexRoute()
        {
            // Arrange
            SetupRequestUrl($"~/{controller}");
            var routes = RegisterRoutesForTest();
            // Act
            RouteData routeData = routes.GetRouteData(moqContext.Object);
            // Assert
            AssertExpectedRouteControllerAndAction(routeData, "Logs", "Index");
        }

        [TestMethod]
        public void MvcRouting_LogsNoActionNoId_IdIsOptional()
        {
            // Arrange
            SetupRequestUrl($"~/{controller}");
            var routes = RegisterRoutesForTest();
            // Act
            RouteData routeData = routes.GetRouteData(moqContext.Object);
            // Assert
            var expectedValues = new Dictionary<string, object>()
            {
                {"id", UrlParameter.Optional }
            };
            AssertDesiredRouteDataIsPresent(routeData, expectedValues);
        }

        [TestMethod]
        public void MvcRouting_LogsIndexWithId_GetsExpectedRoute()
        {
            // Arrange
            string action = "Index";
            string id = "7777";
            SetupRequestUrl($"~/{controller}/{action}/{id}");
            var routes = RegisterRoutesForTest();
            // Act
            RouteData routeData = routes.GetRouteData(moqContext.Object);
            // Assert
            AssertExpectedRouteControllerAndAction(routeData, "Logs", "Index");
        }

        [TestMethod]
        public void MvcRouting_LogsIndexWithId_UsesIdValue()
        {
            // Arrange
            string action = "Index";
            string id = "7777";
            SetupRequestUrl($"~/{controller}/{action}/{id}");
            var routes = RegisterRoutesForTest();
            // Act
            RouteData routeData = routes.GetRouteData(moqContext.Object);
            // Assert
            var expectedValues = new Dictionary<string, object>()
            {
                {"id", id }
            };
            AssertDesiredRouteDataIsPresent(routeData, expectedValues);
        }

        //These 2 are controller tests not routing tests!
        //[TestMethod]
        //public void MvcRouting_LogsIndexWithId_SetsDefaultPagingParameters()
        //{
        //    throw new NotImplementedException("");
        //    // Arrange
        //    string action = "Index";
        //    string id = "7777";
        //    SetupRequestUrl($"~/{controller}/{action}/{id}");
        //    var routes = RegisterRoutesForTest();
        //    // Act
        //    RouteData routeData = routes.GetRouteData(moqContext.Object);
        //    // Assert
        //    var expectedValues = new Dictionary<string, string>()
        //    {
        //        {"id", id },
        //        {"page", "1" },
        //        {"pageSize", defaultPageSize.ToString() }
        //    };
        //    AssertDesiredRouteDataIsPresent(routeData, expectedValues);
        //}

        //[TestMethod]
        //public void MvcRouting_LogsIndexWithId_PagingParametersAreApplied()
        //{
        //    throw new NotImplementedException();
        //    string action = "Index";
        //    string id = "19";
        //    SetupRequestUrl($"~/{controller}/{action}/{id}");
        //    var routes = RegisterRoutesForTest();
        //    //var queryStringValues = new NameValueCollection
        //    //    {
        //    //        { "page", "3" },
        //    //        { "pageSize", "22" }
        //    //    };
        //    //moqRequest.Setup(r => r.QueryString).Returns(queryStringValues);
        //    // Act
        //    RouteData routeData = routes.GetRouteData(moqContext.Object);
        //   // var queryString = moqContext.Object.Request.QueryString;
        //    // Assert
        //    //AssertExpectedRouteData(routeData: routeData, controller: "Logs", action: "Index", id: "19");
        //    //Assert.IsNotNull(queryString);
        //    //Assert.AreEqual(id, queryString["id"]);
        //    //Assert.AreEqual(3, queryString["page"]);
        //    //Assert.AreEqual(22, queryString["pageSize"]);
        //    var expectedValues = new Dictionary<string, string>()
        //    {
        //        {"id", id },
        //        {"page", "3" },
        //        {"pageSize", "22" }
        //    };
        //    AssertDesiredRouteDataIsPresent(routeData, expectedValues);
        //}
    }    
}