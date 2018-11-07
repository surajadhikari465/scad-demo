CREATE PROCEDURE dbo.[GetVendor_ByVendorID]
	@Find int
as
begin
	set nocount on

	select 
		vendor.vendor_id,
		vendor.companyname
	from
		vendor (nolock)
	where 
		vendor.vendor_id = @Find
	order by vendor.companyname

	set nocount off
end
SET QUOTED_IDENTIFIER ON
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetVendor_ByVendorID] TO [IRMASupportRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetVendor_ByVendorID] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetVendor_ByVendorID] TO [IRMAReportsRole]
    AS [dbo];

