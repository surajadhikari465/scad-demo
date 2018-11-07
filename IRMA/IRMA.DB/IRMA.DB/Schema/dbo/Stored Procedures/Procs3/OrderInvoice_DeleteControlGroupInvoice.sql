-- Delete an existing record from the OrderInvoice_ControlGroupInvoice table.
CREATE PROCEDURE dbo.OrderInvoice_DeleteControlGroupInvoice
	@OrderInvoice_ControlGroup_ID int,
	@InvoiceType int,
	@OrderHeader_ID int
AS
BEGIN
	DELETE FROM dbo.OrderInvoice_ControlGroupInvoice
	WHERE OrderInvoice_ControlGroup_ID = @OrderInvoice_ControlGroup_ID
			AND InvoiceType = @InvoiceType
			AND OrderHeader_ID = @OrderHeader_ID
END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[OrderInvoice_DeleteControlGroupInvoice] TO [IRMAClientRole]
    AS [dbo];

