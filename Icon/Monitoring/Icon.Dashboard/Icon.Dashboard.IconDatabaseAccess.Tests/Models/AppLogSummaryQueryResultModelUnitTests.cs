using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Icon.Dashboard.CommonDatabaseAccess;

namespace Icon.Dashboard.IconDatabaseAccess.Tests
{
    [TestClass]
    public class AppLogSummaryQueryResultModelUnitTests
    {
        [TestMethod]
        public void AppLogSummaryQueryResult_Constructor_ShouldSet_SimpleValues()
        {
            //Arrange
            string appName = "ooBLAdsodd";
            int appId = 5464;
            TimeSpan timeSpan = new TimeSpan(3, 30, 15);
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
            string appName = "OPREIDSF.0928rjsd.sadjfo";
            int appId = -7798984;
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
