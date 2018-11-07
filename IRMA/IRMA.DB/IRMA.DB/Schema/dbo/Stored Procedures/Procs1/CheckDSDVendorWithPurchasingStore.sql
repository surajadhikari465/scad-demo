CREATE PROCEDURE [dbo].[CheckDSDVendorWithPurchasingStore]
	@Vendor_ID INT,
	@Purchasing_Store_Vendor_ID INT
AS
-- **************************************************************************
-- Procedure: CheckDSDVendorWithPurchasingStore()
--    Author: Damon Floyd
--      Date: 10/09/2012
--
-- Description:
-- Checks to see if a particular Vendor is a DSD Vendor for the purchasing
-- store selected on the Order Add screen.  (Uses Vendor ID, inexplicably)
--
-- Modification History:
-- Date        Init		Comment
-- 10/09/2012  DF		Creation
--
-- ***************************************************************************
BEGIN
	DECLARE @IsDSDVendor BIT = 0
    -- Insert statements for procedure here
    IF EXISTS (
		SELECT dvs.Store_No FROM DSDVendorStore dvs WITH (nolock)
		LEFT OUTER JOIN Vendor v ON dvs.Store_No = v.Store_no
		WHERE dvs.Vendor_ID = @Vendor_ID AND v.Vendor_ID = @Purchasing_Store_Vendor_ID
	)
	BEGIN
		SELECT @IsDSDVendor = 1
	END
	
	SELECT @IsDSDVendor As IsDSDVendor
END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[CheckDSDVendorWithPurchasingStore] TO [IRMAAdminRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[CheckDSDVendorWithPurchasingStore] TO [IRMASupportRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[CheckDSDVendorWithPurchasingStore] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[CheckDSDVendorWithPurchasingStore] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[CheckDSDVendorWithPurchasingStore] TO [IRMAReportsRole]
    AS [dbo];

