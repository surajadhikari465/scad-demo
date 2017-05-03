-- Reads all of the records from the OrderInvoice_ControlGroupInvoice that are assigned to the
-- input control group id.
CREATE PROCEDURE dbo.OrderInvoice_GetControlGroupInvoices
	@OrderInvoice_ControlGroup_ID int
AS
BEGIN
	SELECT 
		CGI.InvoiceType,
		CGI.Return_Order,
		ISNULL(CGI.InvoiceCost,0) AS InvoiceCost,
		ISNULL(CGI.InvoiceFreight,0) AS InvoiceFreight,
		ISNULL((ISNULL(CGI.InvoiceCost,0) + ISNULL(CGI.InvoiceFreight,0)),0) AS InvoiceTotal,
		CGI.InvoiceDate,
		CGI.InvoiceNumber,
		CGI.OrderHeader_ID,
		CGI.Vendor_ID,
		V.Vendor_Key,
		V.CompanyName,
		CGI.ValidationCode,
		VC.ValidationCodeType,
		VC.Description AS ValidationDescription
	FROM dbo.OrderInvoice_ControlGroupInvoice CGI
	INNER JOIN dbo.Vendor V ON
		CGI.Vendor_ID = V.Vendor_ID
	LEFT JOIN dbo.ValidationCode VC ON
		CGI.ValidationCode = VC.ValidationCode
	WHERE
		CGI.OrderInvoice_ControlGroup_ID = @OrderInvoice_ControlGroup_ID
END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[OrderInvoice_GetControlGroupInvoices] TO [IRMAClientRole]
    AS [dbo];

