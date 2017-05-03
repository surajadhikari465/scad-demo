CREATE PROCEDURE dbo.ReceiveOrderItem 
    @OrderItem_ID int,
    @DateReceived DateTime,
    @QuantityIn decimal(18,4),
    @WeightIn decimal(18,4),
    @User_ID int
AS 

BEGIN
    SET NOCOUNT ON

    DECLARE @error_no int
    SELECT @error_no = 0

    BEGIN TRAN
    
    DECLARE @Quantity decimal(18,4), @OrderType_ID int, @Vendor_ID int

    SELECT @Quantity = ISNULL(QuantityAllocated, QuantityOrdered),
           @OrderType_ID = OrderHeader.OrderType_ID,
           @Vendor_ID = OrderHeader.Vendor_ID
    FROM OrderItem (nolock) 
    INNER JOIN 
        OrderHeader (nolock) 
        ON OrderItem.OrderHeader_ID = OrderHeader.OrderHeader_ID
    WHERE OrderItem.OrderItem_ID = @OrderItem_ID

    SELECT @error_no = @@ERROR

    IF @error_no = 0
    BEGIN
		IF @OrderType_ID = 2
			BEGIN
				UPDATE OrderItem 
				SET QuantityReceived = ISNULL(@QuantityIn, ISNULL(QuantityAllocated, QuantityOrdered + (CASE WHEN DiscountType = 3 THEN QuantityDiscount ELSE 0 END))),
					ReceivedItemCost= CASE WHEN QuantityOrdered <> 0 
										   THEN ((LineItemCost / QuantityOrdered) * ISNULL(@QuantityIn, @Quantity) + (ISNULL(dbo.fn_GetCurrentHandlingCharge(OrderItem.Item_Key, @Vendor_ID),0.00) * @QuantityIn))
										   ELSE 0 END,
					ReceivedItemHandling = CASE WHEN QuantityOrdered <> 0 
												THEN (LineItemHandling / QuantityOrdered) * ISNULL(@QuantityIn, @Quantity) 
												ELSE 0 END,
					ReceivedItemFreight = CASE WHEN @OrderType_ID = 1	--  Purchase Order 
											   THEN ReceivedItemFreight 
											   ELSE CASE WHEN QuantityOrdered <> 0 
														 THEN (LineItemFreight / QuantityOrdered) * ISNULL(@QuantityIn, @Quantity) 
														 ELSE 0 END
											   END,
					DateReceived = @DateReceived,
					Total_Weight = ISNULL(@WeightIn, 0)
				FROM OrderItem (rowlock)
				WHERE OrderItem_ID = @OrderItem_ID
		    
				SELECT @error_no = @@ERROR
			END
		ELSE
			BEGIN
				UPDATE OrderItem 
				SET QuantityReceived = ISNULL(@QuantityIn, ISNULL(QuantityAllocated, QuantityOrdered + (CASE WHEN DiscountType = 3 THEN QuantityDiscount ELSE 0 END))),
					ReceivedItemCost= CASE WHEN QuantityOrdered <> 0 
										   THEN ((LineItemCost / QuantityOrdered) * ISNULL(@QuantityIn, @Quantity))
										   ELSE 0 END,
					ReceivedItemHandling = CASE WHEN QuantityOrdered <> 0 
												THEN (LineItemHandling / QuantityOrdered) * ISNULL(@QuantityIn, @Quantity) 
												ELSE 0 END,
					ReceivedItemFreight = CASE WHEN @OrderType_ID = 1	--  Purchase Order 
											   THEN ReceivedItemFreight 
											   ELSE CASE WHEN QuantityOrdered <> 0 
														 THEN (LineItemFreight / QuantityOrdered) * ISNULL(@QuantityIn, @Quantity) 
														 ELSE 0 END
											   END,
					DateReceived = @DateReceived,
					Total_Weight = ISNULL(@WeightIn, 0)
				FROM OrderItem (rowlock)
				WHERE OrderItem_ID = @OrderItem_ID
		    
				SELECT @error_no = @@ERROR
			END
    END

    IF @error_no = 0
    BEGIN
        EXEC UpdateOrderItemUnitsReceived @OrderItem_ID

        SELECT @error_no = @@ERROR
    END

    IF @error_no = 0
    BEGIN
        EXEC InsertReceivingItemHistory @OrderItem_ID, @User_ID
    
        SELECT @error_no = @@ERROR
    END

    SET NOCOUNT OFF

    IF @error_no = 0
	    COMMIT TRAN
    ELSE
    BEGIN
        IF @@TRANCOUNT <> 0
            ROLLBACK TRAN
        DECLARE @Severity smallint
        SELECT @Severity = ISNULL((SELECT severity FROM master.dbo.sysmessages WHERE error = @error_no), 16)
        RAISERROR ('ReceiveOrderItem failed with @@ERROR: %d', @Severity, 1, @error_no)
    END
END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[ReceiveOrderItem] TO [IRMASupportRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[ReceiveOrderItem] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[ReceiveOrderItem] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[ReceiveOrderItem] TO [IRMAReportsRole]
    AS [dbo];

