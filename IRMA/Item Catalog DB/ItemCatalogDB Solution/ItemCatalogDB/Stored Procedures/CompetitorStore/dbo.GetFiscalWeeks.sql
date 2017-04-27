 if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[GetFiscalWeeks]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[GetFiscalWeeks]
GO

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
go  