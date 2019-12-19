using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WebSupport.Helpers;

namespace WebSupport.Tests.Helpers
{
    [TestClass]
    public class ScheduledJobHelpersTests
    {
        
        [TestMethod]
        public void DetermineNextScheduledTime_OriginalScheduledDateTimeIsNotNullCurrentTimeIsBeforeNextRunTime_NextRunTimeShouldBeNextFutureInterval()
        {
            var currentDateTime = new DateTime(2019,12,1,7,30,0);
            var originalDateTime = new DateTime(2019, 11, 12, 8, 0, 0);
            var intervalInSeconds = 30; 
            
            var expected = new DateTime(2019, 12, 1, 7, 30, 30); 
            var result = ScheduledJobHelpers.DetermineNextScheduledTime(currentDateTime, originalDateTime, intervalInSeconds);

            Assert.AreEqual(expected, result);

        }


        [TestMethod]
        public void DetermineNextScheduledTime_OriginalScheduledDateIsNotNullCurrentTimeIsAfterNextRunTime_NextRunTimeShouldBeNextFutureInterval()
        {
            var currentDateTime = new DateTime(2019, 12, 1, 8, 30, 0);
            var originalDateTime = new DateTime(2019, 11, 12, 8, 0, 0);
            var intervalInSeconds = 30;

            var expected = new DateTime(2019, 12, 1, 8, 30, 30);
            var result = ScheduledJobHelpers.DetermineNextScheduledTime(currentDateTime, originalDateTime, intervalInSeconds);

            Assert.AreEqual(expected, result);
        }


        [TestMethod]
        public void DetermineNextScheduledTime_OriginalScheduledDateIsNullUseCurrentDateTimeInstead_NextRunTimeShouldBeNextFutureInterval()
        {
            var currentDateTime = new DateTime(2019, 12, 1, 6, 30, 0);
            DateTime? originalDateTime = null;
            var intervalInSeconds = 86400;

            var expected = new DateTime(2019, 12, 2, 6, 30, 0);
            var result = ScheduledJobHelpers.DetermineNextScheduledTime(currentDateTime, originalDateTime, intervalInSeconds);

            Assert.AreEqual(expected, result);
        }

    }
}
