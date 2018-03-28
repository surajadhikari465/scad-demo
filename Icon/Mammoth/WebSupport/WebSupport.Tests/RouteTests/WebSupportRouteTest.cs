using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Routing;
using WebSupport;

namespace WebSupport.Tests.RouteTests
{
    [TestClass]
    public class WebSupportRouteTest
    {
        protected MvcRoutingTestHelper routeTestHelper;
        protected Mock<HttpContextBase> moqContext;
        protected Mock<HttpRequestBase> moqRequest;

        [TestInitialize]
        public void SetupTestContext()
        {
            routeTestHelper = new MvcRoutingTestHelper();
            // Setup Mocks
            moqContext = new Mock<HttpContextBase>();
            moqRequest = new Mock<HttpRequestBase>();
            moqContext.Setup(x => x.Request).Returns(moqRequest.Object);
        }
        
        [TestMethod]
        public void HomeRoute_IndexAction_WithNoParameters_ShouldGetHomeIndexRoute()
        {
            // Arrange
            const string homeController = "Home";
            const string indexAction = "Index";
            routeTestHelper.SetupRequestUrl(moqRequest, $"~/{homeController}/{indexAction}");
            var routes = routeTestHelper.RegisterRoutesForTest();
            // Act
            RouteData routeData = routes.GetRouteData(moqContext.Object);
            // Assert
            MvcRoutingTestHelper.AssertExpectedRouteControllerAndAction(
                routeData: routeData,
                controller: homeController,
                action: indexAction);
        }

        [TestMethod]
        public void HomeRoute_NoAction_WithNoParameters_ShouldGetHomeIndexRoute()
        {
            // Arrange
            const string homeController = "Home";
            const string actionIndex = "Index";
            routeTestHelper.SetupRequestUrl(moqRequest, $"~/{homeController}");
            var routes = routeTestHelper.RegisterRoutesForTest();
            // Act
            RouteData routeData = routes.GetRouteData(moqContext.Object);
            // Assert
            MvcRoutingTestHelper.AssertExpectedRouteControllerAndAction(
                routeData: routeData,
                controller: homeController,
                action: actionIndex);
        }

        [TestMethod]
        public void PriceResetRoute_IndexAction_WithNoParameters_ShouldGetPriceResetIndexRoute()
        {
            // Arrange
            const string priceResetController = "PriceReset";
            const string actionIndex = "Index";
            routeTestHelper.SetupRequestUrl(moqRequest, $"~/{priceResetController}/{actionIndex}");
            var routes = routeTestHelper.RegisterRoutesForTest();
            // Act
            RouteData routeData = routes.GetRouteData(moqContext.Object);
            // Assert
            MvcRoutingTestHelper.AssertExpectedRouteControllerAndAction(
                routeData: routeData,
                controller: priceResetController,
                action: actionIndex);
        }

        [TestMethod]
        public void PriceResetRoute_NoAction_WithNoParameters_ShouldGetPriceResetIndexRoute()
        {
            // Arrange
            const string controller = "PriceReset";
            const string actionIndex = "Index";
            routeTestHelper.SetupRequestUrl(moqRequest, $"~/{controller}");
            var routes = routeTestHelper.RegisterRoutesForTest();
            // Act
            RouteData routeData = routes.GetRouteData(moqContext.Object);
            // Assert
            MvcRoutingTestHelper.AssertExpectedRouteControllerAndAction(
                routeData: routeData,
                controller: controller,
                action: actionIndex);
        }

        [TestMethod]
        public void RegionGpmStatusRoute_NoAction_WithNoParameters_ShouldGetRegionGpmStatusRoute()
        {
            // Arrange
            const string controller = "RegionGpmStatus";
            const string actionIndex = "Index";
            routeTestHelper.SetupRequestUrl(moqRequest, $"~/{controller}");
            var routes = routeTestHelper.RegisterRoutesForTest();
            // Act
            RouteData routeData = routes.GetRouteData(moqContext.Object);
            // Assert
            MvcRoutingTestHelper.AssertExpectedRouteControllerAndAction(
                routeData: routeData,
                controller: controller,
                action: actionIndex);
        }
    }
}
