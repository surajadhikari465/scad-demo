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
    public  class GetUserRoleUnitTests : _AuthorizationAttributeUnitTestBase
    {
        [TestMethod]
        public void WhenUserIs_Null_Role_Should_BeUnauthorized()
        {
            // Arrange
            // Act
            var role = DashboardAuthorization.GetUserRole(null);
            // Assert
            role.Should().Be(UserRoleEnum.Unauthorized);
        }

        [TestMethod]
        public void WhenUserIs_NotInAnyGroup_Role_Should_BeUnauthorized()
        {
            // Arrange
            var user = base.JoeSchmo;
            // Act
            var role = DashboardAuthorization.GetUserRole(user);
            // Assert
            role.Should().Be(UserRoleEnum.Unauthorized);
        }

        [TestMethod]
        public void WhenUserIs_InApplications_Role_Should_BeApplications()
        {
            // Arrange
            var user = base.SallyTheBA;
            // Act
            var role = DashboardAuthorization.GetUserRole(user);
            // Assert
            role.Should().Be(UserRoleEnum.ReadOnly);
        }

        [TestMethod]
        public void WhenUserIs_InDevelopers_Role_Should_BeDeveloper()
        {
            // Arrange
            var user = base.DeveloperBob;
            // Act
            var role = DashboardAuthorization.GetUserRole(user);
            // Assert
            role.Should().Be(UserRoleEnum.EditingPrivileges);
        }

        [TestMethod]
        public void WhenUserIs_InDevelopersButNotApplications_Role_Should_BeDeveloper()
        {
            // Arrange
            var user = base.UserInDevelopersButNotApplicationsGroup;
            // Act
            var role = DashboardAuthorization.GetUserRole(user);
            // Assert
            role.Should().Be(UserRoleEnum.EditingPrivileges);
        }
    }
}
