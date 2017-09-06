using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Routing;

namespace WebSupport.Tests.RouteTests
{
    public class MvcRoutingTestHelper
    {
        public static void AssertExpectedRouteControllerAndAction(RouteData routeData, string controller, string action)
        {
            Assert.IsNotNull(routeData);
            Assert.AreEqual(controller, routeData.Values["controller"]);
            Assert.AreEqual(action, routeData.Values["action"]);
        }

        public static void AssertUndesirableRouteDataIsAbsent(RouteData routeData, IList<string> routeValuesWeDontWantToSee)
        {
            if (routeValuesWeDontWantToSee != null && routeValuesWeDontWantToSee.Count > 0)
            {
                foreach (var keyWhichShouldHaveNoValue in routeValuesWeDontWantToSee)
                {
                    Assert.IsNull(routeData.Values[keyWhichShouldHaveNoValue]);
                }
            }
        }

        public static void AssertDesiredRouteDataIsPresent(RouteData routeData, IDictionary<string, object> expectedRouteValues, IList<string> routeValuesWeDontWantToSee = null)
        {
            if (expectedRouteValues != null && expectedRouteValues.Count > 0)
            {
                foreach (var expectedPair in expectedRouteValues)
                {
                    Assert.AreEqual(expectedPair.Value, routeData.Values[expectedPair.Key]);
                }
            }
            AssertUndesirableRouteDataIsAbsent(routeData, routeValuesWeDontWantToSee);
        }

        public RouteCollection RegisterRoutesForTest(RouteCollection routeCollection = null)
        {
            if (routeCollection == null)
            {
                routeCollection = new RouteCollection();
            }
            RouteConfig.RegisterRoutes(routeCollection);
            return routeCollection;
        }

        public void SetupRequestUrl(Mock<HttpRequestBase> moqHttpRequest, string relativePath)
        {
            var routes = new RouteCollection();
            RouteConfig.RegisterRoutes(routes);
            moqHttpRequest.Setup(r => r.AppRelativeCurrentExecutionFilePath).Returns(relativePath);
        }
    }
}
