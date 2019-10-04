using Icon.Dashboard.Mvc.Enums;
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
            var authorizer = new DashboardAuthorizer(base.StandardReadOnlyGroups, base.StandardPrivilegedGroups);
            authorizer.RequiredRole = UserAuthorizationLevelEnum.None;
            var nullUser = (IPrincipal)null;
            // Act
            var isAuthorized = authorizer.IsAuthorized(nullUser, authorizer.RequiredRole);
            // Assert
            Assert.IsTrue(isAuthorized);
        }

        [TestMethod]
        public void IsAuthorized_WithStandardSecurityGroups_WhenRequiredLevelIsReadOnlyAndUserNotInAnyGroups_ShouldNotBeAuthorized()
        {
            // Arrange
            var authorizer = new DashboardAuthorizer(base.StandardReadOnlyGroups, base.StandardPrivilegedGroups);
            authorizer.RequiredRole = UserAuthorizationLevelEnum.ReadOnly;
            user.Setup(u => u.IsInRole(It.IsAny<string>())).Returns(false);
            // Act
            var isAuthorized = authorizer.IsAuthorized(user.Object, authorizer.RequiredRole );
            // Assert
            Assert.IsFalse(isAuthorized);
        }

        [TestMethod]
        public void IsAuthorized_WithStandardSecurityGroups_WhenRequiredLevelIsReadOnlyAndUserInIrmaApplications_ShouldBeAuthorized()
        {
            // Arrange
            var authorizer = new DashboardAuthorizer(base.StandardReadOnlyGroups, base.StandardPrivilegedGroups);
            authorizer.RequiredRole = UserAuthorizationLevelEnum.ReadOnly;
            user.Setup(u => u.IsInRole(IRMAApplicationsRole)).Returns(true);
            // Act
            var isAuthorized = authorizer.IsAuthorized(user.Object, authorizer.RequiredRole );
            // Assert
            Assert.IsTrue(isAuthorized);
        }

        [TestMethod]
        public void IsAuthorized_WithStandardSecurityGroups_WhenRequiredLevelIsReadOnlyAndUserInIrmaDevelopers_ShouldBeAuthorized()
        {
            // Arrange
            var authorizer = new DashboardAuthorizer(base.StandardReadOnlyGroups, base.StandardPrivilegedGroups);
            authorizer.RequiredRole = UserAuthorizationLevelEnum.ReadOnly;
            user.Setup(u => u.IsInRole(IRMADevelopersRole)).Returns(true);
            // Act
            var isAuthorized = authorizer.IsAuthorized(user.Object, authorizer.RequiredRole );
            // Assert
            Assert.IsTrue(isAuthorized);
        }

        [TestMethod]
        public void IsAuthorized_WithStandardSecurityGroups_WhenRequiredLevelIsPrivilegedAndUserInNoGroup_ShouldNotBeAuthorized()
        {
            // Arrange
            var authorizer = new DashboardAuthorizer(base.StandardReadOnlyGroups, base.StandardPrivilegedGroups);
            authorizer.RequiredRole = UserAuthorizationLevelEnum.EditingPrivileges;
            // Act
            var isAuthorized = authorizer.IsAuthorized(user.Object, authorizer.RequiredRole );
            // Assert
            Assert.IsFalse(isAuthorized);
        }

        [TestMethod]
        public void IsAuthorized_WithStandardSecurityGroups_WhenRequiredLevelIsPrivilegedAndUserInIrmaApplications_ShouldNotBeAuthorized()
        {
            // Arrange
            var authorizer = new DashboardAuthorizer(base.StandardReadOnlyGroups, base.StandardPrivilegedGroups);
            authorizer.RequiredRole = UserAuthorizationLevelEnum.EditingPrivileges;
            user.Setup(u => u.IsInRole(IRMAApplicationsRole)).Returns(true);
            // Act
            var isAuthorized = authorizer.IsAuthorized(user.Object, authorizer.RequiredRole );
            // Assert
            Assert.IsFalse(isAuthorized);
        }

        [TestMethod]
        public void IsAuthorized_WithStandardSecurityGroups_WhenRequiredLevelIsPrivilegedAndUserInIrmaDevelopers_ShouldBeAuthorized()
        {
            // Arrange
            var authorizer = new DashboardAuthorizer(base.StandardReadOnlyGroups, base.StandardPrivilegedGroups);
            authorizer.RequiredRole = UserAuthorizationLevelEnum.EditingPrivileges;
            user.Setup(u => u.IsInRole(IRMADevelopersRole)).Returns(true);
            // Act
            var isAuthorized = authorizer.IsAuthorized(user.Object, authorizer.RequiredRole );
            // Assert
            Assert.IsTrue(isAuthorized);
        }

        [TestMethod]
        public void IsAuthorized_WithStandardSecurityGroups_WhenRequiredLevelIsPrivilegedAndUserIsInIrmaDevelopersAndApplications_ShouldBeAuthorized()
        {
            // Arrange
            var authorizer = new DashboardAuthorizer(base.StandardReadOnlyGroups, base.StandardPrivilegedGroups);
            authorizer.RequiredRole = UserAuthorizationLevelEnum.ReadOnly;
            user.Setup(u => u.IsInRole(IRMAApplicationsRole)).Returns(true);
            user.Setup(u => u.IsInRole(IRMADevelopersRole)).Returns(true);
            // Act
            var isAuthorized = authorizer.IsAuthorized(user.Object, authorizer.RequiredRole );
            // Assert
            Assert.IsTrue(isAuthorized);
        }

        [TestMethod]
        public void IsAuthorized_WithStandardSecurityGroups_WhenRequiredLevelIsNoneAndUserNotInAnySecurityGroups_ShouldBeAuthorized()
        {
            // Arrange
            var authorizer = new DashboardAuthorizer(base.StandardReadOnlyGroups, base.StandardPrivilegedGroups);
            authorizer.RequiredRole = UserAuthorizationLevelEnum.None;
            user.Setup(u => u.IsInRole(IRMAApplicationsRole)).Returns(false);
            user.Setup(u => u.IsInRole(IRMADevelopersRole)).Returns(false);
            user.Setup(u => u.IsInRole(IRMASupportRole)).Returns(false);
            // Act
            var isAuthorized = authorizer.IsAuthorized(user.Object, authorizer.RequiredRole );
            // Assert
            Assert.IsTrue(isAuthorized);
        }

        [TestMethod]
        public void IsAuthorized_WithStandardSecurityGroups_WhenRequiredLevelIsNoneAndUseInIrmaApplications_ShouldBeAuthorized()
        {
            // Arrange
            var authorizer = new DashboardAuthorizer(base.StandardReadOnlyGroups, base.StandardPrivilegedGroups);
            authorizer.RequiredRole = UserAuthorizationLevelEnum.None;
            user.Setup(u => u.IsInRole(IRMAApplicationsRole)).Returns(true);
            // Act
            var isAuthorized = authorizer.IsAuthorized(user.Object, authorizer.RequiredRole );
            // Assert
            Assert.IsTrue(isAuthorized);
        }

        [TestMethod]
        public void IsAuthorized_WithStandardSecurityGroups_WhenRequiredLevelIsNoneAndUseInIrmaDevelopers_ShouldBeAuthorized()
        {
            // Arrange
            var authorizer = new DashboardAuthorizer(base.StandardReadOnlyGroups, base.StandardPrivilegedGroups);
            authorizer.RequiredRole = UserAuthorizationLevelEnum.None;
            user.Setup(u => u.IsInRole(IRMADevelopersRole)).Returns(true);
            // Act
            var isAuthorized = authorizer.IsAuthorized(user.Object, authorizer.RequiredRole );
            // Assert
            Assert.IsTrue(isAuthorized);
        }

        [TestMethod]
        public void IsAuthorized_WithStandardSecurityGroups_WhenRequiredLevelIsNoneAndUseInIrmaSupport_ShouldBeAuthorized()
        {
            // Arrange
            var authorizer = new DashboardAuthorizer(base.StandardReadOnlyGroups, base.StandardPrivilegedGroups);
            authorizer.RequiredRole = UserAuthorizationLevelEnum.None;
            user.Setup(u => u.IsInRole(IRMASupportRole)).Returns(true);
            // Act
            var isAuthorized = authorizer.IsAuthorized(user.Object, authorizer.RequiredRole );
            // Assert
            Assert.IsTrue(isAuthorized);
        }

        [TestMethod]
        public void IsAuthorized_WithStandardSecurityGroups_WhenRequiredLevelIsReadOnlyAndUseInIrmaSupport_ShouldBeAuthorized()
        {
            // Arrange
            var authorizer = new DashboardAuthorizer(base.StandardReadOnlyGroups, base.StandardPrivilegedGroups);
            authorizer.RequiredRole = UserAuthorizationLevelEnum.ReadOnly;
            user.Setup(u => u.IsInRole(IRMASupportRole)).Returns(true);
            // Act
            var isAuthorized = authorizer.IsAuthorized(user.Object, authorizer.RequiredRole );
            // Assert
            Assert.IsTrue(isAuthorized);
        }

        [TestMethod]
        public void IsAuthorized_WithStandardSecurityGroups_WhenRequiredLevelIsPrivilegedAndUseInIrmaSupport_ShouldNotBeAuthorized()
        {
            // Arrange
            var authorizer = new DashboardAuthorizer(base.StandardReadOnlyGroups, base.StandardPrivilegedGroups);
            authorizer.RequiredRole = UserAuthorizationLevelEnum.EditingPrivileges;
            user.Setup(u => u.IsInRole(IRMASupportRole)).Returns(true);
            // Act
            var isAuthorized = authorizer.IsAuthorized(user.Object, authorizer.RequiredRole );
            // Assert
            Assert.IsFalse(isAuthorized);
        }

        [TestMethod]
        public void IsAuthorized_WithStandardSecurityGroups_WhenRequiredLevelIsPrivilegedAndUseInIrmaSupportAndIrmaApplications_ShouldNotBeAuthorized()
        {
            // Arrange
            var authorizer = new DashboardAuthorizer(base.StandardReadOnlyGroups, base.StandardPrivilegedGroups);
            authorizer.RequiredRole = UserAuthorizationLevelEnum.EditingPrivileges;
            user.Setup(u => u.IsInRole(IRMAApplicationsRole)).Returns(true);
            user.Setup(u => u.IsInRole(IRMASupportRole)).Returns(true);
            // Act
            var isAuthorized = authorizer.IsAuthorized(user.Object, authorizer.RequiredRole );
            // Assert
            Assert.IsFalse(isAuthorized);
        }

        [TestMethod]
        public void IsAuthorized_WithStandardSecurityGroups_WhenRequiredLevelIsPrivilegedAndUseInIrmaSupportAndIrmaApplicationsAndIrmaDev_ShouldBeAuthorized()
        {
            // Arrange
            var authorizer = new DashboardAuthorizer(base.StandardReadOnlyGroups, base.StandardPrivilegedGroups);
            authorizer.RequiredRole = UserAuthorizationLevelEnum.EditingPrivileges;
            user.Setup(u => u.IsInRole(IRMAApplicationsRole)).Returns(true);
            user.Setup(u => u.IsInRole(IRMASupportRole)).Returns(true);
            user.Setup(u => u.IsInRole(IRMADevelopersRole)).Returns(true);
            // Act
            var isAuthorized = authorizer.IsAuthorized(user.Object, authorizer.RequiredRole );
            // Assert
            Assert.IsTrue(isAuthorized);
        }

        [TestMethod]
        public void IsAuthorized_WithStandardSecurityGroups_WhenRequiredLevelIsReadOnlyAndUseInIrmaSupportAndIrmaApplicationsAndIrmaDev_ShouldBeAuthorized()
        {
            // Arrange
            var authorizer = new DashboardAuthorizer(base.StandardReadOnlyGroups, base.StandardPrivilegedGroups);
            authorizer.RequiredRole = UserAuthorizationLevelEnum.ReadOnly;
            user.Setup(u => u.IsInRole(IRMAApplicationsRole)).Returns(true);
            user.Setup(u => u.IsInRole(IRMASupportRole)).Returns(true);
            user.Setup(u => u.IsInRole(IRMADevelopersRole)).Returns(true);
            // Act
            var isAuthorized = authorizer.IsAuthorized(user.Object, authorizer.RequiredRole );
            // Assert
            Assert.IsTrue(isAuthorized);
        }

        [TestMethod]
        public void IsAuthorized_WithNullSecurityGroupsInfo_WhenRequiredLevelIsReadOnlyAndUserInAllGruops_ShouldNotBeAuthorized()
        {
            // Arrange
            // simulate no security group entries in config file
            List<string> readOnlyGroups = null;
            List<string> privilegedGroups = null;
            var authorizer = new DashboardAuthorizer(readOnlyGroups, privilegedGroups);
            authorizer.RequiredRole = UserAuthorizationLevelEnum.ReadOnly;
            user.Setup(u => u.IsInRole(IRMAApplicationsRole)).Returns(true);
            user.Setup(u => u.IsInRole(IRMADevelopersRole)).Returns(true);
            user.Setup(u => u.IsInRole(IRMASupportRole)).Returns(true);
            // Act
            var isAuthorized = authorizer.IsAuthorized(user.Object, authorizer.RequiredRole );
            // Assert
            Assert.IsFalse(isAuthorized);
        }

        [TestMethod]
        public void IsAuthorized_WithNoSecurityGroupsInfo_WhenRequiredLevelIsReadOnlyAndUserInAllGruops_ShouldNotBeAuthorized()
        {
            // Arrange
            // simulate no security group entries in config file
            var readOnlyGroups = new List<string>{""};
            var privilegedGroups = new List<string>{""};
            var authorizer = new DashboardAuthorizer(readOnlyGroups, privilegedGroups);
            authorizer.RequiredRole = UserAuthorizationLevelEnum.ReadOnly;
            user.Setup(u => u.IsInRole(IRMAApplicationsRole)).Returns(true);
            user.Setup(u => u.IsInRole(IRMADevelopersRole)).Returns(true);
            user.Setup(u => u.IsInRole(IRMASupportRole)).Returns(true);
            // Act
            var isAuthorized = authorizer.IsAuthorized(user.Object, authorizer.RequiredRole );
            // Assert
            Assert.IsFalse(isAuthorized);
        }

        [TestMethod]
        public void IsAuthorized_WithNoPrivilegedSecurityGroup_WhenRequiredLevelIsReadOnlyAndUserInAllGroups_ShouldBeAuthorized()
        {
            // Arrange
            var readOnlyGroups = new List<string>{IRMAApplicationsRole, IRMASupportRole};
            var privilegedGroups = new List<string>{""};
            var authorizer = new DashboardAuthorizer(readOnlyGroups, privilegedGroups);
            authorizer.RequiredRole = UserAuthorizationLevelEnum.ReadOnly;
            user.Setup(u => u.IsInRole(IRMAApplicationsRole)).Returns(true);
            user.Setup(u => u.IsInRole(IRMADevelopersRole)).Returns(true);
            user.Setup(u => u.IsInRole(IRMASupportRole)).Returns(true);
            // Act
            var isAuthorized = authorizer.IsAuthorized(user.Object, authorizer.RequiredRole );
            // Assert
            Assert.IsTrue(isAuthorized);
        }

        [TestMethod]
        public void GetAuthorizationLevel_WithNoReadOnlySecurityGroup_WhenRequiredLevelIsPriviligedAndUserInIrmaDevelopers_ShouldBeAuthorized()
        {
            // Arrange
            var readOnlyGroups = new List<string>{""};
            var privilegedGroups = new List<string>{IRMADevelopersRole};
            var authorizer = new DashboardAuthorizer(readOnlyGroups, privilegedGroups);
            authorizer.RequiredRole = UserAuthorizationLevelEnum.ReadOnly;
            user.Setup(u => u.IsInRole(IRMADevelopersRole)).Returns(true);
            // Act
            var isAuthorized = authorizer.IsAuthorized(user.Object, authorizer.RequiredRole );
            // Assert
            Assert.IsTrue(isAuthorized);
        }

        [TestMethod]
        public void GetAuthorizationLevel_WithNoPrivilegedSecurityGroup_WhenRequiredLevelIsPriviligedAndUserInIrmaDevelopers_ShouldNotBeAuthorized()
        {
            // Arrange
            var readOnlyGroups = new List<string>{IRMAApplicationsRole, IRMASupportRole};
            var privilegedGroups = new List<string>{""};
            var authorizer = new DashboardAuthorizer(readOnlyGroups, privilegedGroups);
            authorizer.RequiredRole = UserAuthorizationLevelEnum.EditingPrivileges;
            user.Setup(u => u.IsInRole(IRMAApplicationsRole)).Returns(true);
            user.Setup(u => u.IsInRole(IRMADevelopersRole)).Returns(true);
            user.Setup(u => u.IsInRole(IRMASupportRole)).Returns(true);
            // Act
            var isAuthorized = authorizer.IsAuthorized(user.Object, authorizer.RequiredRole );
            // Assert
            Assert.IsFalse(isAuthorized);
        }
    }
}
