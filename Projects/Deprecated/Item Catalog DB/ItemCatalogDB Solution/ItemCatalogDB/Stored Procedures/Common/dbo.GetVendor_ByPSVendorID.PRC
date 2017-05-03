
/****** Object:  StoredProcedure [dbo].[GetVendor_ByPSVendorID]    Script Date: 10/13/2006 10:39:04 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetVendor_ByPSVendorID]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[GetVendor_ByPSVendorID]
go

/****** Object:  StoredProcedure [dbo].[GetVendor_ByPSVendorID]    Script Date: 10/13/2006 10:38:26 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
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
go
