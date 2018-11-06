CREATE PROCEDURE dbo.InsertReceivingItemHistory
    @OrderItem_ID int, 
    @User_ID int
AS 

BEGIN
    SET NOCOUNT ON

    DECLARE @error_no int, @Severity smallint
    SET @error_no = 0
    
    DECLARE @Item_Key int, @Vendor_ID int, @ReceiveLocation_ID int, @Transfer_To_SubTeam int, @Transfer_SubTeam int, @Return_Order bit,
        @UnitsReceived decimal(18,4), @VendEXEWarehouse int, @CustEXEWarehouse int, @CreatedBy int, @IsVendorRegional bit, @IsReceiveLocationRegional bit, @CostedByWeight bit,
        @DateReceived datetime, @OrderType_ID tinyint,
        @Weight decimal(18,4), @Quantity decimal(18,4)

	DECLARE @QuantityReceived decimal(18,4)

    SET @Quantity = 0
    SET @Weight = 0

    SELECT @Item_Key = Item.Item_Key, 
           @Vendor_ID = Vendor.Store_No, 
           @ReceiveLocation_ID = ReceiveLocation.Store_No,
           @Transfer_To_SubTeam = OrderHeader.Transfer_To_SubTeam, 
           @Transfer_SubTeam = OrderHeader.Transfer_SubTeam, 
           @Return_Order = OrderHeader.Return_Order,
           @UnitsReceived = OrderItem.UnitsReceived,
           @QuantityReceived = OrderItem.QuantityReceived,
           @VendEXEWarehouse = CASE WHEN EXEDistributed = 1 AND (VendStore.EXEWarehouse IS NOT NULL) THEN VendStore.EXEWarehouse ELSE NULL END,
           @CustEXEWarehouse = CASE WHEN EXEDistributed = 1 AND (CustStore.EXEWarehouse IS NOT NULL) THEN CustStore.EXEWarehouse ELSE NULL END,
           @CreatedBy = OrderHeader.CreatedBy,
           @IsVendorRegional = dbo.fn_VendorType(Vendor.PS_Vendor_ID, Vendor.WFM, Vendor.Store_No, VendStore.Internal),
           @IsReceiveLocationRegional = dbo.fn_VendorType(ReceiveLocation.PS_Vendor_ID, ReceiveLocation.WFM, ReceiveLocation.Store_No, CustStore.Internal),
           @DateReceived = OrderItem.DateReceived,
           @OrderType_ID = OrderHeader.OrderType_ID,
           @CostedByWeight = CostedByWeight
    FROM OrderItem (nolock)
    INNER JOIN
        OrderHeader (nolock)
        ON (OrderItem.OrderHeader_ID = OrderHeader.OrderHeader_ID)
    INNER JOIN
        Item (nolock)
        ON (Item.Item_Key = OrderItem.Item_Key)
    INNER JOIN
        Vendor (nolock)
        ON (Vendor.Vendor_ID = OrderHeader.Vendor_ID)
    INNER JOIN
        Vendor ReceiveLocation (nolock)
        ON (ReceiveLocation.Vendor_ID = OrderHeader.ReceiveLocation_ID)
    INNER JOIN
        Store CustStore (nolock)
        ON (CustStore.Store_No = ReceiveLocation.Store_No)
    LEFT JOIN
        Store VendStore (nolock)
        ON (VendStore.Store_No = Vendor.Store_No)
    WHERE OrderItem.OrderItem_ID = @OrderItem_ID

    SELECT @error_no = @@ERROR

    IF @error_no <> 0
    BEGIN
        SET NOCOUNT OFF
        IF @@TRANCOUNT <> 0
            ROLLBACK TRAN
        SELECT @Severity = ISNULL((SELECT severity FROM master.dbo.sysmessages WHERE error = @error_no), 16)
        RAISERROR ('InsertReceivingItemHistory failed with @@ERROR: %d', @Severity, 1, @error_no)
        RETURN
    END

    -- No inventory adjustment for external vendor returns created for an EXE warehouse by an actual user (as opposed to system, User_ID = 0)
    IF (@Return_Order = 1) AND (@OrderType_ID = 1) AND (@CustEXEWarehouse IS NOT NULL) AND (@CreatedBy <> 0) 
        RETURN

    IF @Item_Key IS NULL
        RETURN

    BEGIN TRAN   

    SELECT @error_no = @@ERROR

    IF @error_no = 0
    BEGIN
        DELETE ItemHistory WHERE OrderItem_ID = @OrderItem_ID
		DELETE ItemHistoryQueue WHERE OrderItem_ID = @OrderItem_ID
    
        SELECT @error_no = @@ERROR
    END

    IF @UnitsReceived > 0
    BEGIN
        IF @CostedByWeight = 1
			BEGIN
	            SET @Weight = @UnitsReceived
				SET	@Quantity = 0
			END
        ELSE
            SET @Quantity = @UnitsReceived
    
        -- Handle the vendor's inventory if the vendor is internal
        IF (@error_no = 0) AND (@Vendor_ID IS NOT NULL) AND (@OrderType_ID IN (2,3)) AND (@IsVendorRegional = 1)
        BEGIN
            -- Distribution
            IF @OrderType_ID = 2
            BEGIN
                -- Remove from inventory
                IF @Return_Order = 0
                BEGIN
                    EXEC InsertItemHistory @Vendor_ID, @Item_Key, @DateReceived, @Quantity, @Weight, 0, 6, NULL, @User_ID, @Transfer_SubTeam, @OrderItem_ID
    
                    SELECT @error_no = @@ERROR
                END
                ELSE
                BEGIN
                    -- If this is not  a return order to a warehouse that uses EXE that was created by a user (other than the system (User_ID = 0))
                    IF NOT ((@VendEXEWarehouse IS NOT NULL) AND (@CreatedBy <> 0))
                    BEGIN
                        EXEC InsertItemHistory @Vendor_ID, @Item_Key, @DateReceived, @Quantity, @Weight, 0, 4, NULL, @User_ID, @Transfer_SubTeam, @OrderItem_ID
    
                        SELECT @error_no = @@ERROR
    
                        IF @error_no = 0
                        BEGIN
                            -- Automatically waste
                            EXEC InsertItemHistory @Vendor_ID, @Item_Key, @DateReceived, @Quantity, @Weight, 0, 14, NULL, @User_ID, @Transfer_SubTeam, @OrderItem_ID
    
                            SELECT @error_no = @@ERROR
                        END
                    END
                END
            END
            -- Transfer
            ELSE 
            BEGIN
                -- Remove from inventory
                IF @Return_Order = 0
                    EXEC InsertItemHistory @Vendor_ID, @Item_Key, @DateReceived, @Quantity, @Weight, 0, 7, NULL, @User_ID, @Transfer_SubTeam, @OrderItem_ID
                -- Add back to inventory
                ELSE
                    EXEC InsertItemHistory @Vendor_ID, @Item_Key, @DateReceived, @Quantity, @Weight, 0, 15, NULL, @User_ID, @Transfer_SubTeam, @OrderItem_ID
    
                SELECT @error_no = @@ERROR
            END
        END
    
        -- Handle receiving location's inventory
        IF (@error_no = 0) AND (@ReceiveLocation_ID IS NOT NULL) AND (@IsReceiveLocationRegional = 1)
        BEGIN
            -- Receive - add to inventory
            IF @Return_Order = 0
                EXEC InsertItemHistory @ReceiveLocation_ID, @Item_Key, @DateReceived, @Quantity, @Weight, 0, 5, NULL, @User_ID, @Transfer_To_SubTeam, @OrderItem_ID
            -- Return - remove from inventory 
            ELSE
                EXEC InsertItemHistory @ReceiveLocation_ID, @Item_Key, @DateReceived, @Quantity, @Weight, 0, 17, NULL, @User_ID, @Transfer_To_SubTeam, @OrderItem_ID
    
            SELECT @error_no = @@ERROR
        END
    END

    SET NOCOUNT OFF

    IF @error_no = 0
	    COMMIT TRAN
    ELSE
    BEGIN
        IF @@TRANCOUNT <> 0
            ROLLBACK TRAN
        SELECT @Severity = ISNULL((SELECT severity FROM master.dbo.sysmessages WHERE error = @error_no), 16)
        RAISERROR ('InsertReceivingItemHistory failed with @@ERROR: %d', @Severity, 1, @error_no)
    END
END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[InsertReceivingItemHistory] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[InsertReceivingItemHistory] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[InsertReceivingItemHistory] TO [IRMAReportsRole]
    AS [dbo];

