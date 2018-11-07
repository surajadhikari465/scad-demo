SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'[dbo].[VendorItemCount]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[VendorItemCount]
GO

CREATE PROCEDURE [dbo].[VendorItemCount]
AS
BEGIN
	SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED

	SELECT
		Vendor.Vendor_Id,
		Vendor.Vendor_Key,
		Vendor.CompanyName,
	   (SELECT count(*) FROM ItemVendor iv WHERE iv.Vendor_id = Vendor.Vendor_id and iv.DeleteDate IS NULL) as ItemCount
	FROM
		Vendor
	WHERE
	   (Store_no IS NULL OR
		Store_no = '')
	ORDER BY 
		Vendor.CompanyName
END
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO
