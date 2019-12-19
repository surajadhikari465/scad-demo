using System;

namespace WebSupport.Helpers
{
    public static class ScheduledJobHelpers
    {
        public static DateTime DetermineNextScheduledTime(DateTime currentDateTime, DateTime? originalStartDateTime, int intervalInSeconds)
        {
            //  prefer to use originalStartDateTime, i otherwise use currentDateTime.
            var previousDateTime = originalStartDateTime ?? currentDateTime;

            // use the previousDateTime and determine what the next interval should be.
            var projectedRunTime = previousDateTime;
            while (projectedRunTime <= currentDateTime)
            {
                projectedRunTime = projectedRunTime.AddSeconds(intervalInSeconds);
            }

            return projectedRunTime;
        }
    }
}
