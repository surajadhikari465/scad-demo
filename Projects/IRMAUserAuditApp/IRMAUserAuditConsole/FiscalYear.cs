using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IRMAUserAuditConsole
{
    public static class FiscalYear
    {
        private static DateTime currentDate = DateTime.Now;

        public static DateTime CurrentDate { 
            get { return currentDate; }
            set { currentDate = value; }
        }

        public static int Quarter()
        {
            DateTime q1Start, q1End, q2Start, q2End, q3Start, q3End, q4Start;

            // set the start of the current FY.  If it's not Oct, FY is the same as the current year
            // if it IS Oct or later, then FY is NEXT year.  ie - Oct 1st 2012 is the start of FY 2013.
            //DateTime fyStart;
            if (DateTime.Now.Month < 10)
                q1Start = new DateTime(DateTime.Now.Year - 1, 10, 1);
            else
                q1Start = new DateTime(DateTime.Now.Year, 10, 1);

            q1End = q1Start.AddDays(16 * 7).AddMilliseconds(-1);
            q2Start = new DateTime(q1End.Ticks).AddMilliseconds(1);
            q2End = q2Start.AddDays(12 * 7).AddMilliseconds(-1);
            q3Start = new DateTime(q2End.Ticks).AddMilliseconds(1);
            q3End = q3Start.AddDays(12 * 7).AddMilliseconds(-1);
            q4Start = new DateTime(q3End.Ticks).AddMilliseconds(1);

            long currentTicks = currentDate.Ticks;

            if (currentTicks > q1Start.Ticks && currentTicks <= q1End.Ticks)
                return 1;
            else if (currentTicks > q2Start.Ticks && currentTicks <= q2End.Ticks)
                return 2;
            else if (currentTicks > q3Start.Ticks && currentTicks <= q3End.Ticks)
                return 3;
            else
                return 4;


        }

        public static int Year()
        {
            return (currentDate.Month < 10 ? currentDate.Year : currentDate.AddYears(1).Year);
        }
    }
}
