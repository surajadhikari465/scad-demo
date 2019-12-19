using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OOS.Model
{
    [Obsolete("Now with dates/times we dont need this. Dont use me!", true)]
    public class InclusiveTimeZoneEndDateSpecification
    {
        private DateTime endDate;
        private DateTime todaysDate;
        

        public InclusiveTimeZoneEndDateSpecification(DateTime todaysDate, DateTime endDate)
        {
            this.endDate = endDate;
            this.todaysDate = todaysDate;
        }

        public DateTime InclusiveEndDate
        {
            get
            {
                var inclusiveEndDate = endDate.AddDays(1);
                if (todaysDate == endDate)
                    inclusiveEndDate = endDate.AddDays(2);
                return inclusiveEndDate;
            }
        }
    }
}
