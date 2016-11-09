CREATE PROCEDURE [dbo].[GetCompetitorImportInfoExistingCount] 
	@CompetitorImportSessionID int
AS
BEGIN

SELECT 
	COUNT(*)
FROM 
	CompetitorImportInfo CII
	INNER JOIN CompetitorPrice CP ON 
		CII.Item_Key = CP.Item_Key
		AND
		CII.CompetitorStoreID = CP.CompetitorStoreID
		AND
		CII.FiscalYear = CP.FiscalYear
		AND
		CII.FiscalPeriod = CP.FiscalPeriod
		AND
		CII.PeriodWeek = CP.PeriodWeek
WHERE
	CII.CompetitorImportSessionID = @CompetitorImportSessionID

END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetCompetitorImportInfoExistingCount] TO [IRMAClientRole]
    AS [dbo];

