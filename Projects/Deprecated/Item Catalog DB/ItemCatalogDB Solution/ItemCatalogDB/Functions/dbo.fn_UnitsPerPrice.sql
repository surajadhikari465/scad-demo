IF EXISTS (SELECT * FROM   sysobjects WHERE  name = N'fn_UnitsPerPrice') 
    DROP FUNCTION fn_UnitsPerPrice
GO

CREATE FUNCTION [dbo].[fn_UnitsPerPrice]
    (@PriceChgTypeID int,
     @Multiple       tinyint,
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
            SET @Result = CAST(@Multiple AS VARCHAR(8))
        ELSE
            SET @Result = CAST(@SaleMultiple AS VARCHAR(8))
    END
    
    return @Result
END

GO
