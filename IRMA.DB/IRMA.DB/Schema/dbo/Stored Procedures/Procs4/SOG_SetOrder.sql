CREATE PROCEDURE dbo.SOG_SetOrder
	@CatalogOrderID int 
AS

-- **************************************************************************
-- Procedure: SOG_SetOrder()
--    Author: Billy Blackerby
--      Date: 3/26/2009
--
-- Description:
-- Utilized by StoreOrderGuide to assign various OrderItems to Orders based upon
-- SubTeam of the items contained in the Order
--
-- Modification History:
-- Date			Init	TFS		Comment
-- 03/26/2009	BBB				Creation
-- 03/27/2009	BBB				Added ExpectedDate pull from CatalogOrder
-- 04/22/2009	BBB				Added CatalogOrderID to WHERE of Initial Order Populate
-- 04/23/2009	BBB				Added join to ItemManager to retrieve newly added
--								Vendor_ID column to Create Orders; corrected issues with
--								cost calculations going into OrderItem; added AvgCost
--								lookup for Warehouse items in OrderItem push; added in
--								isnull traps for cost values
-- 04/28/2009	BBB				Added in clean-up to CatalogOrder and CatalogOrderItem
-- 05/05/2009	BBB				Reversed values for PackDesc1 and PackDesc2 in OrderItem
--								push
-- 05/27/2009	BBB				Added in check for DeleteDate against SIV in VCH pull
-- 06/04/2009	BBB				Added in calls to AvgCostHistory and a CASE statement
--								to handle the different cost calls for Warehouse/Kitchen
-- 06/05/2009	BBB				Updated joins to Store and SIV based upon the Catalog.Store
--								instead of based upon Catalog.Vendor
-- 06/06/2009	BBB				Updated ExpectedDate, OrderDate, SentDate values to only
--								pass the date value to OrderHeader
-- 06/09/2009	BBB				Removed comments from Clean-up of CatalogOrder and CatalogOrderItem
-- 06/10/2009	BBB				Updated call to AvgCost to utilize vendorID instead of StoreNo
-- 06/10/2009	RDS				Updated call to AvgCost to utilize Vendor's StoreNo instead of vendorID
-- 02/25/2010   RDE				tfs 12033 Made adjustments to LandedCost and MarkupCost for NON KITCHEN orders only.
--								Once the regions decide what they expect for Kitchen Orders those changes will be made too, if needed.
-- 12/08/2011	BBB		3744	added OrderedCost; removed WITH RECOMPLILE;
-- 04/27/2012   VAA		5524    added Order Item details for SOG confirmation email;
-- 05/02/2012   MZ      5524    Populate the OrderExternalSourceID when SOG order is pushed to IRMA
-- **************************************************************************
BEGIN
    SET NOCOUNT ON
	--**************************************************************************
	--Collect OrderItems
	--**************************************************************************
    CREATE TABLE #OrderItems
			(
			CatalogOrderItemID		int,
			CatalogOrder			int,
			CatalogItemID			int,
			SubTeamNo				int,
			)

    INSERT INTO #OrderItems
		SELECT
			[CatalogOrderItemID]	= coi.CatalogOrderItemID,
			[CatalogOrder]			= coi.CatalogOrderID,
			[CatalogItemID]			= coi.CatalogItemID,
			[SubTeamNo]				= i.SubTeam_No
		FROM
			CatalogOrderItem			(nolock) coi
			INNER JOIN CatalogItem		(nolock) ci		ON	coi.CatalogItemID		= ci.CatalogItemID
			INNER JOIN Item				(nolock) i		ON	ci.ItemKey				= i.Item_Key
		WHERE
			coi.CatalogOrderID = @CatalogOrderID

	--**************************************************************************
	--Populate initial Order with SubTeam
	--**************************************************************************
	UPDATE
		CatalogOrder
	SET
		FromSubTeamID = 
						(
						SELECT DISTINCT TOP 1
							SubTeamID
						FROM
							CatalogOrderItem
						WHERE 
							CatalogOrderID = @CatalogOrderID
						ORDER BY
							SubTeamID
						)
	WHERE
		CatalogOrderID = @CatalogOrderID

	--**************************************************************************
	--Create Orders based upon number of SubTeams in OrderItems
	--**************************************************************************
	INSERT INTO 
		CatalogOrder
	(
		CatalogID,
		VendorID,
		StoreID,
		UserID,
		FromSubTeamID,
		ToSubTeamID,
		ParentID,
		ExpectedDate
	)
	(		
		SELECT DISTINCT
			co.CatalogID,
			im.Vendor_ID,
			co.StoreID,
			co.UserID,
			coi.SubTeamID,
			c.SubTeam,
			@CatalogOrderID,
			co.ExpectedDate
		FROM
			CatalogOrderItem			(nolock) coi
			INNER JOIN	CatalogOrder	(nolock) co ON coi.CatalogOrderID	= co.CatalogOrderID
			INNER JOIN  Catalog			(nolock) c	ON co.CatalogID			= c.CatalogID
			INNER JOIN  ItemManager		(nolock) im	ON c.ManagedByID		= im.Manager_ID
		WHERE
			co.CatalogOrderID	= @CatalogOrderID
			AND	coi.SubTeamID	<>	(
									SELECT 
										FromSubTeamID 
									FROM 
										CatalogOrder 
									WHERE 
										CatalogOrderID = @CatalogOrderID
									)
	)
	--**************************************************************************
	--Populate Orders based upon Item SubTeam
	--**************************************************************************
	UPDATE
		CatalogOrderItem
	SET
		CatalogOrderItem.CatalogOrderID = CatalogOrder.CatalogOrderID
	FROM
		CatalogOrderItem
		INNER JOIN CatalogOrder	ON	CatalogOrderItem.SubTeamID	= CatalogOrder.FromSubTeamID
								AND	CatalogOrder.ParentID		= CatalogOrderItem.CatalogOrderID
	WHERE 
		(CatalogOrderItem.CatalogOrderID = @CatalogOrderID)

	--**************************************************************************
	--Collect Orders
	--**************************************************************************
	CREATE TABLE #PushOrders
			(
			CatalogOrderID		int,
			OrderHeaderDesc		varchar(4000), 
			Vendor_ID			int, 
			PurchaseLocation_ID	int, 
			ReceiveLocation_ID	int, 
			CreatedBy			int, 
			OrderDate			smalldatetime, 
			Sent				bit, 
			Fax_Order			bit,
			Expected_Date		smalldatetime, 
			SentDate			smalldatetime, 
			QuantityDiscount	decimal(9,2), 
			DiscountType		int, 
			Transfer_To_Subteam	int, 
			Transfer_SubTeam	int,
			Return_Order		bit, 
			WarehouseSent		bit, 
			OrderType_ID		int, 
			ProductType_ID		int, 
			FromQueue			bit
			)

    INSERT INTO #PushOrders
		SELECT
			[CatalogOrderID]		= co.CatalogOrderID,
			[OrderHeaderDesc]		= 'SOG Order: ' + CONVERT(varchar(8), co.CatalogOrderID), 
			[Vendor_ID]				= co.VendorID, 
			[PurchaseLocation_ID]	= v.Vendor_ID, 
			[ReceiveLocation_ID]	= v.Vendor_ID, 
			[CreatedBy]				= co.UserID, 
			[OrderDate]				= CONVERT(varchar(10), GETDATE(), 101), 
			[Sent]					= 1, 
			[Fax_Order]				= 0,
			[Expected_Date]			= CONVERT(varchar(10), co.ExpectedDate, 101), 
			[SentDate]				= CONVERT(varchar(10), GETDATE(), 101),
			[QuantityDiscount]		= 0, 
			[DiscountType]			= 0, 
			[Transfer_To_Subteam]	= co.ToSubTeamID,
			[Transfer_SubTeam]		= co.FromSubTeamID,
			[Return_Order]			= 0, 
			[WarehouseSent]			= 0, 
			[OrderType_ID]			= 2, 
			[ProductType_ID]		= 1, 
			[FromQueue]				= 0
		FROM
			CatalogOrder		(nolock) co
			INNER JOIN Store	(nolock) s	ON co.StoreID	= s.Store_No
			INNER JOIN Vendor	(nolock) v	ON s.Store_No	= v.Store_No
		WHERE
			co.CatalogOrderID	= @CatalogOrderID
			OR co.ParentID		= @CatalogOrderID

	--**************************************************************************
	--Collect OrderItems
	--**************************************************************************
    CREATE TABLE #PushItems
			(
			CatalogOrderItemID		int,
			CatalogOrderID			int,
			CatalogItemID			int,
			SubTeamNo				int,
			OrderHeader_ID			int, 
			Item_Key				int,
			QuantityOrdered			int,
			QuantityUnit			int,
			Cost					money,
			UnitCost				money,
			UnitExtCost				money,
			LineItemCost			money,
			LandedCost				money,
			MarkupCost				money,
			CostUnit				int,
			QuantityDiscount		decimal(18,4),
			DiscountType			int, 
			HandlingUnit			int,
			FreightUnit				int,
			Package_Desc1			decimal(9,4),
			Package_Desc2			decimal(9,4),
			Package_Unit_ID			int,
			Retail_Unit_ID			int,
			NetVendorItemDiscount	money,
			Freight3Party			money,
			LineItemFreight3Party	money, 
			HandlingCharge			money
			)

    INSERT INTO #PushItems
		SELECT
			[CatalogOrderItemID]	= coi.CatalogOrderItemID,
			[CatalogOrderID]		= coi.CatalogOrderID,
			[CatalogItemID]			= coi.CatalogItemID,
			[SubTeamNo]				= i.SubTeam_No,
			[OrderHeader_ID]		= 0, 
			[Item_Key]				= i.Item_Key,
			[QuantityOrdered]		= coi.Quantity,
			[QuantityUnit]			= vch.CostUnit,
			[Cost]					= CASE im.Value
										WHEN 'Kitchen' THEN
											vch.UnitCost * vch.PackSize
										ELSE
											vch.AvgCostHistory * vch.PackSize
									  END,
			[UnitCost]				= CASE im.Value
										WHEN 'Kitchen' THEN
											vch.UnitCost
										ELSE
											vch.AvgCostHistory
									  END,
			[UnitExtCost]			= CASE im.Value
										WHEN 'Kitchen' THEN
											vch.UnitCost
										ELSE
											vch.AvgCostHistory
									  END,
			[LineItemCost]			= CASE im.Value
										WHEN 'Kitchen' THEN
											(vch.UnitCost * vch.PackSize) * coi.Quantity
										ELSE
											(vch.AvgCostHistory * vch.PackSize) * coi.Quantity
									  END,
			[LandedCost]			= CASE im.Value
										WHEN 'Kitchen' THEN
											(vch.UnitCost * vch.PackSize) * coi.Quantity
										ELSE
											(vch.AvgCostHistory * vch.PackSize) 
									  END,
			[MarkupCost]			= CASE im.Value
										WHEN 'Kitchen' THEN
											(vch.UnitCost * vch.PackSize) * coi.Quantity
										ELSE
											(vch.AvgCostHistory * vch.PackSize) + CASE ISNULL(iv.CaseDistHandlingChargeOverride, 0)
																					WHEN 0 THEN
																						ISNULL(v.CaseDistHandlingCharge, 0)
																					ELSE
																						iv.CaseDistHandlingChargeOverride
																					END
									  END,
			[CostUnit]				= vch.CostUnit,
			[QuantityDiscount]		= 0,
			[DiscountType]			= 0, 
			[HandlingUnit]			= vch.CostUnit,
			[FreightUnit]			= vch.FreightUnit,
			[Package_Desc1]			= vch.PackSize,
			[Package_Desc2]			= i.Package_Desc2,
			[Package_Unit_ID]		= i.Package_Unit_ID,
			[Retail_Unit_ID]		= i.Retail_Unit_ID,
			[NetVendorItemDiscount]	= 0,
			[Freight3Party]			= 0,
			[LineItemFreight3Party]	= 0,  
			[HandlingCharge]		=	CASE ISNULL(iv.CaseDistHandlingChargeOverride, 0)
											WHEN 0 THEN
												ISNULL(v.CaseDistHandlingCharge, 0)
											ELSE
												iv.CaseDistHandlingChargeOverride
										END
		FROM
			CatalogOrderItem			(nolock) coi
			INNER JOIN CatalogOrder		(nolock) co		ON	coi.CatalogOrderId		= co.CatalogOrderId
			INNER JOIN CatalogItem		(nolock) ci		ON	coi.CatalogItemID		= ci.CatalogItemID
			INNER JOIN Item				(nolock) i		ON	ci.ItemKey				= i.Item_Key
			INNER JOIN Catalog			(nolock) c		ON  co.CatalogID			= c.CatalogID
			INNER JOIN ItemManager		(nolock) im		ON	c.ManagedByID			= im.Manager_ID
			INNER JOIN Vendor			(nolock) v		ON	co.VendorID				= v.Vendor_ID
			INNER JOIN Store			(nolock) s		ON	co.StoreID				= s.Store_No
			INNER JOIN ItemVendor		(nolock) iv		ON	i.Item_Key				= iv.Item_Key
														AND	v.Vendor_ID				= iv.Vendor_ID
			INNER JOIN #OrderItems		(nolock) oi		ON	coi.CatalogOrderItemID	= oi.CatalogOrderItemID
			OUTER APPLY
						(
							--**************************************************************************
							-- Select the latest NetCost and Average Cost for the item
							--**************************************************************************
							SELECT TOP 1 
								[UnitCost]			=	(vch2.UnitCost / vch2.Package_Desc1),
								[PackSize]			=	vch2.Package_Desc1,
								[CostUnit]			=	vch2.CostUnit_ID,
								[FreightUnit]		=	vch2.FreightUnit_ID,
								[AvgCostHistory]	=	IsNull(dbo.fn_AvgCostHistory(i.Item_Key, v.Store_No, i.SubTeam_No, GETDATE()), 0)
							FROM
								VendorCostHistory			(nolock) vch2
								INNER JOIN StoreItemVendor	(nolock) siv2	ON	siv2.StoreItemVendorID	= vch2.StoreItemVendorID
																			AND	siv2.Item_Key			= i.Item_Key
																			AND siv2.Vendor_ID			= v.Vendor_ID
																			AND siv2.Store_No			= s.Store_No
							WHERE 
								StartDate			<=	GETDATE()
								AND EndDate			>=	GETDATE()
								AND siv2.DeleteDate IS	NULL
							ORDER BY 
								VendorCostHistoryID DESC
						) vch
						
	--**************************************************************************
	--Push From #PushOrders into IRMA OrderHeader
	--**************************************************************************
	INSERT INTO
		OrderHeader
	(
		OrderHeaderDesc, 
		Vendor_ID, 
		PurchaseLocation_ID, 
		ReceiveLocation_ID, 
		CreatedBy, 
		OrderDate, 
		Sent, 
		Fax_Order,
		Expected_Date, 
		SentDate, 
		QuantityDiscount, 
		DiscountType, 
		Transfer_To_Subteam, 
		Transfer_SubTeam,
		Return_Order, 
		WarehouseSent, 
		OrderType_ID, 
		ProductType_ID, 
		FromQueue,
		OrderedCost,
        OrderExternalSourceID

	)
	(
		SELECT
			OrderHeaderDesc, 
			Vendor_ID, 
			PurchaseLocation_ID, 
			ReceiveLocation_ID, 
			CreatedBy, 
			OrderDate, 
			Sent, 
			Fax_Order,
			Expected_Date, 
			SentDate, 
			QuantityDiscount, 
			DiscountType, 
			Transfer_To_Subteam, 
			Transfer_SubTeam,
			Return_Order, 
			WarehouseSent, 
			OrderType_ID, 
			ProductType_ID, 
			FromQueue,
			(SELECT SUM(LineItemCost) FROM #PushItems WHERE CatalogOrderID = #PushOrders.CatalogOrderID),
		    (SELECT ID FROM OrderExternalSource WHERE Description = 'SOG')
		FROM
			#PushOrders
	)

	--**************************************************************************
	--Push From #PushItems into IRMA OrderItem
	--**************************************************************************
	INSERT INTO
		OrderItem
	(
		OrderHeader_ID, 
		Item_Key,
		QuantityOrdered,
		QuantityUnit,
		Cost,
		UnitCost,
		UnitExtCost,
		LineItemCost,
		LandedCost,
		MarkupCost,
		CostUnit,
		QuantityDiscount,
		DiscountType, 
		HandlingUnit,
		FreightUnit,
		Package_Desc1,
		Package_Desc2,
		Package_Unit_ID,
		Retail_Unit_ID,
		NetVendorItemDiscount,
		Freight3Party,
		LineItemFreight3Party, 
		HandlingCharge
	)
	(
		SELECT 
			(SELECT OrderHeader_ID FROM OrderHeader WHERE OrderHeaderDesc = 'SOG Order: ' + CONVERT(varchar(8), CatalogOrderID)),
			Item_Key,
			QuantityOrdered,
			QuantityUnit,
			Cost,
			UnitCost,
			UnitExtCost,
			LineItemCost,
			LandedCost,
			MarkupCost,
			CostUnit,
			QuantityDiscount,
			DiscountType, 
			HandlingUnit,
			FreightUnit,
			Package_Desc1,
			Package_Desc2,
			Package_Unit_ID,
			Retail_Unit_ID,
			NetVendorItemDiscount,
			Freight3Party,
			LineItemFreight3Party, 
			HandlingCharge
		FROM
			#PushItems
	)

	--**************************************************************************
	--Drop Temp Tables
	--**************************************************************************
	DROP TABLE #OrderItems
	DROP TABLE #PushItems
	DROP TABLE #PushOrders

	--**************************************************************************
	--Return newly created orderheaders and item details
	--**************************************************************************	
	SELECT 
		oh.OrderHeader_ID , i.Item_Description, ii.identifier, CONVERT(DECIMAL(10,2),oi.QuantityOrdered), CONVERT(DECIMAL(10,2),oi.Cost)
	FROM 
		OrderHeader					(nolock) oh
		INNER JOIN CatalogOrder		(nolock) co ON oh.OrderHeaderDESC = ('SOG Order: ' + CONVERT(varchar(8), co.CatalogOrderID))
		INNER JOIN OrderItem		(nolock) oi ON oh.OrderHeader_ID = oi.OrderHeader_ID
		INNER JOIN Item				(nolock) i	ON oi.Item_Key = i.Item_Key
		INNER JOIN ItemIdentifier	(nolock) ii ON i.item_key = ii.item_key
	WHERE 
	    ii.Default_Identifier = 1  AND	
		(co.CatalogOrderID	= @CatalogOrderID 
		OR co.ParentID		= @CatalogOrderID)
		
	ORDER BY 
		OrderHeader_ID, i.Item_Description DESC
		
	--**************************************************************************
	--Clean up CatalogOrder and CatalogOrderItem tables
	--**************************************************************************	
	DELETE 
		CatalogOrderItem
	FROM
		CatalogOrderItem	coi
		JOIN CatalogOrder	co ON co.CatalogOrderID = coi.CatalogOrderID
	WHERE
		coi.CatalogOrderID	= @CatalogOrderID
		OR co.ParentID		= @CatalogOrderID
	
	DELETE FROM
		CatalogOrder
	WHERE
		CatalogOrderID	= @CatalogOrderID 
		OR ParentID		= @CatalogOrderID

    SET NOCOUNT OFF
END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[SOG_SetOrder] TO [IRMASLIMRole]
    AS [dbo];

