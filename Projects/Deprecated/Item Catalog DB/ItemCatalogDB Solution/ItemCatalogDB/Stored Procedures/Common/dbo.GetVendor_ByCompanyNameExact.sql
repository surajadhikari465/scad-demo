if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[GetVendor_ByCompanyNameExact]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[GetVendor_ByCompanyNameExact]
GO


CREATE PROCEDURE dbo.GetVendor_ByCompanyNameExact
	@CompanyName varchar(50)
AS 

SELECT 
	Vendor_ID,
	Vendor_Key,
	CompanyName
FROM 
	Vendor
WHERE 
	CompanyName = @CompanyName

GO 