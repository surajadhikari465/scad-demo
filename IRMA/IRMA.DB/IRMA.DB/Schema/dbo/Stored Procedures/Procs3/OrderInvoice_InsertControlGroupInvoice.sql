-- Insert a new record into the OrderInvoice_ControlGroupInvoice table.
CREATE PROCEDURE dbo.OrderInvoice_InsertControlGroupInvoice
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
		INSERT INTO dbo.OrderInvoice_ControlGroupInvoice (
			OrderInvoice_ControlGroup_ID,
			InvoiceType,
			Return_Order,
			InvoiceCost,
			InvoiceFreight,
			InvoiceDate,
			InvoiceNumber,
			OrderHeader_ID,
			Vendor_ID,
			UpdateUser_ID
		) VALUES (
			@OrderInvoice_ControlGroup_ID,
			@InvoiceType,
			@Return_Order,
			@InvoiceCost,
			@InvoiceFreight,
			@InvoiceDate,
			@InvoiceNumber,
			@OrderHeader_ID,
			@Vendor_ID,
			@UpdateUser_ID
		)
	END
END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[OrderInvoice_InsertControlGroupInvoice] TO [IRMAClientRole]
    AS [dbo];

