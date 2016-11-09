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
    public class ApiJobsRoutingUnitTests : _MvcRoutingUnitTestBase
    {
        string controller = "ApiJobs";

        [TestMethod]
        public void MvcRouting_ApiJobsIndexNoId_GetsExpectedRoute()
        {
            // Arrange
            string action = "Index";
            SetupRequestUrl($"~/{controller}/{action}");
            var routes = RegisterRoutesForTest();
            // Act
            RouteData routeData = routes.GetRouteData(moqContext.Object);
            // Assert
            //really we'd like it to interpret the 2nd segment as the action and assume the optional id is missing, like this
            AssertExpectedRouteControllerAndAction(routeData, "ApiJobs", "Index");
            ////but it is interpreting the 2nd segment as the id and assuming the action was omitted (defaults to Index)
            //AssertExpectedRouteData(routeData, controller, "Index", action);
        }

        [TestMethod]
        public void MvcRouting_ApiJobsNoActionNoId_GetsExpectedDefaultRoute()
        {
            // Arrange
            string action = "";
            SetupRequestUrl($"~/{controller}/{action}");
            var routes = RegisterRoutesForTest();
            // Act
            RouteData routeData = routes.GetRouteData(moqContext.Object);
            // Assert
            AssertExpectedRouteControllerAndAction(routeData, "ApiJobs", "Index");
        }

        [TestMethod]
        public void MvcRouting_ApiJobsIndexWithId_GetsExpectedRoute()
        {
            // Arrange
            string action = "Index";
            string id = "hey";
            SetupRequestUrl($"~/{controller}/{action}/{id}");
            var routes = RegisterRoutesForTest();
            // Act
            RouteData routeData = routes.GetRouteData(moqContext.Object);
            // Assert
            AssertExpectedRouteControllerAndAction(routeData, "ApiJobs", "Index");
        }

        [TestMethod]
        public void MvcRouting_ApiJobsIndexWithId_UsesIdValue()
        {
            // Arrange
            string action = "Index";
            string id = "hey";
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
    }    
}