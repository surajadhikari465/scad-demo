CREATE FUNCTION [dbo].[fn_SaleItemUnitsPerPrice]
    (@PriceChgTypeID int,
     @SaleMultiple   tinyint)
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
            SET @Result = CAST(@SaleMultiple AS VARCHAR(8))
    END
    
    return @Result
END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[fn_SaleItemUnitsPerPrice] TO [IRMAClientRole]
    AS [dbo];

