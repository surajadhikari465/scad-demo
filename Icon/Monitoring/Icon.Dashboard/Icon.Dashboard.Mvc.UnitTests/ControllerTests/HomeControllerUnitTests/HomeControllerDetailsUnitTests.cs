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
    public class HomeControllerDetailsUnitTests : _HomeControllerUnitTestBase
    {
        [TestMethod]
        public void MvcHomeController_Details_Get_Service_Should_ReturnViewResult()
        {
            //Given
            var testService = base.FakeServiceViewModelA;
            mockDataServiceWrapper
                .Setup(s => s.GetApplication( It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
                .Returns(testService);
            var controller = ConstructController();

            //When
            ActionResult result = controller.Details(testService.Name, testService.Server);

            //Then
            Assert.IsInstanceOfType(result, typeof(ViewResult));
        }

        [TestMethod]
        public void MvcHomeController_Details_Post_Service_Should_IssueStartCommand()
        {
            //Given
            var testService = base.FakeServiceViewModelA;
            mockDataServiceWrapper
                .Setup(s => s.GetApplication( It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
                .Returns(testService);
            var controller = ConstructController();
            string cmd = "Start";

            //When
            ActionResult result = controller.Details(testService.Name, testService.Server, cmd);

            //Then
            mockDataServiceWrapper.Verify(s => s
                .ExecuteServiceCommand( It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), cmd),
                Times.Once);
        }

        [TestMethod]
        public void MvcHomeController_Details_Post_Service_Should_IssueStopCommand()
        {
            //Given
            var testService = base.FakeServiceViewModelA;
            mockDataServiceWrapper
                .Setup(s => s.GetApplication( It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
                .Returns(testService);
            var controller = ConstructController();
            string cmd = "Stop";

            //When
            ActionResult result = controller.Details(testService.Name, testService.Server, cmd);

            //Then
            mockDataServiceWrapper.Verify(s => s
                .ExecuteServiceCommand( It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), cmd),
                Times.Once);
        }

        [TestMethod]
        public void MvcHomeController_Details_Post_Service_Should_IssueAnyCommand()
        {
            //Given
            var testService = base.FakeServiceViewModelA;
            mockDataServiceWrapper
                .Setup(s => s.GetApplication( It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
                .Returns(testService);
            var controller = ConstructController();
            string cmd = "DoSomething";

            //When
            ActionResult result = controller.Details(testService.Name, testService.Server, cmd);

            //Then
            mockDataServiceWrapper.Verify(s => s
                .ExecuteServiceCommand( It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), cmd),
                Times.Once);
        }

        [TestMethod]
        public void MvcHomeController_Details_Post_Service_Should_RedirectToGetAfterExecutingCommand()
        {
            //Given
            var testService = base.FakeServiceViewModelA;
            mockDataServiceWrapper
                .Setup(s => s.GetApplication( It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
                .Returns(testService);
            var controller = ConstructController();
            string cmd = "Start";

            //When
            ActionResult result = controller.Details(testService.Name, testService.Server, cmd);

            //Then
            Assert.IsInstanceOfType(result, typeof(RedirectToRouteResult));
            RedirectToRouteResult routeResult = result as RedirectToRouteResult;
        }

        [TestMethod]
        public void MvcHomeController_Details_Post_Service_Should_RedirectToGetWithExpectedRouteValues()
        {
            //Given
            var testService = base.FakeServiceViewModelA;
            mockDataServiceWrapper
                .Setup(s => s.GetApplication( It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
                .Returns(testService);
            var controller = ConstructController();
            string cmd = "Start";

            //When
            ActionResult result = controller.Details(testService.Name, testService.Server, cmd);

            //Then
            Assert.IsInstanceOfType(result, typeof(RedirectToRouteResult));
            RedirectToRouteResult routeResult = result as RedirectToRouteResult;
            Assert.AreEqual(routeResult.RouteValues["action"], "Details");
            Assert.AreEqual(routeResult.RouteValues["controller"], "Home");
            Assert.AreEqual(routeResult.RouteValues["application"], testService.Name.ToString());
            Assert.AreEqual(routeResult.RouteValues["server"], testService.Server.ToString());
        }
    }
}
