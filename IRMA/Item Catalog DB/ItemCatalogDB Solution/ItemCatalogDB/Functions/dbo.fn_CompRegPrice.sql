IF EXISTS (SELECT * 
	   FROM   sysobjects 
	   WHERE  name = N'fn_CompRegPrice')
	DROP FUNCTION fn_CompRegPrice
GO

CREATE FUNCTION dbo.fn_CompRegPrice 
	(@Item_Key int, 
	 @CompetitorStoreId int,
     @Effective_Date datetime)
RETURNS smallmoney
AS
BEGIN
    DECLARE @Price smallmoney

    SELECT @Price = ISNULL((SELECT TOP 1 
			CP.Price / (CASE WHEN CP.PriceMultiple > 1 THEN CP.PriceMultiple ELSE 1 END)
            FROM CompetitorPrice CP (nolock)
            INNER JOIN FiscalWeek FW (nolock)
				ON	CP.FiscalYear	= FW.FiscalYear      
				AND	CP.FiscalPeriod	= FW.FiscalPeriod      
				AND	CP.PeriodWeek	= FW.PeriodWeek
            WHERE CP.Item_Key = @Item_Key
            AND CP.CompetitorStoreId = @CompetitorStoreId
            AND FW.StartDate <= @Effective_Date
            ORDER BY FW.StartDate DESC),
				(SELECT TOP 1 
				CP.Price / (CASE WHEN CP.PriceMultiple > 1 THEN CP.PriceMultiple ELSE 1 END)
				FROM CompetitorPrice CP (nolock)
				INNER JOIN FiscalWeek FW (nolock)
					ON	CP.FiscalYear	= FW.FiscalYear      
					AND	CP.FiscalPeriod	= FW.FiscalPeriod      
					AND	CP.PeriodWeek	= FW.PeriodWeek
				WHERE CP.Item_Key = @Item_Key
				AND CP.CompetitorStoreId = @CompetitorStoreId
				AND FW.StartDate > @Effective_Date
				ORDER BY FW.StartDate ASC))

    RETURN @Price
END
GO
