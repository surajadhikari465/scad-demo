-- =================================================================================
-- Author:		Sekhara
-- Create date: 11/30/2007
-- To display all Facilities in a drop down box for all DailyItemAverageCost Change report.
-- =================================================================================
CREATE PROCEDURE [dbo].[GetAvgCostAllFacilities]

AS
BEGIN

SET NOCOUNT ON

-- Fetching all Facilities to show in the ListBox.
SELECT distinct Store.Store_Name, store.Store_No 
FROM Store
inner join Vendor
on store.store_no = vendor.store_no
where Vendor.InternalCustomer > 0
and Store.Internal > 0
and (Store.Manufacturer > 0 or Store.Distribution_Center > 0)
order by Store.Store_Name 
END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetAvgCostAllFacilities] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetAvgCostAllFacilities] TO [IRMAReportsRole]
    AS [dbo];

