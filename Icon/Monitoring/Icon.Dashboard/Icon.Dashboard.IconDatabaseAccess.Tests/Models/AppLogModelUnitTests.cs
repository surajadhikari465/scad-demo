using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Icon.Dashboard.IconDatabaseAccess.Tests
{
    [TestClass]
    public class AppLogModelUnitTests
    {
        [TestMethod]
        public void AppLogAppNameProperty_WhenAppIsNotNull_ShouldGetNameValue()
        {
            //Given
            const int expectedAppId = 777;
            const string expectedAppName = "AnApp.OfInterest";
            App sampleApp = new App()
            {
                AppID = expectedAppId,
                AppName = expectedAppName
            };
            AppLog log = new AppLog()
            {
                AppLogID = 121,
                AppID = expectedAppId,
                UserName = "shadly",
                InsertDate = new DateTime(2016, 10, 24, 18, 30, 30),
                LogDate = new DateTime(2016, 10, 24, 18, 30, 30),
                Level = "Warning",
                Logger = "my_logger",
                Message = "too much pressure",
                App = sampleApp
            };
            //When
            var actualAppName = log.AppName;
            //Then
            Assert.AreEqual(expectedAppName, actualAppName);
        }

        [TestMethod]
        public void AppLogAppNameProperty_WhenAppIsNull_ShouldGetNull()
        {
            //Given
            const int expectedAppId = 777;
            const string expectedAppName = null;
            App sampleApp = null;
            AppLog log = new AppLog()
            {
                AppLogID = 121,
                AppID = expectedAppId,
                UserName = "shadly",
                InsertDate = new DateTime(2016, 10, 24, 18, 30, 30),
                LogDate = new DateTime(2016, 10, 24, 18, 30, 30),
                Level = "Warning",
                Logger = "my_logger",
                Message = "too much pressure",
                App = sampleApp
            };
            //When
            var actualAppName = log.AppName;
            //Then
            Assert.AreEqual(expectedAppName, actualAppName);
        }

        [TestMethod]
        public void AppLogLoggingTimestampProperty_WhenLogDateIsNotNull_ShouldGetLogDateValue()
        {
            //Given
            const int expectedAppId = 777;
            const string expectedAppName = "AnApp.OfInterest";
            App sampleApp = new App()
            {
                AppID = expectedAppId,
                AppName = expectedAppName
            };
            DateTime expectedLogDate = new DateTime(2016, 10, 24, 18, 30, 30);
            AppLog log = new AppLog()
            {
                AppLogID = 121,
                AppID = expectedAppId,
                UserName = "shadly",
                InsertDate = new DateTime(2016, 10, 24, 18, 30, 30),
                LogDate = expectedLogDate,
                Level = "Warning",
                Logger = "my_logger",
                Message = "too much pressure",
                App = sampleApp
            };
            //When
            var actualLogDate = log.LoggingTimestamp;
            //Then
            Assert.AreEqual(expectedLogDate, actualLogDate);
        }
    }
}
