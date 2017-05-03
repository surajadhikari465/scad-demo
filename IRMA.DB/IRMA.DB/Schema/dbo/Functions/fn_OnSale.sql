CREATE function dbo.fn_OnSale
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
GRANT EXECUTE
    ON OBJECT::[dbo].[fn_OnSale] TO [IRMAAdminRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[fn_OnSale] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[fn_OnSale] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[fn_OnSale] TO [IRMAReportsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[fn_OnSale] TO [IMHARole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[fn_OnSale] TO [DataMigration]
    AS [dbo];

