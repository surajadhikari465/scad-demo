using Icon.Dashboard.Mvc.Enums;
using Icon.Dashboard.Mvc.Models;
using Icon.Dashboard.Mvc.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Icon.Dashboard.Mvc.UnitTests.ServiceTests
{
    [TestClass]
    public class CookieManagerUnitTests
    {
        EnvironmentCookieManager cookieManager;
        const string nameForNameCookie = "environment cookie";
        const string nameForServersCookie = "app servers cookie";
        const int duration = 4;
        Mock<HttpRequestBase> mockRequest = new Mock<HttpRequestBase>();
        Mock<HttpResponseBase> mockResponse = new Mock<HttpResponseBase>();

        [TestInitialize]
        public void InitializeTest()
        {
            var fakeUri = new Uri("http://fakeserver/IconDashboard/Home/Index", UriKind.Absolute);
            this.mockRequest.SetupGet(r => r.Url).Returns(fakeUri);

            var emptyReqCookieCollection = new HttpCookieCollection { };
            this.mockRequest.SetupGet(r => r.Cookies).Returns(emptyReqCookieCollection);

            var emptyRespCookieCollection = new HttpCookieCollection { };
            this.mockResponse.SetupGet(r => r.Cookies).Returns(emptyRespCookieCollection);

            this.cookieManager = new EnvironmentCookieManager(duration, nameForNameCookie, nameForServersCookie);
        }

        private void AddRequestEnvironmentNameCookie(string valueForCookie, string nameForCookie = nameForNameCookie)
        {
            AddRequestCookie(this.mockRequest.Object, valueForCookie, nameForCookie);
        }

        private void AddResponseEnvironmentNameCookie(string valueForCookie, string nameForCookie = nameForNameCookie)
        {
            AddResponseCookie(this.mockResponse.Object, valueForCookie, nameForCookie);
        }

        private void AddRequestAppServersCookie(string valueForCookie, string nameForCookie = nameForServersCookie)
        {
            AddRequestCookie(this.mockRequest.Object, valueForCookie, nameForCookie);
        }

        private void AddResponseAppServersCookie(string valueForCookie, string nameForCookie = nameForServersCookie)
        {
            AddResponseCookie(this.mockResponse.Object, valueForCookie, nameForCookie);
        }

        private void AddRequestCookie(HttpRequestBase request, string cookieValue, string cookieName)
        {
            request.Cookies.Add(new HttpCookie(name: cookieName, value: cookieValue));
        }

        private void AddResponseCookie(HttpResponseBase response, string cookieValue, string cookieName)
        {
            response.Cookies.Add(new HttpCookie(name: cookieName, value: cookieValue));
        }

        [TestMethod]
        public void Constructor_SetsProperties()
        {
            // Arrange
            string testEnvironmentNameCookieName = "ABCD 1234";
            string testServerCookieName = "293&~{}\\";
            int testCookieDuration = 2;
            var cookieMgr = new EnvironmentCookieManager(testCookieDuration, testEnvironmentNameCookieName, testServerCookieName);
            // Act
            cookieMgr.SetCookieParameters(testCookieDuration, testEnvironmentNameCookieName, testServerCookieName);
            // Assert
            Assert.AreEqual(testEnvironmentNameCookieName, cookieMgr.EnvironmentNameCookieName);
            Assert.AreEqual(testServerCookieName, cookieMgr.EnvironmentAppServersCookieName);
            Assert.AreEqual(testCookieDuration, cookieMgr.EnvironmentCookieDurationHours);
        }

        [TestMethod]
        public void GetEnvironmentCookieIfPresent_WhenNoCookies_ReturnsNull()
        {
            // Arrange
            // (initialization adds empty cookie collections by default)
            // Act
            var cookieModel = cookieManager.GetEnvironmentCookieIfPresent(mockRequest.Object);
            // Assert
            Assert.IsNull(cookieModel);
            // Cookies property access: once only for access
            mockRequest.Verify(r => r.Cookies, Times.Once);
        }

        [TestMethod]
        public void GetEnvironmentCookieIfPresent_WhenStandardEnvironmentWithNoCustomServers_ReturnsExpectedModel()
        {
            // Arrange
            var envForTest = EnvironmentEnum.Perf;
            var expectedEnvironmentName = envForTest.ToString();
            AddRequestEnvironmentNameCookie(expectedEnvironmentName);
            // Act
            var cookieModel = cookieManager.GetEnvironmentCookieIfPresent(mockRequest.Object);
            // Assert
            Assert.IsNotNull(cookieModel);
            Assert.AreEqual(expectedEnvironmentName, cookieModel.Name);
            Assert.AreEqual(envForTest, cookieModel.EnvironmentEnum);
            Assert.IsNotNull(cookieModel.AppServers);
            Assert.AreEqual(0, cookieModel.AppServers.Count);
            // Cookies property access: once for setup, twice for checks for each cookie, once for value of cookie that's there
            mockRequest.Verify(r => r.Cookies, Times.Exactly(4));
        }

        [TestMethod]
        public void GetEnvironmentCookieIfPresent_WhenStandardEnvironmentWithCustomServers_ReturnsExpectedModel()
        {
            // Arrange
            var envForTest = EnvironmentEnum.Perf;
            var expectedEnvironmentName = EnvironmentEnum.Perf.ToString();
            var expectedServersList = new List<string> { "vm-icon-dev1","vm-icon-qa3","mammoth-app01-qa" };
            var expectedServers = "vm-icon-dev1,vm-icon-qa3,mammoth-app01-qa";
            AddRequestEnvironmentNameCookie(expectedEnvironmentName);
            AddRequestAppServersCookie(expectedServers);
            // Act
            var cookieModel = cookieManager.GetEnvironmentCookieIfPresent(mockRequest.Object);
            // Assert
            // expect the cookie manager to return the app server values from the cookie even if they will not be used
            Assert.IsNotNull(cookieModel);
            Assert.AreEqual(expectedEnvironmentName, cookieModel.Name);
            Assert.AreEqual(envForTest, cookieModel.EnvironmentEnum);
            Assert.IsNotNull(cookieModel.AppServers);
            CustomAsserts.ListsAreEqual(expectedServersList, cookieModel.AppServers);
            // Cookies property access: once for setup, one to check if exists, one to get value 
            //  (x2 for both cookies)
            mockRequest.Verify(r => r.Cookies, Times.Exactly(6));
        }

        [TestMethod]
        public void GetEnvironmentCookieIfPresent_WhenCustomEnvironmentSetWithServers_ReturnsExpectedModel()
        {
            // Arrange
            var expectedEnvironmentName = "custom2";
            var expectedEnvironmentEnum = EnvironmentEnum.Custom;
            var expectedServersList = new List<string> { "vm-icon-dev1","vm-icon-qa3","mammoth-app01-qa" };
            var expectedServers = "vm-icon-dev1,vm-icon-qa3,mammoth-app01-qa";
            AddRequestEnvironmentNameCookie(expectedEnvironmentName);
            AddRequestAppServersCookie(expectedServers);
            // Act
            var cookieModel = cookieManager.GetEnvironmentCookieIfPresent(mockRequest.Object);
            // Assert
            Assert.IsNotNull(cookieModel);
            Assert.AreEqual(expectedEnvironmentName, cookieModel.Name);
            Assert.AreEqual(expectedEnvironmentEnum, cookieModel.EnvironmentEnum);
            Assert.IsNotNull(cookieModel.AppServers);
            CustomAsserts.ListsAreEqual(expectedServersList, cookieModel.AppServers);
            // Cookies property access: once for setup, one to check if exists, one to get value 
            //  (x2 for both cookies)
            mockRequest.Verify(r => r.Cookies, Times.Exactly(6));
        }

        [TestMethod]
        public void GetEnvironmentCookieIfPresent_WhenCustomEnvironmentSetWithNoServers_ReturnsExpectedModel()
        {
            // Arrange
            var envForTest = EnvironmentEnum.Custom;
            var expectedEnvironmentName = "custom2";
            AddRequestEnvironmentNameCookie(expectedEnvironmentName);
            // Act
            var cookieModel = cookieManager.GetEnvironmentCookieIfPresent(mockRequest.Object);
            // Assert
            // although custom environments should always have app servers associate with them,
            //  expect the cookie manager just to return the values from the cookies & leave 
            //  other logic elsewhere
            Assert.IsNotNull(cookieModel);
            Assert.AreEqual(expectedEnvironmentName, cookieModel.Name);
            Assert.AreEqual(envForTest, cookieModel.EnvironmentEnum);
            Assert.IsNotNull(cookieModel.AppServers);
            Assert.AreEqual(0, cookieModel.AppServers.Count);
            // Cookies property access: once for setup + once each to check if each cookie exists
            //  + one more to get value of name cookie
            mockRequest.Verify(r => r.Cookies, Times.Exactly(4));
        }

        [TestMethod]
        public void SetEnvironmentCookies_WhenNullEnvironmentModel_ShouldNotAddCookies()
        {
            // Arrange
            var cookieNameForEnvironment = nameForNameCookie;
            var cookieNameForServers = nameForServersCookie;
            EnvironmentCookieModel cookieModel = null;
            // Act
            cookieManager.SetEnvironmentCookies(mockRequest.Object, mockResponse.Object, cookieModel);
            // Assert
            // should not add cookies to response 
            var responseCookieForEnvironmentName = this.mockResponse.Object.Cookies[cookieNameForEnvironment];
            var responseCookieForServers = this.mockResponse.Object.Cookies[cookieNameForServers];
            Assert.IsNull(responseCookieForEnvironmentName);
            Assert.IsNull(responseCookieForServers);
        }

        [TestMethod]
        public void SetEnvironmentCookies_WhenEnvironmentModelNameIsNull_ShouldNotAddCookies()
        {
            // Arrange
            var cookieNameForEnvironment = nameForNameCookie;
            var cookieNameForServers = nameForServersCookie;
            var expectedEnvironmentEnum = EnvironmentEnum.Custom;
            string expectedEnvironmentName = null;
            var cookieModel = new EnvironmentCookieModel(expectedEnvironmentName, expectedEnvironmentEnum);
            // Act
            cookieManager.SetEnvironmentCookies(mockRequest.Object, mockResponse.Object, cookieModel);
            // Assert
            var responseCookieForEnvironmentName = this.mockResponse.Object.Cookies[cookieNameForEnvironment];
            Assert.IsNull(responseCookieForEnvironmentName);
            var responseCookieForServers = this.mockResponse.Object.Cookies[cookieNameForServers];
            Assert.IsNull(responseCookieForServers);
        }

        [TestMethod]
        public void SetEnvironmentCookies_WhenEnvironmentModelNameIsBlank_ShouldNotAddCookies()
        {
            // Arrange
            var cookieNameForEnvironment = nameForNameCookie;
            var cookieNameForServers = nameForServersCookie;
            var expectedEnvironmentEnum = EnvironmentEnum.Custom;
            var expectedEnvironmentName = "";
            var cookieModel = new EnvironmentCookieModel(expectedEnvironmentName, expectedEnvironmentEnum);
            // Act
            cookieManager.SetEnvironmentCookies(mockRequest.Object, mockResponse.Object, cookieModel);
            // Assert
            var responseCookieForEnvironmentName = this.mockResponse.Object.Cookies[cookieNameForEnvironment];
            Assert.IsNull(responseCookieForEnvironmentName);
            var responseCookieForServers = this.mockResponse.Object.Cookies[cookieNameForServers];
            Assert.IsNull(responseCookieForServers);
        }

        [TestMethod]
        public void SetEnvironmentCookies_WhenEnvironmentModelNameIsWhitespace_ShouldNotAddCookies()
        {
            // Arrange
            var cookieNameForEnvironment = nameForNameCookie;
            var cookieNameForServers = nameForServersCookie;
            var expectedEnvironmentEnum = EnvironmentEnum.Custom;
            var expectedEnvironmentName = " \t\r\n";
            var cookieModel = new EnvironmentCookieModel(expectedEnvironmentName, expectedEnvironmentEnum);
            // Act
            cookieManager.SetEnvironmentCookies(mockRequest.Object, mockResponse.Object, cookieModel);
            // Assert
            var responseCookieForEnvironmentName = this.mockResponse.Object.Cookies[cookieNameForEnvironment];
            Assert.IsNull(responseCookieForEnvironmentName);
            var responseCookieForServers = this.mockResponse.Object.Cookies[cookieNameForServers];
            Assert.IsNull(responseCookieForServers);
        }

        [TestMethod]
        public void SetEnvironmentCookies_WhenEnvironmentModelEnumIsUndefined_ShouldNotAddCookies()
        {
            // Arrange
            var cookieNameForEnvironment = nameForNameCookie;
            var cookieNameForServers = nameForServersCookie;
            var expectedEnvironmentEnum = EnvironmentEnum.Undefined;
            var expectedEnvironmentName = "Ed's Env";
            var cookieModel = new EnvironmentCookieModel(expectedEnvironmentName, expectedEnvironmentEnum);
            // Act
            cookieManager.SetEnvironmentCookies(mockRequest.Object, mockResponse.Object, cookieModel);
            // Assert
            var responseCookieForEnvironmentName = this.mockResponse.Object.Cookies[cookieNameForEnvironment];
            Assert.IsNull(responseCookieForEnvironmentName);
            var responseCookieForServers = this.mockResponse.Object.Cookies[cookieNameForServers];
            Assert.IsNull(responseCookieForServers);
        }

        [TestMethod]
        public void SetEnvironmentCookies_WhenStandardAltEnvironment_ShouldAddNameCookie()
        {
            // Arrange
            var cookieNameForEnvironment = nameForNameCookie;
            var expectedEnvironmentEnum = EnvironmentEnum.Perf;
            var expectedEnvironmentName = expectedEnvironmentEnum.ToString();
            var cookieModel = new EnvironmentCookieModel(expectedEnvironmentName, expectedEnvironmentEnum);            
            // Act
            cookieManager.SetEnvironmentCookies(mockRequest.Object, mockResponse.Object, cookieModel);
            // Assert
            var responseCookieForEnvironmentName = this.mockResponse.Object.Cookies[cookieNameForEnvironment];
            Assert.IsNotNull(responseCookieForEnvironmentName);
        }

        [TestMethod]
        public void SetEnvironmentCookies_WhenServersButNoName_ShouldNotAddNameCookie()
        {
            // Arrange
            var cookieNameForEnvironment = nameForNameCookie;
            var cookieModel = new EnvironmentCookieModel();
            cookieModel.AppServers = new List<string> { "vm-icon-test1", "vm-icon-test2" };            
            // Act
            cookieManager.SetEnvironmentCookies(mockRequest.Object, mockResponse.Object, cookieModel);
            // Assert
            var responseCookieForEnvironmentName = this.mockResponse.Object.Cookies[cookieNameForEnvironment];
            Assert.IsNull(responseCookieForEnvironmentName);
        }

        [TestMethod]
        public void SetEnvironmentCookies_WhenServersButNoName_ShouldNotAddServersCookie()
        {
            // Arrange
            var cookieNameForServers = nameForServersCookie;
            var cookieModel = new EnvironmentCookieModel();
            cookieModel.AppServers = new List<string> { "vm-icon-test1", "vm-icon-test2" };
            // Act
            cookieManager.SetEnvironmentCookies(mockRequest.Object, mockResponse.Object, cookieModel);
            // Assert
            var responseCookieForServers = this.mockResponse.Object.Cookies[cookieNameForServers];
            Assert.IsNull(responseCookieForServers);
        }

        [TestMethod]
        public void SetEnvironmentCookies_WhenStandardAltEnvironment_ShouldAddNameCookieWithExpectedValue()
        {
            // Arrange
            var cookieNameForEnvironment = nameForNameCookie;
            var expectedEnvironmentEnum = EnvironmentEnum.Perf;
            var expectedEnvironmentName = expectedEnvironmentEnum.ToString();
            var cookieModel = new EnvironmentCookieModel(expectedEnvironmentName, expectedEnvironmentEnum);            
            // Act
            cookieManager.SetEnvironmentCookies(mockRequest.Object, mockResponse.Object, cookieModel);
            // Assert
            var responseCookieForEnvironmentName = this.mockResponse.Object.Cookies[cookieNameForEnvironment];
            Assert.AreEqual(expectedEnvironmentName, responseCookieForEnvironmentName.Value);
        }

        [TestMethod]
        public void SetEnvironmentCookies_WhenStandardAltEnvironment_ShouldAddNameCookieWithWithExpectedExpiry()
        {
            // Arrange
            var cookieNameForEnvironment = nameForNameCookie;
            var expectedExpiry = DateTime.Now.AddHours(duration);
            var expectedEnvironmentEnum = EnvironmentEnum.Perf;
            var expectedEnvironmentName = expectedEnvironmentEnum.ToString();
            var cookieModel = new EnvironmentCookieModel(expectedEnvironmentName, expectedEnvironmentEnum);
            // Act
            cookieManager.SetEnvironmentCookies(mockRequest.Object, mockResponse.Object, cookieModel);
            // Assert
            var responseCookieForEnvironmentName = this.mockResponse.Object.Cookies[cookieNameForEnvironment];
            CustomAsserts.TimesCloseEnough(expectedExpiry, responseCookieForEnvironmentName.Expires);
        }

        [TestMethod]
        public void SetEnvironmentCookies_WhenStandardAltEnvironment_ShouldNotAddServersCookie()
        {
            // Arrange
            var cookieNameForServers = nameForServersCookie;
            var expectedEnvironmentEnum = EnvironmentEnum.Perf;
            var expectedEnvironmentName = expectedEnvironmentEnum.ToString();
            var cookieModel = new EnvironmentCookieModel(expectedEnvironmentName, expectedEnvironmentEnum);            
            // Act
            cookieManager.SetEnvironmentCookies(mockRequest.Object, mockResponse.Object, cookieModel);
            // Assert
            var responseCookieForServers = this.mockResponse.Object.Cookies[cookieNameForServers];
            Assert.IsNull(responseCookieForServers);
        }

        [TestMethod]
        public void SetEnvironmentCookies_WhenCustomEnvironmentWithServers_ShouldAddNameCookie()
        {
            // Arrange
            var cookieNameForEnvironment = nameForNameCookie;
            var expectedEnvironmentEnum = EnvironmentEnum.Custom;
            var expectedEnvironmentName = "Ed's Env";
            var cookieModel = new EnvironmentCookieModel(expectedEnvironmentName, expectedEnvironmentEnum);
            cookieModel.AppServers = new List<string> { "vm-icon-test1", "vm-icon-test2" };
            // Act
            cookieManager.SetEnvironmentCookies(mockRequest.Object, mockResponse.Object, cookieModel);
            // Assert
            var responseCookieForEnvironmentName = this.mockResponse.Object.Cookies[cookieNameForEnvironment];
            Assert.IsNotNull(responseCookieForEnvironmentName);
        }

        [TestMethod]
        public void SetEnvironmentCookies_WhenCustomEnvironmentWithServers_ShouldAddNameCookieWithExpectedValue()
        {
            // Arrange
            var cookieNameForEnvironment = nameForNameCookie;
            var expectedEnvironmentEnum = EnvironmentEnum.Custom;
            var expectedEnvironmentName = "Ed's Env";
            var cookieModel = new EnvironmentCookieModel(expectedEnvironmentName, expectedEnvironmentEnum);
            cookieModel.AppServers = new List<string> { "vm-icon-test1", "vm-icon-test2" };
            // Act
            cookieManager.SetEnvironmentCookies(mockRequest.Object, mockResponse.Object, cookieModel);
            // Assert
            var responseCookieForEnvironmentName = this.mockResponse.Object.Cookies[cookieNameForEnvironment];
            Assert.AreEqual(expectedEnvironmentName, responseCookieForEnvironmentName.Value);
        }

        [TestMethod]
        public void SetEnvironmentCookies_WhenCustomEnvironmentWithServers_ShouldAddNameCookieWithExpectedExpiry()
        {
            // Arrange
            var cookieNameForEnvironment = nameForNameCookie;
            var expectedExpiry = DateTime.Now.AddHours(duration);
            var expectedEnvironmentEnum = EnvironmentEnum.Custom;
            var expectedEnvironmentName = "Ed's Env";
            var cookieModel = new EnvironmentCookieModel(expectedEnvironmentName, expectedEnvironmentEnum);
            cookieModel.AppServers = new List<string> { "vm-icon-test1", "vm-icon-test2" };
            // Act
            cookieManager.SetEnvironmentCookies(mockRequest.Object, mockResponse.Object, cookieModel);
            // Assert
            var responseCookieForEnvironmentName = this.mockResponse.Object.Cookies[cookieNameForEnvironment];
            CustomAsserts.TimesCloseEnough(expectedExpiry, responseCookieForEnvironmentName.Expires);
        }

        [TestMethod]
        public void SetEnvironmentCookies_WhenCustomEnvironmentWithNoServers_ShouldNotAddNameCookie()
        {
            // Arrange
            var cookieNameForEnvironment = nameForNameCookie;
            var expectedEnvironmentEnum = EnvironmentEnum.Custom;
            var expectedEnvironmentName = "Ed's Env";
            var cookieModel = new EnvironmentCookieModel(expectedEnvironmentName, expectedEnvironmentEnum);
            // Act
            cookieManager.SetEnvironmentCookies(mockRequest.Object, mockResponse.Object, cookieModel);
            // Assert
            var responseCookieForEnvironmentName = this.mockResponse.Object.Cookies[cookieNameForEnvironment];
            Assert.IsNull(responseCookieForEnvironmentName);
        }

        [TestMethod]
        public void SetEnvironmentCookies_WhenCustomEnvironmentWithNoServers_ShouldNotAddServersCookie()
        {
            // Arrange
            var cookieNameForServers = nameForServersCookie;
            var expectedEnvironmentEnum = EnvironmentEnum.Custom;
            var expectedEnvironmentName = "Ed's Env";
            var cookieModel = new EnvironmentCookieModel(expectedEnvironmentName, expectedEnvironmentEnum);
            // Act
            cookieManager.SetEnvironmentCookies(mockRequest.Object, mockResponse.Object, cookieModel);
            // Assert
            var responseCookieForServers = this.mockResponse.Object.Cookies[cookieNameForServers];
            Assert.IsNull(responseCookieForServers);
        }

        [TestMethod]
        public void SetEnvironmentCookies_WhenCustomEnvironmentWithServersButNoName_ShouldNotAddNameCookie()
        {
            // Arrange
            var cookieNameForEnvironment = nameForNameCookie;
            var cookieModel = new EnvironmentCookieModel();
            cookieModel.AppServers = new List<string> { "vm-icon-test1", "vm-icon-test2" };
            // Act
            cookieManager.SetEnvironmentCookies(mockRequest.Object, mockResponse.Object, cookieModel);
            // Assert
            var responseCookieForEnvironmentName = this.mockResponse.Object.Cookies[cookieNameForEnvironment];
            Assert.IsNull(responseCookieForEnvironmentName);
        }

        [TestMethod]
        public void SetEnvironmentCookies_WhenCustomEnvironmentWithServersButNoName_ShouldNotAddServersCookie()
        {
            // Arrange
            var cookieNameForServers = nameForServersCookie;
            var cookieModel = new EnvironmentCookieModel();
            cookieModel.AppServers = new List<string> { "vm-icon-test1", "vm-icon-test2" };
            // Act
            cookieManager.SetEnvironmentCookies(mockRequest.Object, mockResponse.Object, cookieModel);
            // Assert
            var responseCookieForServers = this.mockResponse.Object.Cookies[cookieNameForServers];
            Assert.IsNull(responseCookieForServers);
        }


        [TestMethod]
        public void SetEnvironmentCookies_WhenCustomEnvironmentWithSingleServer_ShouldAddNameCookie()
        {
            // Arrange
            var cookieNameForEnvironment = nameForNameCookie;
            var expectedEnvironmentEnum = EnvironmentEnum.Custom;
            var expectedEnvironmentName = "Ed's Env";
            var cookieModel = new EnvironmentCookieModel(expectedEnvironmentName, expectedEnvironmentEnum);
            cookieModel.AppServers = new List<string> { "vm-icon-test1" };
            // Act
            cookieManager.SetEnvironmentCookies(mockRequest.Object, mockResponse.Object, cookieModel);
            // Assert
            var responseCookieForEnvironmentName = this.mockResponse.Object.Cookies[cookieNameForEnvironment];
            Assert.IsNotNull(responseCookieForEnvironmentName);
        }

        [TestMethod]
        public void SetEnvironmentCookies_WhenCustomEnvironmentWithSingleServer_ShouldAddServersCookie()
        {
            // Arrange
            var cookieNameForServers = nameForServersCookie;
            var expectedEnvironmentEnum = EnvironmentEnum.Custom;
            var expectedEnvironmentName = "Ed's Env";
            var cookieModel = new EnvironmentCookieModel(expectedEnvironmentName, expectedEnvironmentEnum);
            cookieModel.AppServers = new List<string> { "vm-icon-test1" };
            // Act
            cookieManager.SetEnvironmentCookies(mockRequest.Object, mockResponse.Object, cookieModel);
            // Assert
            var responseCookieForServers = this.mockResponse.Object.Cookies[cookieNameForServers];
            Assert.IsNotNull(responseCookieForServers);
        }

        [TestMethod]
        public void SetEnvironmentCookies_WhenCustomEnvironmentWithSingleServer_ShouldAddServersCookieWithExpectedValue()
        {
            // Arrange
            var cookieNameForServers = nameForServersCookie;
            var expectedEnvironmentEnum = EnvironmentEnum.Custom;
            var nameCookieValue = "Ed's Env";
            var expectedServersValue = "vm-icon-test1";
            var cookieModel = new EnvironmentCookieModel(nameCookieValue, expectedEnvironmentEnum);
            cookieModel.AppServers = new List<string> { "vm-icon-test1" };
            // Act
            cookieManager.SetEnvironmentCookies(mockRequest.Object, mockResponse.Object, cookieModel);
            // Assert
            var responseCookieForServers = this.mockResponse.Object.Cookies[cookieNameForServers];
            Assert.IsNotNull(responseCookieForServers);
            Assert.AreEqual(expectedServersValue, responseCookieForServers.Value);
        }

        [TestMethod]
        public void SetEnvironmentCookies_WhenCustomEnvironmentWithSingleServer_ShouldAddServersCookieWithExpectedExpiry()
        {
            // Arrange
            var cookieNameForServers = nameForServersCookie;
            var expectedExpiry = DateTime.Now.AddHours(duration);
            var expectedEnvironmentEnum = EnvironmentEnum.Custom;
            var nameCookieValue = "Ed's Env";
            var cookieModel = new EnvironmentCookieModel(nameCookieValue, expectedEnvironmentEnum);
            cookieModel.AppServers = new List<string> { "vm-icon-test1" };
            // Act
            cookieManager.SetEnvironmentCookies(mockRequest.Object, mockResponse.Object, cookieModel);
            // Assert
            var responseCookieForServers = this.mockResponse.Object.Cookies[cookieNameForServers];
            CustomAsserts.TimesCloseEnough(expectedExpiry, responseCookieForServers.Expires);
        }

        [TestMethod]
        public void SetEnvironmentCookies_WhenCustomEnvironmentWithDualServers_ShouldAddNameCookie()
        {
            // Arrange
            var cookieNameForEnvironment = nameForNameCookie;
            var expectedEnvironmentEnum = EnvironmentEnum.Custom;
            var nameCookieValue = "Ed's Env";
            var cookieModel = new EnvironmentCookieModel(nameCookieValue, expectedEnvironmentEnum);
            cookieModel.AppServers = new List<string> { "vm-icon-test1", "vm-icon-test2" };
            // Act
            cookieManager.SetEnvironmentCookies(mockRequest.Object, mockResponse.Object, cookieModel);
            // Assert
            var responseCookieForEnvironmentName = this.mockResponse.Object.Cookies[cookieNameForEnvironment];
            Assert.IsNotNull(responseCookieForEnvironmentName);
        }

        [TestMethod]
        public void SetEnvironmentCookies_WhenCustomEnvironmentWithDualServers_ShouldAddServersCookie()
        {
            // Arrange
            var cookieNameForServers = nameForServersCookie;
            var expectedEnvironmentEnum = EnvironmentEnum.Custom;
            var nameCookieValue = "Ed's Env";
            var cookieModel = new EnvironmentCookieModel(nameCookieValue, expectedEnvironmentEnum);
            cookieModel.AppServers = new List<string> { "vm-icon-test1", "vm-icon-test2" };
            // Act
            cookieManager.SetEnvironmentCookies(mockRequest.Object, mockResponse.Object, cookieModel);
            // Assert
            var responseCookieForServers = this.mockResponse.Object.Cookies[cookieNameForServers];
            Assert.IsNotNull(responseCookieForServers);
        }

        [TestMethod]
        public void SetEnvironmentCookies_WhenCustomEnvironmentWithDualServers_ShouldAddServersCookieWithExpectedValue()
        {
            // Arrange
            var cookieNameForServers = nameForServersCookie;
            var expectedEnvironmentEnum = EnvironmentEnum.Custom;
            var expectedServersValue = "vm-icon-test1,vm-icon-test2";
            var nameCookieValue = "Ed's Env";
            var cookieModel = new EnvironmentCookieModel(nameCookieValue, expectedEnvironmentEnum);
            cookieModel.AppServers = new List<string> { "vm-icon-test1", "vm-icon-test2" };
            // Act
            cookieManager.SetEnvironmentCookies(mockRequest.Object, mockResponse.Object, cookieModel);
            // Assert
            var responseCookieForServers = this.mockResponse.Object.Cookies[cookieNameForServers];
            Assert.AreEqual(expectedServersValue, responseCookieForServers.Value);
        }

        [TestMethod]
        public void SetEnvironmentCookies_WhenCustomEnvironmentWithDualServers_ShouldAddServersCookieWithExpectedExpiry()
        {
            // Arrange
            var cookieNameForServers = nameForServersCookie;
            var expectedExpiry = DateTime.Now.AddHours(duration);
            var expectedEnvironmentEnum = EnvironmentEnum.Custom;
            var nameCookieValue = "Ed's Env";
            var cookieModel = new EnvironmentCookieModel(nameCookieValue, expectedEnvironmentEnum);
            cookieModel.AppServers = new List<string> { "vm-icon-test1", "vm-icon-test2" };
            // Act
            cookieManager.SetEnvironmentCookies(mockRequest.Object, mockResponse.Object, cookieModel);
            // Assert
            var responseCookieForServers = this.mockResponse.Object.Cookies[cookieNameForServers];
            CustomAsserts.TimesCloseEnough(expectedExpiry, responseCookieForServers.Expires);
        }

        [TestMethod]
        public void SetEnvironmentCookies_WhenCustomEnvironmentWithMultipleServers_ShouldAddNameCookie()
        {
            // Arrange
            var cookieNameForEnvironment = nameForNameCookie;
            var expectedEnvironmentEnum = EnvironmentEnum.Custom;
            var nameCookieValue = "Ed's Env";
            var cookieModel = new EnvironmentCookieModel(nameCookieValue, expectedEnvironmentEnum);
            cookieModel.AppServers = new List<string> { "vm-icon-qa1", "vm-icon-qa2", "mammoth-app01-qa" };
            // Act
            cookieManager.SetEnvironmentCookies(mockRequest.Object, mockResponse.Object, cookieModel);
            // Assert
            var responseCookieForEnvironmentName = this.mockResponse.Object.Cookies[cookieNameForEnvironment];
            Assert.IsNotNull(responseCookieForEnvironmentName);
        }

        [TestMethod]
        public void SetEnvironmentCookies_WhenCustomEnvironmentWithMultipleServers_ShouldAddServersCookie()
        {
            // Arrange
            var cookieNameForServers = nameForServersCookie;
            var expectedEnvironmentEnum = EnvironmentEnum.Custom;
            var nameCookieValue = "Ed's Env";
            var cookieModel = new EnvironmentCookieModel(nameCookieValue, expectedEnvironmentEnum);
            cookieModel.AppServers = new List<string> { "vm-icon-qa1", "vm-icon-qa2", "mammoth-app01-qa" };
            // Act
            cookieManager.SetEnvironmentCookies(mockRequest.Object, mockResponse.Object, cookieModel);
            // Assert
            var responseCookieForServers = this.mockResponse.Object.Cookies[cookieNameForServers];
            Assert.IsNotNull(responseCookieForServers);
        }

        [TestMethod]
        public void SetEnvironmentCookies_WhenCustomEnvironmentWithMultipleServers_ShouldAddServersCookieWithExpectedValue()
        {
            // Arrange
            var cookieNameForServers = nameForServersCookie;
            var expectedEnvironmentEnum = EnvironmentEnum.Custom;
            var nameCookieValue = "Ed's Env";
            var expectedServersValue = "vm-icon-qa1,vm-icon-qa2,mammoth-app01-qa";
            var cookieModel = new EnvironmentCookieModel(nameCookieValue, expectedEnvironmentEnum);
            cookieModel.AppServers = new List<string> { "vm-icon-qa1", "vm-icon-qa2", "mammoth-app01-qa" };
            // Act
            cookieManager.SetEnvironmentCookies(mockRequest.Object, mockResponse.Object, cookieModel);
            // Assert
            var responseCookieForServers = this.mockResponse.Object.Cookies[cookieNameForServers];
            Assert.AreEqual(expectedServersValue, responseCookieForServers.Value);
        }

        [TestMethod]
        public void SetEnvironmentCookies_WhenCustomEnvironmentWithMultipleServers_ShouldAddServersCookieWithExpectedExpiry()
        {
            // Arrange
            var cookieNameForServers = nameForServersCookie;
            var expectedExpiry = DateTime.Now.AddHours(duration);
            var expectedEnvironmentEnum = EnvironmentEnum.Custom;
            var nameCookieValue = "Ed's Env";
            var cookieModel = new EnvironmentCookieModel(nameCookieValue, expectedEnvironmentEnum);
            cookieModel.AppServers = new List<string> { "vm-icon-qa1", "vm-icon-qa2", "mammoth-app01-qa" };
            // Act
            cookieManager.SetEnvironmentCookies(mockRequest.Object, mockResponse.Object, cookieModel);
            // Assert
            var responseCookieForServers = this.mockResponse.Object.Cookies[cookieNameForServers];
            CustomAsserts.TimesCloseEnough(expectedExpiry, responseCookieForServers.Expires);
        }

        [TestMethod]
        public void ClearEnvironmentCookies_WhenNoCookiesSet_ShouldNoThrowAndShouldBeNoCookies()
        {
            // Arrange
            var cookieNameForEnvironment = nameForNameCookie;
            var cookieNameForServers = nameForServersCookie;
            // (initialization adds empty cookie collections by default)
            // Act
            cookieManager.ClearEnvironmentCookies(mockRequest.Object, mockResponse.Object);
            // Assert
            var responseCookieForEnvironmentName = this.mockResponse.Object.Cookies[cookieNameForEnvironment];
            var responseCookieForServers = this.mockResponse.Object.Cookies[cookieNameForServers];
            Assert.IsNull(responseCookieForEnvironmentName);
            Assert.IsNull(responseCookieForServers);
            // Cookies property access: once for each of the two cookies
            mockRequest.Verify(r => r.Cookies, Times.Exactly(2));
        }

        [TestMethod]
        public void ClearEnvironmentCookies_WhenOnlyEnvironmentNameCookieSet_SetsCookieToExpired()
        {
            // Arrange
            var cookieNameForEnvironment = nameForNameCookie;
            var cookieNameForServers = nameForServersCookie;
            AddRequestEnvironmentNameCookie("Perf");
            // Act
            cookieManager.ClearEnvironmentCookies(mockRequest.Object, mockResponse.Object);
            // Assert
            var responseCookieForEnvironmentName = this.mockResponse.Object.Cookies[cookieNameForEnvironment];
            var responseCookieForServers = this.mockResponse.Object.Cookies[cookieNameForServers];
            Assert.IsNotNull(responseCookieForEnvironmentName);
            Assert.IsTrue(responseCookieForEnvironmentName.Expires < DateTime.Now);
            Assert.IsNull(responseCookieForServers);
            // Cookies property access: once for setup, then one each for checking if they exist
            mockRequest.Verify(r => r.Cookies, Times.Exactly(3));
        }

        [TestMethod]
        public void ClearEnvironmentCookies_WhenOnlyAppServersCookieSet_SetsCookieToExpired()
        {
            // Arrange
            var cookieNameForEnvironment = nameForNameCookie;
            var cookieNameForServers = nameForServersCookie;
            AddRequestAppServersCookie("vm-icon-test1,vm-icon-test2");
            // Act
            cookieManager.ClearEnvironmentCookies(mockRequest.Object, mockResponse.Object);
            // Assert
            var responseCookieForEnvironmentName = this.mockResponse.Object.Cookies[cookieNameForEnvironment];
            var responseCookieForServers = this.mockResponse.Object.Cookies[cookieNameForServers];
            Assert.IsNull(responseCookieForEnvironmentName);
            Assert.IsNotNull(responseCookieForServers);
            Assert.IsTrue(responseCookieForServers.Expires < DateTime.Now);
            // Cookies property access: once for setup, then one each for checking if they exist
            mockRequest.Verify(r => r.Cookies, Times.Exactly(3));
        }

        [TestMethod]
        public void ClearEnvironmentCookies_WhenEnvironmentNameAndAppServersCookiesSet_SetsCookiesToExpired()
        {
            // Arrange
            var cookieNameForEnvironment = nameForNameCookie;
            var cookieNameForServers = nameForServersCookie;
            AddRequestEnvironmentNameCookie("Tst1");
            AddRequestAppServersCookie("vm-icon-test1,vm-icon-test2");
            // Act
            cookieManager.ClearEnvironmentCookies(mockRequest.Object, mockResponse.Object);
            // Assert
            var responseCookieForEnvironmentName = this.mockResponse.Object.Cookies[cookieNameForEnvironment];
            var responseCookieForServers = this.mockResponse.Object.Cookies[cookieNameForServers];
            Assert.IsNotNull(responseCookieForEnvironmentName);
            Assert.IsNotNull(responseCookieForServers);
            Assert.IsTrue(responseCookieForEnvironmentName.Expires < DateTime.Now);
            Assert.IsTrue(responseCookieForServers.Expires < DateTime.Now);
            // Cookies property access: twice for each of the two cookies (one for setup, one to check if exists)
            mockRequest.Verify(r => r.Cookies, Times.Exactly(4));
            //// should add two expired cookies to response 
            //mockResponse.Verify(r => r.Cookies.Add(It.IsAny<HttpCookie>()), Times.Exactly(2));
        }

        [TestMethod]
        public void GetEnvironmentNameCookieValueOrNull_WhenNameCookieAbsent_ReturnsNull()
        {  
            // Arrange
            // (initialization adds empty cookie collections by default)
            // Act
            var cookieVal = cookieManager.GetEnvironmentNameCookieValueOrNull(mockRequest.Object);
            // Assert
            Assert.IsNull(cookieVal);
            // Cookies property access: single attempt for cookie value
            mockRequest.Verify(r => r.Cookies, Times.Once);
        }

        [TestMethod]
        public void GetEnvironmentNameCookieValueOrNull_WhenNameCookieHasNullValue_ReturnsNull()
        {
            // Arrange
            AddRequestEnvironmentNameCookie(null);
            // Act
            var cookieVal = cookieManager.GetEnvironmentNameCookieValueOrNull(mockRequest.Object);
            // Assert
            Assert.IsNull(cookieVal);
            // Cookies property access: once for setup, once for check to see if there, once for value
            mockRequest.Verify(r => r.Cookies, Times.Exactly(3));
        }

        [TestMethod]
        public void GetEnvironmentNameCookieValueOrNull_WhenNameCookieHasBlankValue_ReturnsNull()
        {
            // Arrange
            AddRequestEnvironmentNameCookie("");
            // Act
            var cookieVal = cookieManager.GetEnvironmentNameCookieValueOrNull(mockRequest.Object);
            // Assert
            Assert.IsNull(cookieVal);
            // Cookies property access: once for setup, once for check to see if there, once for value
            mockRequest.Verify(r => r.Cookies, Times.Exactly(3));
        }

        [TestMethod]
        public void GetEnvironmentNameCookieValueOrNull_WhenNameCookieHasWhitespaceValue_ReturnsNull()
        {
            // Arrange
            AddRequestEnvironmentNameCookie(" \t ");
            // Act
            var cookieVal = cookieManager.GetEnvironmentNameCookieValueOrNull(mockRequest.Object);
            // Assert
            Assert.IsNull(cookieVal);
            // Cookies property access: once for setup, once for check to see if there, once for value
            mockRequest.Verify(r => r.Cookies, Times.Exactly(3));
        }

        [TestMethod]
        public void GetEnvironmentNameCookieValueOrNull_WhenNameCookieHasValue_ReturnsValue()
        {
            // Arrange
            var expectedEnvironmentName = EnvironmentEnum.Perf.ToString();
            AddRequestEnvironmentNameCookie(expectedEnvironmentName);
            // Act
            var cookieVal = cookieManager.GetEnvironmentNameCookieValueOrNull(mockRequest.Object);
            // Assert
            Assert.IsNotNull(cookieVal);
            Assert.AreEqual(expectedEnvironmentName, cookieVal);
            // Cookies property access: once for setup, once for check to see if there, once for value
            mockRequest.Verify(r => r.Cookies, Times.Exactly(3));
        }

        [TestMethod]
        public void GetEnvironmentNameCookieValueOrNull_WhenUnrelatedCookieHasValue_ReturnsNull()
        {
            // Arrange
            AddRequestCookie(this.mockRequest.Object, "xyz123", "somethingElse");
            // Act
            var cookieVal = cookieManager.GetEnvironmentNameCookieValueOrNull(mockRequest.Object);
            // Assert
            Assert.IsNull(cookieVal);
            // Cookies property access: once for setup, then again to access
            mockRequest.Verify(r => r.Cookies, Times.Exactly(2));
        }

        [TestMethod]
        public void GetAppServersCookieValueOrNull_WhenAppServersCookieAbsent_ReturnsNull()
        {
            // Arrange
            // (initialization adds empty cookie collections by default)
            // Act
            var cookieVal = cookieManager.GetEnvironmentNameCookieValueOrNull(mockRequest.Object);
            // Assert
            Assert.IsNull(cookieVal);
            // Cookies property access: single attempt for cookie value
            mockRequest.Verify(r => r.Cookies, Times.Once);
        }

        [TestMethod]
        public void GetAppServersCookieValueOrNull_WhenAppServersCookieHasNullValue_ReturnsNull()
        {
            // Arrange
            AddRequestAppServersCookie(null);
            // Act
            var cookieVal = cookieManager.GetAppServersCookieValueOrNull(mockRequest.Object);
            // Assert
            Assert.IsNull(cookieVal);
            // Cookies property access: once for setup, once for check to see if there, once for value
            mockRequest.Verify(r => r.Cookies, Times.Exactly(3));
        }

        [TestMethod]
        public void GetAppServersCookieValueOrNull_WhenBlankAppServersCookie_ReturnsNull()
        {
            // Arrange
            AddRequestAppServersCookie("");
            // Act
            var cookieVal = cookieManager.GetAppServersCookieValueOrNull(mockRequest.Object);
            // Assert
            Assert.IsNull(cookieVal);
            // Cookies property access: once for setup, once for check to see if there, once for value
            mockRequest.Verify(r => r.Cookies, Times.Exactly(3));
        }

        [TestMethod]
        public void GetAppServersCookieValueOrNull_WhenWhitespaceAppServersCookie_ReturnsNull()
        {
            // Arrange
            AddRequestAppServersCookie("\r\n");
            // Act
            var cookieVal = cookieManager.GetAppServersCookieValueOrNull(mockRequest.Object);
            // Assert
            Assert.IsNull(cookieVal);
            // Cookies property access: once for setup, once for check to see if there, once for value
            mockRequest.Verify(r => r.Cookies, Times.Exactly(3));
        }

        [TestMethod]
        public void GetAppServersCookieValueOrNull_WhenSingleValueInAppServersCookie_ReturnsOneItemList()
        {
            // Arrange
            var expectedEnvironmentNames = new List<string>
            {
                "vm-icon-test1"
            };
            AddRequestAppServersCookie(string.Join(",", expectedEnvironmentNames));
            // Act
            var cookieVal = cookieManager.GetAppServersCookieValueOrNull(mockRequest.Object);
            // Assert
            Assert.IsNotNull(cookieVal);
            CustomAsserts.ListsAreEqual(expectedEnvironmentNames, cookieVal);
            // Cookies property access: once for setup, once for check to see if there, once for value
            mockRequest.Verify(r => r.Cookies, Times.Exactly(3));
        }

        [TestMethod]
        public void GetAppServersCookieValueOrNull_WhenTwoValuesInAppServersCookie_ReturnsExpectedList()
        {
            // Arrange
            var expectedEnvironmentNames = new List<string>
            {
                "vm-icon-qa3",
                "vm-icon-qa4"
            };
            AddRequestAppServersCookie(string.Join(",", expectedEnvironmentNames));
            // Act
            var cookieVal = cookieManager.GetAppServersCookieValueOrNull(mockRequest.Object);
            // Assert
            Assert.IsNotNull(cookieVal);
            CustomAsserts.ListsAreEqual(expectedEnvironmentNames, cookieVal);
            // Cookies property access: once for setup, once for check to see if there, once for value
            mockRequest.Verify(r => r.Cookies, Times.Exactly(3));
        }

        [TestMethod]
        public void GetAppServersCookieValueOrNull_WhenMultipleValuesInAppServersCookie_ReturnsExpectedList()
        {
            // Arrange
            var expectedEnvironmentNames = new List<string>
            {
                "vm-icon-qa1",
                "vm-icon-qa2",
                "mammoth-app01-qa"
            };
            AddRequestAppServersCookie(string.Join(",", expectedEnvironmentNames));
            // Act
            var cookieVal = cookieManager.GetAppServersCookieValueOrNull(mockRequest.Object);
            // Assert
            Assert.IsNotNull(cookieVal);
            CustomAsserts.ListsAreEqual(expectedEnvironmentNames, cookieVal);
            // Cookies property access: once for setup, once for check to see if there, once for value
            mockRequest.Verify(r => r.Cookies, Times.Exactly(3));
        }

        [TestMethod]
        public void GetAppServersCookieValueOrNull_WhenUnrelatedCookieHasValue_ReturnsNull()
        {
            // Arrange
            AddRequestCookie(this.mockRequest.Object, "xyz123", "somethingElse");
            // Act
            var cookieVal = cookieManager.GetAppServersCookieValueOrNull(mockRequest.Object);
            // Assert
            Assert.IsNull(cookieVal);
            // Cookies property access: once for setup, then again to access
            mockRequest.Verify(r => r.Cookies, Times.Exactly(2));
        }
    }
}
