using FluentAssertions;
using Icon.Dashboard.Mvc.Filters;
using Icon.Dashboard.Mvc.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Web;

namespace Icon.Dashboard.Mvc.UnitTests.FilterTests.AuthorizationAttributeUnitTest
{
    [TestClass]
    public class _AuthorizationAttributeUnitTestBase
    {
        protected Mock<IPrincipal> MockGeneralUser = new Mock<IPrincipal>();
        protected Mock<IPrincipal> MockReadOnlyUser = new Mock<IPrincipal>();
        protected Mock<IPrincipal> MockPrivilegedUser = new Mock<IPrincipal>();
        protected Mock<IPrincipal> MockPrivilegedButNotReadOnlyUser = new Mock<IPrincipal>();

        protected IPrincipal JoeSchmo => MockGeneralUser.Object;
        protected IPrincipal SallyTheBA => MockReadOnlyUser.Object;
        protected IPrincipal DeveloperBob => MockPrivilegedUser.Object;
        protected IPrincipal UserInDevelopersButNotApplicationsGroup => MockPrivilegedButNotReadOnlyUser.Object;

        [TestInitialize]
        public void TestInit()
        {
            MockGeneralUser.Setup(i => i.IsInRole(String.Empty)).Returns(false);
            MockGeneralUser.Setup(i => i.IsInRole(DashboardAuthorization.ReadOnlyGroupRole)).Returns(false);
            MockGeneralUser.Setup(i => i.IsInRole(DashboardAuthorization.PrivilegedGroupRole)).Returns(false);
            MockReadOnlyUser.Setup(i => i.IsInRole(String.Empty)).Returns(false);
            MockReadOnlyUser.Setup(i => i.IsInRole(DashboardAuthorization.ReadOnlyGroupRole)).Returns(true);
            MockReadOnlyUser.Setup(i => i.IsInRole(DashboardAuthorization.PrivilegedGroupRole)).Returns(false);
            MockPrivilegedUser.Setup(i => i.IsInRole(String.Empty)).Returns(false);
            MockPrivilegedUser.Setup(i => i.IsInRole(DashboardAuthorization.ReadOnlyGroupRole)).Returns(true);
            MockPrivilegedUser.Setup(i => i.IsInRole(DashboardAuthorization.PrivilegedGroupRole)).Returns(true);
            MockPrivilegedButNotReadOnlyUser.Setup(i => i.IsInRole(String.Empty)).Returns(false);
            MockPrivilegedButNotReadOnlyUser.Setup(i => i.IsInRole(DashboardAuthorization.ReadOnlyGroupRole)).Returns(false);
            MockPrivilegedButNotReadOnlyUser.Setup(i => i.IsInRole(DashboardAuthorization.PrivilegedGroupRole)).Returns(true);
        }
    }
}