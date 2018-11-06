CREATE FUNCTION [dbo].[fn_GetOrigReceivedItemCost]
    (
      @QuantityOrdered DECIMAL(18, 4) ,
      @QuantityUnit INT ,
      @QuantityReceived DECIMAL(18, 4) ,
      @Total_Weight DECIMAL(18, 4) ,
      @Cost MONEY ,
      @CostUnit INT ,
      @OrderQuantityDiscount DECIMAL(18, 4) , -- OrderHeader
      @OrderDiscountType INT , -- OrderHeader
      @ItemQuantityDiscount DECIMAL(18, 4) , -- OrderItem
      @ItemDiscountType INT , -- OrderItem
      @Freight DECIMAL(18, 4) ,
      @FreightUnit INT ,  -- This is not used currently since it is always the same as CostUnit per the UI rules
      @MarkupPercent DECIMAL(18, 4) ,
      @Package_Desc1 DECIMAL(9, 4) ,
      @Package_Desc2 DECIMAL(9, 4) ,
      @Package_Unit_ID INT ,
      @IsCostedByWeight BIT ,
      @MarkupDollars MONEY
    )
RETURNS MONEY
AS 
    BEGIN



        DECLARE @Unit INT ,
            @WorkingCost DECIMAL(38, 28) ,
            @WorkingFreight DECIMAL(38, 28) ,
            @CCCost DECIMAL(38, 28) ,
            @UnitsReceived DECIMAL(18, 4) ,
            @WorkingUnitCost DECIMAL(38, 28) ,
            @WorkingUnitExtCost DECIMAL(38, 28)

            

    --Output Params

        DECLARE @LandedCost MONEY ,
            @MarkupCost MONEY ,
            @LineItemCost MONEY ,
            @LineItemFreight MONEY ,
            @ReceivedItemCost MONEY ,
            @ReceivedItemFreight MONEY ,
            @UnitCost MONEY ,
            @UnitExtCost MONEY



        IF @Total_Weight IS NULL 
            SET @Total_Weight = 0

        

        IF @QuantityReceived IS NULL 
            SET @QuantityReceived = 0

       

        IF ( @Total_Weight > 0 )
            AND ( @QuantityReceived = 0 ) 
            SET @Total_Weight = 0



        SELECT  @Unit = CASE WHEN @IsCostedByWeight = 1
                             THEN ( SELECT  Unit_ID
                                    FROM    ItemUnit (NOLOCK)
                                    WHERE   EDISysCode = 'LB'
                                  )
                             ELSE ( SELECT  Unit_ID
                                    FROM    ItemUnit (NOLOCK)
                                    WHERE   EDISysCode = 'UN'
                                  )
                        END



        SET @WorkingCost = @Cost

        SET @WorkingFreight = @Freight



    --SET @CCCost = @WorkingFreight

    --EXEC CostConversion @CCCost, @CostUnit, @QuantityUnit, @Package_Desc1, @Package_Desc2, @Package_Unit_ID, 0, 0, @NewAmount = @WorkingFreight OUTPUT

    --

    --SET @MarkupCost = @WorkingCost + @WorkingFreight + ISNULL(@MarkupDollars, 0)

    --

    ---- Ordered Line Item totals

    --SET @LineItemCost = (@WorkingCost + ISNULL(@MarkupDollars, 0)) * (@QuantityOrdered - CASE WHEN @ItemDiscountType = 3 THEN @ItemQuantityDiscount ELSE 0 END)

    --IF @LineItemCost < 0

    --    SET @LineItemCost = 0



    --IF @WorkingFreight > 0

    --BEGIN

    --    SET @CCCost = @WorkingCost + @WorkingFreight                    

    --    EXEC CostConversion @CCCost, @QuantityUnit, @Unit, @Package_Desc1, @Package_Desc2, @Package_Unit_ID, @Total_Weight, @QuantityReceived, @NewAmount = @WorkingUnitExtCost OUTPUT

    --    SET @UnitExtCost = @WorkingUnitExtCost

    --END

    --ELSE

    --    SET @UnitExtCost = @UnitCost



    --SET @ReceivedItemFreight = (ISNULL(@WorkingUnitExtCost, 0) - @WorkingUnitCost) * @UnitsReceived



    ---- Apply Percent/Cash Discounts to the received line item totals

    --IF @QuantityReceived > 0

    --    EXEC CalculateOrderDiscountForLineItem @OrderDiscountType, @OrderQuantityDiscount, @ItemDiscountType, @ItemQuantityDiscount, @ReceivedItemCost OUTPUT, @ReceivedItemFreight OUTPUT

    

    -- Markup

    --  Convert to quantity unit first in case we are using markup dollars, which applies to the quantity unit

        SET @CCCost = @WorkingCost

        SELECT  @WorkingCost = dbo.fn_CostConversion(@CCCost, @CostUnit,
                                                     @QuantityUnit,
                                                     @Package_Desc1,
                                                     @Package_Desc2,
                                                     @Package_Unit_ID)

    

    -- Mark up

        IF @MarkupDollars IS NULL

    -- Apply the markup percent to the working cost
            SET @WorkingCost = @WorkingCost * ( 100 + ISNULL(@MarkupPercent, 0) )
                / 100

    

    -- Unit costs

        SELECT  @WorkingUnitCost = dbo.fn_CostConversion(@WorkingCost,
                                                         @QuantityUnit, @Unit,
                                                         @Package_Desc1,
                                                         @Package_Desc2,
                                                         @Package_Unit_ID)

    --EXEC CostConversion @WorkingCost, @QuantityUnit, @Unit, @Package_Desc1, @Package_Desc2, @Package_Unit_ID, @Total_Weight, @QuantityReceived, @NewAmount = @WorkingUnitCost OUTPUT

        SET @UnitCost = @WorkingUnitCost

    

    -- Units received

        IF @Total_Weight > 0 
            SET @UnitsReceived = @Total_Weight

        ELSE 
            SELECT  @UnitsReceived = dbo.fn_CostConversion(@QuantityReceived,
                                                           @Unit,
                                                           @QuantityUnit,
                                                           @Package_Desc1,
                                                           @Package_Desc2,
                                                           @Package_Unit_ID)

        --EXEC CostConversion @QuantityReceived, @Unit, @QuantityUnit, @Package_Desc1, @Package_Desc2, @Package_Unit_ID, 0, 0, @UnitsReceived OUTPUT

    

    -- Apply discounts to received line item totals

     --1=Cash Discount; 2=Percent; 3=Free Items (supported in the caller, CalculateOrderItemCosts); 4=Landed Percent

        SELECT  @ReceivedItemCost = CASE @ItemDiscountType
                                      WHEN 1
                                      THEN ( @WorkingUnitCost * @UnitsReceived )
                                           - ( @ItemQuantityDiscount
                                               * @UnitsReceived )
                                      WHEN 2
                                      THEN ( @WorkingUnitCost * ( ( 100
                                                              - @ItemQuantityDiscount )
                                                              / 100 ) )
                                           * @UnitsReceived
                                      WHEN 3
                                      THEN @WorkingUnitCost * ( @UnitsReceived
                                                              - @ItemQuantityDiscount )
                                      ELSE ( @WorkingUnitCost * @UnitsReceived )	

                    -- TFS 11879 - Changing default for no discount from 0 to (@WorkingUnitCost * @UnitsReceived)

                    --0
                                    END

    

    -- The only supported PO level value now is 2, which is "Percent".  We used to have 1=Cash Discount and 3=Landed Percent.

        IF ISNULL(@OrderDiscountType, 0) = 2 
            SELECT  @ReceivedItemCost = ROUND(( @WorkingUnitCost * ( ( 100
                                                              - @OrderQuantityDiscount )
                                                              / 100 ) )
                                              * @UnitsReceived, 2)



        IF @ReceivedItemCost < 0 
            SET @ReceivedItemCost = 0

    

        RETURN @ReceivedItemCost

    END