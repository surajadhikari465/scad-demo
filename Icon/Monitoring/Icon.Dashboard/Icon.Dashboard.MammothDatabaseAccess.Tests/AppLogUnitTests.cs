using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using FluentAssertions;
using Icon.Dashboard.CommonDatabaseAccess;
using Icon.Dashboard.MammothDatabaseAccess;

namespace Icon.Dashboard.MammothnDatabaseAccess.Tests
{
    [TestClass]
    public class AppLogUnitTests
    {
        protected AppLog CreateAppLogEntityForTest(string appName = "appName")
        {
            var app = new App()
            {
                AppID = 13,
                AppName = appName
            };
            var appLog = new AppLog()
            {
                AppLogID = 1000001,
                AppID = app.AppID,
                Level = "Warning",
                Logger = "D.Logga",
                UserName = "aludhe",
                MachineName = "safd234r",
                InsertDate = new DateTime(2016, 10, 13, 15, 10, 22),
                LogDate = new DateTime(2016, 10, 13, 15, 10, 22),
                Thread = null,
                Message = null,
                CallSite = null,
                Exception = null,
                StackTrace = null,
                App = app
            };
            return appLog;
        }

        [TestMethod]
        public void WnenAppLogEntityHasAppSubEntity_ThenPartialAppNameProperty_ShouldMatch()
        {
            //Arrange
            string appName =  "mammoth.somekind.OFAPP";
            var appLog = CreateAppLogEntityForTest(appName);
            //Act
            //Assert
            appLog.AppName.ShouldBeEquivalentTo(appName);
        }

        [TestMethod]
        public void WnenAppLogEntityHasLogDateHasValue_ThenPartialLoggingTimestampProperty_ShouldMatch()
        {
            //Arrange
            DateTime expectedDate = new DateTime(2016, 6, 26, 14, 14, 14);
            var appLog = CreateAppLogEntityForTest();
            appLog.LogDate = expectedDate;
            //Act
            //Assert
            appLog.LoggingTimestamp.ShouldBeEquivalentTo(expectedDate);
        }

        [TestMethod]
        public void WnenAppLogEntityHasLogDateIsNull_ThenPartialLoggingTimestampProperty_ShouldHaveDefaultValue()
        {
            //Arrange
            var appLog = CreateAppLogEntityForTest();
            appLog.LogDate = null;
            //Act
            //Assert
            appLog.LoggingTimestamp.Should().Be(default(DateTime));
        }

    }
}
