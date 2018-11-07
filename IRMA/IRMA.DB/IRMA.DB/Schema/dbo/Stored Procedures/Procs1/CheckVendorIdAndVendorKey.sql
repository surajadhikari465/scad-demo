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
GRANT EXECUTE
    ON OBJECT::[dbo].[CheckVendorIdAndVendorKey] TO [IRMAClientRole]
    AS [dbo];

