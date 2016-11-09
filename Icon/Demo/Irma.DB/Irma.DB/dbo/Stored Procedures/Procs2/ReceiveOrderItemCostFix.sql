CREATE PROCEDURE dbo.ReceiveOrderItemCostFix 
    @OrderItem_ID int
AS 

BEGIN
    SET NOCOUNT ON

    -- This procedure is used to correct cost.  Update OrderItem.Cost (and OrderItem.Freight is necessary) before calling.

    DECLARE @error_no int
    SELECT @error_no = 0

    BEGIN TRAN

    DECLARE @Unit int

    DECLARE @ReceiveLocation_ID int, @QuantityOrdered decimal(18,4), @QuantityAllocated decimal(18,4), @QuantityReceived decimal(18,4), @Total_Weight decimal(18,4), 
            @DiscountType int, @QuantityDiscount decimal(18,4), 
            @CostUnit int, @FreightUnit int, @QuantityUnit int,
            @PD1 decimal(9,4), @PD2 decimal(9,4), @PDU int, @MarkupPercent decimal(18,4),
            @UnitCost decimal(18,4), @UnitFreight decimal(18,4), @LandedCost decimal(18,4), @MUCost decimal(18,4), @MUFreight decimal(18,4),
            @IsCostUnitPackage bit, @IsCostedByWeight bit

    DECLARE @Cost decimal(18,4), @Freight decimal(18,4),
            @LineItemCost decimal(18,4), @LineItemFreight decimal(18,4), 
            @ReceivedItemCost decimal(18,4), @ReceivedItemFreight decimal(18,4)

    SELECT @ReceiveLocation_ID = ReceiveLocation_ID, @Cost = Cost, @Freight = Freight, @DiscountType = OI.DiscountType,
           @QuantityDiscount = OI.QuantityDiscount, @CostUnit = CostUnit, @FreightUnit = FreightUnit, @QuantityUnit = QuantityUnit,
           @PD1 = OI.Package_Desc1, @PD2 = OI.Package_Desc2, @PDU = OI.Package_Unit_ID, @MarkupPercent = MarkupPercent,
           @QuantityOrdered = QuantityOrdered, @QuantityAllocated = QuantityAllocated, @QuantityReceived = QuantityReceived, @Total_Weight = Total_Weight,
           @UnitCost = UnitCost, @UnitFreight = UnitExtCost - UnitCost,
           @IsCostUnitPackage = ISNULL(CU.IsPackageUnit, 0),
           @IsCostedByWeight = CostedByWeight
    FROM OrderHeader OH (nolock)
    INNER JOIN 
        OrderItem OI (nolock) 
        ON OI.OrderHeader_ID = OH.OrderHeader_ID
    INNER JOIN
        Item (nolock)
        ON Item.Item_Key = OI.Item_Key
    LEFT JOIN
        ItemUnit CU (nolock)
        ON CU.Unit_ID = OI.CostUnit
    WHERE OI.OrderItem_ID = @OrderItem_ID

    SELECT @error_no = @@ERROR

    IF @error_no = 0
    BEGIN
        SELECT @Unit = CASE WHEN @IsCostedByWeight = 1 THEN  @PDU ELSE (SELECT Unit_ID FROM ItemUnit (nolock) WHERE UnitSysCode = 'unit') END

        SELECT @error_no = @@ERROR
    END

    IF @error_no = 0
    BEGIN
        EXEC CostConversion @Cost, 
                            @CostUnit, 
                            @Unit, 
                            @PD1,
                            @PD2,
                            @PDU,
                            0, 
                            0,
                            @NewAmount = @UnitCost OUTPUT
    
        SELECT @error_no = @@ERROR
    
        IF @error_no = 0
        BEGIN
            EXEC CostConversion @Freight, 
                                @FreightUnit, 
                                @Unit, 
                                @PD1,
                                @PD2,
                                @PDU,
                                0, 
                                0,
                                @NewAmount = @UnitFreight OUTPUT
    
            SELECT @error_no = @@ERROR
        END
    END

    IF @error_no = 0
    BEGIN
        --SELECT @MUCost = CASE @DiscountType WHEN 1 THEN @Cost - @QuantityDiscount
        --                                    WHEN 2 THEN @Cost * ((100 - @QuantityDiscount) / 100)
        --                                           ELSE @Cost END
        SELECT @MUCost = CASE @DiscountType WHEN 1 THEN @Cost - @QuantityDiscount
                                            WHEN 2 THEN @Cost - (@Cost * (@QuantityDiscount / 100))
                                                   ELSE @Cost END


        SELECT @LandedCost = @MUCost + @Freight
    
        SELECT @MUCost = @MUCost * (100 + @MarkupPercent) / 100
    END

    IF @error_no = 0
    BEGIN
        EXEC CostConversion @MUCost, 
                            @CostUnit, 
                            @QuantityUnit, 
                            @PD1,
                            @PD2,
                            @PDU,
                            @Total_Weight, 
                            @QuantityReceived,
                            @NewAmount = @ReceivedItemCost OUTPUT
    
        SELECT @error_no = @@ERROR
    END

    IF @error_no = 0
    BEGIN
        EXEC CostConversion @MUCost, 
                            @CostUnit, 
                            @QuantityUnit, 
                            @PD1,
                            @PD2,
                            @PDU,
                            0, 
                            0,
                            @NewAmount = @LineItemCost OUTPUT
        
        SELECT @error_no = @@ERROR
    END

    IF @error_no = 0
        SELECT @MUFreight = @Freight * (100 + @MarkupPercent) / 100

    IF @error_no = 0
    BEGIN
        EXEC CostConversion @MUFreight, 
                            @FreightUnit, 
                            @QuantityUnit, 
                            @PD1,
                            @PD2,
                            @PDU,
                            @Total_Weight, 
                            @QuantityReceived,
                            @NewAmount = @ReceivedItemFreight OUTPUT
        
        SELECT @error_no = @@ERROR
    END

    IF @error_no = 0
    BEGIN
        EXEC CostConversion @MUFreight, 
                            @FreightUnit, 
                            @QuantityUnit, 
                            @PD1,
                            @PD2,
                            @PDU,
                            0, 
                            0,
                            @NewAmount = @LineItemFreight OUTPUT
        
        SELECT @error_no = @@ERROR
    END

    IF @error_no = 0
    BEGIN    
        IF @DiscountType = 3
        BEGIN
            IF @QuantityReceived < @QuantityDiscount
            BEGIN
                SELECT @ReceivedItemCost = 0
            END
            ELSE
            BEGIN
                SELECT @ReceivedItemCost = @ReceivedItemCost * (@QuantityReceived - @QuantityDiscount)
            END
        END
        ELSE
        BEGIN
            SELECT @ReceivedItemCost = @ReceivedItemCost * @QuantityReceived
        END
    
        SELECT @ReceivedItemFreight = @ReceivedItemFreight * @QuantityReceived
    
        SELECT @LineItemCost = @LineItemCost * ISNULL(@QuantityAllocated, @QuantityOrdered)
        SELECT @LineItemFreight = @LineItemFreight * ISNULL(@QuantityAllocated, @QuantityOrdered)
    END

    IF @error_no = 0
    BEGIN
        EXEC CostConversion @MUCost, 
                            @CostUnit, 
                            @QuantityUnit, 
                            @PD1,
                            @PD2,
                            @PDU,
                            0, 
                            0,
                            @NewAmount = @MUCost OUTPUT
        
        SELECT @error_no = @@ERROR
    END    

    IF @error_no = 0
    BEGIN
        EXEC CostConversion @LandedCost, 
                            @CostUnit, 
                            @QuantityUnit, 
                            @PD1,
                            @PD2,
                            @PDU,
                            0, 
                            0,
                            @NewAmount = @LandedCost OUTPUT
        
        SELECT @error_no = @@ERROR
    END

    IF @error_no = 0
    BEGIN  
        UPDATE OrderItem 
        SET LineItemCost = @LineItemCost,
            LineItemFreight = @LineItemFreight,
            ReceivedItemCost= @ReceivedItemCost,
            ReceivedItemFreight = @ReceivedItemFreight,
            UnitExtCost = @UnitCost + @UnitFreight,
            UnitCost = @UnitCost,
            MarkupCost = @MUCost,
            LandedCost = @LandedCost
        FROM OrderItem (rowlock)
        WHERE OrderItem_ID = @OrderItem_ID
    
        SELECT @error_no = @@ERROR
    END

    IF @error_no = 0
    BEGIN  
        EXEC UpdateOrderItemUnitsReceived @OrderItem_ID
    
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
        RAISERROR ('ReceiveOrderItemCostFix failed with @@ERROR: %d', @Severity, 1, @error_no)
    END
END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[ReceiveOrderItemCostFix] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[ReceiveOrderItemCostFix] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[ReceiveOrderItemCostFix] TO [IRMAReportsRole]
    AS [dbo];

