CREATE PROCEDURE dbo.UpdateOrderStatus 
@OrderHeader_ID int,
@InvoiceNumber varchar(20),
@InvoiceDate varchar(10),
@VendorDoc_ID varchar(16),
@VendorDocDate varchar(10),
@PartialShipment bit =0,
@ChangeAllowed bit = NULL OUTPUT

AS
BEGIN
    SET NOCOUNT ON
  
	-- Save updated invoice or document data for an order.  This can only be done for orders that are not
	-- in the closed (or after) state because the invoice data is part of the three way matching process that is run
	-- when an order is closed.  To change the values for a closed order, the order must first be re-opened. 
	
	-- Verify the change is allowed before updating the data.
	SELECT @ChangeAllowed = CASE WHEN (SELECT COUNT(1) FROM dbo.OrderHeader (nolock) WHERE 
									(CloseDate IS NULL OR (CloseDate IS NOT NULL AND ApprovedDate IS NULL))
									AND OrderHeader_ID = @OrderHeader_ID) = 1 
						THEN 1 
						ELSE 0
						END
	 
	-- Make the update if the order is in the correct state.
	IF @ChangeAllowed = 1
	BEGIN
		UPDATE OrderHeader
		SET InvoiceNumber = UPPER(@InvoiceNumber), 
			InvoiceDate = @InvoiceDate,
			VendorDoc_ID = @VendorDoc_ID,
			VendorDocDate = @VendorDocDate,
			PartialShipment=@PartialShipment
		WHERE OrderHeader_ID = @OrderHeader_ID
			AND (CloseDate IS NULL OR (CloseDate IS NOT NULL AND ApprovedDate IS NULL))
	END    
    SET NOCOUNT OFF
END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[UpdateOrderStatus] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[UpdateOrderStatus] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[UpdateOrderStatus] TO [IRMAReportsRole]
    AS [dbo];

