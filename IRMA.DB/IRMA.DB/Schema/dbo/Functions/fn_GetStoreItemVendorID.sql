create function [dbo].[fn_GetStoreItemVendorID]
	(@Item_Key int, @Store_No int)
returns int
as
begin
	declare @SIV_ID int

	select @SIV_ID = (
		select 
			top 1 siv.storeitemvendorid
		from
			storeitemvendor siv (nolock)
		where
			siv.item_key = @Item_Key
			and siv.store_no = @Store_No
			and siv.primaryvendor = 1
	)
	return @SIV_ID
end
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[fn_GetStoreItemVendorID] TO [IRMASupportRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[fn_GetStoreItemVendorID] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[fn_GetStoreItemVendorID] TO [IRMAReportsRole]
    AS [dbo];

