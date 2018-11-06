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
GRANT EXECUTE
    ON OBJECT::[dbo].[EInvoicing_GetErrorHistory] TO [IRMAAdminRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[EInvoicing_GetErrorHistory] TO [IRMAClientRole]
    AS [dbo];

