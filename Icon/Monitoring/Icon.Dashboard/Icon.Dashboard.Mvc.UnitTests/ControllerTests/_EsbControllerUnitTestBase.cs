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
    public abstract class _EsbControllerUnitTestBase : _MvcControllerUnitTestBase
    {
        public _EsbControllerUnitTestBase() : base() { }

        protected override abstract string testActionName { get; }

        protected override string testControllerName => "Esb";

        protected EsbController ConstructController()
        {
            return ConstructController(
                base.authorizer,
                base.dashboardConfigManager,
                base.iconDbServiceWrapper,
                base.mammothDbServiceWrapper,
                base.environmentCookieManager,
                base.wmiServiceWrapper);
        }

        protected EsbController ConstructController(
            IDashboardAuthorizer dashboardAuthorizer = null,
            IDashboardDataManager dashboardConfigManager = null,
            IIconDatabaseServiceWrapper iconLoggingWrapper = null,
            IMammothDatabaseServiceWrapper mammothLoggingWrapper = null,
            IEnvironmentCookieManager environmentCookieManager = null,
            IRemoteWmiServiceWrapper remoteWmiServiceWrapper = null)
        {
            var controller = new EsbController(
                dashboardAuthorizer,
                dashboardConfigManager,
                iconLoggingWrapper,
                mammothLoggingWrapper,
                environmentCookieManager,
                remoteWmiServiceWrapper);
            base.InitializeTestControllerContext(controller);
            return controller;
        }
    }
}
