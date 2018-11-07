CREATE PROCEDURE dbo.InsertOrderItemCredit2
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
    @Retail_Unit_ID int,  
    @CreditReason_ID int,  
    @HandlingCharge smallmoney,  
    @NewOrderItem_ID int OUTPUT  
AS  
BEGIN  
    SET NOCOUNT ON  
  
    DECLARE @Return_Order bit

	if @HandlingUnit = -1
	begin
	/*	
		FIX for hardcoded units in ItemCatalogLib  Order.VB  AddDB()
		A call to this stored proc was made with @HandlingUnit hardcoded to 1
		Not all 3.2 IRMA regions had an ItemUnit with UnitId 1 (FL started with 81)
		This is a hack to get around a problem. @Handling is hardcoded to 0 and not used here.
		This is just to get a valid (but not used) value for @HandlingUnit
	*/
	
		set @HandlingUnit = (select top 1 unit_id from itemunit (nolock))
	end	
  
    SELECT @Return_Order = Return_Order FROM dbo.OrderHeader (nolock) WHERE OrderHeader_ID = @OrderHeader_ID  
  
    INSERT INTO OrderItem (OrderHeader_ID, Item_Key, Units_Per_Pallet, QuantityUnit, QuantityOrdered, Cost, CostUnit, Handling,   
                           HandlingUnit, Freight, FreightUnit, AdjustedCost, QuantityDiscount, DiscountType, LandedCost, LineItemCost,   
                           LineItemFreight, LineItemHandling, UnitCost, UnitExtCost, Package_Desc1, Package_Desc2, Package_Unit_ID,   
                           MarkupPercent, MarkupCost, Retail_Unit_ID, CreditReason_ID, Origin_ID, CountryProc_ID,HandlingCharge, OrderItemCOOL, OrderItemBIO, SustainabilityRankingID)   
    SELECT  
        @OrderHeader_ID, @Item_Key, @Units_Per_Pallet, @QuantityUnit, @QuantityOrdered, @Cost, @CostUnit, @Handling,   
        @HandlingUnit, @Freight, @FreightUnit, @AdjustedCost, @QuantityDiscount, @DiscountType, @LandedCost, @LineItemCost,   
        @LineItemFreight, @LineItemHandling, @UnitCost, @UnitExtCost, @Package_Desc1, @Package_Desc2, @Package_Unit_ID,   
        @MarkupPercent, @MarkupCost, @Retail_Unit_ID, @CreditReason_ID,  
        CASE WHEN SubTeam_No = 2800 AND @Return_Order = 0 THEN Origin_ID ELSE NULL END,  
        CASE WHEN SubTeam_No = 2800 AND @Return_Order = 0 THEN CountryProc_ID ELSE NULL END,  
        @HandlingCharge, COOL, BIO, SustainabilityRankingID  
    FROM dbo.Item (nolock)  
    WHERE Item_Key = @Item_Key  
  
    SELECT @NewOrderItem_ID = SCOPE_IDENTITY()  
              
    SET NOCOUNT OFF
END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[InsertOrderItemCredit2] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[InsertOrderItemCredit2] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[InsertOrderItemCredit2] TO [IRMAReportsRole]
    AS [dbo];

