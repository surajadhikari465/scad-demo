using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Web;
using System.Web.Routing;
using NUnit.Framework;
using Rhino.Mocks;

namespace OutOfStock.UnitTests
{
    public class RouteTester
    {
        public static void Match(string url, string controller, string action, object routeProperties = null, string httpMethod = "GET")
        {
            RouteCollection routes = new RouteCollection();
            MvcApplication.RegisterRoutes(routes);
            RouteData result = routes.GetRouteData(CreateHttpContext(url, httpMethod));
            Assert.IsNotNull(result);
            Assert.IsTrue(TestIncomingRouteResult(result, controller, action, routeProperties));
        }



        private static HttpContextBase CreateHttpContext(string targetUrl = null, string httpMethod = "GET")
        {
            var mockRequest = MockRepository.GenerateMock<HttpRequestBase>();
            mockRequest.Expect(m => m.AppRelativeCurrentExecutionFilePath).Return(targetUrl);
            mockRequest.Expect(m => m.HttpMethod).Return(httpMethod);

            var mockResponse = MockRepository.GeneratePartialMock<HttpResponseBase>();
            mockResponse.Expect(m => m.ApplyAppPathModifier(Arg<string>.Is.Anything)).IgnoreArguments().Do((Func<string, string>)(arg => arg));

            var mockContext = MockRepository.GenerateMock<HttpContextBase>();
            mockContext.Expect(m => m.Request).Return(mockRequest);
            mockContext.Expect(m => m.Response).Return(mockResponse);
            return mockContext;
        }

        private static bool TestIncomingRouteResult(RouteData routeResult, string controller, string action, object propertySet = null)
        {
            Func<object, object, bool> valCompare = (v1, v2) => StringComparer.InvariantCultureIgnoreCase.Compare(v1, v2) == 0;
            bool result = valCompare(routeResult.Values["controller"], controller) &&
                          valCompare(routeResult.Values["action"], action);
            if (propertySet != null)
            {
                PropertyInfo[] propInfo = propertySet.GetType().GetProperties();
                foreach (PropertyInfo pi in propInfo)
                {
                    if (!(routeResult.Values.ContainsKey(pi.Name) && valCompare(routeResult.Values[pi.Name], pi.GetValue(propertySet, null))))
                    {
                        result = false;
                        break;
                    }
                }
            }
            return result;
        }

        public static void Fail(string url)
        {
            RouteCollection routes = new RouteCollection();
            MvcApplication.RegisterRoutes(routes);
            RouteData result = routes.GetRouteData(CreateHttpContext(url));
            Assert.IsTrue(result == null || result.Route == null);
        }

    }
}
