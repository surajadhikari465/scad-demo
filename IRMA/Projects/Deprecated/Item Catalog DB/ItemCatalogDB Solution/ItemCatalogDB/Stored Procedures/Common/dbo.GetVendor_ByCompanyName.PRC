/* Use this stored procedure for vendor search menus. 
 * User can enter a search string for company name and get back a list of vendor ids and company names that match. 
 */

/****** Object:  StoredProcedure [dbo].[GetVendor_ByCompanyName]    Script Date: 10/13/2006 10:39:04 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetVendor_ByCompanyName]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[GetVendor_ByCompanyName]
go

/****** Object:  StoredProcedure [dbo].[GetVendor_ByCompanyName]    Script Date: 10/13/2006 10:38:26 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
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
go