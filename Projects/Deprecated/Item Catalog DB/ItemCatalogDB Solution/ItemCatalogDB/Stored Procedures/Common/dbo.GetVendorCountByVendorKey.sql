IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[GetVendorCountByVendorKey]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[GetVendorCountByVendorKey]
GO

-- Count the number of Vendor records that match the vendor key input.
CREATE PROCEDURE dbo.GetVendorCountByVendorKey
	@Vendor_Key varchar(10),
	@VendorCount int OUTPUT,
	@VendorId int OUTPUT
AS
BEGIN
	SELECT @VendorCount = COUNT(1) FROM dbo.Vendor WHERE Vendor_Key = @Vendor_Key
	
	IF @VendorCount = 1
	BEGIN
		-- Return the matching vendor id
		SELECT @VendorId = Vendor_Id FROM dbo.Vendor WHERE Vendor_Key = @Vendor_Key 
	END
END
GO  