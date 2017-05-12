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
    public abstract class _DashboardAuthorizationFilterTestBase
    {
        protected const string NameForApplicationsRole = "IRMA.Applications";
        protected const string NameForDevelopersRole = "IRMA.Developers";

        protected IPrincipal UserInApplicationsGroup
        {
            get
            {
                var user = new Mock<IPrincipal>();
                user.Setup(u => u.IsInRole(NameForApplicationsRole)).Returns(true);
                user.Setup(u => u.IsInRole(NameForDevelopersRole)).Returns(false);
                return user.Object;
            }
        }

        protected IPrincipal UserInDevelopersGroup
        {
            get
            {
                var user = new Mock<IPrincipal>();
                user.Setup(u => u.IsInRole(NameForApplicationsRole)).Returns(true);
                user.Setup(u => u.IsInRole(NameForDevelopersRole)).Returns(true);
                return user.Object;
            }
        }

        protected IPrincipal UserNotInAnyGroup
        {
            get
            {
                var user = new Mock<IPrincipal>();
                user.Setup(u => u.IsInRole(It.IsAny<string>())).Returns(false);
                return user.Object;
            }
        }

        protected IPrincipal UserInDevelopersButNotApplicationsGroup
        {
            get
            {
                var user = new Mock<IPrincipal>();
                user.Setup(u => u.IsInRole(NameForApplicationsRole)).Returns(false);
                user.Setup(u => u.IsInRole(NameForDevelopersRole)).Returns(true);
                return user.Object;
            }
        }

        protected IPrincipal UserNull
        {
            get
            {
                return (IPrincipal)null;
            }
        }

        protected UserRoleEnum Role_Unauthorized => UserRoleEnum.Unauthorized;
        protected UserRoleEnum Role_Applications => UserRoleEnum.ReadOnly;
        protected UserRoleEnum Role_Developer => UserRoleEnum.EditingPrivileges;
    }
}
