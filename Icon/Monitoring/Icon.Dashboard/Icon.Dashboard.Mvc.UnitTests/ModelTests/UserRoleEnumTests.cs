using Icon.Dashboard.Mvc.Enums;
using Icon.Dashboard.Mvc.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Icon.Dashboard.Mvc.UnitTests.ModelUnitTests
{
    //[Flags]
    //public enum DashboardUserRole
    //{
    //    Unauthorized = 0x0,
    //    IrmaApplications = 0x1,
    //    IrmaDeveloper = IrmaApplications | 0x2
    //}

    [TestClass]
    public class UserAuthorizationLevelEnumUnitTests
    {
        [TestMethod]
        public void DashboardUserAuthorizationLevelEnum_DefaultValue_ShouldBeUnauthorized()
        {
            // Arrange
            // Act
            var userRole = new UserAuthorizationLevelEnum();
            // Assert
            Assert.AreEqual(UserAuthorizationLevelEnum.None, userRole);
        }

        [TestMethod]
        public void DashboardUserAuthorizationLevelEnum_EditingLevel_IsSupersetOfReadOnly()
        {
            // Arrange
            // Act
            var devRole = UserAuthorizationLevelEnum.EditingPrivileges;
            // Assert
            Assert.AreEqual(0x01, (int)(devRole & UserAuthorizationLevelEnum.ReadOnly));
            Assert.AreEqual(0x03, (int)(devRole | UserAuthorizationLevelEnum.ReadOnly));
            Assert.AreEqual(0x02, (int)(devRole ^ UserAuthorizationLevelEnum.ReadOnly));
            Assert.AreEqual(0x03, (int)(devRole & UserAuthorizationLevelEnum.EditingPrivileges));
            Assert.AreEqual(0x03, (int)(devRole | UserAuthorizationLevelEnum.EditingPrivileges));
            Assert.AreEqual(0x00, (int)(devRole ^ UserAuthorizationLevelEnum.EditingPrivileges));
        }
    }
}