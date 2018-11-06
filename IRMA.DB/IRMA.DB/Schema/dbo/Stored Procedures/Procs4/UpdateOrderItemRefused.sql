CREATE PROCEDURE [dbo].[UpdateOrderItemRefused]
	@OrderItemRefusedID int,
	@Identifier varchar(13),
	@VendorItemNumber varchar (255),
	@Description varchar (60),
	@Unit varchar (25),
	@Cost	decimal(18,4),
	@RefusedQuantity decimal(18,4),
	@InvoiceQuantity decimal(18,4),
	@DiscrepancyCodeID int,
	@UserAddedEntry bit
AS 
-- **************************************************************************
-- Procedure: UpdateOrderItemRefused()
--    Author: Faisal Ahmed
--      Date: 03/08/2013
--
-- Description:
-- This procedure updates a record into OrderItemRefused table
--
-- Modification History:
-- Date       	Init  	TFS   	Comment
-- 03/08/2013	FA   	8325	Initial Code
-- 03/11/2013	FA		8325	Added code to update invoice cost for refused items
-- **************************************************************************
BEGIN

	UPDATE OrderItemRefused
	SET
		Identifier			= @Identifier,
		VendorItemNumber	= @VendorItemNumber,
		Description			= @Description,
		Unit				= @Unit,
		InvoiceQuantity		= @InvoiceQuantity,
		RefusedQuantity		= @RefusedQuantity,
		DiscrepancyCodeID	= @DiscrepancyCodeID,
		UserAddedEntry		= @userAddedEntry,
		InvoiceCost			= @Cost
	WHERE OrderItemRefusedID = @OrderItemRefusedID
END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[UpdateOrderItemRefused] TO [IRMAClientRole]
    AS [dbo];

