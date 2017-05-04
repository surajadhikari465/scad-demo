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
GRANT EXECUTE
    ON OBJECT::[dbo].[VendorItemCount] TO [IRMAReportsRole]
    AS [dbo];

