SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

IF EXISTS (SELECT * FROM sysobjects WHERE type = 'P' AND name = 'SOG_GetVendorList')
	BEGIN
		DROP Procedure [dbo].SOG_GetVendorList
	END
GO

CREATE PROCEDURE dbo.SOG_GetVendorList
	@Catalog	bit
WITH RECOMPILE
AS

-- **************************************************************************
-- Procedure: SOG_GetVendorList
--    Author: Billy Blackerby
--      Date: 4/11/2009
--
-- Description:
-- Utilized by StoreOrderGuide to return a list of vendors for filters
--
-- Modification History:
-- Date			Init	Comment
-- 04/11/2009	BBB		Creation
-- **************************************************************************	
BEGIN
    SET NOCOUNT ON
	--**************************************************************************
	--Main SQL
	--**************************************************************************
	IF @Catalog = 1
		BEGIN
			SELECT DISTINCT
				[VendorID]		= v.Vendor_ID,
				[VendorName]	= v.CompanyName
			FROM 
				Vendor		(nolock) v
			WHERE
				v.Vendor_ID > 0
			ORDER BY 
				VendorID, 
				VendorName
		END
	ELSE
		BEGIN
			SELECT
				[VendorID]		= 0,
				[VendorName]	= 'All Vendors'
				
			UNION
			
			SELECT DISTINCT
				[VendorID]		= v.Vendor_ID,
				[VendorName]	= v.CompanyName
			FROM 
				Vendor		(nolock) v
			WHERE
				v.Vendor_ID > 0
			ORDER BY 
				VendorID, 
				VendorName
		END

    SET NOCOUNT OFF
END

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO