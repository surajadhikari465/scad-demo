SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[UpdateOrderItemInfo]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
    drop procedure [dbo].[UpdateOrderItemInfo]
GO


CREATE PROCEDURE dbo.UpdateOrderItemInfo
        @OrderItem_ID				int, 
		@QuantityOrdered			decimal(18,4), 
	    @QuantityReceived			decimal(18,4), 
		@Total_Weight				decimal(18,4), 
	    @Units_Per_Pallet			smallint, 
		@Cost						money, 
		@Handling					money, 
		@Freight					money, 
		@AdjustedCost				money, 
		@LandedCost					money, 
		@QuantityDiscount			decimal(18,4), 
		@LineItemCost				money, 
		@LineItemFreight			money, 
		@LineItemHandling			money, 
		@ReceivedItemCost			money, 
		@ReceivedItemFreight		money, 
		@ReceivedItemHandling		money, 
		@Freight3Party				money,
		@LineItemFreight3Party		money,
		@UnitCost					money, 
		@UnitExtCost				money, 
		@QuantityUnit				int, 
		@CostUnit					int, 
		@HandlingUnit				int, 
		@FreightUnit				int, 
		@DiscountType				int, 
		@DateReceived				datetime, 
		@OriginalDateReceived		datetime, 
		@ExpirationDate				smalldatetime,
		@MarkupPercent				decimal(18,4),
		@MarkupCost					money,
		@Package_Desc1				decimal(9,4),
		@Package_Desc2				decimal(9,4),
		@Package_Unit_ID			int,
		@Retail_Unit_ID				int,
		@Origin_ID					int,
		@CountryProc_ID				int,
		@CreditReason_ID			int,
		@QuantityAllocated			decimal(18,4),
		@User_ID					int,
		@Lot_No						varchar(12),
		@ReasonCodeDetailID			int,
		@HandlingCharge				smallmoney,
		@CatchWeightCostPerWeight	money,
		@SustainabilityRankingID	int 
AS

-- **************************************************************************
-- Procedure: UpdateOrderItemInfo
--    Author: n/a
--      Date: n/a
--
-- Description: n/a
--
-- Modification History:
-- Date       	Init  	TFS   	Comment
-- 2011/12/19	KM		3744	Added update history template; extension change; coding standards;
--								moved security grants to SecurityGrants.sql
-- **************************************************************************

BEGIN
    SET NOCOUNT ON
    
    DECLARE
		@Error_No	int

    SELECT 
		@Error_No = 0

    BEGIN TRAN

    DECLARE 
		@OrderStart				datetime, 
		@OrderEnd				datetime,
		@OrderType_ID			int, 
		@Vendor_ID				int, 
		@Transfer_From_SubTeam	int, 
		@ReceiveLocation_ID		int, 
		@Return_Order			bit,
		@Transfer_To_SubTeam	int
            
	--MD 8/20/2009: Bug 10817 Removing all references to @OriginalCost and @OriginalCostUnit because these 
	--are now inserted using a trigger on OrderItem

    SELECT 
		@Vendor_ID				= Vendor_ID, 
		@Transfer_From_SubTeam	= Transfer_SubTeam, 
		@OrderType_ID			= OrderType_ID,
		@ReceiveLocation_ID		= ReceiveLocation_ID, 
		@Return_Order			= Return_Order, 
		@Transfer_To_SubTeam	= Transfer_To_SubTeam
    FROM 
		OrderHeader (nolock) 
    WHERE 
		OrderHeader_ID =	(SELECT TOP 1
								OrderHeader_ID 
                            FROM
								OrderItem (nolock) 
                            WHERE 
								OrderItem_ID = @OrderItem_ID)

    SELECT 
		@OrderStart		=	OrderStart, 
		@OrderEnd		=	CASE
								WHEN @Transfer_From_SubTeam = @Transfer_To_SubTeam THEN OrderEnd -- not transfer
								ELSE OrderEndTransfers -- is transfer
							END
    FROM 
		ZoneSubTeam				(nolock)	zst	
        INNER JOIN Vendor		(nolock)	v	ON  @Vendor_ID				= v.Vendor_ID
												AND zst.Supplier_Store_No	= v.Store_No
												AND zst.SubTeam_No			= @Transfer_From_SubTeam
        INNER JOIN Vendor		(nolock)	rl	ON	@ReceiveLocation_ID		= rl.Vendor_ID
        INNER JOIN Store		(nolock)	s	ON	rl.Store_No				= s.Store_No 
												AND zst.Zone_ID				= s.Zone_ID
    WHERE 
		@Return_Order		= 0 
		AND @OrderType_ID	= 2
		AND NOT EXISTS (SELECT *
						FROM
							Users (nolock)
						WHERE
							User_ID = @User_ID AND Warehouse = 1)


    IF (DATEDIFF(minute, CONVERT(varchar(255), ISNULL(@OrderStart, CONVERT(smalldatetime, GETDATE())), 108), CONVERT(varchar(255), CONVERT(smalldatetime, GETDATE()), 108)) >= 0)
		OR	(DATEDIFF(minute, CONVERT(varchar(255), CONVERT(smalldatetime, GETDATE()), 108), CONVERT(varchar(255), ISNULL(@OrderEnd, CONVERT(smalldatetime, GETDATE())), 108)) >= 0)
    BEGIN
        UPDATE
			OrderItem
        SET 
			QuantityOrdered			= @QuantityOrdered, 
            QuantityReceived		= @QuantityReceived, 
            Total_Weight			= @Total_Weight, 
            Units_Per_Pallet		= @Units_Per_Pallet, 
            Cost					= @Cost, 
			CostUnit				= @CostUnit, 
            Handling				= @Handling, 
            Freight					= @Freight, 
            AdjustedCost			= @AdjustedCost, 
            LandedCost				= @LandedCost, 
            QuantityDiscount		= @QuantityDiscount, 
            LineItemCost			= @LineItemCost, 
            LineItemFreight			= @LineItemFreight, 
            LineItemHandling		= @LineItemHandling, 
            ReceivedItemCost		= @ReceivedItemCost, 
            ReceivedItemFreight		= @ReceivedItemFreight, 
            ReceivedItemHandling	= @ReceivedItemHandling, 
			Freight3Party			= @Freight3Party,
			LineItemFreight3Party	= @LineItemFreight3Party,
            UnitCost				= @UnitCost, 
            UnitExtCost				= @UnitExtCost, 
            QuantityUnit			= @QuantityUnit, 
            HandlingUnit			= @HandlingUnit, 
            FreightUnit				= @FreightUnit, 
            DiscountType			= @DiscountType, 
            DateReceived			= @DateReceived, 
            OriginalDateReceived	= @OriginalDateReceived, 
            ExpirationDate			= @ExpirationDate,
            MarkupPercent			= @MarkupPercent,
            MarkupCost				= @MarkupCost,
            Package_Desc1			= @Package_Desc1,
            Package_Desc2			= @Package_Desc2,
            Package_Unit_ID			= @Package_Unit_ID,
            Retail_Unit_ID			= @Retail_Unit_ID,
            Origin_ID				= @Origin_ID,
            CountryProc_ID			= @CountryProc_ID,
            CreditReason_ID			= @CreditReason_ID,
            QuantityAllocated		= ISNULL(@QuantityAllocated, QuantityAllocated),
            Lot_No					= @Lot_No,
			ReasonCodeDetailID		= @ReasonCodeDetailID,
			HandlingCharge			= @HandlingCharge,
			CatchWeightCostPerWeight = @CatchWeightCostPerWeight,
            SustainabilityRankingID = @SustainabilityRankingID
        WHERE
			OrderItem_ID = @OrderItem_ID
    
        SELECT @Error_No = @@ERROR
            
        IF @Error_No = 0
        BEGIN
            EXEC UpdateOrderItemUnitsReceived @OrderItem_ID
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
            RAISERROR ('UpdateOrderItemInfo failed with @@ERROR: %d', @Severity, 1, @Error_No)
        END
    END
    ELSE
    BEGIN
        DECLARE @WindowStart varchar(255), @WindowEnd varchar(255)
        SELECT @WindowStart = CONVERT(varchar(255), @OrderStart, 108), @WindowEnd = CONVERT(varchar(255), @OrderEnd, 108)
        RAISERROR(50002, 16, 1, @WindowStart, @WindowEnd)
    END
END
GO


SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO