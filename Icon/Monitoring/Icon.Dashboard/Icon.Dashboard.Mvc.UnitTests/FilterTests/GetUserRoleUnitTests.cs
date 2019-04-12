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
    public  class GetUserRoleUnitTests : _DashboardAuthorizationFilterTestBase
    {
        [TestMethod]
        public void GetAuthorizationLevel_WithStandardSecurityGroups_WhenUserNull_ShouldHaveAuthLevelNone()
        {
            // Arrange
            base.SetStandardSecurityGroups();
            var nullUser = (IPrincipal)null;
            // Act
            var authLevel = DashboardAuthorization.GetAuthorizationLevel(nullUser);
            // Assert
            authLevel.Should().Be(UserAuthorizationLevelEnum.None);
        }

        [TestMethod]
        public void GetAuthorizationLevel_WithStandardSecurityGroupsl_WhenUserInNoGroups_ShouldHaveAuthLevelNone()
        {
            // Arrange
            base.SetStandardSecurityGroups();
            user.Setup(u => u.IsInRole(It.IsAny<string>())).Returns(false);
            // Act
            var authLevel = DashboardAuthorization.GetAuthorizationLevel(user.Object);
            // Assert
            authLevel.Should().Be(UserAuthorizationLevelEnum.None);
        }

        [TestMethod]
        public void GetAuthorizationLevel_WithStandardSecurityGroups_WhenUserInIrmaApplications_ShouldHaveAuthLevelReadOnly()
        {
            // Arrange
            base.SetStandardSecurityGroups();
            user.Setup(u => u.IsInRole(IRMAApplicationsRole)).Returns(true);
            // Act
            var authLevel = DashboardAuthorization.GetAuthorizationLevel(user.Object);
            // Assert
            authLevel.Should().Be(UserAuthorizationLevelEnum.ReadOnly);
        }

        [TestMethod]
        public void GetAuthorizationLevel_WithStandardSecurityGroups_WhenUserInIrmaDevelopersAndIrmaApplications_ShouldHaveAuthLevelPrivileged()
        {
            // Arrange
            base.SetStandardSecurityGroups();
            user.Setup(u => u.IsInRole(IRMAApplicationsRole)).Returns(true);
            user.Setup(u => u.IsInRole(IRMADevelopersRole)).Returns(true);
            user.Setup(u => u.IsInRole(IRMASupportRole)).Returns(false);
            // Act
            var authLevel = DashboardAuthorization.GetAuthorizationLevel(user.Object);
            // Assert
            authLevel.Should().Be(UserAuthorizationLevelEnum.EditingPrivileges);
        }

        [TestMethod]
        public void GetAuthorizationLevel_WithStandardSecurityGroups_WhenUserInIrmaDevelopersRole_ShouldHaveAuthLevelPrivileged()
        {
            // Arrange
            base.SetStandardSecurityGroups();
            user.Setup(u => u.IsInRole(IRMAApplicationsRole)).Returns(false);
            user.Setup(u => u.IsInRole(IRMADevelopersRole)).Returns(true);
            user.Setup(u => u.IsInRole(IRMASupportRole)).Returns(false);
            // Act
            var authLevel = DashboardAuthorization.GetAuthorizationLevel(user.Object);
            // Assert
            authLevel.Should().Be(UserAuthorizationLevelEnum.EditingPrivileges);
        }

        [TestMethod]
        public void GetAuthorizationLevel_WithStandardSecurityGroups_WhenUserInIrmaSupport_ShouldHaveAuthLevelReadOnly()
        {
            // Arrange
            base.SetStandardSecurityGroups();
            user.Setup(u => u.IsInRole(IRMAApplicationsRole)).Returns(false);
            user.Setup(u => u.IsInRole(IRMADevelopersRole)).Returns(false);
            user.Setup(u => u.IsInRole(IRMASupportRole)).Returns(true);
            // Act
            var authLevel = DashboardAuthorization.GetAuthorizationLevel(user.Object);
            // Assert
            authLevel.Should().Be(UserAuthorizationLevelEnum.ReadOnly);
        }

        [TestMethod]
        public void GetAuthorizationLevel_WithStandardSecurityGroups_WhenUserInIrmaSupportAndIrmaApplications_ShouldHaveAuthLevelReadOnly()
        {
            // Arrange
            base.SetStandardSecurityGroups();
            user.Setup(u => u.IsInRole(IRMAApplicationsRole)).Returns(true);
            user.Setup(u => u.IsInRole(IRMADevelopersRole)).Returns(false);
            user.Setup(u => u.IsInRole(IRMASupportRole)).Returns(true);
            // Act
            var authLevel = DashboardAuthorization.GetAuthorizationLevel(user.Object);
            // Assert
            authLevel.Should().Be(UserAuthorizationLevelEnum.ReadOnly);
        }


        [TestMethod]
        public void GetAuthorizationLevel_WithStandardSecurityGroups_WhenUserInIrmaSupportAndIrmaDevelopers_ShouldHaveAuthLevelPrivileged()
        {
            // Arrange
            base.SetStandardSecurityGroups();
            user.Setup(u => u.IsInRole(IRMAApplicationsRole)).Returns(true);
            user.Setup(u => u.IsInRole(IRMADevelopersRole)).Returns(true);
            user.Setup(u => u.IsInRole(IRMASupportRole)).Returns(true);
            // Act
            var authLevel = DashboardAuthorization.GetAuthorizationLevel(user.Object);
            // Assert
            authLevel.Should().Be(UserAuthorizationLevelEnum.EditingPrivileges);
        }

        [TestMethod]
        public void GetAuthorizationLevel_WithNullSecurityGroupsInfo_ShouldHaveAuthLevelNone()
        {
            // Arrange
            // simulate no security group entries in config file
            DashboardAuthorization.ReadOnlyGroups = null;
            DashboardAuthorization.PrivilegedGroups = null;
            user.Setup(u => u.IsInRole(IRMAApplicationsRole)).Returns(true);
            user.Setup(u => u.IsInRole(IRMADevelopersRole)).Returns(true);
            user.Setup(u => u.IsInRole(IRMASupportRole)).Returns(true);
            // Act
            var authLevel = DashboardAuthorization.GetAuthorizationLevel(user.Object);
            // Assert
            authLevel.Should().Be(UserAuthorizationLevelEnum.None);
        }

        [TestMethod]
        public void GetAuthorizationLevel_WithNoSecurityGroupsInfo_ShouldHaveAuthLevelNone()
        {
            // Arrange
            // simulate no security group entries in config file
            DashboardAuthorization.ReadOnlyGroups = "";
            DashboardAuthorization.PrivilegedGroups = "";
            user.Setup(u => u.IsInRole(IRMAApplicationsRole)).Returns(true);
            user.Setup(u => u.IsInRole(IRMADevelopersRole)).Returns(true);
            user.Setup(u => u.IsInRole(IRMASupportRole)).Returns(true);
            // Act
            var authLevel = DashboardAuthorization.GetAuthorizationLevel(user.Object);
            // Assert
            authLevel.Should().Be(UserAuthorizationLevelEnum.None);
        }

        [TestMethod]
        public void GetAuthorizationLevel_WithSingleEntryInReadOnlyAndPrivilegedSecurityGroups_WhenUserInIrmaApplications_ShouldHaveAuthLevelReadOnly()
        {
            // Arrange
            DashboardAuthorization.ReadOnlyGroups = $"{IRMAApplicationsRole}";
            DashboardAuthorization.PrivilegedGroups = $"{IRMADevelopersRole}"; 
            user.Setup(u => u.IsInRole(IRMAApplicationsRole)).Returns(true);
            user.Setup(u => u.IsInRole(IRMADevelopersRole)).Returns(false);
            // Act
            var authLevel = DashboardAuthorization.GetAuthorizationLevel(user.Object);
            // Assert
            authLevel.Should().Be(UserAuthorizationLevelEnum.ReadOnly);
        }

        [TestMethod]
        public void GetAuthorizationLevel_WithSingleEntryInReadOnlyAndPrivilegedSecurityGroups_WhenUserInIrmaDevelopers_ShouldHaveAuthLevelReadOnly()
        {
            // Arrange
            DashboardAuthorization.ReadOnlyGroups = $"{IRMAApplicationsRole}";
            DashboardAuthorization.PrivilegedGroups = $"{IRMADevelopersRole}";
            user.Setup(u => u.IsInRole(IRMAApplicationsRole)).Returns(true);
            user.Setup(u => u.IsInRole(IRMADevelopersRole)).Returns(true);
            // Act
            var authLevel = DashboardAuthorization.GetAuthorizationLevel(user.Object);
            // Assert
            authLevel.Should().Be(UserAuthorizationLevelEnum.EditingPrivileges);
        }

        [TestMethod]
        public void GetAuthorizationLevel_WithNoPrivilegedSecurityGroup_WhenUserInIrmaApplicationsAndIrmaDevelopers_ShouldHaveAuthLevelReadOnly()
        {
            // Arrange
            DashboardAuthorization.ReadOnlyGroups = $"{IRMAApplicationsRole},{IRMASupportRole}";
            DashboardAuthorization.PrivilegedGroups = "";
            user.Setup(u => u.IsInRole(IRMAApplicationsRole)).Returns(true);
            user.Setup(u => u.IsInRole(IRMADevelopersRole)).Returns(true);
            // Act
            var authLevel = DashboardAuthorization.GetAuthorizationLevel(user.Object);
            // Assert
            authLevel.Should().Be(UserAuthorizationLevelEnum.ReadOnly);
        }

        [TestMethod]
        public void GetAuthorizationLevel_WithNoReadOnlySecurityGroup_WhenUserInIrmaDevelopers_ShouldHaveAuthLevelPrivileged()
        {
            // Arrange
            DashboardAuthorization.ReadOnlyGroups = "";
            DashboardAuthorization.PrivilegedGroups = $"{IRMADevelopersRole}"; 
            user.Setup(u => u.IsInRole(IRMADevelopersRole)).Returns(true);
            // Act
            var authLevel = DashboardAuthorization.GetAuthorizationLevel(user.Object);
            // Assert
            authLevel.Should().Be(UserAuthorizationLevelEnum.EditingPrivileges);
        }

        [TestMethod]
        public void GetAuthorizationLevel_WithNoReadOnlySecurityGroup_WhenUserInIrmaApplications_ShouldHaveAuthLevelNone()
        {
            // Arrange
            DashboardAuthorization.ReadOnlyGroups = "";
            DashboardAuthorization.PrivilegedGroups = $"{IRMADevelopersRole}";
            user.Setup(u => u.IsInRole(IRMAApplicationsRole)).Returns(true);
            // Act
            var authLevel = DashboardAuthorization.GetAuthorizationLevel(user.Object);
            // Assert
            authLevel.Should().Be(UserAuthorizationLevelEnum.None);
        }
    }
}
