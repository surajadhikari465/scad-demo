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
        public void RegisterRoutes_BlankUrl_ShouldRouteToDefaultHome()
        {
            // Arrange
            var routes = base.SetMockPathAndRegisterRoutes($"~/");
            // Act
            RouteData routeData = routes.GetRouteData(moqContext.Object);
            // Assert
            AssertExpectedRouteControllerAndAction(routeData, "Home", "Index");
        }

        [TestMethod]
        public void MvcRouting_UnknownControllerUnknownActionNoId_GetsDefaultHomeRoute()
        {
            // Arrange
            string controller = "aaaa";
            string action = "bbbb";
            SetupRequestUrl($"~/{controller}/{action}");
            var routes = RegisterRoutesForTest();
            // Act
            RouteData routeData = routes.GetRouteData(moqContext.Object);
            // Assert
            AssertExpectedRouteControllerAndAction(routeData, "Home", "Index");
        }

        [TestMethod]
        public void RegisterRoutes_UnknownControllerAndAction_ShouldRouteToDefaultHome()
        {
            // Arrange
            string controller = "aaaa";
            string action = "bbbb";
            var routes = base.SetMockPathAndRegisterRoutes($"~/{controller}/{action}");
            // Act
            RouteData routeData = routes.GetRouteData(moqContext.Object);
            // Assert
            AssertExpectedRouteControllerAndAction(routeData, "Home", "Index");
        }

        [TestMethod]
        public void RegisterRoutes_UnknownControllerAndActionWithParam_ShouldRouteToDefaultHome()
        {
            // Arrange
            string controller = "aaaa";
            string action = "bbbb";
            string id = "cccc";
            var routes = base.SetMockPathAndRegisterRoutes($"~/{controller}/{action}/{id}");
            // Act
            RouteData routeData = routes.GetRouteData(moqContext.Object);
            // Assert
            AssertExpectedRouteControllerAndAction(routeData, "Home", "Index");
        }

        [TestMethod]
        public void RegisterRoutes_UnknownControllerAndActionWithParam_ShouldRouteToDefaultHomeWithoutParam()
        {
            // Arrange
            string controller = "aaaa";
            string action = "bbbb";
            string id = "cccc";
            var routes = base.SetMockPathAndRegisterRoutes($"~/{controller}/{action}/{id}");
            // Act
            RouteData routeData = routes.GetRouteData(moqContext.Object);
            // Assert
            var expectedAbsentValues = new List<string>()  { "id" };
            AssertUndesirableRouteDataIsAbsent(routeData, expectedAbsentValues);
        }

        [TestMethod]
        public void RegisterRoutes_UnknownControllerAndAction_ShouldRouteToDefaultHomeWithoutParam()
        {
            // Arrange
            string controller = "aaaa";
            string action = "bbbb";
            var routes = base.SetMockPathAndRegisterRoutes($"~/{controller}/{action}");
            // Act
            RouteData routeData = routes.GetRouteData(moqContext.Object);
            // Assert
            var expectedAbsentValues = new List<string>() { "id" };
            AssertUndesirableRouteDataIsAbsent(routeData, expectedAbsentValues);
        }

        [TestMethod]
        public void RegisterRoutes_UnknownControllerNoActionNoParam_ShouldRouteToDefaultHome()
        {
            // Arrange
            string controller = "abcdef";
            string action = "";
            var routes = base.SetMockPathAndRegisterRoutes($"~/{controller}/{action}");
            // Act
            RouteData routeData = routes.GetRouteData(moqContext.Object);
            // Assert
            AssertExpectedRouteControllerAndAction(routeData, "Home", "Index");
        }

        [TestMethod]
        public void RegisterRoutes_TooManySegments_ShouldRouteToDefaultHome()
        {
            // Arrange
            var routes = base.SetMockPathAndRegisterRoutes($"~/a/b/c/d/e/f");
            // Act
            RouteData routeData = routes.GetRouteData(moqContext.Object);
            // Assert
            AssertExpectedRouteControllerAndAction(routeData, "Home", "Index");
        }

        [TestMethod]
        public void RegisterRoutes_Sample_FormsTest()
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
        public void RegisterRoutes_Sample_QueryStringTest()
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