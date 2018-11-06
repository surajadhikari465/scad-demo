IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[CheckVendorIsDSDForStore]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[CheckVendorIsDSDForStore]
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


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