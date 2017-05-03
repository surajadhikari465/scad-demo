CREATE PROCEDURE dbo.ReceiveOrderItem4 
    @OrderItem_ID				int,
    @DateReceived				datetime,
    @Quantity					decimal(18,4),
    @Weight						decimal(18,4), 
	@RecvDiscrepancyReasonID	int,
    @User_ID					int,
	@ReceivedViaGun             bit = 0 
AS
-- **************************************************************************
-- Procedure: ReceiveOrderItem4()
--    Author: n/a
--      Date: n/a
--
-- Description:
-- This procedure is called from numerous places to update OrderItem values.
--
-- Modification History:
-- Date       	Init  	TFS   	Comment
-- 10/19/2010	BSR		13593	Added time offset logic to fix avg cost update 
-- 01/27/2011	BS		1005	Merge from 4.x into 4.2?
-- 03/17/2011	MY		1695	***COMING SOON***
-- 03/25/2011	TL		1728	Changed new code that sets @EInvoiceQuantity var so it doesn't set the var to '0'.
--								Then I added it to the check that determines the final qty passed to ReceiveOrderItem3, so the
--								new code is: @Quantity = isnull(@Quantity, isnull(@EInvoiceQuantity, @OrderedQuantity))
--								which means we take @Quantity, @EInvoiceQuantity, or @OrderedQuantity, in that order, whichever is not null.
--								Added IF check against the qty and weight values passed in because we only need to query the order if either of these
--								values is NULL (we FWD the passed-in values if we get non-null values for them), so this will save time
--								when both qty and weight values are provided.  Added comments & formatting.  Added change log history placeholders.
-- 03/28/2011	BBB   	1732	Updated for code standards and readability
-- 07/25/2011   MD      2459    Added Receiving Discrepancy Reason Code ID
-- 2011/12/23	KM		3744	Extension change; code formatting;
-- 2011/12/30	KM		3744	Modify call to ReceiveOrderItem3 - remove argument "0" for the deprecated
--								@Correction parameter;
-- 2011/12/30	KM		3744	Remove unused local variable @error_no; minor coding standards;
-- 2013/01/30	BS		9975	Added @IsClosed variable to accept the output parameter from ReceiveOrderItem3 stored proc
-- 2013/05/29   MZ		12473	Added @ReceivedViaGun parameter to the stored proc so that RD (Receiving Document) POs can be auto received with the 
--                              ReceivedViaGun flag always marked as true on the OrderItem table. 	
-- 2013/07/02	BJL		12752	If PO is EInvoice'd and items are missing from the EInvoice, then quantity received is zero.
-- **************************************************************************						

BEGIN
    SET NOCOUNT ON

	--**************************************************************************
	--Declare internal variables
	--**************************************************************************
	DECLARE
		@Cost					decimal(18,4), 
		@Freight				decimal(18,4),
		@LineItemCost			decimal(18,4),
		@LineItemFreight		decimal(18,4), 
		@ReceivedItemCost		decimal(18,4),
		@ReceivedItemFreight	decimal(18,4),
		@OrderedQuantity		decimal(18,4), 
		@OrderedWeight			decimal(18,4),
		@EInvoiceQuantity		decimal(18,4),
		@IsClosed				int,
		@eInvoiceId				int

	--**************************************************************************
	-- Adjust for regional timezone.
	--**************************************************************************
    SELECT @DateReceived = dbo.fn_GetSystemDateTime()
	
	--**************************************************************************
	--We can skip the following quad-join query if non-null values were passed in for both @Quantity and @Weight.
	--**************************************************************************
	IF @Quantity IS NULL OR @Weight IS NULL
		BEGIN
			SELECT
				@EInvoiceQuantity	=	eInvoiceQuantity, -- We do NOT want an isnull() here that returns '0' because @EInvoiceQuantity is checked below, so we want to take the literal value from OrderItem, even if it's NULL.
				@OrderedQuantity	=	ISNULL(QuantityAllocated, QuantityOrdered),
				@OrderedWeight		=	CASE 
											WHEN (CostedByWeight = 1) AND (iuq.IsPackageUnit = 1) THEN oi.Package_Desc1 * oi.Package_Desc2 * ISNULL(QuantityAllocated, QuantityOrdered)
											ELSE 0
										END,
				@eInvoiceId			=	oh.eInvoice_Id
			FROM 
				OrderItem				(nolock) oi 
				INNER JOIN OrderHeader	(nolock) oh		ON oi.OrderHeader_ID	= oh.OrderHeader_ID
				INNER JOIN Item			(nolock) i		ON oi.Item_Key			= i.Item_Key 
				INNER JOIN ItemUnit 	(nolock) iuq	ON oi.QuantityUnit		= iuq.Unit_ID 
			WHERE
				oi.OrderItem_ID = @OrderItem_ID

			-- Grab the final values for qty and weight to pass to ReceiveOrderItem3 SP below, which updates OrderItem info.
			SELECT
				@Quantity	= CASE WHEN @eInvoiceId IS NOT NULL 
									THEN ISNULL(@Quantity, ISNULL(@EInvoiceQuantity, 0)) -- If no qty was passed in, use e-inv qty, but if there's no e-inv qty, use zero. 
									ELSE ISNULL(@Quantity, @OrderedQuantity) END, -- For non- EInvoice'd orders if no qty was passed in, use ordered qty.
				@Weight		= ISNULL(@Weight, @OrderedWeight) -- If no weight was passed in, use ordered weight.
		END

	--**************************************************************************
	--Call ReceiveOrderItem3
	--**************************************************************************
    EXEC ReceiveOrderItem3
		@OrderItem_ID,
		@DateReceived,
		@Quantity,
		NULL,
		@Weight,
		@RecvDiscrepancyReasonID,
		@User_ID,
		@Cost					= @Cost,
        @Freight				= @Freight,
		@LineItemCost			= @LineItemCost,
		@LineItemFreight		= @LineItemFreight,
		@ReceivedItemCost		= @ReceivedItemCost,
		@ReceivedItemFreight	= @ReceivedItemFreight,
		@AlreadyClosed			= @IsClosed,
		@ReceivedViaGun			= @ReceivedViaGun
END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[ReceiveOrderItem4] TO [IRMASupportRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[ReceiveOrderItem4] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[ReceiveOrderItem4] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[ReceiveOrderItem4] TO [IRMAReportsRole]
    AS [dbo];

