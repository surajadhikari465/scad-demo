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
        [TestMethod]
        public void IsAuthorized_WithStandardSecurityGroups_WhenRequiredLevelIsNoneAndUserIsNull_ShouldBeAuthorized()
        {
            // Arrange
            base.SetStandardSecurityGroups();
            var requiredAuthLevel = UserAuthorizationLevelEnum.None;
            var user = (IPrincipal)null;
            // Act
            var isAuthorized = DashboardAuthorization.IsAuthorized(user, requiredAuthLevel);
            // Assert
            isAuthorized.Should().Be(true);
        }

        [TestMethod]
        public void IsAuthorized_WithStandardSecurityGroups_WhenRequiredLevelIsReadOnlyAndUserNotInAnyGroups_ShouldNotBeAuthorized()
        {
            // Arrange
            base.SetStandardSecurityGroups();
            var requiredAuthLevel = UserAuthorizationLevelEnum.ReadOnly;
            user.Setup(u => u.IsInRole(It.IsAny<string>())).Returns(false);
            // Act
            var isAuthorized = DashboardAuthorization.IsAuthorized(user.Object, requiredAuthLevel);
            // Assert
            isAuthorized.Should().Be(false);
        }

        [TestMethod]
        public void IsAuthorized_WithStandardSecurityGroups_WhenRequiredLevelIsReadOnlyAndUserInIrmaApplications_ShouldBeAuthorized()
        {
            // Arrange
            base.SetStandardSecurityGroups();
            var requiredAuthLevel = UserAuthorizationLevelEnum.ReadOnly;
            user.Setup(u => u.IsInRole(IRMAApplicationsRole)).Returns(true);
            // Act
            var isAuthorized = DashboardAuthorization.IsAuthorized(user.Object, requiredAuthLevel);
            // Assert
            isAuthorized.Should().Be(true);
        }

        [TestMethod]
        public void IsAuthorized_WithStandardSecurityGroups_WhenRequiredLevelIsReadOnlyAndUserInIrmaDevelopers_ShouldBeAuthorized()
        {
            // Arrange
            base.SetStandardSecurityGroups();
            var requiredAuthLevel = UserAuthorizationLevelEnum.ReadOnly;
            user.Setup(u => u.IsInRole(IRMADevelopersRole)).Returns(true);
            // Act
            var isAuthorized = DashboardAuthorization.IsAuthorized(user.Object, requiredAuthLevel);
            // Assert
            isAuthorized.Should().Be(true);
        }

        [TestMethod]
        public void IsAuthorized_WithStandardSecurityGroups_WhenRequiredLevelIsPrivilegedAndUserInNoGroup_ShouldNotBeAuthorized()
        {
            // Arrange
            base.SetStandardSecurityGroups();
            var requiredAuthLevel = UserAuthorizationLevelEnum.EditingPrivileges;
            // Act
            var isAuthorized = DashboardAuthorization.IsAuthorized(user.Object, requiredAuthLevel);
            // Assert
            isAuthorized.Should().Be(false);
        }

        [TestMethod]
        public void IsAuthorized_WithStandardSecurityGroups_WhenRequiredLevelIsPrivilegedAndUserInIrmaApplications_ShouldNotBeAuthorized()
        {
            // Arrange
            base.SetStandardSecurityGroups();
            var requiredAuthLevel = UserAuthorizationLevelEnum.EditingPrivileges;
            user.Setup(u => u.IsInRole(IRMAApplicationsRole)).Returns(true);
            // Act
            var isAuthorized = DashboardAuthorization.IsAuthorized(user.Object, requiredAuthLevel);
            // Assert
            isAuthorized.Should().Be(false);
        }

        [TestMethod]
        public void IsAuthorized_WithStandardSecurityGroups_WhenRequiredLevelIsPrivilegedAndUserInIrmaDevelopers_ShouldBeAuthorized()
        {
            // Arrange
            base.SetStandardSecurityGroups();
            var requiredAuthLevel = UserAuthorizationLevelEnum.EditingPrivileges;
            user.Setup(u => u.IsInRole(IRMADevelopersRole)).Returns(true);
            // Act
            var isAuthorized = DashboardAuthorization.IsAuthorized(user.Object, requiredAuthLevel);
            // Assert
            isAuthorized.Should().Be(true);
        }

        [TestMethod]
        public void IsAuthorized_WithStandardSecurityGroups_WhenRequiredLevelIsPrivilegedAndUserIsInIrmaDevelopersAndApplications_ShouldBeAuthorized()
        {
            // Arrange
            base.SetStandardSecurityGroups();
            var requiredAuthLevel = UserAuthorizationLevelEnum.ReadOnly;
            user.Setup(u => u.IsInRole(IRMAApplicationsRole)).Returns(true);
            user.Setup(u => u.IsInRole(IRMADevelopersRole)).Returns(true);
            // Act
            var isAuthorized = DashboardAuthorization.IsAuthorized(user.Object, requiredAuthLevel);
            // Assert
            isAuthorized.Should().Be(true);
        }

        [TestMethod]
        public void IsAuthorized_WithStandardSecurityGroups_WhenRequiredLevelIsNoneAndUserNotInAnySecurityGroups_ShouldBeAuthorized()
        {
            // Arrange
            base.SetStandardSecurityGroups();
            var requiredAuthLevel = UserAuthorizationLevelEnum.None;
            user.Setup(u => u.IsInRole(IRMAApplicationsRole)).Returns(false);
            user.Setup(u => u.IsInRole(IRMADevelopersRole)).Returns(false);
            user.Setup(u => u.IsInRole(IRMASupportRole)).Returns(false);
            // Act
            var isAuthorized = DashboardAuthorization.IsAuthorized(user.Object, requiredAuthLevel);
            // Assert
            isAuthorized.Should().Be(true);
        }

        [TestMethod]
        public void IsAuthorized_WithStandardSecurityGroups_WhenRequiredLevelIsNoneAndUseInIrmaApplications_ShouldBeAuthorized()
        {
            // Arrange
            base.SetStandardSecurityGroups();
            var requiredAuthLevel = UserAuthorizationLevelEnum.None;
            user.Setup(u => u.IsInRole(IRMAApplicationsRole)).Returns(true);
            // Act
            var isAuthorized = DashboardAuthorization.IsAuthorized(user.Object, requiredAuthLevel);
            // Assert
            isAuthorized.Should().Be(true);
        }

        [TestMethod]
        public void IsAuthorized_WithStandardSecurityGroups_WhenRequiredLevelIsNoneAndUseInIrmaDevelopers_ShouldBeAuthorized()
        {
            // Arrange
            base.SetStandardSecurityGroups();
            var requiredAuthLevel = UserAuthorizationLevelEnum.None;
            user.Setup(u => u.IsInRole(IRMADevelopersRole)).Returns(true);
            // Act
            var isAuthorized = DashboardAuthorization.IsAuthorized(user.Object, requiredAuthLevel);
            // Assert
            isAuthorized.Should().Be(true);
        }

        [TestMethod]
        public void IsAuthorized_WithStandardSecurityGroups_WhenRequiredLevelIsNoneAndUseInIrmaSupport_ShouldBeAuthorized()
        {
            // Arrange
            base.SetStandardSecurityGroups();
            var requiredAuthLevel = UserAuthorizationLevelEnum.None;
            user.Setup(u => u.IsInRole(IRMASupportRole)).Returns(true);
            // Act
            var isAuthorized = DashboardAuthorization.IsAuthorized(user.Object, requiredAuthLevel);
            // Assert
            isAuthorized.Should().Be(true);
        }

        [TestMethod]
        public void IsAuthorized_WithStandardSecurityGroups_WhenRequiredLevelIsReadOnlyAndUseInIrmaSupport_ShouldBeAuthorized()
        {
            // Arrange
            base.SetStandardSecurityGroups();
            var requiredAuthLevel = UserAuthorizationLevelEnum.ReadOnly;
            user.Setup(u => u.IsInRole(IRMASupportRole)).Returns(true);
            // Act
            var isAuthorized = DashboardAuthorization.IsAuthorized(user.Object, requiredAuthLevel);
            // Assert
            isAuthorized.Should().Be(true);
        }

        [TestMethod]
        public void IsAuthorized_WithStandardSecurityGroups_WhenRequiredLevelIsPrivilegedAndUseInIrmaSupport_ShouldNotBeAuthorized()
        {
            // Arrange
            base.SetStandardSecurityGroups();
            var requiredAuthLevel = UserAuthorizationLevelEnum.EditingPrivileges;
            user.Setup(u => u.IsInRole(IRMASupportRole)).Returns(true);
            // Act
            var isAuthorized = DashboardAuthorization.IsAuthorized(user.Object, requiredAuthLevel);
            // Assert
            isAuthorized.Should().Be(false);
        }

        [TestMethod]
        public void IsAuthorized_WithStandardSecurityGroups_WhenRequiredLevelIsPrivilegedAndUseInIrmaSupportAndIrmaApplications_ShouldNotBeAuthorized()
        {
            // Arrange
            base.SetStandardSecurityGroups();
            var requiredAuthLevel = UserAuthorizationLevelEnum.EditingPrivileges;
            user.Setup(u => u.IsInRole(IRMAApplicationsRole)).Returns(true);
            user.Setup(u => u.IsInRole(IRMASupportRole)).Returns(true);
            // Act
            var isAuthorized = DashboardAuthorization.IsAuthorized(user.Object, requiredAuthLevel);
            // Assert
            isAuthorized.Should().Be(false);
        }

        [TestMethod]
        public void IsAuthorized_WithStandardSecurityGroups_WhenRequiredLevelIsPrivilegedAndUseInIrmaSupportAndIrmaApplicationsAndIrmaDev_ShouldBeAuthorized()
        {
            // Arrange
            base.SetStandardSecurityGroups();
            var requiredAuthLevel = UserAuthorizationLevelEnum.EditingPrivileges;
            user.Setup(u => u.IsInRole(IRMAApplicationsRole)).Returns(true);
            user.Setup(u => u.IsInRole(IRMASupportRole)).Returns(true);
            user.Setup(u => u.IsInRole(IRMADevelopersRole)).Returns(true);
            // Act
            var isAuthorized = DashboardAuthorization.IsAuthorized(user.Object, requiredAuthLevel);
            // Assert
            isAuthorized.Should().Be(true);
        }


        [TestMethod]
        public void IsAuthorized_WithStandardSecurityGroups_WhenRequiredLevelIsReadOnlyAndUseInIrmaSupportAndIrmaApplicationsAndIrmaDev_ShouldBeAuthorized()
        {
            // Arrange
            base.SetStandardSecurityGroups();
            var requiredAuthLevel = UserAuthorizationLevelEnum.ReadOnly;
            user.Setup(u => u.IsInRole(IRMAApplicationsRole)).Returns(true);
            user.Setup(u => u.IsInRole(IRMASupportRole)).Returns(true);
            user.Setup(u => u.IsInRole(IRMADevelopersRole)).Returns(true);
            // Act
            var isAuthorized = DashboardAuthorization.IsAuthorized(user.Object, requiredAuthLevel);
            // Assert
            isAuthorized.Should().Be(true);
        }

        [TestMethod]
        public void IsAuthorized_WithNullSecurityGroupsInfo_WhenRequiredLevelIsReadOnlyAndUserInAllGruops_ShouldNotBeAuthorized()
        {
            // Arrange
            // simulate no security group entries in config file
            DashboardAuthorization.ReadOnlyGroups = null;
            DashboardAuthorization.PrivilegedGroups = null;
            var requiredAuthLevel = UserAuthorizationLevelEnum.ReadOnly;
            user.Setup(u => u.IsInRole(IRMAApplicationsRole)).Returns(true);
            user.Setup(u => u.IsInRole(IRMADevelopersRole)).Returns(true);
            user.Setup(u => u.IsInRole(IRMASupportRole)).Returns(true);
            // Act
            var isAuthorized = DashboardAuthorization.IsAuthorized(user.Object, requiredAuthLevel);
            // Assert
            isAuthorized.Should().Be(false);
        }

        [TestMethod]
        public void IsAuthorized_WithNoSecurityGroupsInfo_WhenRequiredLevelIsReadOnlyAndUserInAllGruops_ShouldNotBeAuthorized()
        {
            // Arrange
            // simulate no security group entries in config file
            DashboardAuthorization.ReadOnlyGroups = "";
            DashboardAuthorization.PrivilegedGroups = "";
            var requiredAuthLevel = UserAuthorizationLevelEnum.ReadOnly;
            user.Setup(u => u.IsInRole(IRMAApplicationsRole)).Returns(true);
            user.Setup(u => u.IsInRole(IRMADevelopersRole)).Returns(true);
            user.Setup(u => u.IsInRole(IRMASupportRole)).Returns(true);
            // Act
            var isAuthorized = DashboardAuthorization.IsAuthorized(user.Object, requiredAuthLevel);
            // Assert
            isAuthorized.Should().Be(false);
        }

        [TestMethod]
        public void IsAuthorized_WithNoPrivilegedSecurityGroup_WhenRequiredLevelIsReadOnlyAndUserInAllGroups_ShouldBeAuthorized()
        {
            // Arrange
            DashboardAuthorization.ReadOnlyGroups = $"{IRMAApplicationsRole},{IRMASupportRole}";
            DashboardAuthorization.PrivilegedGroups = "";
            var requiredAuthLevel = UserAuthorizationLevelEnum.ReadOnly;
            user.Setup(u => u.IsInRole(IRMAApplicationsRole)).Returns(true);
            user.Setup(u => u.IsInRole(IRMADevelopersRole)).Returns(true);
            user.Setup(u => u.IsInRole(IRMASupportRole)).Returns(true);
            // Act
            var isAuthorized = DashboardAuthorization.IsAuthorized(user.Object, requiredAuthLevel);
            // Assert
            isAuthorized.Should().Be(true);
        }


        [TestMethod]
        public void GetAuthorizationLevel_WithNoReadOnlySecurityGroup_WhenRequiredLevelIsPriviligedAndUserInIrmaDevelopers_ShouldBeAuthorized()
        {
            // Arrange
            DashboardAuthorization.ReadOnlyGroups = "";
            DashboardAuthorization.PrivilegedGroups = $"{IRMADevelopersRole}";
            var requiredAuthLevel = UserAuthorizationLevelEnum.ReadOnly;
            user.Setup(u => u.IsInRole(IRMADevelopersRole)).Returns(true);
            // Act
            var isAuthorized = DashboardAuthorization.IsAuthorized(user.Object, requiredAuthLevel);
            // Assert
            isAuthorized.Should().Be(true);
        }

        [TestMethod]
        public void GetAuthorizationLevel_WithNoPrivilegedSecurityGroup_WhenRequiredLevelIsPriviligedAndUserInIrmaDevelopers_ShouldNotBeAuthorized()
        {
            // Arrange
            DashboardAuthorization.ReadOnlyGroups = $"{IRMAApplicationsRole},{IRMASupportRole}";
            DashboardAuthorization.PrivilegedGroups = "";
            var requiredAuthLevel = UserAuthorizationLevelEnum.EditingPrivileges;
            user.Setup(u => u.IsInRole(IRMAApplicationsRole)).Returns(true);
            user.Setup(u => u.IsInRole(IRMADevelopersRole)).Returns(true);
            user.Setup(u => u.IsInRole(IRMASupportRole)).Returns(true);
            // Act
            var isAuthorized = DashboardAuthorization.IsAuthorized(user.Object, requiredAuthLevel);
            // Assert
            isAuthorized.Should().Be(false);
        }
    }
}
