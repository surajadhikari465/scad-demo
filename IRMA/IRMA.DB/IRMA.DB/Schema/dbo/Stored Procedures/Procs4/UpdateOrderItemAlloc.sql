CREATE PROCEDURE dbo.UpdateOrderItemAlloc
    @OrderItem_ID int,
    @QuantityOrdered decimal(18,4),
    @QuantityAllocated decimal(18,4),
    @Package_Desc1 decimal(9,4)
AS

BEGIN
    SET NOCOUNT ON

    DECLARE @Error_No int
    SELECT @Error_No = 0

    DECLARE @Unit int
    SELECT @Unit = Unit_ID FROM ItemUnit WITH(nolock) WHERE UnitSysCode = 'unit'

    DECLARE @Cost decimal(18,4), @Freight decimal(18,4), @DiscountType int, @QuantityDiscount decimal(18,4), 
            @CostUnit int, @FreightUnit int, @QuantityUnit int,
            @PD1 decimal(9,4), @PD2 decimal(9,4), @PDU int, @MarkupPercent decimal(18,4),
            @LineItemCost decimal(18,4), @LineItemFreight decimal(18,4), @OrigQuantityOrdered decimal(18,4), @OrigQuantityAllocated decimal(18,4), 
            @UnitCost decimal(18,4), @UnitFreight decimal(18,4), @MUCost decimal(18,4), @MUFreight decimal(18,4)

    BEGIN TRAN

    SELECT @Cost = Cost, @Freight = Freight, @DiscountType = OI.DiscountType,
           @QuantityDiscount = OI.QuantityDiscount, @CostUnit = CostUnit, @FreightUnit = FreightUnit, @QuantityUnit = QuantityUnit,
           @PD1 = Package_Desc1, @PD2 = Package_Desc2, @PDU = Package_Unit_ID, @MarkupPercent = MarkupPercent,
           @OrigQuantityOrdered = QuantityOrdered, @OrigQuantityAllocated = QuantityAllocated,
           @LineItemCost = LineItemCost, @LineItemFreight = LineItemFreight
    FROM OrderHeader OH
    INNER JOIN OrderItem OI ON OI.OrderHeader_ID = OH.OrderHeader_ID
    WHERE OI.OrderItem_ID = @OrderItem_ID

    SELECT @Error_No = @@ERROR
    
    IF ((ISNULL(@QuantityAllocated, ISNULL(@OrigQuantityAllocated, -1)) <> ISNULL(@OrigQuantityAllocated, -1)) 
        OR (ISNULL(@Package_Desc1, @PD1) <> @PD1)
        OR (ISNULL(@QuantityOrdered, @OrigQuantityOrdered) <> @OrigQuantityOrdered)
       ) 
       AND (@Error_No = 0)
    BEGIN
        IF ISNULL(@Package_Desc1, @PD1) <> @PD1
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
        
            SELECT @Error_No = @@ERROR
    
            IF @Error_No = 0
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
            
                SELECT @Error_No = @@ERROR        
            END

            SELECT @PD1 = @Package_Desc1
    
            IF @Error_No = 0
            BEGIN
                EXEC CostConversion @UnitCost, 
                                    @Unit, 
                                    @CostUnit, 
                                    @PD1,
                                    @PD2,
                                    @PDU,
                                    0, 
                                    0,
                                    @NewAmount = @Cost OUTPUT
            
                SELECT @Error_No = @@ERROR        
            END

            IF @Error_No = 0
            BEGIN
                EXEC CostConversion @UnitFreight, 
                                    @Unit, 
                                    @FreightUnit, 
                                    @PD1,
                                    @PD2,
                                    @PDU,
                                    0, 
                                    0,
                                    @NewAmount = @Freight OUTPUT
            
                SELECT @Error_No = @@ERROR        
            END
        END

        IF @Error_No = 0
        BEGIN
            --SELECT @MUCost = CASE @DiscountType WHEN 1 THEN @Cost - @QuantityDiscount
            --                                  WHEN 2 THEN @Cost * ((100 - @QuantityDiscount) / 100)
            --                                  ELSE @Cost END
            SELECT @MUCost = CASE @DiscountType WHEN 1 THEN @Cost - @QuantityDiscount
                                              WHEN 2 THEN @Cost - (@Cost * (@QuantityDiscount / 100))
                                              ELSE @Cost END

    
            SELECT @MUCost = @MUCost * (100 + @MarkupPercent) / 100
            
            EXEC CostConversion @MUCost, 
                                @CostUnit, 
                                @QuantityUnit, 
                                @PD1, 
                                @PD2,
                                @PDU,
                                0, 
                                0,
                                @NewAmount = @LineItemCost OUTPUT
        
            SELECT @Error_No = @@ERROR
        END
    
        IF @Error_No = 0
        BEGIN
            SELECT @MUFreight = @Freight * (100 + @MarkupPercent) / 100

            EXEC CostConversion @MUFreight, 
                                @FreightUnit, 
                                @QuantityUnit, 
                                @PD1, 
                                @PD2,
                                @PDU,
                                0, 
                                0,
                                @NewAmount = @LineItemFreight OUTPUT
        
            SELECT @Error_No = @@ERROR
        END

        SELECT @LineItemCost = @LineItemCost * CASE WHEN @QuantityAllocated IS NOT NULL THEN @QuantityAllocated ELSE @QuantityOrdered END
        SELECT @LineItemFreight = @LineItemFreight * CASE WHEN @QuantityAllocated IS NOT NULL THEN @QuantityAllocated ELSE @QuantityOrdered END
    END

    IF @Error_No = 0
    BEGIN
        UPDATE OrderItem
        SET QuantityAllocated = ISNULL(@QuantityAllocated, QuantityAllocated),
            QuantityOrdered = ISNULL(@QuantityOrdered, QuantityOrdered),
            Package_Desc1 = ISNULL(@Package_Desc1, Package_Desc1),
            Cost = @Cost,
            Freight = @Freight,
            LineItemCost = @LineItemCost,
            LineItemFreight = @LineItemFreight 
        WHERE OrderItem_ID = @OrderItem_ID

        SELECT @Error_No = @@ERROR
    END

    IF @Error_No = 0
    BEGIN
        COMMIT TRAN
        SET NOCOUNT OFF
    END
    ELSE
    BEGIN
        ROLLBACK TRAN
        DECLARE @Severity smallint
        SELECT @Severity = ISNULL((SELECT severity FROM master.dbo.sysmessages WHERE error = @Error_No), 16)
        SET NOCOUNT OFF
        RAISERROR ('UpdateOrderItemAlloc failed with @@ERROR: %d', @Severity, 1, @Error_No)
    END

    SET NOCOUNT OFF
END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[UpdateOrderItemAlloc] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[UpdateOrderItemAlloc] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[UpdateOrderItemAlloc] TO [IRMAReportsRole]
    AS [dbo];

