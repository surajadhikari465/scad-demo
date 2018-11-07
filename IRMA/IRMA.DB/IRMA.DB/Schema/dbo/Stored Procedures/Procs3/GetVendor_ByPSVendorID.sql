CREATE PROCEDURE dbo.[GetVendor_ByPSVendorID]
	@Find varchar(10)
as
begin
	set nocount on
	
	declare @Search varchar(12)	
	select @Search = ('%' + @Find + '%')		

	select
		vendor.vendor_id,
		vendor.companyname,
		vendor.ps_vendor_id
	from 
		vendor (nolock)
	where 
		vendor.ps_vendor_id Like @Search
	order by 
		vendor.companyname

	set nocount off
end
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetVendor_ByPSVendorID] TO [IRMASupportRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetVendor_ByPSVendorID] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetVendor_ByPSVendorID] TO [IRMAReportsRole]
    AS [dbo];

