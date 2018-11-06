CREATE PROCEDURE [dbo].[GetFiscalWeeks] 
	@StartDate SmallDateTime = NULL,
	@EndDate SmallDateTime = NULL
AS
BEGIN

SELECT
	FiscalYear,
	FiscalPeriod,
	PeriodWeek,
	YearWeek,
	FiscalWeekDescription,
	StartDate,
	EndDate
FROM
	FiscalWeek
WHERE
	(@StartDate IS NULL OR StartDate > @StartDate)
	AND
	(@EndDate IS NULL OR EndDate < @EndDate)
ORDER BY
	StartDate

END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetFiscalWeeks] TO [IRMAClientRole]
    AS [dbo];

