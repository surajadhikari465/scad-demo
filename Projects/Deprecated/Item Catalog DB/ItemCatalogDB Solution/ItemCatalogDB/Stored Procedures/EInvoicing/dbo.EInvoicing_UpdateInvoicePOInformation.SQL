
IF EXISTS (
       SELECT *
       FROM   sys.objects
       WHERE  OBJECT_ID = OBJECT_ID(N'[dbo].[EInvoicing_UpdateInvoicePOInformation]')
              AND TYPE IN (N'P', N'PC')
   )
    DROP PROCEDURE [dbo].[EInvoicing_UpdateInvoicePOInformation]
GO

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

