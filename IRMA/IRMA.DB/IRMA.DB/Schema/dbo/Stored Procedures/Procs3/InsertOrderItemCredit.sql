CREATE PROCEDURE dbo.InsertOrderItemCredit
    @OrderHeader_ID		int, 
    @Item_Key			int, 
    @Units_Per_Pallet	smallint, 
    @QuantityUnit		int, 
    @QuantityOrdered	decimal(18,4), 
    @Cost				decimal(18,4), 
    @CostUnit			int, 
    @Handling			decimal(18,4), 
    @HandlingUnit		int, 
    @Freight			decimal(18,4), 
    @FreightUnit		int, 
    @AdjustedCost		money, 
    @QuantityDiscount	decimal(18,4), 
    @DiscountType		int, 
    @LandedCost			money, 
    @LineItemCost		money, 
    @LineItemFreight	money, 
    @LineItemHandling	money, 
    @UnitCost			money, 
    @UnitExtCost		money,
    @Package_Desc1		decimal(9,4),
    @Package_Desc2		decimal(9,4),
    @Package_Unit_ID	int,
    @MarkupPercent		decimal(18,4),
    @MarkupCost			money,
    @Retail_Unit_ID		int,
    @CreditReason_ID	int,
    @User_ID			int,
    @VendorDiscountAmt	decimal(10,4),
    @HandlingCharge		smallmoney,
	@ReasonCodeDetailID int = null
AS
-- ****************************************************************************************************************
-- Procedure: InsertOrderItemCredit
--    Author: n/a
--      Date: n/a
--
-- Description: n/a
--
-- Modification History:
-- Date       	Init  	TFS   	Comment
-- 12/28/2012	KM		3744	Added update history template; coding standards;
-- 10/12/2012   FA      8254    Added parameter ReasonCode
-- 12/20/2012   FA      9571    Updated code to insert CreditReason_ID for DSD Credits
-- ****************************************************************************************************************
BEGIN
    SET NOCOUNT ON

    DECLARE 
		@OrderStart				datetime, 
		@OrderEnd				datetime,
        @OrderType_ID			int, 
        @Vendor_ID				int, 
        @Transfer_From_SubTeam	int, 
        @ReceiveLocation_ID		int, 
        @Return_Order			bit, 
        @DSD_Order              bit,
        @Transfer_To_Subteam	int,
		@DSD_CreditReason_ID	int
		
--**************************************************************************
-- Populate internal variables
--**************************************************************************    
    SELECT 
		@Vendor_ID				= Vendor_ID, 
		@Transfer_From_SubTeam	= Transfer_SubTeam, 
		@OrderType_ID			= OrderType_ID,
        @ReceiveLocation_ID		= ReceiveLocation_ID, 
        @Return_Order			= Return_Order, 
        @DSD_Order              = DSDOrder,
        @Transfer_To_Subteam	= Transfer_To_Subteam
    FROM
		OrderHeader (nolock) 
	WHERE 
		OrderHeader_ID = @OrderHeader_ID

    SELECT @DSD_CreditReason_ID = CreditReason_ID FROM CreditReasons WHERE CreditReason = 'Guaranteed Sale'
    
    SELECT
		@OrderStart = OrderStart, 
		@OrderEnd	=	CASE	
							WHEN @Transfer_To_Subteam = @Transfer_from_Subteam THEN OrderEnd -- not transfer
            				ELSE OrderEndTransfers -- is transfer
						END
    FROM 
		ZoneSubTeam						(nolock) zst
		INNER JOIN Vendor				(nolock) v		ON	@Vendor_ID				= v.Vendor_ID
														AND ZST.Supplier_Store_No	= v.Store_No 
														AND ZST.SubTeam_No			= @Transfer_From_SubTeam
		INNER JOIN Vendor 				(nolock) vr		ON	@ReceiveLocation_ID		= vr.Vendor_ID 
		INNER JOIN Store				(nolock) s		ON	vr.Store_No				= s.Store_No 
														AND ZST.Zone_ID				= s.Zone_ID 
    WHERE 
		@Return_Order		= 0 
		AND @OrderType_ID	= 2
		AND NOT EXISTS	(SELECT * 
						FROM 
							Users (nolock) 
						WHERE 
							User_ID = @User_ID AND Warehouse = 1
						)

--**************************************************************************
-- Main SQL
--**************************************************************************    
    IF @orderType_ID	<> 2 
						OR (DATEDIFF(minute, CONVERT(varchar(255), ISNULL(@OrderStart, CONVERT(smalldatetime, GETDATE())), 108), CONVERT(varchar(255), CONVERT(smalldatetime, GETDATE()), 108)) >= 0) 
						OR (DATEDIFF(minute, CONVERT(varchar(255), CONVERT(smalldatetime, GETDATE()), 108), CONVERT(varchar(255), ISNULL(@OrderEnd, CONVERT(smalldatetime, GETDATE())), 108)) >= 0)
		BEGIN
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
                CreditReason_ID, 
                Origin_ID, 
                CountryProc_ID, 
                NetVendorItemDiscount,
                HandlingCharge, 
                OrderItemCOOL, 
                OrderItemBIO, 
                SustainabilityRankingID,
				ReasonCodeDetailID
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
				CASE WHEN @DSD_Order = 1 AND @Return_Order = 1 THEN ISNULL(@DSD_CreditReason_ID,0) ELSE @CreditReason_ID END,
				CASE WHEN SubTeam_No = 2800 AND @Return_Order = 0 THEN Origin_ID ELSE NULL END,
				CASE WHEN SubTeam_No = 2800 AND @Return_Order = 0 THEN CountryProc_ID ELSE NULL END,
				@VendorDiscountAmt, 
				@HandlingCharge, 
				COOL, 
				BIO, 
				SustainabilityRankingID,
				@ReasonCodeDetailID
			FROM
				Item (nolock)
			WHERE 
				Item.Item_Key = @Item_Key
		END
    ELSE
		BEGIN
		  DECLARE @WindowStart varchar(255), @WindowEnd varchar(255)
		  SELECT @WindowStart = CONVERT(varchar(255), @OrderStart, 108), @WindowEnd = CONVERT(varchar(255), @OrderEnd, 108)
		  RAISERROR(50002, 16, 1, @WindowStart, @WindowEnd)
		END
    
	IF @@error = 0 
		SELECT CAST(@@IDENTITY AS int)
		        
    SET NOCOUNT OFF
END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[InsertOrderItemCredit] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[InsertOrderItemCredit] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[InsertOrderItemCredit] TO [IRMAReportsRole]
    AS [dbo];

