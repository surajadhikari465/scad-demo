using System;

namespace WholeFoods.Common.IRMALib.Dates
{
    public interface IDateRepository
    {
        int CurrentFiscalPeriod();
        int CurrentFiscalQuarter();
        int CurrentFiscalYear();
        int FiscalPeriod(DateTime _date);
        int FiscalQuarter(DateTime _date);
        int FiscalYear(DateTime _date);
        DateTime? GetQuarterStart(int quarter, int year);
        DateTime LastWeekdayInQuarter(DateTime _quarterDate);
        DateTime NearestWeekdayAfter(DateTime _date);
        DateTime NearestWeekdayBefore(DateTime _date);
    }
}