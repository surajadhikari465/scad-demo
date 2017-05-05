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

namespace Icon.Dashboard.Mvc.UnitTests.FilterTests
{
    [TestClass]
    public class IsAuthorizedUnitTests : _DashboardAuthorizationFilterTestBase
    {
        protected bool No = false;
        protected bool Yes = true;

        [TestMethod]
        public void WhenRequiredRole_IsUnauthorized_AndUser_IsNull_Should_BeAuthorized()
        {
            // Arrange
            var user = base.UserNull;
            var requiredRole = base.Role_Unauthorized;
            // Act
            var isAuthorized = DashboardAuthorization.IsAuthorized(user, requiredRole);
            // Assert
            isAuthorized.Should().Be(Yes);
        }

        [TestMethod]
        public void WhenRequiredRole_IsApplications_AndUser_IsNonIrma_Should_NotBeAuthorized()
        {
            // Arrange
            var user = base.UserNotInAnyGroup;
            var requiredRole = base.Role_Applications;
            // Act
            var isAuthorized = DashboardAuthorization.IsAuthorized(user, requiredRole);
            // Assert
            isAuthorized.Should().Be(No);
        }

        [TestMethod]
        public void WhenRequiredRole_IsApplications_AndUser_IsInApplications_Should_BeAuthorized()
        {
            // Arrange
            var user = base.UserInApplicationsGroup;
            var requiredRole = base.Role_Applications;
            // Act
            var isAuthorized = DashboardAuthorization.IsAuthorized(user, requiredRole);
            // Assert
            isAuthorized.Should().Be(Yes);
        }

        [TestMethod]
        public void WhenRequiredRole_IsApplications_AndUser_IsInDevelopers_Should_BeAuthorized()
        {
            // Arrange
            var user = base.UserInDevelopersGroup;
            var requiredRole = base.Role_Applications;
            // Act
            var isAuthorized = DashboardAuthorization.IsAuthorized(user, requiredRole);
            // Assert
            isAuthorized.Should().Be(Yes);
        }

        [TestMethod]
        public void WhenRequiredRole_IsApplications_AndUser_IsInDevelopersButNotApplications_Should_BeAuthorized()
        {
            // Arrange
            var user = base.UserInDevelopersButNotApplicationsGroup;
            var requiredRole = base.Role_Applications;
            // Act
            var isAuthorized = DashboardAuthorization.IsAuthorized(user, requiredRole);
            // Assert
            isAuthorized.Should().Be(Yes);
        }

        [TestMethod]
        public void WhenRequiredRole_IsDeveloper_AndUser_IsNonIrma_Should_NotBeAuthorized()
        {
            // Arrange
            var user = base.UserNotInAnyGroup;
            var requiredRole = base.Role_Developer;
            // Act
            var isAuthorized = DashboardAuthorization.IsAuthorized(user, requiredRole);
            // Assert
            isAuthorized.Should().Be(No);
        }

        [TestMethod]
        public void WhenRequiredRole_IsDeveloper_AndUser_IsInApplications_Should_NotBeAuthorized()
        {
            // Arrange
            var user = base.UserInApplicationsGroup;
            var requiredRole = base.Role_Developer;
            // Act
            var isAuthorized = DashboardAuthorization.IsAuthorized(user, requiredRole);
            // Assert
            isAuthorized.Should().Be(No);
        }

        [TestMethod]
        public void WhenRequiredRole_IsDeveloper_AndUser_IsInDevelopers_Should_BeAuthorized()
        {
            // Arrange
            var user = base.UserInDevelopersGroup;
            var requiredRole = base.Role_Developer;
            // Act
            var isAuthorized = DashboardAuthorization.IsAuthorized(user, requiredRole);
            // Assert
            isAuthorized.Should().Be(Yes);
        }

        [TestMethod]
        public void WhenRequiredRole_IsDeveloper_AndUser_IsInDevelopersButNotApplications_Should_BeAuthorized()
        {
            // Arrange
            var user = base.UserInDevelopersButNotApplicationsGroup;
            var requiredRole = base.Role_Developer;
            // Act
            var isAuthorized = DashboardAuthorization.IsAuthorized(user, requiredRole);
            // Assert
            isAuthorized.Should().Be(Yes);
        }

        [TestMethod]
        public void WhenRequiredRole_IsNone_AndUser_IsNonIrma_Should_BeAuthorized()
        {
            // Arrange
            var user = base.UserNotInAnyGroup;
            var requiredRole = base.Role_Unauthorized;
            // Act
            var isAuthorized = DashboardAuthorization.IsAuthorized(user, requiredRole);
            // Assert
            isAuthorized.Should().Be(Yes);
        }

        [TestMethod]
        public void WhenRequiredRole_IsNone_AndUser_IsInApplications_Should_BeAuthorized()
        {
            // Arrange
            var user = base.UserInApplicationsGroup;
            var requiredRole = base.Role_Unauthorized;
            // Act
            var isAuthorized = DashboardAuthorization.IsAuthorized(user, requiredRole);
            // Assert
            isAuthorized.Should().Be(Yes);
        }

        [TestMethod]
        public void WhenRequiredRole_IsNone_AndUser_IsInDevelopers_Should_BeAuthorized()
        {
            // Arrange
            var user = base.UserInDevelopersGroup;
            var requiredRole = base.Role_Unauthorized;
            // Act
            var isAuthorized = DashboardAuthorization.IsAuthorized(user, requiredRole);
            // Assert
            isAuthorized.Should().Be(Yes);
        }

        [TestMethod]
        public void WhenRequiredRole_IsNone_AndUser_IsInDevelopersButNotApplications_Should_BeAuthorized()
        {
            // Arrange
            var user = base.UserInDevelopersButNotApplicationsGroup;
            var requiredRole = base.Role_Unauthorized;
            // Act
            var isAuthorized = DashboardAuthorization.IsAuthorized(user, requiredRole);
            // Assert
            isAuthorized.Should().Be(Yes);
        }
    }
}
