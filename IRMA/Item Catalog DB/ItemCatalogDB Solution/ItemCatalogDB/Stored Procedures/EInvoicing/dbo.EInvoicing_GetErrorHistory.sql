 
IF EXISTS (
       SELECT *
       FROM   sysobjects
       WHERE  NAME = 'EInvoicing_GetErrorHistory'
              AND xtype = 'P'
   )
    DROP PROCEDURE dbo.EInvoicing_GetErrorHistory
GO

CREATE PROCEDURE dbo.EInvoicing_GetErrorHistory
	@EInvoiceId INT
AS
BEGIN
	SELECT eeh.ErrorHistoryId,
	       eeh.EInvoiceId,
	       eeh.[Timestamp],
	       eeh.ErrorInformation
	FROM   EInvoicing_ErrorHistory eeh
	WHERE  eeh.EInvoiceId = @EInvoiceId
	ORDER BY
	       eeh.[Timestamp] DESC
END
GO 
