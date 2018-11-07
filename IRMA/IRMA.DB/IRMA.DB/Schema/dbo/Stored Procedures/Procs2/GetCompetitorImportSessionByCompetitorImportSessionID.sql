CREATE PROCEDURE [dbo].[GetCompetitorImportSessionByCompetitorImportSessionID] 
	@CompetitorImportSessionID int
AS
BEGIN

SELECT
	CompetitorImportInfoID,
	CompetitorImportSessionID, 
	Item_Key,
	(SELECT TOP 1 Identifier 
		FROM ItemIdentifier 
		WHERE ItemIdentifier.Item_Key = CII.Item_Key 
		ORDER BY Default_Identifier DESC) AS WFMIdentifier,
	CompetitorID,
	CompetitorLocationID,
	CompetitorStoreID,
	FiscalYear,
	FiscalPeriod,
	PeriodWeek,
	Competitor,
	Location,
	CompetitorStore,
	UPCCode,
	Description,
	Size,
	Unit_ID,
	PriceMultiple,
	Price,
	SaleMultiple,
	Sale,
	DateChecked
FROM
	CompetitorImportInfo CII
WHERE
	CompetitorImportSessionID = @CompetitorImportSessionID
ORDER BY
	Competitor,
	Location,
	CompetitorStore

END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetCompetitorImportSessionByCompetitorImportSessionID] TO [IRMAClientRole]
    AS [dbo];

