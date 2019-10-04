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
    public class EsbRoutingUnitTests : _MvcRoutingUnitTestBase
    {
        string controller = "Esb";

        [TestMethod]
        public void RegisterRoutes_Esb_Index_GetsExpectedRoute()
        {
            // Arrange
            string action = "Index";
            var routes = base.SetMockPathAndRegisterRoutes($"~/{controller}/{action}");
            // Act
            RouteData routeData = routes.GetRouteData(moqContext.Object);
            // Assert
            //really we'd like it to interpret the 2nd segment as the action and assume the optional id is missing, like this
            AssertExpectedRouteControllerAndAction(routeData, controller, action);
            ////but it is interpreting the 2nd segment as the id and assuming the action was omitted (defaults to Index)
            //AssertExpectedRouteData(routeData, controller, "Index", action);
        }

        [TestMethod]
        public void RegisterRoutes_Esb_NoAction_GetsExpectedDefaultRoute()
        {
            // Arrange
            string action = "";
            var routes = base.SetMockPathAndRegisterRoutes($"~/{controller}/{action}");
            // Act
            RouteData routeData = routes.GetRouteData(moqContext.Object);
            // Assert
            AssertExpectedRouteControllerAndAction(routeData, controller, "Index");
        }

        [TestMethod]
        public void RegisterRoutes_Esb_Details_WithName_GetsExpectedRoute()
        {
            // Arrange
            string action = "Details";
            string id = "hey";
            var routes = base.SetMockPathAndRegisterRoutes($"~/{controller}/{action}/{id}");
            // Act
            RouteData routeData = routes.GetRouteData(moqContext.Object);
            // Assert
            AssertExpectedRouteControllerAndAction(routeData, controller, action);
        }
    }    
}