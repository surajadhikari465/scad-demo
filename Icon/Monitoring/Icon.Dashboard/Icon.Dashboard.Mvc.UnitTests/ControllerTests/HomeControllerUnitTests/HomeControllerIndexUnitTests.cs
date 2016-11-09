using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Icon.Dashboard.Mvc.Controllers;
using System.Web.Mvc;
using System.Web;
using Icon.Dashboard.Mvc.Services;
using Moq;
using System.Collections.Generic;
using Icon.Dashboard.Mvc.ViewModels;
using Icon.Dashboard.DataFileAccess.Models;
using System.Threading.Tasks;

namespace Icon.Dashboard.Mvc.UnitTests.ControllerTests.HomeControllerUnitTests
{
    [TestClass]
    public class HomeControllerIndexUnitTests : _HomeControllerUnitTestBase
    {
        public HomeControllerIndexUnitTests() : base() { }

        [TestMethod]
        public void MvcHomeController_Index_Get_Should_ReturnViewResult()
        {
            //Given
            var fakeData = base.AllFakeAppViewModels;
            base.mockDataServiceWrapper
                .Setup(s => s.GetApplicationListViewModels(It.IsAny<HttpServerUtilityBase>(), It.IsAny<string>(), It.IsAny<string>()))
                .Returns(fakeData);
            var controller = ConstructController();

            //When
            ActionResult result = controller.Index();

            //Then
            Assert.IsInstanceOfType(result, typeof(ViewResult));
        }

        [TestMethod]
        public void MvcHomeController_Index_Get_Should_ReturnViewResultWithData()
        {
            //Given
            var fakeData = base.AllFakeAppViewModels;
            int expectedCount = fakeData.Count;
            base.mockDataServiceWrapper
                .Setup(s => s.GetApplicationListViewModels(It.IsAny<HttpServerUtilityBase>(), It.IsAny<string>(), It.IsAny<string>()))
                .Returns(fakeData);
            var controller = ConstructController();

            //When
            var result = controller.Index();

            //Then
            var viewResult = (ViewResult)result;
            Assert.IsNotNull(viewResult.Model);
            var modelResult = viewResult.Model as List<IconApplicationViewModel>;
            Assert.AreEqual(expectedCount, modelResult.Count);
        }

        [TestMethod]
        public void MvcHomeController_Index_Post_Should_IssueStartCommand()
        {
            //Given
            mockDataServiceWrapper
                .Setup(s => s.GetApplication(It.IsAny<HttpServerUtilityBase>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
                .Returns(fakeServiceA);
            var controller = ConstructController();
            const string cmd = "Start";

            //When
            var result = controller.Index(fakeServiceA.Name, fakeServiceA.Server, cmd);

            //Then
            mockDataServiceWrapper.Verify(s => s
                .ExecuteCommand(It.IsAny<HttpServerUtilityBase>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), cmd),
                Times.Once);
        }

        [TestMethod]
        public void MvcHomeController_Index_Post_Should_IssueStopCommand()
        {
            //Given
            mockDataServiceWrapper
                .Setup(s => s.GetApplication(It.IsAny<HttpServerUtilityBase>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
                .Returns(fakeServiceA);
            var controller = ConstructController();
            const string cmd = "Stop";

            //When
            var result = controller.Index(fakeServiceA.Name, fakeServiceA.Server, cmd);

            //Then
            mockDataServiceWrapper.Verify(s => s
                .ExecuteCommand(It.IsAny<HttpServerUtilityBase>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), cmd),
                Times.Once);
        }

        [TestMethod]
        public void MvcHomeController_Index_Post_ShouldIssueAnyCommand()
        {
            //Given
            mockDataServiceWrapper
                .Setup(s => s.GetApplication(It.IsAny<HttpServerUtilityBase>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
                .Returns(fakeServiceA);
            var controller = ConstructController();
            const string cmd = "DoIt";

            //When
            var result = controller.Index(fakeServiceA.Name, fakeServiceA.Server, cmd);

            //Then
            mockDataServiceWrapper.Verify(s => s
                .ExecuteCommand(It.IsAny<HttpServerUtilityBase>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), cmd),
                Times.Once);
        }

        [TestMethod]
        public void MvcHomeController_Index_Post_Should_RedirectToGetAfterExecutingCommand()
        {
            //Given
            mockDataServiceWrapper
                .Setup(s => s.GetApplication(It.IsAny<HttpServerUtilityBase>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
                .Returns(fakeServiceA);
            var controller = ConstructController();
            const string cmd = "Start";

            //When
            var result = controller.Index(fakeServiceA.Name, fakeServiceA.Server, cmd);

            //Then
            Assert.IsInstanceOfType(result, typeof(RedirectToRouteResult));
            RedirectToRouteResult routeResult = result as RedirectToRouteResult;
        }

        [TestMethod]
        public void MvcHomeController_Index_Post_Should_RedirectToGetWithExpectedRouteValues()
        {
            //Given
            mockDataServiceWrapper
                .Setup(s => s.GetApplication(It.IsAny<HttpServerUtilityBase>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
                .Returns(fakeServiceA);
            var controller = ConstructController();
            const string cmd = "Start";

            //When
            var result = controller.Index(fakeServiceA.Name, fakeServiceA.Server, cmd);

            //Then
            Assert.IsInstanceOfType(result, typeof(RedirectToRouteResult));
            RedirectToRouteResult routeResult = result as RedirectToRouteResult;
            Assert.AreEqual(routeResult.RouteValues["action"], "Index");
            Assert.AreEqual(routeResult.RouteValues["controller"], "Home");
        }
    }
}
