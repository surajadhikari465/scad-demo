CREATE PROCEDURE dbo.EInvoicing_ArchiveEInvoice
	@EInvoiceId INT,
	@ArchiveType INT
AS
BEGIN
	-- @archivetype = 0 = system 1 = user
	UPDATE EInvoicing_Invoices
	SET    Archived = @ArchiveType,
		   ArchivedDate = GETDATE()
	WHERE  EInvoice_Id = @EInvoiceId
END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[EInvoicing_ArchiveEInvoice] TO [IRMAAdminRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[EInvoicing_ArchiveEInvoice] TO [IRMAClientRole]
    AS [dbo];

