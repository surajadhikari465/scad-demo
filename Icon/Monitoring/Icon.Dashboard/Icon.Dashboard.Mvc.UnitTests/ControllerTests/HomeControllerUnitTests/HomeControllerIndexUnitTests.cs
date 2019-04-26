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
    public class HomeControllerIndexUnitTests : _HomeControllerUnitTestBase
    {
        public HomeControllerIndexUnitTests() : base() { }


        [TestMethod]
        public void MvcHomeController_Index_Get_Should_ReturnViewResult()
        {
            //Given
            var fakeData = base.AllFakeAppViewModels;
            base.mockEsbEnvironmentMgmtSvc
                .Setup(s => s.GetEsbEnvironmentDefinitions())
                .Returns(new List<EsbEnvironmentViewModel>());
            base.mockRemoteWmiSerivceWrapper
                .Setup(s => s.LoadRemoteServices(It.IsAny<DashboardEnvironmentViewModel>(), It.IsAny<bool>()))
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
            base.mockRemoteWmiSerivceWrapper
                .Setup(s => s.LoadRemoteServices(It.IsAny<DashboardEnvironmentViewModel>(), It.IsAny<bool>()))
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
            var controller = ConstructController();
            const string cmd = "Start";

            //When
            var result = controller.Index(FakeServiceViewModelA.Name, FakeServiceViewModelA.Server, cmd);

            //Then
            mockRemoteWmiSerivceWrapper.Verify(s => s
                .ExecuteServiceCommand( It.IsAny<string>(), It.IsAny<string>(), cmd),
                Times.Once);
        }

        [TestMethod]
        public void MvcHomeController_Index_Post_Should_IssueStopCommand()
        {
            //Given
            var controller = ConstructController();
            const string cmd = "Stop";

            //When
            var result = controller.Index(FakeServiceViewModelA.Name, FakeServiceViewModelA.Server, cmd);

            //Then
            mockRemoteWmiSerivceWrapper.Verify(s => s
                .ExecuteServiceCommand( It.IsAny<string>(),  It.IsAny<string>(), cmd ),
                Times.Once);
        }

        [TestMethod]
        public void MvcHomeController_Index_Post_ShouldIssueAnyCommand()
        {
            //Given
            var controller = ConstructController();
            const string cmd = "DoIt";

            //When
            var result = controller.Index(FakeServiceViewModelA.Name, FakeServiceViewModelA.Server, cmd);

            //Then
            mockRemoteWmiSerivceWrapper.Verify(s => s
                .ExecuteServiceCommand( It.IsAny<string>(), It.IsAny<string>(),  cmd),
                Times.Once);
        }

        [TestMethod]
        public void MvcHomeController_Index_Post_Should_RedirectToGetAfterExecutingCommand()
        {
            //Given
            var controller = ConstructController();
            const string cmd = "Start";

            //When
            var result = controller.Index(FakeServiceViewModelA.Name, FakeServiceViewModelA.Server, cmd);

            //Then
            Assert.IsInstanceOfType(result, typeof(RedirectToRouteResult));
            RedirectToRouteResult routeResult = result as RedirectToRouteResult;
        }

        [TestMethod]
        public void MvcHomeController_Index_Post_Should_RedirectToGetWithExpectedRouteValues()
        {
            //Given
            var controller = ConstructController();
            const string cmd = "Start";

            //When
            var result = controller.Index(FakeServiceViewModelA.Name, FakeServiceViewModelA.Server, cmd);

            //Then
            Assert.IsInstanceOfType(result, typeof(RedirectToRouteResult));
            RedirectToRouteResult routeResult = result as RedirectToRouteResult;
            Assert.AreEqual(routeResult.RouteValues["action"], "Index");
            Assert.AreEqual(routeResult.RouteValues["controller"], "Home");
        }
    }
}
