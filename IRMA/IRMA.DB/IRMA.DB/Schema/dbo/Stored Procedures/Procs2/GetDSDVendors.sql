CREATE PROCEDURE [dbo].[GetDSDVendors] 
	@iStoreNo int
AS
-- ********************************************************************************
-- Procedure: [GetDSDVendor]
--    Author: Amudha Sethuraman
--      Date: 10/02/2012
--
-- Description:
-- This procedure is called from retrieve DSD Vendors by Store No. for WFM Dropdown
-- **********************************************************************************
BEGIN

SET NOCOUNT ON

	SELECT V.Vendor_ID  As VendorID
		  ,V.CompanyName As VendorName
	  FROM DSDVendorStore DSDV (NOLOCK)
	  INNER JOIN Vendor V (NOLOCK) ON DSDV.Vendor_ID = V.Vendor_ID 
	  WHERE DSDV.Store_No = @iStoreNo
		AND DSDV.BeginDate <= GETDATE()
  
 SET NOCOUNT OFF  
 
END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetDSDVendors] TO [IRMAAdminRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetDSDVendors] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetDSDVendors] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetDSDVendors] TO [IRMAReportsRole]
    AS [dbo];

