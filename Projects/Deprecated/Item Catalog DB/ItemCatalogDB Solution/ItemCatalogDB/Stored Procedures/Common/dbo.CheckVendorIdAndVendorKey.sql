IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[CheckVendorIdAndVendorKey]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[CheckVendorIdAndVendorKey]
GO

-- Verify that the vendor id and vendor key values correspond to the same record in the Vendor table
CREATE PROCEDURE dbo.CheckVendorIdAndVendorKey
	@Vendor_ID int,
	@Vendor_Key varchar(10),
	@VendorMatch bit OUTPUT
AS
BEGIN
	SELECT @VendorMatch = CASE WHEN EXISTS (SELECT 1 
									FROM dbo.Vendor
									WHERE Vendor_ID = @Vendor_ID
										AND Vendor_Key = @Vendor_Key) 
							THEN 1 -- TRUE because at least one match was found
							ELSE 0 -- FALSE because a match was not found
							END
END
GO 