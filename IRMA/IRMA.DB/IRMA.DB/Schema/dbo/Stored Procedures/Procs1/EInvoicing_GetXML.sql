 CREATE PROCEDURE dbo.EInvoicing_GetXML
 @EInvoiceId INT
 AS
 BEGIN
 
	SELECT	EInvoicing_Invoices.InvoiceXML 
	FROM	dbo.EInvoicing_Invoices 
	WHERE	EInvoicing_Invoices.EInvoice_Id = @EInvoiceId
	
 
 END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[EInvoicing_GetXML] TO [IRMAAdminRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[EInvoicing_GetXML] TO [IRMAClientRole]
    AS [dbo];

