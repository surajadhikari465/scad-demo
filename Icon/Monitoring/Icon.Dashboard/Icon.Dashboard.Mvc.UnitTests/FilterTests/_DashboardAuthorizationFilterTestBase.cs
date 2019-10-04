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
        protected const string IRMAApplicationsRole = "IRMA.Applications";
        protected const string IRMADevelopersRole = "IRMA.Developers";
        protected const string IRMASupportRole = "IRMA.Support";
        protected List<string> StandardReadOnlyGroups = new List<string> {IRMAApplicationsRole, IRMASupportRole};
        protected List<string> StandardPrivilegedGroups = new List<string> { IRMADevelopersRole };

        protected Mock<IPrincipal> user = new Mock<IPrincipal>();
    }
}
