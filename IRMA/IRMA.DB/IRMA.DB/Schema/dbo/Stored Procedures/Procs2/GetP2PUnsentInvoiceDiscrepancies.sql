CREATE Procedure [dbo].[GetP2PUnsentInvoiceDiscrepancies]

AS

-- Used in the Procurement to Payment (P2P) portion of IRMA.  Called
-- from the ItemCatalogLib.Order.GetP2PUnsentInvoiceDiscrepancies procedure.	 

SET NOCOUNT ON 
	-- Set the "we're processing invoice discrepancies" flag
	UPDATE OrderHeader
	SET    InvoiceProcessingDiscrepancy = 1
	WHERE  InvoiceDiscrepancy = 1
	  AND  InvoiceDiscrepancySentDate IS NULL
	  AND  UploadedDate IS NOT NULL

SET NOCOUNT OFF  

	-- Retrieve the records for processing	 
	SELECT DISTINCT OH.Vendor_ID as VendorID,
		   Vendor.Email as VendorEmail
	FROM   OrderHeader (nolock) OH
	INNER JOIN Vendor (nolock) ON Vendor.Vendor_ID = OH.Vendor_ID
	WHERE  InvoiceProcessingDiscrepancy = 1
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetP2PUnsentInvoiceDiscrepancies] TO [IRMAAdminRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetP2PUnsentInvoiceDiscrepancies] TO [IRMAClientRole]
    AS [dbo];

