CREATE PROCEDURE dbo.EInvoicing_UpdateInvoicePOInformation
	@EInvoiceId INT,
	@NewPO VARCHAR(50),
	@CurrentUser INT
AS
BEGIN
	-- if original_po_num has already been populated, dont update it again.
	UPDATE EInvoicing_Invoices
	SET    Original_PO_Num = CASE 
	                              WHEN original_po_num IS NULL THEN po_num
	                              ELSE original_po_num
	                         END,
	       po_num = @NewPO,
	       EditedBy = @CurrentUser,
	       EditedDate = GETDATE()
	WHERE EInvoice_Id = @EInvoiceId
END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[EInvoicing_UpdateInvoicePOInformation] TO [IRMAAdminRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[EInvoicing_UpdateInvoicePOInformation] TO [IRMAClientRole]
    AS [dbo];

