using FluentAssertions;
using Icon.Dashboard.Mvc.Filters;
using Icon.Dashboard.Mvc.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace Icon.Dashboard.Mvc.UnitTests.FilterTests.AuthorizationAttributeUnitTest
{
    [TestClass]
    public class IsAuthorizedUnitTests : _AuthorizationAttributeUnitTestBase
    {
        protected bool No = false;
        protected bool Yes = true;

        [TestMethod]
        public void WhenRequiredRole_IsUnauthorized_AndUser_IsNull_Should_BeAuthorized()
        {
            // Arrange
            // Act
            var isAuthorized = DashboardAuthorization.IsAuthorized(null, UserRoleEnum.Unauthorized);
            // Assert
            isAuthorized.Should().Be(Yes);
        }

        [TestMethod]
        public void WhenRequiredRole_IsApplications_AndUser_IsNonIrma_Should_NotBeAuthorized()
        {
            // Arrange
            var user = base.JoeSchmo;
            // Act
            var isAuthorized = DashboardAuthorization.IsAuthorized(user, UserRoleEnum.ReadOnly);
            // Assert
            isAuthorized.Should().Be(No);
        }

        [TestMethod]
        public void WhenRequiredRole_IsApplications_AndUser_IsInApplications_Should_BeAuthorized()
        {
            // Arrange
            var user = base.SallyTheBA;
            // Act
            var isAuthorized = DashboardAuthorization.IsAuthorized(user, UserRoleEnum.ReadOnly);
            // Assert
            isAuthorized.Should().Be(Yes);
        }

        [TestMethod]
        public void WhenRequiredRole_IsApplications_AndUser_IsInDevelopers_Should_BeAuthorized()
        {
            // Arrange
            var user = base.DeveloperBob;
            // Act
            var isAuthorized = DashboardAuthorization.IsAuthorized(user, UserRoleEnum.ReadOnly);
            // Assert
            isAuthorized.Should().Be(Yes);
        }

        [TestMethod]
        public void WhenRequiredRole_IsApplications_AndUser_IsInDevelopersButNotApplications_Should_BeAuthorized()
        {
            // Arrange
            var user = base.UserInDevelopersButNotApplicationsGroup;
            // Act
            var isAuthorized = DashboardAuthorization.IsAuthorized(user, UserRoleEnum.ReadOnly);
            // Assert
            isAuthorized.Should().Be(Yes);
        }

        [TestMethod]
        public void WhenRequiredRole_IsDeveloper_AndUser_IsNonIrma_Should_NotBeAuthorized()
        {
            // Arrange
            var user = base.JoeSchmo;
            // Act
            var isAuthorized = DashboardAuthorization.IsAuthorized(user, UserRoleEnum.EditingPrivileges);
            // Assert
            isAuthorized.Should().Be(No);
        }

        [TestMethod]
        public void WhenRequiredRole_IsDeveloper_AndUser_IsInApplications_Should_NotBeAuthorized()
        {
            // Arrange
            var user = base.SallyTheBA;
            // Act
            var isAuthorized = DashboardAuthorization.IsAuthorized(user, UserRoleEnum.EditingPrivileges);
            // Assert
            isAuthorized.Should().Be(No);
        }

        [TestMethod]
        public void WhenRequiredRole_IsDeveloper_AndUser_IsInDevelopers_Should_BeAuthorized()
        {
            // Arrange
            var user = base.DeveloperBob;
            // Act
            var isAuthorized = DashboardAuthorization.IsAuthorized(user, UserRoleEnum.EditingPrivileges);
            // Assert
            isAuthorized.Should().Be(Yes);
        }

        [TestMethod]
        public void WhenRequiredRole_IsDeveloper_AndUser_IsInDevelopersButNotApplications_Should_BeAuthorized()
        {
            // Arrange
            var user = base.UserInDevelopersButNotApplicationsGroup;
            // Act
            var isAuthorized = DashboardAuthorization.IsAuthorized(user, UserRoleEnum.EditingPrivileges);
            // Assert
            isAuthorized.Should().Be(Yes);
        }

        [TestMethod]
        public void WhenRequiredRole_IsNone_AndUser_IsNonIrma_Should_BeAuthorized()
        {
            // Arrange
            var user = base.JoeSchmo;
            // Act
            var isAuthorized = DashboardAuthorization.IsAuthorized(user, UserRoleEnum.Unauthorized);
            // Assert
            isAuthorized.Should().Be(Yes);
        }

        [TestMethod]
        public void WhenRequiredRole_IsNone_AndUser_IsInApplications_Should_BeAuthorized()
        {
            // Arrange
            var user = base.SallyTheBA;
            // Act
            var isAuthorized = DashboardAuthorization.IsAuthorized(user, UserRoleEnum.Unauthorized);
            // Assert
            isAuthorized.Should().Be(Yes);
        }

        [TestMethod]
        public void WhenRequiredRole_IsNone_AndUser_IsInDevelopers_Should_BeAuthorized()
        {
            // Arrange
            var user = base.DeveloperBob;
            // Act
            var isAuthorized = DashboardAuthorization.IsAuthorized(user, UserRoleEnum.Unauthorized);
            // Assert
            isAuthorized.Should().Be(Yes);
        }

        [TestMethod]
        public void WhenRequiredRole_IsNone_AndUser_IsInDevelopersButNotApplications_Should_BeAuthorized()
        {
            // Arrange
            var user = base.UserInDevelopersButNotApplicationsGroup;
            // Act
            var isAuthorized = DashboardAuthorization.IsAuthorized(user, UserRoleEnum.Unauthorized);
            // Assert
            isAuthorized.Should().Be(Yes);
        }
    }
}
