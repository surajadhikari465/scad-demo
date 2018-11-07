CREATE PROCEDURE dbo.OrderInvoice_CheckInvoiceExistsInOpenControlGroup (
	@OrderHeader_ID int,
	@OrderInvoice_ControlGroup_ID int,
	@InvoiceType int,
	@DoesExist bit OUTPUT
)
AS
BEGIN
	-- Check to see if the invoice exists in the OrderInvoice_ControlGroupInvoice table
	-- for a different control group in the OPEN status.
	SELECT @DoesExist =  CASE WHEN 
							(SELECT COUNT(1) FROM dbo.OrderInvoice_ControlGroupInvoice CGI
							 INNER JOIN dbo.OrderInvoice_ControlGroup CG ON
								CG.OrderInvoice_ControlGroup_ID = CGI.OrderInvoice_ControlGroup_ID
							 WHERE 
								CGI.OrderHeader_ID = @OrderHeader_ID AND 
								CGI.OrderInvoice_ControlGroup_ID <> @OrderInvoice_ControlGroup_ID AND 
								CGI.InvoiceType = @InvoiceType AND
								CG.OrderInvoice_ControlGroupStatus_ID = 1) > 0
						THEN 1
						ELSE 0 
						END
END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[OrderInvoice_CheckInvoiceExistsInOpenControlGroup] TO [IRMAClientRole]
    AS [dbo];

