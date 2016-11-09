
CREATE PROCEDURE [dbo].[PDX_CalendarHierarchyFile]
AS 
BEGIN
SET NOCOUNT ON
set transaction isolation level read uncommitted

select 
convert(varchar, d1.Date_Key, 112) as CALENDAR_DATE,
convert(varchar(10), d1.Date_Key, 126) as DAY_LABEL,
(select convert(varchar, max(d2.Date_Key), 112) from date d2 
group by d2.Year, d2.Quarter, d2.Period, d2.Week
  having d2.Year = d1.Year
       and d2.Quarter = d1.Quarter
       and d2.Period = d1.Period
       and d2.Week = d1.Week) as WEEK_ID,
convert(varchar(4), Year) + '-' + convert(varchar(1),Week) as FISCAL_WEEK_LABEL,
convert(varchar(4), Year) + right('00' + convert(varchar,Period), 2) as PERIOD_ID,
convert(varchar(4), Year) + '-' + right('00' + convert(nvarchar,Period), 2) as FISCAL_PERIOD_LABEL,
convert(varchar(4), Year) + right('00' + convert(varchar,Quarter), 2) as QUARTER_ID,
convert(varchar(4), Year) + '-' + right('00' + convert(varchar,Quarter), 2) as FISCAL_QUARTER_LABEL,
convert(varchar(4), Year) as YEAR_ID,
convert(varchar(4), Year) as FISCAL_YEAR_LABEL
from date d1
where Date_Key >= '2014-01-01'
and Date_Key < '2021-01-01'
order by d1.Date_Key
END


print '[' + convert(nvarchar, getdate(), 121) + '] ' + 'Finish: [PDX_CalendarHierarchyFile.sql]'