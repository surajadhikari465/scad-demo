CREATE FUNCTION dbo.fn_CompPrice 
	(@Item_Key int, 
	 @CompetitorStoreId int,
     @Effective_Date datetime)
RETURNS smallmoney
AS
BEGIN
    DECLARE @Price smallmoney

    SELECT @Price = ISNULL((SELECT TOP 1 
			CASE WHEN CP.Sale IS NOT NULL THEN
				CP.Sale / (CASE WHEN CP.SaleMultiple > 1 THEN CP.SaleMultiple ELSE 1 END)
			ELSE
				CP.Price / (CASE WHEN CP.PriceMultiple > 1 THEN CP.PriceMultiple ELSE 1 END)
			END
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
				CASE WHEN CP.Sale IS NOT NULL THEN
					CP.Sale / (CASE WHEN CP.SaleMultiple > 1 THEN CP.SaleMultiple ELSE 1 END)
				ELSE
					CP.Price / (CASE WHEN CP.PriceMultiple > 1 THEN CP.PriceMultiple ELSE 1 END)
				END
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
GRANT EXECUTE
    ON OBJECT::[dbo].[fn_CompPrice] TO [IRMASupportRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[fn_CompPrice] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[fn_CompPrice] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[fn_CompPrice] TO [IRMAReportsRole]
    AS [dbo];

