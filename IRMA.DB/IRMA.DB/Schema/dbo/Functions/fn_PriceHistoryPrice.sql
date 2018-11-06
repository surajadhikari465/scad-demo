CREATE FUNCTION dbo.fn_PriceHistoryPrice 
	(@Item_Key int, 
	 @Store_No int,
     @Effective_Date datetime)
RETURNS smallmoney
AS
BEGIN
    DECLARE @Price smallmoney

    SELECT @Price = (SELECT TOP 1 dbo.fn_Price(PriceChgTypeId, Multiple, Price, PricingMethod_ID, Sale_Multiple, Sale_Price)
                    FROM PriceHistory (nolock)
                    WHERE Item_Key = @Item_Key
                    AND Store_No = @Store_No
                    AND Effective_Date <= @Effective_Date
                    ORDER BY Effective_Date DESC)

    RETURN @Price
END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[fn_PriceHistoryPrice] TO [IRMASupportRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[fn_PriceHistoryPrice] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[fn_PriceHistoryPrice] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[fn_PriceHistoryPrice] TO [IRMAReportsRole]
    AS [dbo];

