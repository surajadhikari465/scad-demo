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
        public void RegisterRoutes_ApiJobs_Index_NoParam_GetsExpectedRoute()
        {
            // Arrange
            string action = "Index";
            var routes = base.SetMockPathAndRegisterRoutes($"~/{controller}/{action}");
            // Act
            RouteData routeData = routes.GetRouteData(moqContext.Object);
            // Assert
            //really we'd like it to interpret the 2nd segment as the action and assume the optional id is missing, like this
            AssertExpectedRouteControllerAndAction(routeData, "ApiJobs", "Index");
            ////but it is interpreting the 2nd segment as the id and assuming the action was omitted (defaults to Index)
            //AssertExpectedRouteData(routeData, controller, "Index", action);
        }

        [TestMethod]
        public void RegisterRoutes_ApiJobs_NoAction_NoParam_GetsExpectedDefaultRoute()
        {
            // Arrange
            string action = "";
            var routes = base.SetMockPathAndRegisterRoutes($"~/{controller}/{action}");
            // Act
            RouteData routeData = routes.GetRouteData(moqContext.Object);
            // Assert
            AssertExpectedRouteControllerAndAction(routeData, "ApiJobs", "Index");
        }

        [TestMethod]
        public void RegisterRoutes_ApiJobs_Index_WithJobType_GetsExpectedRoute()
        {
            // Arrange
            string action = "Index";
            string jobType = "hey";
            var routes = base.SetMockPathAndRegisterRoutes($"~/{controller}/{action}/{jobType}");
            // Act
            RouteData routeData = routes.GetRouteData(moqContext.Object);
            // Assert
            AssertExpectedRouteControllerAndAction(routeData, "ApiJobs", "Index");
        }

        [TestMethod]
        public void RegisterRoutes_ApiJobs_Index_WithJobType_UsesJobTypeValue()
        {
            // Arrange
            string action = "Index";
            string jobType = "hey";
            var routes = base.SetMockPathAndRegisterRoutes($"~/{controller}/{action}/{jobType}");
            // Act
            RouteData routeData = routes.GetRouteData(moqContext.Object);
            // Assert
            var expectedValues = new Dictionary<string, object>()
            {
                {"jobType", jobType }
            };
            AssertDesiredRouteDataIsPresent(routeData, expectedValues);
        }


        [TestMethod]
        public void RegisterRoutes_ApiJobs_RedrawPagingPartial_NoParams_GetsExpectedRoute()
        {
            // Arrange
            string action = "RedrawPaging";
            var routes = base.SetMockPathAndRegisterRoutes($"~/{controller}/{action}");
            // Act
            RouteData routeData = routes.GetRouteData(moqContext.Object);
            // Assert
            AssertExpectedRouteControllerAndAction(routeData: routeData, controller: "ApiJobs", action: action);
        }

        [TestMethod]
        public void RegisterRoutes_ApiJobs_RedrawPagingPartial_WithJobType_GetsExpectedRoute()
        {
            // Arrange
            string action = "RedrawPaging";
            string jobTypeParam = "Locale";
            var routes = base.SetMockPathAndRegisterRoutes($"~/{controller}/{action}/{jobTypeParam}");
            // Act
            RouteData routeData = routes.GetRouteData(moqContext.Object);
            // Assert
            AssertExpectedRouteControllerAndAction(routeData: routeData, controller: "ApiJobs", action: action);
        }

        [TestMethod]
        public void RegisterRoutes_ApiJobs_RedrawPagingPartial_WithJobType_HasParamValue()
        {
            // Arrange
            string action = "RedrawPaging";
            string jobTypeParam = "Locale";
            var routes = base.SetMockPathAndRegisterRoutes($"~/{controller}/{action}/{jobTypeParam}");
            // Act
            RouteData routeData = routes.GetRouteData(moqContext.Object);
            // Assert
            var expectedValues = new Dictionary<string, object>()
            {
                {"jobType", jobTypeParam }
            };
            AssertDesiredRouteDataIsPresent(routeData, expectedValues);
        }

        //[TestMethod]
        //public void RegisterRoutes_HomeTaskPartialWithQueryParameters_HasExectedQueryValues()
        //{
        //    // Arrange
        //    string action = "TaskPartial";
        //    string param1 = "aServer";
        //    string param2 = "myApplication";
        //    var routes = base.SetMockPathAndRegisterRoutes($"~/{controller}/{action}/{param1}/{param2}");
        //    // Act
        //    RouteData routeData = routes.GetRouteData(moqContext.Object);
        //    // Assert
        //    var expectedValues = new Dictionary<string, object>()
        //    {
        //        {"appServer", param1 },
        //        {"servicename", param2 }
        //    };
        //    AssertDesiredRouteDataIsPresent(routeData, expectedValues);
        //}

        //[TestMethod]
        //public void RegisterRoutes_HomeIconApiServicePartialWithQueryParameters_GetsHomeTaskPartialRoute()
        //{
        //    // Arrange
        //    string action = "IconApiServicePartial";
        //    string param1 = "aServer";
        //    string param2 = "myApplication";
        //    var routes = base.SetMockPathAndRegisterRoutes($"~/{controller}/{action}/{param1}/{param2}");
        //    // Act
        //    RouteData routeData = routes.GetRouteData(moqContext.Object);
        //    // Assert
        //    AssertExpectedRouteControllerAndAction(routeData: routeData, controller: "Home", action: action);
        //}

        //[TestMethod]
        //public void RegisterRoutes_HomeIconApiServicePartialWithQueryParameters_HasExectedQueryValues()
        //{
        //    // Arrange
        //    string action = "IconApiServicePartial";
        //    string param1 = "aServer";
        //    string param2 = "myApplication";
        //    var routes = base.SetMockPathAndRegisterRoutes($"~/{controller}/{action}/{param1}/{param2}");
        //    // Act
        //    RouteData routeData = routes.GetRouteData(moqContext.Object);
        //    // Assert
        //    var expectedValues = new Dictionary<string, object>()
        //    {
        //        {"appServer", param1 },
        //        {"serviceName", param2 }
        //    };
        //    AssertDesiredRouteDataIsPresent(routeData, expectedValues);
        //}
    }    
}