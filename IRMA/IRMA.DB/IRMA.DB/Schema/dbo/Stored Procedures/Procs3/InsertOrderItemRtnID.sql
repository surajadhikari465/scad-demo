CREATE PROCEDURE dbo.InsertOrderItemRtnID
	@OrderHeader_ID				int, 
	@Item_Key					int, 
	@Units_Per_Pallet			smallint, 
	@QuantityUnit				int, 
	@QuantityOrdered			decimal(18,4), 
	@Cost						decimal(18,4), 
	@CostUnit					int, 
	@Handling					decimal(18,4), 
	@HandlingUnit				int, 
	@Freight					decimal(18,4), 
	@FreightUnit				int, 
	@AdjustedCost				money, 
	@QuantityDiscount			decimal(18,4), 
	@DiscountType				int, 
	@LandedCost					money, 
	@LineItemCost				money, 
	@LineItemFreight			money, 
	@LineItemHandling			money, 
	@UnitCost					money, 
	@UnitExtCost				money,
	@Package_Desc1				decimal(9,4),
	@Package_Desc2				decimal(9,4),
	@Package_Unit_ID			int,
	@MarkupPercent				decimal(18,4),
	@MarkupCost					money,
	@Retail_Unit_ID				int,
	@CostAdjustmentReason_ID	int,
	@OrderItemID				int OUTPUT
AS
-- **************************************************************************
-- Procedure: InsertOrderItemRtnID
--    Author: n/a
--      Date: n/a
--
-- Description: n/a
--
-- Modification History:
-- Date       	Init  	TFS   	Comment
-- 2011/12/22	KM		3744	Added update history template; extension change; coding standards
-- **************************************************************************
BEGIN
    SET NOCOUNT ON

    DECLARE 
		@Return_Order	bit

    SELECT 
		@Return_Order = Return_Order 
	FROM 
		OrderHeader (nolock) 
	WHERE 
		OrderHeader_ID = @OrderHeader_ID
    
    INSERT INTO OrderItem 
    (
		OrderHeader_ID, 
		Item_Key, 
		Units_Per_Pallet, 
		QuantityUnit, 
		QuantityOrdered, 
		Cost, 
		CostUnit, 
		Handling, 
		HandlingUnit, 
		Freight, 
		FreightUnit, 
		AdjustedCost, 
		QuantityDiscount, 
		DiscountType, 
		LandedCost, 
		LineItemCost, 
		LineItemFreight, 
		LineItemHandling, 
		UnitCost, 
		UnitExtCost, 
		Package_Desc1, 
		Package_Desc2, 
		Package_Unit_ID, 
		MarkupPercent, 
		MarkupCost, 
		Retail_Unit_ID, 
		CostAdjustmentReason_ID, 
		Origin_ID, 
		CountryProc_ID, 
		OrderItemCool, 
		OrderItemBIO, 
		SustainabilityRankingID
	) 
    SELECT 
        @OrderHeader_ID,
        @Item_Key, 
        @Units_Per_Pallet, 
        @QuantityUnit, 
        @QuantityOrdered, 
        @Cost, 
        @CostUnit, 
        @Handling, 
        @HandlingUnit, 
        @Freight, 
        @FreightUnit, 
        @AdjustedCost, 
        @QuantityDiscount, 
        @DiscountType, 
        @LandedCost, 
        @LineItemCost, 
        @LineItemFreight, 
        @LineItemHandling, 
        @UnitCost, 
        @UnitExtCost, 
        @Package_Desc1, 
        @Package_Desc2, 
        @Package_Unit_ID, 
        @MarkupPercent, 
        @MarkupCost, 
        @Retail_Unit_ID, 
        @CostAdjustmentReason_ID,
        CASE WHEN SubTeam_No = 2800 AND @Return_Order = 0 THEN Origin_ID ELSE NULL END,
        CASE WHEN SubTeam_No = 2800 AND @Return_Order = 0 THEN CountryProc_ID ELSE NULL END,
		Cool, 
		BIO, 
		SustainabilityRankingID 
    FROM
		Item (nolock)
    WHERE
		Item_Key = @Item_Key

    SELECT @OrderItemID = scope_identity()
            
    SET NOCOUNT OFF
END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[InsertOrderItemRtnID] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[InsertOrderItemRtnID] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[InsertOrderItemRtnID] TO [IRMAReportsRole]
    AS [dbo];

