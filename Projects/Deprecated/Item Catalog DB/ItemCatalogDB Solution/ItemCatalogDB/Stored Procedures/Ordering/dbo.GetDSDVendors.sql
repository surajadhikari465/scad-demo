SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[GetDSDVendors]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[GetDSDVendors]
GO

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

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

