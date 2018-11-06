ALTER function dbo.fn_OnSale
    (@PriceChgTypeID int)
returns bit
as
begin
	declare @result bit

	select @result = isnull((select On_Sale
					from	PriceChgType
					where	PriceChgTypeID = @PriceChgTypeID),0)

    return @result
end    
GO
