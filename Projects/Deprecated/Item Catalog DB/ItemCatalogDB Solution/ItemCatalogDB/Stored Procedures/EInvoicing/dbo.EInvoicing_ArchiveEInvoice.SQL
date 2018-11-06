
IF EXISTS (
	   SELECT *
	   FROM   sys.objects
	   WHERE  OBJECT_ID = OBJECT_ID(N'[dbo].[EInvoicing_ArchiveEInvoice]')
			  AND TYPE IN (N'P', N'PC')
   )
	DROP PROCEDURE [dbo].[EInvoicing_ArchiveEInvoice]
GO

SET QUOTED_IDENTIFIER ON 
go

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

 