IF EXISTS (SELECT name 
	   FROM   sysobjects 
	   WHERE  name = N'EInvoicing_GetEInvoiceDisplay_InvoiceMessage' 
	   AND 	  type = 'P')
	DROP PROCEDURE EInvoicing_GetEInvoiceDisplay_InvoiceMessage
GO

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

