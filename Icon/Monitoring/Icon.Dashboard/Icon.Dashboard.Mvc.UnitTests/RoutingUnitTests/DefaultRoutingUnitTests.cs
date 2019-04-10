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
    public class DefaultRoutingUnitTests : _MvcRoutingUnitTestBase
    {
        [TestMethod]
        public void MvcRouting_UrlWithoutAction_GetsDefaultActionRoute()
        {
            // Arrange
            //string controller = "aaaa";
            //string action = "bbbb";
            SetupRequestUrl($"~/");
            var routes = RegisterRoutesForTest();
            // Act
            RouteData routeData = routes.GetRouteData(moqContext.Object);
            // Assert
            AssertExpectedRouteControllerAndAction(routeData, "Home", "Index");
        }

        [TestMethod]
        public void MvcRouting_UnknownControllerUnknownActionNoId_GetsHomeAndCustomActionRoute()
        {
            // Arrange
            string controller = "aaaa";
            string action = "bbbb";
            SetupRequestUrl($"~/{controller}/{action}");
            var routes = RegisterRoutesForTest();
            // Act
            RouteData routeData = routes.GetRouteData(moqContext.Object);
            // Assert
            AssertExpectedRouteControllerAndAction(routeData, "Home", action);
        }

        [TestMethod]
        public void MvcRouting_UnknownControllerActionWithId_GetsHomeAndCustomActionRoute()
        {
            // Arrange
            string controller = "aaaa";
            string action = "bbbb";
            string id = "cccc";
            SetupRequestUrl($"~/{controller}/{action}/{id}");
            var routes = RegisterRoutesForTest();
            // Act
            RouteData routeData = routes.GetRouteData(moqContext.Object);
            // Assert
            AssertExpectedRouteControllerAndAction(routeData, "Home", action);
        }

        [TestMethod]
        public void MvcRouting_UnknownControllerActionWithId_ShouldIgnoreId()
        {
            //TODO should this work?
            // Arrange
            string controller = "aaaa";
            string action = "bbbb";
            string id = "cccc";
            SetupRequestUrl($"~/{controller}/{action}/{id}");
            var routes = RegisterRoutesForTest();
            // Act
            RouteData routeData = routes.GetRouteData(moqContext.Object);
            // Assert
            var expectedAbsentValues = new List<string>()  { "id" };
            AssertUndesirableRouteDataIsAbsent(routeData, expectedAbsentValues);
        }

        [TestMethod]
        public void MvcRouting_UnknownControllerUnknownActionNoId_IdShouldNotBeTHere()
        {
            // Arrange
            string controller = "aaaa";
            string action = "bbbb";
            SetupRequestUrl($"~/{controller}/{action}");
            var routes = RegisterRoutesForTest();
            // Act
            RouteData routeData = routes.GetRouteData(moqContext.Object);
            // Assert
            var expectedAbsentValues = new List<string>() { "id" };
            AssertUndesirableRouteDataIsAbsent(routeData, expectedAbsentValues);
        }

        [TestMethod]
        public void MvcRouting_UnknownControllerNoActionNoId_GetsExpectedDefaultAction()
        {
            // Arrange
            string controller = "abcdef";
            string action = "";
            SetupRequestUrl($"~/{controller}/{action}");
            var routes = RegisterRoutesForTest();
            // Act
            RouteData routeData = routes.GetRouteData(moqContext.Object);
            // Assert
            AssertExpectedRouteControllerAndAction(routeData, controller, "Index");
        }

        [TestMethod]
        public void MvcRouting_TooManySegments_ReturnsNull()
        {
            // Arrange
            SetupRequestUrl($"~/a/b/c/d/e/f");
            var routes = RegisterRoutesForTest();
            // Act
            RouteData routeData = routes.GetRouteData(moqContext.Object);
            // Assert
            Assert.IsNull(routeData);
        }


        [TestMethod]
        public void MvcRouting_Sample_FormsTest()
        {
            // Arrange            
            var formValues = new NameValueCollection
                {
                    { "FirstName", "Andi" },
                    { "LastName", "Bedland" }
                };
            moqRequest.Setup(r => r.Form).Returns(formValues);
            // Act
            var forms = moqContext.Object.Request.Form;
            // Assert
            Assert.IsNotNull(forms);
            Assert.AreEqual("Andi", forms["FirstName"]);
            Assert.AreEqual("Bedland", forms["LastName"]);
        }

        [TestMethod]
        public void MvcRouting_Sample_QueryStringTest()
        {
            // Arrange            
            var queryStringValues = new NameValueCollection
                {
                    { "FirstName", "Andi" },
                    { "LastName", "Bedland" }
                };
            moqRequest.Setup(r => r.QueryString).Returns(queryStringValues);
            // Act
            var queryString = moqContext.Object.Request.QueryString;
            // Assert
            Assert.IsNotNull(queryString);
            Assert.AreEqual("Andi", queryString["FirstName"]);
            Assert.AreEqual("Bedland", queryString["LastName"]);
        }
    }
    
}