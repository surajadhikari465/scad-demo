using FluentAssertions;
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
    public class UserRoleEnumUnitTests
    {
        [TestMethod]
        public void DashboardUserRoleEnum_DefaultValue_ShouldBeUnauthorized()
        {
            // Arrange
            // Act
            var userRole = new UserRoleEnum();
            // Assert
            userRole.ShouldBeEquivalentTo(UserRoleEnum.Unauthorized);
        }

        [TestMethod]
        public void DashboardUserRoleEnum_IrmaDeveloper_IsSupersetOfIrmaApplications()
        {
            // Arrange
            // Act
            var devRole = UserRoleEnum.EditingPrivileges;
            // Assert
            ((int)(devRole & UserRoleEnum.ReadOnly)).ShouldBeEquivalentTo(0x01);
            ((int)(devRole | UserRoleEnum.ReadOnly)).ShouldBeEquivalentTo(0x03);
            ((int)(devRole ^ UserRoleEnum.ReadOnly)).ShouldBeEquivalentTo(0x02);
            ((int)(devRole & UserRoleEnum.EditingPrivileges)).ShouldBeEquivalentTo(0x03);
            ((int)(devRole | UserRoleEnum.EditingPrivileges)).ShouldBeEquivalentTo(0x03);
            ((int)(devRole ^ UserRoleEnum.EditingPrivileges)).ShouldBeEquivalentTo(0x00);
        }
    }
}