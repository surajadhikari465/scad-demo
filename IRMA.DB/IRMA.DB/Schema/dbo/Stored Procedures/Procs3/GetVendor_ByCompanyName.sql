CREATE PROCEDURE dbo.[GetVendor_ByCompanyName]
	@Find varchar(50)
as
begin
	set nocount on
	
	declare @Search varchar(51)	
	select @Search = ('%' + @Find + '%')	

	select
		vendor.vendor_id,
		vendor.companyname
	from 
		vendor (nolock)
	where 
		vendor.companyname Like @Search
	order by 
		vendor.companyname

	set nocount off
end
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetVendor_ByCompanyName] TO [IRMASupportRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetVendor_ByCompanyName] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetVendor_ByCompanyName] TO [IRMAReportsRole]
    AS [dbo];

