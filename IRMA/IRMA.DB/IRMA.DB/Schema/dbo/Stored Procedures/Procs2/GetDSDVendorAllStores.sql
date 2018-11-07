CREATE PROCEDURE [dbo].[GetDSDVendorAllStores]
	 @Vendor_ID int
AS
-- **************************************************************************
-- Procedure: GetDSDVendorAllStores()
--    Author: Hui Kou
--      Date: 09.25.2012
--
-- Description:
-- This procedure is called from the WFM Mobile app to return item data to the 
-- order interface
--
-- Modification History:
-- Date       	Init  	TFS   	Comment
-- 10/10/2012	HK   	7419	Creation

-- **************************************************************************
BEGIN
    SET NOCOUNT ON  

	select 
		Store_No = s.Store_No
		,Store_Name = s.Store_Name
		,IsReceivingDocument = case when dvs.store_no is null then 0 else 1 end
	from
		store s
		left join DSDVendorStore  dvs
			on s.store_no = dvs.store_no and dvs.vendor_id = @vendor_id
	where
		(mega_store = 1 or wfm_store = 1 or distribution_center = 1 or manufacturer = 1)

    SET NOCOUNT OFF
END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetDSDVendorAllStores] TO [IRMAAdminRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetDSDVendorAllStores] TO [IRMASupportRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetDSDVendorAllStores] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetDSDVendorAllStores] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetDSDVendorAllStores] TO [IRMAReportsRole]
    AS [dbo];

