if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[DeleteCompetitorPrice]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[DeleteCompetitorPrice]
GO

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
go    