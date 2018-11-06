-- Update an existing record in the OrderInvoice_ControlGroupInvoice table.
CREATE PROCEDURE dbo.OrderInvoice_UpdateControlGroupInvoice
	@OrderInvoice_ControlGroup_ID int,
	@InvoiceType int,
	@OrderHeader_ID int,
	@Return_Order bit,
	@InvoiceCost money,
	@InvoiceFreight smallmoney,
	@InvoiceDate smalldatetime,
	@InvoiceNumber varchar(16),
	@Vendor_ID int,
	@UpdateUser_ID int,
	@ValidationCode int OUTPUT
AS
BEGIN
	-- Validate the invoice data passes the business validation rules before adding it to the database.
	EXEC dbo.OrderInvoice_ValidateControlGroupInvoice @OrderHeader_ID, @OrderInvoice_ControlGroup_ID, @Vendor_ID, @InvoiceNumber, @InvoiceType, @Return_Order, @UpdateUser_ID, @ValidationCode OUTPUT

	-- If the validation was a SUCCESS or WARNING, update the order invoice control group data.
	IF @ValidationCode = 0 OR dbo.fn_IsWarningValidationCode(@ValidationCode) = 1
	BEGIN
		UPDATE dbo.OrderInvoice_ControlGroupInvoice SET 
			Return_Order = @Return_Order,
			InvoiceCost = @InvoiceCost,
			InvoiceFreight = @InvoiceFreight,
			InvoiceDate = @InvoiceDate,
			InvoiceNumber = @InvoiceNumber,
			Vendor_ID = @Vendor_ID,
			UpdateUser_ID = @UpdateUser_ID
		 WHERE OrderInvoice_ControlGroup_ID = @OrderInvoice_ControlGroup_ID
			AND InvoiceType = @InvoiceType
			AND OrderHeader_ID = @OrderHeader_ID
	END
END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[OrderInvoice_UpdateControlGroupInvoice] TO [IRMAClientRole]
    AS [dbo];

