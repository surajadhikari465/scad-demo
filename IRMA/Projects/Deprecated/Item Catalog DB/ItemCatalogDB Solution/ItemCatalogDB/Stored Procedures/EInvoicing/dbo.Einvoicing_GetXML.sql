 IF EXISTS (SELECT * FROM sysobjects WHERE name = 'EInvoicing_GetXML' )
 DROP PROCEDURE dbo.EINvoicing_GetXML
 GO
 
 
 
 CREATE PROCEDURE dbo.EInvoicing_GetXML
 @EInvoiceId INT
 AS
 BEGIN
 
	SELECT	EInvoicing_Invoices.InvoiceXML 
	FROM	dbo.EInvoicing_Invoices 
	WHERE	EInvoicing_Invoices.EInvoice_Id = @EInvoiceId
	
 
 END
 GO
 
 GRANT EXECUTE ON dbo.EInvoicing_GetXML TO IRMAADMINROLE
 GRANT EXECUTE ON dbo.EInvoicing_GetXML TO IRMACLIENTROLE
 GO
 
 
 
 
 