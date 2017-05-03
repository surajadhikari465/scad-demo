IF EXISTS ( SELECT  *
            FROM    SYSOBJECTS
            WHERE   NAME = 'fn_CalculateOrderItemCosts' ) 
    DROP FUNCTION dbo.fn_CalculateOrderItemCosts
    GO

CREATE FUNCTION [dbo].[fn_CalculateOrderItemCosts]
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
      @MarkupDollars SMALLMONEY
    )
RETURNS @Results TABLE
    (
      LandedCost MONEY ,
      MarkupCost MONEY ,
      LineItemCost MONEY ,
      LineItemFreight MONEY ,
      ReceivedItemCost MONEY ,
      ReceivedItemFreight MONEY ,
      UnitCost MONEY ,
      UnitExtCost MONEY
    )
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
                                    WHERE   EDISysCode = 'EA'
                                  )
                        END



        SET @WorkingCost = @Cost

        SET @WorkingFreight = @Freight



-- Landed Cost

        SET @CCCost = @WorkingCost + @WorkingFreight

        EXEC CostConversion @CCCost, @CostUnit, @QuantityUnit, @Package_Desc1,
            @Package_Desc2, @Package_Unit_ID, 0, 0,
            @NewAmount = @LandedCost OUTPUT



-- Markup

--  Convert to quantity unit first in case we are using markup dollars, which applies to the quantity unit

        SET @CCCost = @WorkingCost

        EXEC CostConversion @CCCost, @CostUnit, @QuantityUnit, @Package_Desc1,
            @Package_Desc2, @Package_Unit_ID, 0, 0,
            @NewAmount = @WorkingCost OUTPUT



-- Mark up

        IF @MarkupDollars IS NULL

    -- Apply the markup percent to the working cost
            SET @WorkingCost = @WorkingCost * ( 100 + ISNULL(@MarkupPercent, 0) )
                / 100



        SET @CCCost = @WorkingFreight

        EXEC CostConversion @CCCost, @CostUnit, @QuantityUnit, @Package_Desc1,
            @Package_Desc2, @Package_Unit_ID, 0, 0,
            @NewAmount = @WorkingFreight OUTPUT



        SET @MarkupCost = @WorkingCost + @WorkingFreight
            + ISNULL(@MarkupDollars, 0)



-- Ordered Line Item totals

        SET @LineItemCost = ( @WorkingCost + ISNULL(@MarkupDollars, 0) )
            * ( @QuantityOrdered
                - CASE WHEN @ItemDiscountType = 3 THEN @ItemQuantityDiscount
                       ELSE 0
                  END )

        IF @LineItemCost < 0 
            SET @LineItemCost = 0



        SET @LineItemFreight = @WorkingFreight * @QuantityOrdered



-- Apply Percent/Cash Discounts to the line item totals

        EXEC CalculateOrderDiscountForLineItem @OrderDiscountType,
            @OrderQuantityDiscount, @ItemDiscountType, @ItemQuantityDiscount,
            @LineItemCost OUTPUT, @LineItemFreight OUTPUT



-- Unit costs

        EXEC CostConversion @WorkingCost, @QuantityUnit, @Unit, @Package_Desc1,
            @Package_Desc2, @Package_Unit_ID, @Total_Weight, @QuantityReceived,
            @NewAmount = @WorkingUnitCost OUTPUT

        SET @UnitCost = @WorkingUnitCost



        IF @WorkingFreight > 0 
            BEGIN

                SET @CCCost = @WorkingCost + @WorkingFreight                    

                EXEC CostConversion @CCCost, @QuantityUnit, @Unit,
                    @Package_Desc1, @Package_Desc2, @Package_Unit_ID,
                    @Total_Weight, @QuantityReceived,
                    @NewAmount = @WorkingUnitExtCost OUTPUT

                SET @UnitExtCost = @WorkingUnitExtCost

            END

        ELSE 
            SET @UnitExtCost = @UnitCost



-- Units received

        IF @Total_Weight > 0 
            SET @UnitsReceived = @Total_Weight

        ELSE 
            EXEC CostConversion @QuantityReceived, @Unit, @QuantityUnit,
                @Package_Desc1, @Package_Desc2, @Package_Unit_ID, 0, 0,
                @UnitsReceived OUTPUT

    

-- Received Line Item Totals

        SELECT  @ReceivedItemCost = @WorkingUnitCost * @UnitsReceived
                - ( CASE WHEN @ItemDiscountType = 3 THEN @ItemQuantityDiscount
                         ELSE 0
                    END * @WorkingCost ) + ( @QuantityReceived
                                             * ISNULL(@MarkupDollars, 0) )

        IF @ReceivedItemCost < 0 
            SET @ReceivedItemCost = 0



        SET @ReceivedItemFreight = ( ISNULL(@WorkingUnitExtCost, 0)
                                     - @WorkingUnitCost ) * @UnitsReceived



-- Apply Percent/Cash Discounts to the received line item totals

        IF @QuantityReceived > 0 
            EXEC CalculateOrderDiscountForLineItem @OrderDiscountType,
                @OrderQuantityDiscount, @ItemDiscountType,
                @ItemQuantityDiscount, @ReceivedItemCost OUTPUT,
                @ReceivedItemFreight OUTPUT



        INSERT  INTO @Results
                ( LandedCost ,
                  MarkupCost ,
                  LineItemCost ,
                  LineItemFreight ,
                  ReceivedItemCost ,
                  ReceivedItemFreight ,
                  UnitCost ,
                  UnitExtCost
                )
                SELECT  @LandedCost AS LandedCost ,
                        @MarkupCost AS MarkupCost ,
                        @LineItemCost AS LineItemCost ,
                        @LineItemFreight AS LineItemFreight ,
                        ROUND(@ReceivedItemCost, 2) AS ReceivedItemCost ,
                        ROUND(@ReceivedItemFreight, 2) AS ReceivedItemFreight ,
                        ROUND(@UnitCost, 2) AS UnitCost ,
                        ROUND(@UnitExtCost, 2) AS UnitExtCost



        RETURN 

    END
GO


