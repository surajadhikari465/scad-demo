SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].GetAllFacilities') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].GetAllFacilities
GO
/*

	grant exec on dbo.GetAllFacilities to IRMAClientRole
	grant exec on dbo.GetAllFacilities to IRMAReportsRole

	exec GetAllFacilities

*/
CREATE PROCEDURE dbo.GetAllFacilities
AS

	SELECT vendor.CompanyName, vendor.Vendor_ID
	FROM Store
	inner join Vendor
		on store.store_no = vendor.store_no
	where vendor.InternalCustomer > 0
	and Store.Internal > 0
	and (Store.Manufacturer > 0 or Store.Distribution_Center > 0)
	and Store.BusinessUnit_ID is not null 
	order by CompanyName

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO










