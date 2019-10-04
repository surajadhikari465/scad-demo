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
using System.Security.Principal;
using Icon.Dashboard.Mvc.Enums;
using Icon.Dashboard.Mvc.Models;

namespace Icon.Dashboard.Mvc.UnitTests.ControllerTests
{
    [TestClass]
    public class BaseControllerIndexUnitTests : _HomeControllerUnitTestBase
    {
        public BaseControllerIndexUnitTests() : base() { }

        protected override string testActionName => "Index";

        [TestMethod]
        public void BuildGlobalViewModel_ShouldReturnObject()
        {
            // Arrange
            var controller = ConstructController();

            // Act
            var globalViewModel = controller.BuildGlobalViewModel();

            // Assert
            Assert.IsNotNull(globalViewModel);
        }

        [TestMethod]
        public void UserMayEdit_WhenAuthorizerReturnsTrue_ReturnsTrue()
        {
            // Arrange
            MockAuthorizer.Setup(a => a.IsAuthorized(It.IsAny<IPrincipal>(), It.IsAny<UserAuthorizationLevelEnum>()))
                .Returns(true);
            var controller = ConstructController();
            // Act
            var isAuthorized = controller.UserMayEdit();
            // Assert
            Assert.AreEqual(true, isAuthorized);
        }

        [TestMethod]
        public void UserMayEdit_WhenAuthorizerReturnsFalse_ReturnsFalse()
        {
            // Arrange
            MockAuthorizer.Setup(a => a.IsAuthorized(It.IsAny<IPrincipal>(), It.IsAny<UserAuthorizationLevelEnum>()))
                .Returns(false);
            var controller = ConstructController();
            // Act
            var isAuthorized = controller.UserMayEdit();
            // Assert
            Assert.AreEqual(false, isAuthorized);
        }
    }
}
