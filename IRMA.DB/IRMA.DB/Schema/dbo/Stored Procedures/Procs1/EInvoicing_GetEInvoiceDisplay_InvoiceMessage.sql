CREATE PROCEDURE [dbo].[EInvoicing_GetEInvoiceDisplay_InvoiceMessage]
@EinvoiceId int
AS  
BEGIN
	
	SELECT	ElementValue 
	FROM	EInvoicing_SummaryData (nolock) 
	WHERE	EInvoice_Id = @EinvoiceID	AND 
			ElementName = 'message'		AND 
			ElementValue IS NOT NULL
END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[EInvoicing_GetEInvoiceDisplay_InvoiceMessage] TO [IRMAAdminRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[EInvoicing_GetEInvoiceDisplay_InvoiceMessage] TO [IRMASupportRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[EInvoicing_GetEInvoiceDisplay_InvoiceMessage] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[EInvoicing_GetEInvoiceDisplay_InvoiceMessage] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[EInvoicing_GetEInvoiceDisplay_InvoiceMessage] TO [IRMAReportsRole]
    AS [dbo];

