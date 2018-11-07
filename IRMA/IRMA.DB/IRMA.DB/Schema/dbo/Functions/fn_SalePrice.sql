CREATE FUNCTION [dbo].[fn_SalePrice]
    (@PriceChgTypeID int,
     @POSSale_Price  smallmoney)
RETURNS VARCHAR(8)
AS
BEGIN
	declare @OnSale BIT
	DECLARE @Result VARCHAR(8)

    IF @PriceChgTypeID IS NULL
        SET @Result = ''
    ELSE        
    BEGIN
	    select @OnSale = isnull((select On_Sale
					    from	PriceChgType
					    where	PriceChgTypeID = @PriceChgTypeID),0)

        IF @OnSale = 0
            SET @Result = ''
        ELSE
            SET @Result = LTRIM(RTRIM(CAST(STR(@POSSale_Price, 8, 2) AS VARCHAR(8))))
    END
    
    return @Result
END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[fn_SalePrice] TO [IRMAClientRole]
    AS [dbo];

