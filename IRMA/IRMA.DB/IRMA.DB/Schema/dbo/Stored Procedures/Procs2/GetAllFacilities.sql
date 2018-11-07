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
GRANT EXECUTE
    ON OBJECT::[dbo].[GetAllFacilities] TO [IRMAAdminRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetAllFacilities] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetAllFacilities] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetAllFacilities] TO [IRMAReportsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetAllFacilities] TO [IRMAExcelRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetAllFacilities] TO [IRMASLIMRole]
    AS [dbo];

