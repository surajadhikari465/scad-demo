CREATE PROCEDURE dbo.OrderInvoice_CheckInvoiceExistsInCurrentControlGroup (
	@OrderHeader_ID int,
	@OrderInvoice_ControlGroup_ID int,
	@InvoiceType int,
	@DoesExist bit OUTPUT
)
AS
BEGIN
	-- Check to see if there is already an invoice in the OrderInvoice_ControlGroupInvoice table
	-- for the given data.
	SELECT @DoesExist =  CASE WHEN 
							(SELECT COUNT(1) FROM dbo.OrderInvoice_ControlGroupInvoice WHERE 
								OrderHeader_ID = @OrderHeader_ID AND 
								OrderInvoice_ControlGroup_ID = @OrderInvoice_ControlGroup_ID AND 
								InvoiceType = @InvoiceType) > 0
						THEN 1
						ELSE 0 
						END
END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[OrderInvoice_CheckInvoiceExistsInCurrentControlGroup] TO [IRMAClientRole]
    AS [dbo];

