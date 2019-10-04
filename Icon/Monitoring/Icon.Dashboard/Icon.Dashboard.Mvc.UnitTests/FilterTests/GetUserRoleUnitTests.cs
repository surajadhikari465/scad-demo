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
    public class GetUserRoleUnitTests : _DashboardAuthorizationFilterTestBase
    {
        private DashboardAuthorizer DashboardAuthorizer = null;

        [TestInitialize]
        public void TestInitialize()
        {
            DashboardAuthorizer = new DashboardAuthorizer(base.StandardReadOnlyGroups, base.StandardPrivilegedGroups);
            DashboardAuthorizer.RequiredRole = UserAuthorizationLevelEnum.ReadOnly;
        }

        [TestMethod]
        public void GetAuthorizationLevel_WithStandardSecurityGroups_WhenUserNull_ShouldHaveAuthLevelNone()
        {
            // Arrange
            var nullUser = (IPrincipal)null;
            // Act
            var authLevel = DashboardAuthorizer.GetAuthorizationLevel(nullUser);
            // Assert
            Assert.AreEqual(UserAuthorizationLevelEnum.None, authLevel);
        }

        [TestMethod]
        public void GetAuthorizationLevel_WithStandardSecurityGroupsl_WhenUserInNoGroups_ShouldHaveAuthLevelNone()
        {
            // Arrange
            user.Setup(u => u.IsInRole(It.IsAny<string>())).Returns(false);
            // Act
            var authLevel = DashboardAuthorizer.GetAuthorizationLevel(user.Object);
            // Assert
            Assert.AreEqual(UserAuthorizationLevelEnum.None, authLevel);
        }

        [TestMethod]
        public void GetAuthorizationLevel_WithStandardSecurityGroups_WhenUserInIrmaApplications_ShouldHaveAuthLevelReadOnly()
        {
            // Arrange
            user.Setup(u => u.IsInRole(IRMAApplicationsRole)).Returns(true);
            // Act
            var authLevel = DashboardAuthorizer.GetAuthorizationLevel(user.Object);
            // Assert
            Assert.AreEqual(UserAuthorizationLevelEnum.ReadOnly, authLevel);
        }

        [TestMethod]
        public void GetAuthorizationLevel_WithStandardSecurityGroups_WhenUserInIrmaDevelopersAndIrmaApplications_ShouldHaveAuthLevelPrivileged()
        {
            // Arrange
            user.Setup(u => u.IsInRole(IRMAApplicationsRole)).Returns(true);
            user.Setup(u => u.IsInRole(IRMADevelopersRole)).Returns(true);
            user.Setup(u => u.IsInRole(IRMASupportRole)).Returns(false);
            // Act
            var authLevel = DashboardAuthorizer.GetAuthorizationLevel(user.Object);
            // Assert
            Assert.AreEqual(UserAuthorizationLevelEnum.EditingPrivileges, authLevel);
        }

        [TestMethod]
        public void GetAuthorizationLevel_WithStandardSecurityGroups_WhenUserInIrmaDevelopersRole_ShouldHaveAuthLevelPrivileged()
        {
            // Arrange
            user.Setup(u => u.IsInRole(IRMAApplicationsRole)).Returns(false);
            user.Setup(u => u.IsInRole(IRMADevelopersRole)).Returns(true);
            user.Setup(u => u.IsInRole(IRMASupportRole)).Returns(false);
            // Act
            var authLevel = DashboardAuthorizer.GetAuthorizationLevel(user.Object);
            // Assert
            Assert.AreEqual(UserAuthorizationLevelEnum.EditingPrivileges, authLevel);
        }

        [TestMethod]
        public void GetAuthorizationLevel_WithStandardSecurityGroups_WhenUserInIrmaSupport_ShouldHaveAuthLevelReadOnly()
        {
            // Arrange
            user.Setup(u => u.IsInRole(IRMAApplicationsRole)).Returns(false);
            user.Setup(u => u.IsInRole(IRMADevelopersRole)).Returns(false);
            user.Setup(u => u.IsInRole(IRMASupportRole)).Returns(true);
            // Act
            var authLevel = DashboardAuthorizer.GetAuthorizationLevel(user.Object);
            // Assert
            Assert.AreEqual(UserAuthorizationLevelEnum.ReadOnly, authLevel);
        }

        [TestMethod]
        public void GetAuthorizationLevel_WithStandardSecurityGroups_WhenUserInIrmaSupportAndIrmaApplications_ShouldHaveAuthLevelReadOnly()
        {
            // Arrange
            user.Setup(u => u.IsInRole(IRMAApplicationsRole)).Returns(true);
            user.Setup(u => u.IsInRole(IRMADevelopersRole)).Returns(false);
            user.Setup(u => u.IsInRole(IRMASupportRole)).Returns(true);
            // Act
            var authLevel = DashboardAuthorizer.GetAuthorizationLevel(user.Object);
            // Assert
            Assert.AreEqual(UserAuthorizationLevelEnum.ReadOnly, authLevel);
        }


        [TestMethod]
        public void GetAuthorizationLevel_WithStandardSecurityGroups_WhenUserInIrmaSupportAndIrmaDevelopers_ShouldHaveAuthLevelPrivileged()
        {
            // Arrange
            user.Setup(u => u.IsInRole(IRMAApplicationsRole)).Returns(true);
            user.Setup(u => u.IsInRole(IRMADevelopersRole)).Returns(true);
            user.Setup(u => u.IsInRole(IRMASupportRole)).Returns(true);
            // Act
            var authLevel = DashboardAuthorizer.GetAuthorizationLevel(user.Object);
            // Assert
            Assert.AreEqual(UserAuthorizationLevelEnum.EditingPrivileges, authLevel);
        }

        [TestMethod]
        public void GetAuthorizationLevel_WithNullSecurityGroupsInfo_ShouldHaveAuthLevelNone()
        {
            // Arrange
            // simulate no security group entries in config file
            List<string> readOnlyGroups = null;
            List<string> privilegedGroups = null;
            DashboardAuthorizer = new DashboardAuthorizer(readOnlyGroups, privilegedGroups);
            user.Setup(u => u.IsInRole(IRMAApplicationsRole)).Returns(true);
            user.Setup(u => u.IsInRole(IRMADevelopersRole)).Returns(true);
            user.Setup(u => u.IsInRole(IRMASupportRole)).Returns(true);
            // Act
            var authLevel = DashboardAuthorizer.GetAuthorizationLevel(user.Object);
            // Assert
            Assert.AreEqual(UserAuthorizationLevelEnum.None, authLevel);
        }

        [TestMethod]
        public void GetAuthorizationLevel_WithNoSecurityGroupsInfo_ShouldHaveAuthLevelNone()
        {
            // Arrange
            // simulate no security group entries in config file
            var readOnlyGroups = new List<string> { "" };
            var privilegedGroups = new List<string> { "" };
            DashboardAuthorizer = new DashboardAuthorizer(readOnlyGroups, privilegedGroups);
            user.Setup(u => u.IsInRole(IRMAApplicationsRole)).Returns(true);
            user.Setup(u => u.IsInRole(IRMADevelopersRole)).Returns(true);
            user.Setup(u => u.IsInRole(IRMASupportRole)).Returns(true);
            // Act
            var authLevel = DashboardAuthorizer.GetAuthorizationLevel(user.Object);
            // Assert
            Assert.AreEqual(UserAuthorizationLevelEnum.None, authLevel);
        }

        [TestMethod]
        public void GetAuthorizationLevel_WithSingleEntryInReadOnlyAndPrivilegedSecurityGroups_WhenUserInIrmaApplications_ShouldHaveAuthLevelReadOnly()
        {
            // Arrange
            var readOnlyGroups = new List<string> { IRMAApplicationsRole };
            var privilegedGroups = new List<string> { IRMADevelopersRole };
            DashboardAuthorizer = new DashboardAuthorizer(readOnlyGroups, privilegedGroups);
            user.Setup(u => u.IsInRole(IRMAApplicationsRole)).Returns(true);
            user.Setup(u => u.IsInRole(IRMADevelopersRole)).Returns(false);
            // Act
            var authLevel = DashboardAuthorizer.GetAuthorizationLevel(user.Object);
            // Assert
            Assert.AreEqual(UserAuthorizationLevelEnum.ReadOnly, authLevel);
        }

        [TestMethod]
        public void GetAuthorizationLevel_WithSingleEntryInReadOnlyAndPrivilegedSecurityGroups_WhenUserInIrmaDevelopers_ShouldHaveAuthLevelReadOnly()
        {
            // Arrange
            var readOnlyGroups = new List<string> { IRMAApplicationsRole };
            var privilegedGroups = new List<string> { IRMADevelopersRole };
            DashboardAuthorizer = new DashboardAuthorizer(readOnlyGroups, privilegedGroups);
            user.Setup(u => u.IsInRole(IRMAApplicationsRole)).Returns(true);
            user.Setup(u => u.IsInRole(IRMADevelopersRole)).Returns(true);
            // Act
            var authLevel = DashboardAuthorizer.GetAuthorizationLevel(user.Object);
            // Assert
            Assert.AreEqual(UserAuthorizationLevelEnum.EditingPrivileges, authLevel);
        }

        [TestMethod]
        public void GetAuthorizationLevel_WithNoPrivilegedSecurityGroup_WhenUserInIrmaApplicationsAndIrmaDevelopers_ShouldHaveAuthLevelReadOnly()
        {
            // Arrange
            var readOnlyGroups = new List<string> { IRMAApplicationsRole, IRMASupportRole };
            var privilegedGroups = new List<string> { "" };
            DashboardAuthorizer = new DashboardAuthorizer(readOnlyGroups, privilegedGroups);
            user.Setup(u => u.IsInRole(IRMAApplicationsRole)).Returns(true);
            user.Setup(u => u.IsInRole(IRMADevelopersRole)).Returns(true);
            // Act
            var authLevel = DashboardAuthorizer.GetAuthorizationLevel(user.Object);
            // Assert
            Assert.AreEqual(UserAuthorizationLevelEnum.ReadOnly, authLevel);
        }

        [TestMethod]
        public void GetAuthorizationLevel_WithNoReadOnlySecurityGroup_WhenUserInIrmaDevelopers_ShouldHaveAuthLevelPrivileged()
        {
            // Arrange
            var readOnlyGroups = new List<string> { "" };
            var privilegedGroups = new List<string> { IRMADevelopersRole };
            DashboardAuthorizer = new DashboardAuthorizer(readOnlyGroups, privilegedGroups);
            user.Setup(u => u.IsInRole(IRMADevelopersRole)).Returns(true);
            // Act
            var authLevel = DashboardAuthorizer.GetAuthorizationLevel(user.Object);
            // Assert
            Assert.AreEqual(UserAuthorizationLevelEnum.EditingPrivileges, authLevel);
        }

        [TestMethod]
        public void GetAuthorizationLevel_WithNoReadOnlySecurityGroup_WhenUserInIrmaApplications_ShouldHaveAuthLevelNone()
        {
            // Arrange
            var readOnlyGroups = new List<string> { "" };
            var privilegedGroups = new List<string> { IRMADevelopersRole };
            DashboardAuthorizer = new DashboardAuthorizer(readOnlyGroups, privilegedGroups);
            user.Setup(u => u.IsInRole(IRMAApplicationsRole)).Returns(true);
            // Act
            var authLevel = DashboardAuthorizer.GetAuthorizationLevel(user.Object);
            // Assert
            Assert.AreEqual(UserAuthorizationLevelEnum.None, authLevel);
        }
    }
}
