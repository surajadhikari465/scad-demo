if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[GetVendor_ByCompanyNameStartsWith]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[GetVendor_ByCompanyNameStartsWith]
GO

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
go