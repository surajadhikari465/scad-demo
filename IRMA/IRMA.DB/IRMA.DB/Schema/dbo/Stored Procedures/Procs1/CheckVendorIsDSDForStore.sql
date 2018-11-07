CREATE PROCEDURE [dbo].[CheckVendorIsDSDForStore]
	@Vendor_ID INT,
	@Store_No INT
AS
-- **************************************************************************
-- Procedure: CheckVendorIsDSDForStore()
--    Author: Damon Floyd
--      Date: 10/04/2012
--
-- Description:
-- Checks to see if a particular Vendor is a DSD Vendor for a particular Store
--
-- Modification History:
-- Date        Init		Comment
-- 10/04/2012  DF		Creation
--
-- ***************************************************************************
BEGIN
	DECLARE @IsDSDVendorForStore BIT

	IF EXISTS (SELECT DSDVendorStoreID FROM DSDVendorStore (NOLOCK) WHERE Vendor_ID = @Vendor_ID AND Store_No = @Store_No)
		SELECT @IsDSDVendorForStore = 1
	ELSE
		SELECT @IsDSDVendorForStore = 0
		
	SELECT @IsDSDVendorForStore AS IsDSDVendorForStore
END
SET QUOTED_IDENTIFIER ON
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[CheckVendorIsDSDForStore] TO [IRMAAdminRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[CheckVendorIsDSDForStore] TO [IRMASupportRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[CheckVendorIsDSDForStore] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[CheckVendorIsDSDForStore] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[CheckVendorIsDSDForStore] TO [IRMAReportsRole]
    AS [dbo];

