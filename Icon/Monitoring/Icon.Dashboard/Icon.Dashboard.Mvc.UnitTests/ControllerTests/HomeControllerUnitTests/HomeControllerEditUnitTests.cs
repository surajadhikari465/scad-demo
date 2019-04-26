using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Icon.Dashboard.Mvc.Controllers;
using System.Web.Mvc;
using System.Web;
using Icon.Dashboard.Mvc.Services;
using Moq;
using System.Collections.Generic;
using Icon.Dashboard.Mvc.ViewModels;
using System.Threading.Tasks;

namespace Icon.Dashboard.Mvc.UnitTests.ControllerTests.HomeControllerUnitTests
{

    [TestClass]
    public class HomeControllerEditUnitTests : _HomeControllerUnitTestBase
    {
        [TestMethod]
        public void MvcHomeController_Edit_Get_Service_Should_ReturnViewResult()
        {
            //Given
            var testService = base.FakeServiceViewModelA;
            mockRemoteWmiSerivceWrapper
                .Setup(s => s.LoadRemoteService( It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<bool>()))
                .Returns(testService);
            var controller = ConstructController();

            //When
            ActionResult result = controller.Edit(testService.Name, testService.Server);

            //Then
            Assert.IsInstanceOfType(result, typeof(ViewResult));
        }

        [TestMethod]
        public void MvcHomeController_Edit_Get_Service_Should_CallGetViewModelMethod()
        {
            //Given
            var testService = base.FakeServiceViewModelA;
            mockRemoteWmiSerivceWrapper
                .Setup(s => s.LoadRemoteService( It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<bool>()))
                .Returns(testService);
            var controller = ConstructController();

            //When
            ActionResult result = controller.Edit(testService.Name, testService.Server);

            //Then
            mockRemoteWmiSerivceWrapper.Verify(s => s.LoadRemoteService( It.IsAny<string>(), It.IsAny<string>(), It.IsAny<bool>()), Times.Once);
        }

        [TestMethod]
        public void MvcHomeController_Edit_Post_Service_Should_CallSaveAppSettings()
        {
            //Given
            var testService = base.FakeServiceViewModelA;
            var postModel = base.FakeServiceViewModelA;
            var controller = ConstructController();

            //When
            ActionResult result = controller.Edit(postModel);

            //Then
            mockRemoteWmiSerivceWrapper.Verify(s => s.SaveRemoteServiceAppSettings( postModel), Times.Once);
        }

        [TestMethod]
        public void MvcHomeController_Edit_Post_Service_Should_RedirectToGetAfterExecutingCommand()
        {
            //Given
            var testService = base.FakeServiceViewModelA;
            var postModel = base.FakeServiceViewModelA;
            var controller = ConstructController();

            //When
            ActionResult result = controller.Edit(postModel);

            //Then
            Assert.IsInstanceOfType(result, typeof(RedirectToRouteResult));
            RedirectToRouteResult routeResult = result as RedirectToRouteResult;
        }

        [TestMethod]
        public void MvcHomeController_Edit_Post_Service_Should_RedirectToGetWithExpectedRouteValues()
        {
            //Given
            var testService = base.FakeServiceViewModelA;
            var postModel = base.FakeServiceViewModelA;
            var controller = ConstructController();

            //When
            ActionResult result = controller.Edit(postModel);

            //Then
            Assert.IsInstanceOfType(result, typeof(RedirectToRouteResult));
            RedirectToRouteResult routeResult = result as RedirectToRouteResult;
            Assert.AreEqual(routeResult.RouteValues["controller"], "Home");
            Assert.AreEqual(routeResult.RouteValues["action"], "Edit");
            Assert.AreEqual(routeResult.RouteValues["application"], testService.Name);
            Assert.AreEqual(routeResult.RouteValues["server"], testService.Server);
        }
    }
}
