CREATE PROCEDURE [dbo].[IsDSDStoreVendorByUPC]
	@UPC      VARCHAR(13),
	@Store_No INT
	
AS
-- **************************************************************************
-- Procedure: IsDSDStoreVendorByUPC()
--    Author: Amudha Sethuraman
--      Date: 11/05/2012
--
-- Description:
-- Validates if a particular UPC scanned for a Store using mobile device is 
-- provided by a DSD Vendor. If so the screen displays msg preventing PO creation for DSD vendor.
--
-- Modification History:
-- Date        Init		Comment
-- 11/05/2012  AS		Creation
--
-- ***************************************************************************
BEGIN
	SELECT CASE 
			WHEN COUNT(*) <> 0 
				THEN 1 
				ELSE 0
			END AS IsDSDVendor
		FROM DSDVendorStore DVS (NOLOCK)
			INNER JOIN StoreItemVendor SIV (NOLOCK) ON DVS.Store_No = SIV.Store_No 
													AND DVS.Vendor_ID = SIV.Vendor_ID 
													AND (SIV.DeleteDate IS NULL OR SIV.DeleteDate > GETDATE())
													AND SIV.PrimaryVendor = 1
			INNER JOIN ItemIdentifier II (NOLOCK) ON SIV.Item_Key = II.Item_Key 
		WHERE SIV.Store_No = @Store_No
			AND II.Identifier = @UPC
			
END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[IsDSDStoreVendorByUPC] TO [IRMAAdminRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[IsDSDStoreVendorByUPC] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[IsDSDStoreVendorByUPC] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[IsDSDStoreVendorByUPC] TO [IRMAReportsRole]
    AS [dbo];

