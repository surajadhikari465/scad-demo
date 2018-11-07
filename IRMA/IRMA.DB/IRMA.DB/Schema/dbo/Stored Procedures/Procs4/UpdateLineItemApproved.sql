CREATE PROCEDURE [dbo].[UpdateLineItemApproved]
	@OrderItemID int,
	@UserID int,
	@ResolutionCodeID int,
	@PayByAgreedCost bit,
	@PaymentTypeID int
AS
	-- **************************************************************************
	-- Procedure: UpdateLineItemApproved
	--    Author: n/a
	--      Date: n/a
	--
	-- Description:
	-- This procedure is called from SPOT (Suspended PO Tool) to approve a line item by a PO Admin
	--
	-- Modification History:
	-- Date			Init	TFS		Comment
	-- 08/13/2012	MZ		7163	Added the function call fn_ReceivedInvoiceCost to determine 
	--								a line item's ReceivedInvoiceCost.
	-- **************************************************************************	
	DECLARE @CentralTimeZoneOffset int,
	        @unit				   int,
		    @pound				   int 
	SELECT  @CentralTimeZoneOffset = CentralTimeZoneOffset FROM Region
	 
	SELECT @pound = Unit_ID FROM ItemUnit (nolock) WHERE EDISysCode = 'LB'  
	SELECT @unit  = Unit_ID FROM ItemUnit (nolock) WHERE EDISysCode = 'EA' 

	UPDATE 
		OrderItem
	SET 
		ApprovedDate      = DATEADD(hour, @CentralTimeZoneOffset, GETDATE()),
		ApprovedByUserId  = @UserID,
		ResolutionCodeID  = @ResolutionCodeID,
		PaymentTypeID     = @PaymentTypeID,
		LineItemSuspended = 0,
		PaidCost		  = CASE WHEN @PayByAgreedCost = 1		                         
		                         THEN ISNULL(dbo.fn_ReceivedInvoiceCost(oi.InvoiceExtendedCost, 																			
											 oi.InvoiceTotalWeight,
                                             oi.eInvoiceQuantity, i.CostedByWeight, oi.QuantityUnit, oi.Package_Desc1, 
                                             oi.Package_Desc2, oi.Package_Unit_ID, oi.UnitsReceived, @unit, @pound),0)					 	  
							ELSE oi.ReceivedItemCost
						   END
	From OrderItem oi
	INNER JOIN Item i on i.Item_Key = oi.Item_key
	WHERE 
		OrderItem_ID = @OrderItemID
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[UpdateLineItemApproved] TO [IRMAClientRole]
    AS [dbo];

