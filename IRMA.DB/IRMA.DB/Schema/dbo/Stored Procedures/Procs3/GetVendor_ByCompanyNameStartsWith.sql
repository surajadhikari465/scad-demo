CREATE PROCEDURE [dbo].[GetVendor_ByCompanyNameStartsWith] 
	@Start varchar(52)
AS
BEGIN

SELECT @Start = @Start + '%'

	select 
		rtrim(companyname) [Value],
		vendor_id [ID] 
	from 
		[Vendor] 
	where 
		CompanyName like @Start
	order by companyname


END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetVendor_ByCompanyNameStartsWith] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetVendor_ByCompanyNameStartsWith] TO [IRMASLIMRole]
    AS [dbo];

