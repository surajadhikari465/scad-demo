CREATE PROCEDURE dbo.EInvoicing_CheckForError
	@InvoiceId INT
AS
BEGIN
	SELECT ei.ErrorCode_id,
	       ISNULL(ec.ErrorMessage, 'Unknown') AS ErrorMessage
	FROM   einvoicing_invoices ei (NOLOCK)
	       LEFT JOIN einvoicing_errorcodes ec (NOLOCK)
	            ON  ei.errorcode_id = ec.errorcode_id
	WHERE  einvoice_id = @InvoiceId
	       AND ei.errorcode_id IS NOT NULL
END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[EInvoicing_CheckForError] TO [IRMAAdminRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[EInvoicing_CheckForError] TO [IRMAClientRole]
    AS [dbo];

