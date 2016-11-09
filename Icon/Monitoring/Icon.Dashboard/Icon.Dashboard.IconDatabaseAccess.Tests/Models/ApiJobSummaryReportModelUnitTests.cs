using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using FluentAssertions;

namespace Icon.Dashboard.IconDatabaseAccess.Tests
{
    [TestClass]
    public class ApiJobSummaryReportModelUnitTests
    {
        [TestMethod]
        public void ApiJobSummaryReport_Constructor_ShouldSet_SimpleValues()
        {
            //Arrangestring messageType, DateTime startTime, DateTime endTime
            string messageType = "zmnv;203r80adsf;&*";
            DateTime startTime = new DateTime(2017, 4, 24, 18, 30, 10);
            DateTime endTime = new DateTime(2017, 9, 26, 10, 20, 20);
            //Act
            var report = new ApiJobSummaryReport(messageType, startTime, endTime);
            //Assert
            report.MessageType.ShouldBeEquivalentTo(messageType);
            report.StartTime.ShouldBeEquivalentTo(startTime);
            report.EndTime.ShouldBeEquivalentTo(endTime);
        }
    }
}
