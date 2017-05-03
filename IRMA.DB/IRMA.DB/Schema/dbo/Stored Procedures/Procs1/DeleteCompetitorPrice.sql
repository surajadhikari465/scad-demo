CREATE PROCEDURE [dbo].[DeleteCompetitorPrice] 
	@Item_Key int,
	@CompetitorStoreID int,
	@FiscalYear smallint,
	@FiscalPeriod tinyint,
	@PeriodWeek tinyint
AS
BEGIN

	DELETE FROM 
		CompetitorPrice
	WHERE
		Item_Key = @Item_Key
		AND
		CompetitorStoreID = @CompetitorStoreID
		AND
		FiscalYear = @FiscalYear
		AND
		FiscalPeriod = @FiscalPeriod
		AND
		PeriodWeek = @PeriodWeek
END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[DeleteCompetitorPrice] TO [IRMAClientRole]
    AS [dbo];

