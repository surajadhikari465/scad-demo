 if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[GetCompetitorPriceSearch]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[GetCompetitorPriceSearch]
GO

CREATE PROCEDURE [dbo].[GetCompetitorPriceSearch] 
	@CompetitorID int = NULL,
	@CompetitorLocationID int = NULL,
	@CompetitorStoreID int = NULL,
	@FiscalYear smallint = NULL,
	@FiscalPeriod tinyint = NULL,
	@PeriodWeek tinyint = NULL,
	@ItemIdentifier varchar(13) = NULL
AS
BEGIN

SELECT 
	*
	FROM
		(SELECT
			CP.Item_Key,
			(SELECT TOP 1 Identifier 
				FROM ItemIdentifier 
				WHERE ItemIdentifier.Item_Key = CP.Item_Key 
				ORDER BY Default_Identifier DESC) AS WFMIdentifier,
			CS.CompetitorID,
			CS.CompetitorLocationID,
			CS.CompetitorStoreID,
			CP.FiscalYear,
			CP.FiscalPeriod,
			CP.PeriodWeek,
			C.Name AS Competitor,
			CL.Name AS Location,
			CS.Name AS CompetitorStore,
			CP.UPCCode,
			CP.Description,
			CP.Size,
			CP.Unit_ID,
			CP.PriceMultiple,
			CP.Price,
			CP.SaleMultiple,
			CP.Sale,
			CP.CheckDate,
			CP.UpdateUserID,
			CP.UpdateDateTime
		FROM
			CompetitorPrice CP
			INNER JOIN CompetitorStore CS ON CP.CompetitorStoreID = CS.CompetitorStoreID
			INNER JOIN Competitor C ON CS.CompetitorID = C.CompetitorID
			INNER JOIN CompetitorLocation CL ON CS.CompetitorLocationID = CL.CompetitorLocationID
		WHERE
			(@CompetitorID IS NULL OR C.CompetitorID = @CompetitorID)
			AND
			(@CompetitorLocationID IS NULL OR CL.CompetitorLocationID = @CompetitorLocationID)
			AND
			(@CompetitorStoreID IS NULL OR CS.CompetitorStoreID = @CompetitorStoreID)
			AND
			(@FiscalYear IS NULL OR CP.FiscalYear = @FiscalYear)
			AND
			(@FiscalPeriod IS NULL OR CP.FiscalPeriod = @FiscalPeriod)
			AND
			(@PeriodWeek IS NULL OR CP.PeriodWeek = @PeriodWeek)) X1
	WHERE
	(@ItemIdentifier IS NULL OR @ItemIdentifier = X1.WFMIdentifier)
	ORDER BY
		Competitor,
		Location,
		CompetitorStore

END
GO