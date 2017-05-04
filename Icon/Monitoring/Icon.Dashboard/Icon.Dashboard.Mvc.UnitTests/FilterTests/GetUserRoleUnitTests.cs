using FluentAssertions;
using Icon.Dashboard.Mvc.Filters;
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
    public  class GetUserRoleUnitTests : _DashboardAuthorizationFilterTestBase
    {
        [TestMethod]
        public void WhenUserIs_Null_Role_Should_BeUnauthorized()
        {
            // Arrange
            var user = base.UserNull;
            // Act
            var role = DashboardAuthorization.GetUserRole(user);
            // Assert
            role.Should().Be(base.Role_Unauthorized);
        }

        [TestMethod]
        public void WhenUserIs_NotInAnyGroup_Role_Should_BeUnauthorized()
        {
            // Arrange
            var user = base.UserNotInAnyGroup;
            // Act
            var role = DashboardAuthorization.GetUserRole(user);
            // Assert
            role.Should().Be(base.Role_Unauthorized);
        }

        [TestMethod]
        public void WhenUserIs_InApplications_Role_Should_BeApplications()
        {
            // Arrange
            var user = base.UserInApplicationsGroup;
            // Act
            var role = DashboardAuthorization.GetUserRole(user);
            // Assert
            role.Should().Be(base.Role_Applications);
        }

        [TestMethod]
        public void WhenUserIs_InDevelopers_Role_Should_BeDeveloper()
        {
            // Arrange
            var user = base.UserInDevelopersGroup;
            // Act
            var role = DashboardAuthorization.GetUserRole(user);
            // Assert
            role.Should().Be(base.Role_Developer);
        }

        [TestMethod]
        public void WhenUserIs_InDevelopersButNotApplications_Role_Should_BeDeveloper()
        {
            // Arrange
            var user = base.UserInDevelopersButNotApplicationsGroup;
            // Act
            var role = DashboardAuthorization.GetUserRole(user);
            // Assert
            role.Should().Be(base.Role_Developer);
        }
    }
}
