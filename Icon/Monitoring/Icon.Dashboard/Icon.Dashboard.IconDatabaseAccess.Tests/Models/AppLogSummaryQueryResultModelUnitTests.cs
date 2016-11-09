using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using FluentAssertions;
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
            queryResult.AppName.ShouldBeEquivalentTo(appName);
            queryResult.AppID.ShouldBeEquivalentTo(appId);
            queryResult.DefinitionOfRecent.ShouldBeEquivalentTo(timeSpan);
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
            queryResult.AppName.ShouldBeEquivalentTo(appName);
            queryResult.AppID.ShouldBeEquivalentTo(appId);
            queryResult.DefinitionOfRecent.ShouldBeEquivalentTo(timeSpan);
            queryResult.LogLevel.ShouldBeEquivalentTo(level);
        }
    }
}
