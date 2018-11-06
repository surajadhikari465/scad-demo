CREATE PROCEDURE dbo.InsertOrderItem
@OrderHeader_ID int, 
@Item_Key int, 
@Units_Per_Pallet smallint, 
@QuantityUnit int, 
@QuantityOrdered decimal(18,4), 
@Cost decimal(18,4), 
@CostUnit int, 
@Handling decimal(18,4), 
@HandlingUnit int, 
@Freight decimal(18,4), 
@FreightUnit int, 
@AdjustedCost money, 
@QuantityDiscount decimal(18,4), 
@DiscountType int, 
@LandedCost money, 
@LineItemCost money, 
@LineItemFreight money, 
@LineItemHandling money, 
@UnitCost money, 
@UnitExtCost money,
@Package_Desc1 decimal(9,4),
@Package_Desc2 decimal(9,4),
@Package_Unit_ID int,
@MarkupPercent decimal(18,4),
@MarkupCost money,
@CostAdjustmentReason_ID int

AS
BEGIN
    SET NOCOUNT ON
    DECLARE @Retail_Unit_ID int
    DECLARE @OrderItemID int

    EXEC InsertOrderItemRtnID  @OrderHeader_ID, @Item_Key, @Units_Per_Pallet, @QuantityUnit, @QuantityOrdered, @Cost, @CostUnit, @Handling, @HandlingUnit,  
                               @Freight, @FreightUnit, @AdjustedCost, @QuantityDiscount, @DiscountType, @LandedCost, @LineItemCost, @LineItemFreight, 
                               @LineItemHandling, @UnitCost, @UnitExtCost, @Package_Desc1, @Package_Desc2, @Package_Unit_ID, @MarkupPercent, @MarkupCost, 
                               @Retail_Unit_ID, @CostAdjustmentReason_ID, @OrderItemID
            
    SET NOCOUNT OFF
END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[InsertOrderItem] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[InsertOrderItem] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[InsertOrderItem] TO [IRMAReportsRole]
    AS [dbo];

