IF EXISTS (SELECT * FROM   sysobjects WHERE  name = N'fn_EDLP_UnitPrice') 
    DROP FUNCTION fn_EDLP_UnitPrice
GO

CREATE FUNCTION [dbo].[fn_EDLP_UnitPrice]
    (@PriceChgTypeID int,
     @Multiple       tinyint,
     @SaleMultiple   tinyint,
     @POSPrice       smallmoney,
     @POSSale_Price  smallmoney,
     @Package_Desc1  decimal(9,4),
     @Package_Desc2  decimal(9,4),
     @TaxJurDesc     varchar(30),
     @UnitAbbr       varchar(5),
     @ItemAttText_6  varchar(50),
     @MSRP_Required  bit)
RETURNS VARCHAR(8)
AS
BEGIN
	DECLARE @OnSale     BIT
    DECLARE @TempResult SMALLMONEY
	DECLARE @Result     VARCHAR(8)
	
    SET @TempResult = 0

    IF @PriceChgTypeID IS NULL
        SET @Result = ''
    ELSE        
    BEGIN
	    select @OnSale = isnull((select On_Sale
					    from	PriceChgType
					    where	PriceChgTypeID = @PriceChgTypeID),0)

        IF @OnSale = 0 AND @MSRP_Required = 1
            IF @Multiple IS NOT NULL
                IF @Multiple > 0
                    SET @TempResult = ((@POSPrice / @Multiple) / (@Package_Desc1 * @Package_Desc2)) * dbo.fn_GetUOMConversionFactor(@TaxJurDesc, @UnitAbbr, @ItemAttText_6)
        
        IF @OnSale = 1 AND @MSRP_Required = 1
            IF @SaleMultiple IS NOT NULL
                IF @SaleMultiple > 0
                    SET @TempResult = ((@POSSale_Price / @SaleMultiple) / (@Package_Desc1 * @Package_Desc2)) * dbo.fn_GetUOMConversionFactor(@TaxJurDesc, @UnitAbbr, @ItemAttText_6)

        IF @TempResult = 0
            SET @Result = ''
        ELSE
            SET @Result = LTRIM(RTRIM(CAST(STR(@TempResult, 8, 3) AS VARCHAR(8))))

    END
    
    return @Result
END


GO
