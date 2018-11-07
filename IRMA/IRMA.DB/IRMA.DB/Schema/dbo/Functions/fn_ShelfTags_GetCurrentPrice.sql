create function dbo.fn_ShelfTags_GetCurrentPrice
    (@PriceChgTypeID int, 
     @PricingMethod_ID int, 
     @POSPrice smallmoney, 
     @POSSale_Price smallmoney)
returns smallmoney
as
begin

    declare @flag int
    declare @result smallmoney
    
    --select @flag = (CASE WHEN @PriceChgTypeID IN (2,3) THEN 1 ELSE 0 END)
	select @flag = On_Sale FROM PriceChgType where PriceChgTypeID = @PriceChgTypeID
    select @result = (CASE WHEN @flag = 1 
        THEN 
            CASE @PricingMethod_ID WHEN 0 THEN @POSSale_Price 
                                  WHEN 1 THEN @POSSale_Price
                                  WHEN 2 THEN @POSPrice
                                  WHEN 4 THEN @POSPrice END
        ELSE @POSPrice END)
    return @result
end