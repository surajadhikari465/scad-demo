if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[Reporting_CompetitorTrend]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[Reporting_CompetitorTrend]
GO

CREATE PROCEDURE dbo.Reporting_CompetitorTrend
	@Store_NoList varchar(MAX) = NULL,
	@CompetitorStoreIDs varchar(MAX) = NULL,
	@CompetitivePriceTypeID int = NULL,
	@Item_Key int,
	@RegularPrice bit,
	@StartFiscalYear smallint,
	@StartFiscalPeriod tinyint,
	@StartPeriodWeek tinyint,
	@EndFiscalYear smallint,
	@EndFiscalPeriod tinyint,
	@EndPeriodWeek tinyint
AS 
BEGIN

DECLARE @SQL varchar(MAX)
DECLARE @PivotFromSQL varchar(MAX)
DECLARE @PivotSelectSQL varchar(MAX)
DECLARE @StartDate smalldatetime
DECLARE @EndDate smalldatetime

-- Get the date range represented by the fiscal weeks selected
SELECT 
	@StartDate = StartDate
FROM
	FiscalWeek
WHERE
	FiscalYear = @StartFiscalYear
	AND
	FiscalPeriod = @StartFiscalPeriod
	AND
	PeriodWeek = @StartPeriodWeek

SELECT
	@EndDate = EndDate
FROM
	FiscalWeek
WHERE
	FiscalYear = @EndFiscalYear
	AND
	FiscalPeriod = @EndFiscalPeriod
	AND
	PeriodWeek = @EndPeriodWeek


-- Create the pivot column text
DECLARE @Index int
DECLARE @FiscalWeekDescription varchar(50)
DECLARE @IndexColumnName varchar(20)

SET @PivotFromSQL = ''
SET @PivotSelectSQL = ''

DECLARE FWCursor CURSOR FOR
	SELECT
		ROW_NUMBER() OVER(ORDER BY StartDate),
		FiscalWeekDescription
	FROM
		FiscalWeek
	WHERE
		StartDate BETWEEN @StartDate AND @EndDate

OPEN FWCursor
	FETCH NEXT FROM FWCursor INTO @Index, @FiscalWeekDescription

WHILE @@FETCH_STATUS = 0
BEGIN
	SET @IndexColumnName = '[' + CONVERT(varchar(20), @Index) + ']'
	SET @PivotFromSQL = @PivotFromSQL + @IndexColumnName + ', '
	SET @PivotSelectSQL = @PivotSelectSQL + @IndexColumnName + ' AS [' + @FiscalWeekDescription + '], '

	FETCH NEXT FROM FWCursor INTO @Index, @FiscalWeekDescription
END

CLOSE FWCursor
DEALLOCATE FWCursor

SET @PivotFromSQL = SUBSTRING(@PivotFromSQL, 0, LEN(@PivotFromSQL))
SET @PivotSelectSQL = SUBSTRING(@PivotSelectSQL, 0, LEN(@PivotSelectSQL))


-- Start building the query proper
SET @SQL = 
'DECLARE @FiscalWeeks TABLE
	(FiscalWeekID int identity(1,1), 
	FiscalYear smallint, 
	FiscalPeriod tinyint, 
	PeriodWeek tinyint,
	StartDate smalldatetime,
	EndDate smalldatetime) '

-- Get the set of fiscal weeks to report across
SET @SQL = @SQL + 
'INSERT INTO @FiscalWeeks
	(FiscalYear, FiscalPeriod, PeriodWeek, StartDate, EndDate)
	SELECT
		FiscalYear,
		FiscalPeriod,
		PeriodWeek,
		StartDate,
		EndDate
	FROM
		FiscalWeek
	WHERE
		StartDate BETWEEN ''' + CONVERT(varchar(20), @StartDate) + ''' AND ''' + CONVERT(varchar(20), @EndDate) + ''' '

IF @Store_NoList IS NOT NULL
BEGIN
	-- WFM price data
	SET @SQL = @SQL + 
		'SELECT 
			S.Store_Name AS Store_Name, ' + 
		@PivotSelectSQL + 
		' FROM
			(SELECT 
				S.Key_Value AS Store_No,
				FW.FiscalWeekID, 
				dbo.' + 
		CASE @RegularPrice WHEN 1 THEN 'fn_PriceHistoryRegPrice' ELSE 'fn_PriceHistoryPrice' END + 
		'(' + CONVERT(varchar(20), @Item_Key) + ', S.Key_Value, FW.EndDate) AS Price ' +
			'FROM
				fn_Parse_List(''' + @Store_NoList + ''', '','') S 
				CROSS JOIN @FiscalWeeks FW) PIVOTME
			PIVOT ( MAX(Price) FOR FiscalWeekID IN (' + @PivotFromSQL + ') ) as pvt
			INNER JOIN Store S ON pvt.Store_No = S.Store_No '
END

IF @Store_NoList IS NOT NULL AND @CompetitorStoreIDs IS NOT NULL
	SET @SQL = @SQL + ' UNION '

IF @CompetitorStoreIDs IS NOT NULL
BEGIN
	-- Competitor price data
	SET @SQL = @SQL + 
	'SELECT 
		C.Name + '' - '' + CL.Name + '' - '' + CS.Name AS Store_Name, '
	+ @PivotSelectSQL + 	
	' FROM
		(SELECT 
			CS.Key_Value AS CompetitorStoreID,
			FW.FiscalWeekID, 
			dbo.' + 
		CASE @RegularPrice WHEN 1 THEN 'fn_CompRegPrice' ELSE 'fn_CompPrice' END + 
		'(' + CONVERT(varchar(20), @Item_Key) + ', CS.Key_Value, FW.EndDate) AS Price ' +
			'FROM
			fn_Parse_List(''' + @CompetitorStoreIDs + ''', '','') CS 
				CROSS JOIN @FiscalWeeks FW) PIVOTME
		PIVOT ( MAX(Price) FOR FiscalWeekID IN (' + @PivotFromSQL + ') ) as pvt
		INNER JOIN CompetitorStore CS ON pvt.CompetitorStoreID = CS.CompetitorStoreID
		INNER JOIN Competitor C ON CS.CompetitorID = C.CompetitorID
		INNER JOIN CompetitorLocation CL ON CL.CompetitorLocationID = CS.CompetitorLocationID'
END

EXEC(@SQL)

END
GO