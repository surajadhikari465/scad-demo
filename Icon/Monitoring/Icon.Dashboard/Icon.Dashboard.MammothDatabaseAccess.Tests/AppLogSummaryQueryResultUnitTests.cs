using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Icon.Dashboard.CommonDatabaseAccess;
using Icon.Dashboard.MammothDatabaseAccess;

namespace Icon.Dashboard.MammothnDatabaseAccess.Tests
{
    [TestClass]
    public class AppLogSummaryQueryResultUnitTests
    {
        [TestMethod]
        public void AppLogSummaryQueryResult_Constructor_ShouldSet_SimpleValues()
        {
            //Arrange
            string appName = "mammoth.aaaa.BBBBBB";
            int appId = 4;
            TimeSpan timeSpan = new TimeSpan(2, 30, 15);
            //Act
            var queryResult = new AppLogSummaryQueryResult(appName, appId, timeSpan);
            //Assert
            Assert.AreEqual(appName, queryResult.AppName);
            Assert.AreEqual(appId, queryResult.AppID);
            Assert.AreEqual(timeSpan, queryResult.DefinitionOfRecent);
        }

        [TestMethod]
        public void AppLogSummaryQueryResult_Constructor_ShouldSet_SimpleValuesAndOptionalLoggingLevel()
        {
            //Arrange
            string appName = "mammoth.aaaa.BBBBBB";
            int appId = -8785;
            TimeSpan timeSpan = new TimeSpan(23, 59, 59);
            LoggingLevel level = LoggingLevel.Warning;
            //Act
            var queryResult = new AppLogSummaryQueryResult(appName, appId, timeSpan, level);
            //Assert
            Assert.AreEqual(appName, queryResult.AppName);
            Assert.AreEqual(appId, queryResult.AppID);
            Assert.AreEqual(timeSpan, queryResult.DefinitionOfRecent);
            Assert.AreEqual(level, queryResult.LogLevel);
        }
    }
}
