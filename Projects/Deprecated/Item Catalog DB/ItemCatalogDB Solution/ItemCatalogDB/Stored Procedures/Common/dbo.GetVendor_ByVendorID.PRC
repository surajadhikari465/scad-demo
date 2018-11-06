
/****** Object:  StoredProcedure [dbo].[GetVendor_ByVendorID]    Script Date: 10/13/2006 14:12:39 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetVendor_ByVendorID]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[GetVendor_ByVendorID]
go

/****** Object:  StoredProcedure [dbo].[GetVendor_ByVendorID]    Script Date: 10/13/2006 14:13:05 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
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
