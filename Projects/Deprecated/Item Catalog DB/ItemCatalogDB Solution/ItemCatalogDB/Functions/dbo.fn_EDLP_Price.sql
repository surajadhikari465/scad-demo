IF EXISTS (SELECT * FROM   sysobjects WHERE  name = N'fn_EDLP_Price') 
    DROP FUNCTION fn_EDLP_Price
GO

CREATE FUNCTION [dbo].[fn_EDLP_Price]
    (@PriceChgTypeID int,
     @POSPrice       smallmoney,
     @POSSale_Price  smallmoney,
     @MSRP_Required  bit)
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

        IF @OnSale = 0 AND @MSRP_Required = 1
            SET @Result = LTRIM(RTRIM(CAST(STR(@POSPrice, 8, 2) AS VARCHAR(8))))
        ELSE            
        IF @OnSale = 1 AND @MSRP_Required = 1
            SET @Result = LTRIM(RTRIM(CAST(STR(@POSSale_Price, 8, 2) AS VARCHAR(8))))
        ELSE
            SET @Result = ''
    END
    
    return @Result
END

GO

