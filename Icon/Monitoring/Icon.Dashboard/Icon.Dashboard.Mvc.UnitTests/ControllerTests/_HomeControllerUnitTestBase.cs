using Icon.Dashboard.Mvc.Controllers;
using Icon.Dashboard.Mvc.Filters;
using Icon.Dashboard.Mvc.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Icon.Dashboard.Mvc.UnitTests.ControllerTests
{
    public abstract class _HomeControllerUnitTestBase : _MvcControllerUnitTestBase
    {
        public _HomeControllerUnitTestBase() : base() { }

        protected override abstract string testActionName { get; }

        protected override string testControllerName => "Home";

        protected HomeController ConstructController()
        {
            return ConstructController(
                base.authorizer,
                base.dashboardConfigManager,
                base.environmentCookieManager,
                base.iconDbServiceWrapper,
                base.mammothDbServiceWrapper,
                base.wmiServiceWrapper);
        }

        protected HomeController ConstructController(
            IDashboardAuthorizer dashboardAuthorizer = null,
            IDashboardDataManager dashboardConfigManager = null,
            IEnvironmentCookieManager environmentCookieManager = null,
            IIconDatabaseServiceWrapper iconDbLoggingWrapper = null,
            IMammothDatabaseServiceWrapper mammothLoggingWrapper = null,
            IRemoteWmiServiceWrapper remoteWmiServiceWrapper = null)
        {
            var controller = new HomeController(
                dashboardAuthorizer,
                dashboardConfigManager,
                iconDbLoggingWrapper,
                mammothDbServiceWrapper,
                environmentCookieManager,
                remoteWmiServiceWrapper);
            base.InitializeTestControllerContext(controller);
            return controller;
        }
    }
}
