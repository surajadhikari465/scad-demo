CREATE Procedure [dbo].[UpdateP2PInvoiceDiscrepancySentDate]
  @Vendor_ID int

AS

-- Used in the Procurement to Payment (P2P) portion of IRMA.  Called
-- from the ItemCatalogLib.Order.UpdateP2PInvoiceDiscrepancySentDate procedure.
	  
UPDATE OrderHeader
SET    InvoiceDiscrepancySentDate = GETDATE(),
       InvoiceProcessingDiscrepancy = 0
WHERE  Vendor_ID = @Vendor_ID
  AND  InvoiceDiscrepancy = 1
  AND  InvoiceDiscrepancySentDate IS NULL
  AND  UploadedDate IS NOT NULL
  AND InvoiceProcessingDiscrepancy = 1
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[UpdateP2PInvoiceDiscrepancySentDate] TO [IRMAAdminRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[UpdateP2PInvoiceDiscrepancySentDate] TO [IRMAClientRole]
    AS [dbo];

