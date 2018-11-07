CREATE PROCEDURE [dbo].[InsertOrderItemRefused]
	@OrderHeader_ID int,
	@OrderItem_ID int,
	@Identifier varchar(13),
	@VendorItemNumber varchar (255),
	@Description varchar (60),
	@Unit varchar (25),
	@InvoiceQuantity decimal(18,4),
	@InvoiceCost money,
	@RefusedQuantity decimal(18,4),
	@DiscrepancyCodeID int,
	@UserAddedEntry bit,
	@eInvoiceId integer

AS 

-- **************************************************************************
-- Procedure: InsertOrderItemRefused()
--    Author: Faisal Ahmed
--      Date: 03/04/2013
--
-- Description:
-- This procedure inserts a record into OrderItemRefused table
--
-- Modification History:
-- Date       	Init  	TFS   	Comment
-- 03/04/2013	FA   	8325	Initial Code
-- **************************************************************************

BEGIN

	INSERT INTO OrderItemRefused
		(OrderHeader_ID, OrderItem_ID, Identifier, VendorItemNumber, [Description], Unit, InvoiceQuantity, InvoiceCost, RefusedQuantity, DiscrepancyCodeID, UserAddedEntry, eInvoice_Id) 
		VALUES (@OrderHeader_ID, @OrderItem_ID, @Identifier, @VendorItemNumber, @Description, @Unit, @InvoiceQuantity, @InvoiceCost, @RefusedQuantity, @DiscrepancyCodeID, @UserAddedEntry, @eInvoiceId)
END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[InsertOrderItemRefused] TO [IRMAClientRole]
    AS [dbo];

