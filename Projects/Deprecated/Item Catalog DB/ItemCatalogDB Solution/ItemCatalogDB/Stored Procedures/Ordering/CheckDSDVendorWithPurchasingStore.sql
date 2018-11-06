IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[CheckDSDVendorWithPurchasingStore]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[CheckDSDVendorWithPurchasingStore]
GO

/****** Object:  StoredProcedure [dbo].[CheckDSDVendorWithPurchasingStore]    Script Date: 10/09/2012 10:52:52 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

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

