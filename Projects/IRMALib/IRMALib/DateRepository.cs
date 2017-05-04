using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Linq;

using WholeFoods.Utility.DataAccess;

using log4net;

namespace WholeFoods.Common.IRMALib.Dates
{
    public class DateRepository
    {
        IRMALibDataClassesDataContext db;

        public DateRepository(string _connectionString)
        {
            db = new IRMALibDataClassesDataContext(_connectionString);
        }

        public int CurrentFiscalQuarter()
        {
            DateTime date = DateTime.Now;
            return FiscalQuarter(date);
        }

        public int CurrentFiscalYear()
        {
            DateTime date = DateTime.Now;
            return FiscalYear(date);
        }

        public int CurrentFiscalPeriod()
        {
            DateTime date = DateTime.Now;
            return FiscalPeriod(date);
        }

        public int FiscalQuarter(DateTime _date)
        {
            var date = (from d in db.Dates
                        where d.Date_Key == _date.Date
                        select d).Take(1);

            //Date date = db.Dates.SingleOrDefault(d => d.Date_Key == _date.Date);
            if (date == null)
                return -1;
            return (int)date.First().Quarter;
        }

        public int FiscalYear(DateTime _date)
        {
            /*
            var date = (from d in db.Dates
                        where d.Date_Key equals _date.Date
                        select d).Take(1);

            */
            var date = db.Dates.SingleOrDefault(d => d.Date_Key == _date.Date);
            if (date == null)
                return -1;

            //DateTime d1 = date.FirstOrDefault().Date_Key;

            return (int)date.Year;
            
        }

        public int FiscalPeriod(DateTime _date)
        {
            Date date = db.Dates.SingleOrDefault(d => d.Date_Key == _date.Date);
            if (date == null)
                return -1;
            return (int)date.Period;
        }

        /// <summary>
        /// Returns the nearest weekday before a given date.  If the given date IS a weekday, returns that date.
        /// </summary>
        /// <param name="_date">The date to check.</param>
        /// <returns>A DateTime object for the nearest weekday.</returns>
        public DateTime NearestWeekdayBefore(DateTime _date)
        {
            var date = db.Dates.SingleOrDefault(d => d.Date_Key == _date.Date);
            if (date.Day_Name.Trim().ToLower() == "saturday")
                return _date.AddDays(-1);

            if (date.Day_Name.Trim().ToLower() == "sunday")
                return _date.AddDays(-2);

            return _date;
        }

        /// <summary>
        /// Returns the nearest weekday after a given date.  If the given date IS a weekday, returns that date.
        /// </summary>
        /// <param name="_date">The date to check.</param>
        /// <returns>A DateTime object for the nearest weekday.</returns>
        public DateTime NearestWeekdayAfter(DateTime _date)
        {
            var date = db.Dates.SingleOrDefault(d => d.Date_Key == _date.Date);
            if (date.Day_Name.Trim().ToLower() == "saturday")
                return _date.AddDays(2);

            if (date.Day_Name.Trim().ToLower() == "sunday")
                return _date.AddDays(1);

            return _date;
        }

        /// <summary>
        /// Given any date within a certain quarter, return the last weekday for that quarter.
        /// </summary>
        /// <param name="_quarterDate"></param>
        /// <returns></returns>
        public DateTime LastWeekdayInQuarter(DateTime _quarterDate)
        {
            var qDate = db.Dates.SingleOrDefault(d => d.Date_Key == _quarterDate.Date);
            var lastWeekdate = db.Dates.Where(d => d.Day_Name.ToLower() != "sunday" && d.Day_Name.ToLower() != "saturday" && d.Quarter == qDate.Quarter && d.Year == qDate.Year).Max(d => d.Date_Key);
            return lastWeekdate.Date;
        }

        public DateTime? GetQuarterStart(int quarter, int year)
        {
            var qStart = db.Dates.OrderBy(d => d.Date_Key).FirstOrDefault(d => d.Year == year && d.Quarter == quarter);
            if (qStart != null)
                return qStart.Date_Key;

            return null;
        }


    }
}
