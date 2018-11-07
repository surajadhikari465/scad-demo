CREATE FUNCTION [dbo].[fn_GetOrigReceivedCost] ( @OrderItem_ID INT )
RETURNS MONEY
AS 
    BEGIN

    --------------------------------------------------------------------------------------------

    -- This function returns a cost based on the originalItemCost in OrderItem:

    --------------------------------------------------------------------------------------------



--For Test

--DECLARE @OrderItem_ID int

--SET @OrderItem_ID = 2456839



--Input Params for SProc

        DECLARE @QuantityOrdered DECIMAL(18, 4) ,
            @QuantityUnit INT ,
            @QuantityReceived DECIMAL(18, 4) ,
            @Total_Weight DECIMAL(18, 4) ,
            @Cost MONEY ,
            @CostUnit INT ,
            @OrderQuantityDiscount DECIMAL(18, 4) ,
            @OrderDiscountType INT ,
            @ItemQuantityDiscount DECIMAL(18, 4) ,
            @ItemDiscountType INT ,
            @Freight DECIMAL(18, 4) ,
            @FreightUnit INT ,
            @MarkupPercent DECIMAL(18, 4) ,
            @Package_Desc1 DECIMAL(9, 4) ,
            @Package_Desc2 DECIMAL(9, 4) ,
            @Package_Unit_ID INT ,
            @IsCostedByWeight BIT ,
            @MarkupDollars MONEY

 

--Output Params for SProc

        DECLARE @LandedCost MONEY ,
            @MarkupCost MONEY ,
            @LineItemCost MONEY ,
            @LineItemFreight MONEY ,
            @ReceivedItemCost MONEY ,
            @ReceivedItemFreight MONEY ,
            @UnitCost MONEY ,
            @UnitExtCost MONEY



        SELECT  @QuantityOrdered = dbo.OrderItem.QuantityOrdered ,
                @QuantityUnit = dbo.OrderItem.QuantityUnit ,
                @QuantityReceived = dbo.OrderItem.QuantityReceived ,
                @Total_Weight = dbo.OrderItem.Total_Weight ,
                @Cost = dbo.OrderItem.OrigReceivedItemCost ,
                @CostUnit = dbo.OrderItem.OrigReceivedItemUnit ,
                @OrderQuantityDiscount = dbo.OrderHeader.QuantityDiscount ,
                @OrderDiscountType = dbo.OrderHeader.DiscountType ,
                @ItemQuantityDiscount = dbo.OrderItem.QuantityDiscount ,
                @ItemDiscountType = dbo.OrderItem.DiscountType ,
                @Freight = dbo.OrderItem.Freight ,
                @FreightUnit = dbo.OrderItem.FreightUnit ,
                @MarkupPercent = dbo.OrderItem.MarkupPercent ,
                @Package_Desc1 = dbo.OrderItem.Package_Desc1 ,
                @Package_Desc2 = dbo.OrderItem.Package_Desc2 ,
                @Package_Unit_ID = dbo.OrderItem.Package_Unit_ID ,
                @IsCostedByWeight = dbo.Item.CostedByWeight ,
                @MarkupDollars = dbo.OrderItem.MarkupCost
        FROM    dbo.OrderItem (NOLOCK)
                INNER JOIN dbo.OrderHeader (NOLOCK) ON dbo.OrderItem.OrderHeader_ID = dbo.OrderHeader.OrderHeader_ID
                INNER JOIN dbo.Item (NOLOCK) ON dbo.OrderItem.Item_Key = dbo.Item.Item_Key
        WHERE   dbo.OrderItem.OrderItem_ID = @OrderItem_ID



        SELECT  @ReceivedItemCost = dbo.fn_GetOrigReceivedItemCost(@QuantityOrdered,
                                                              @QuantityUnit,
                                                              @QuantityReceived,
                                                              @Total_Weight,
                                                              @Cost, @CostUnit,
                                                              @OrderQuantityDiscount,
                                                              @OrderDiscountType,
                                                              @ItemQuantityDiscount,
                                                              @ItemDiscountType,
                                                              @Freight,
                                                              @FreightUnit,
                                                              @MarkupPercent,
                                                              @Package_Desc1,
                                                              @Package_Desc2,
                                                              @Package_Unit_ID,
                                                              @IsCostedByWeight,
                                                              @MarkupDollars)



        RETURN @ReceivedItemCost



    END