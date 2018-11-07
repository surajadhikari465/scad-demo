CREATE FUNCTION dbo.fn_PriceHistoryRegPrice 
	(@Item_Key int, 
	 @Store_No int,
     @Effective_Date datetime)
RETURNS smallmoney
AS
BEGIN
    DECLARE @Price smallmoney

    SELECT @Price = (SELECT TOP 1 Price / (CASE WHEN Multiple > 1 THEN Multiple ELSE 1 END)
                    FROM PriceHistory (nolock)
                    WHERE Item_Key = @Item_Key
                    AND Store_No = @Store_No
                    AND Effective_Date <= @Effective_Date
                    ORDER BY Effective_Date DESC)

    RETURN @Price
END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[fn_PriceHistoryRegPrice] TO [IRMASupportRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[fn_PriceHistoryRegPrice] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[fn_PriceHistoryRegPrice] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[fn_PriceHistoryRegPrice] TO [IRMAReportsRole]
    AS [dbo];

