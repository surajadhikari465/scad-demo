SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[GetOrderItemInfo]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[GetOrderItemInfo]
GO

CREATE PROCEDURE [dbo].[GetOrderItemInfo]
	@OrderItem_ID	int,
	@OrderHeader_ID	int,
	@Record			int

AS 

	-- **************************************************************************
	-- Procedure: GetOrderItemInfo()
	--    Author: n/a
	--      Date: n/a
	--
	-- Modification History:
	-- Date			Init	TFS		Comment
	-- 2011/01/24	TTL		759		Added @CostDate var and updated to include any vendor lead-time in the date used to pull vendor cost attributes.
	-- 2011/07/20	MD		2095	Added ReasonCodeDetailID to result set.
	-- 2011/12/08	KM		3744	Added PaidCost to result set; added boolean logic for deprecated
	--								GetOrderItemInfoPrevious, Next, and Last stored procedures; coding standards.
	-- 2011/12/12	KM		3744	More coding standards.
	-- 2011/12/14	KM		3744	Fixed joins.
	-- 2011/12/15	KM		3744	Used OUTER APPLY with VendorCost fn; deprecated unused internal variables.
	-- 2011/12/16	KM		3744	Commenting out PaidCost addition for now, until 4.4 builds run successfully.
	-- 2011/12/20	KM		3744	Corrected join to Einvoicing_Header table.
	-- 2011/12/31	BBB		3744	PaidLineItemCost Logic;
	-- 01.31.2012	BBB		4395	PaidCost logic was looking at Order instead of Item Suspension; fixes to logic and values; 
	-- 02.09.2012	BBB		4395	Changed order of operations for PaidCost Logic and applied additional comments;
	-- 03/28/2012	DN		4865	PaidLineItemCost field should not be populated if the item is not received/paid for.
	-- 2013/01/04	KM		9251	Check ItemOverride for new 4.8 override values (Sustainability, CostedByWeight);
	-- 02/19/2015	DN		15842	Added Brand_Name to the SELECT statement	
	-- **************************************************************************

BEGIN 
	SET NOCOUNT ON
	
	--**************************************************************************
	--Declare internal variables
	--**************************************************************************
	
	DECLARE 
		@CurrentRecord					int,
		@TotalRecords					int,
		@PREV							int = -3,
		@NEXT							int = -2,
		@LAST							int = -1,
		@CURR							int =  0

	--**************************************************************************
	--Populate internal variables
	--**************************************************************************
	-- Case statement for navigation through records; these values are determined in OrderItems.vb.											  
	SET @OrderItem_ID =
		CASE
			WHEN @Record = @PREV THEN
				(SELECT MAX(OrderItem_ID) 
				FROM OrderItem (nolock) 
				WHERE OrderItem_ID < @OrderItem_ID  AND OrderHeader_ID = @OrderHeader_ID)
	
			WHEN @Record = @NEXT THEN
				(SELECT MIN(OrderItem_ID) 
				FROM dbo.OrderItem (nolock)
				WHERE OrderItem_ID > @OrderItem_ID  AND OrderHeader_ID = @OrderHeader_ID)
		
			WHEN @Record = @LAST THEN
				(SELECT MAX(OrderItem_ID) 
				FROM dbo.OrderItem (nolock) 
				WHERE OrderHeader_ID = @OrderHeader_ID)
		
			WHEN @Record = @CURR THEN
				@OrderItem_ID
		END


	--TODO: potentially deprecate
	SELECT
		@CurrentRecord = ( SELECT COUNT(*) 
							FROM OrderItem (nolock) 
							WHERE OrderHeader_ID = @OrderHeader_ID AND 
							OrderItem_ID <= @OrderItem_ID ),						
		@TotalRecords = COUNT(*) 
	FROM
		OrderItem (nolock) 	
	WHERE
		OrderHeader_ID = @OrderHeader_ID 
	
	--**************************************************************************
	--Main SQL
	--**************************************************************************
	
	SELECT 
		OrderItem_ID,
		[Store_No]						=	sr.Store_no,
		[ReceiveLocationIsDistribution]	=	sr.distribution_center, 
		[CurrentVendorCost]				=	CASE 
												WHEN (oh.OrderType_ID = 2 AND sv.Distribution_Center = 1) OR oh.OrderType_ID = 3 THEN 
													dbo.fn_AvgCostHistory(i.Item_Key, v.Store_no, oh.Transfer_SubTeam, GETDATE()) * oi.Package_Desc1
												ELSE 
													VCA.UnitCost 
											END,
		[CurrentVendorPack]				=	VCA.Package_Desc1,
		oi.Item_Key, 
		oi.QuantityOrdered, 
		oi.QuantityReceived, 
		oi.Total_Weight, 
		oi.Units_Per_Pallet, 
		oi.Cost, 
		[OriginalCost]					=	CASE 
												WHEN OrigReceivedItemCost = 0 OR OrigReceivedItemCost IS NULL THEN 
													Cost
												ELSE OrigReceivedItemCost
											END,
			 
		[OriginalCostUnit]				=	OrigReceivedItemUnit,
		
		--TODO: ItemUnit joins
		[OriginalCostUnitName]			=	(SELECT TOP(1)
												dbo.ItemUnit.Unit_Name 
											FROM
												dbo.ItemUnit (nolock) 
											WHERE
												dbo.ItemUnit.Unit_ID =	(CASE
																			WHEN 
																				CASE
																					WHEN OrigReceivedItemUnit IS NULL OR OrigReceivedItemUnit <= 0 THEN CostUnit 
																					ELSE OrigReceivedItemUnit 
																				END >= 0 THEN
																							CASE
																								WHEN OrigReceivedItemUnit IS NULL OR OrigReceivedItemUnit <= 0 THEN CostUnit 
																								ELSE OrigReceivedItemUnit 
																							END
																			ELSE oi.CostUnit
																		END)),
		oi.CostUnit, 
		oi.Handling, 
		oi.Freight, 
		oi.AdjustedCost, 
		oi.LandedCost, 
		oi.QuantityDiscount, 
		oi.LineItemCost, 
		oi.LineItemFreight, 
		oi.LineItemHandling, 
		oi.ReceivedItemCost, 
		oi.ReceivedItemFreight, 
		oi.ReceivedItemHandling, 
		oi.Freight3Party,
		oi.LineItemFreight3Party,
		oi.UnitCost, 
		oi.UnitExtCost, 
		oi.QuantityUnit, 
		oi.HandlingUnit, 
		oi.FreightUnit, 
		oi.DiscountType, 
		oi.DateReceived, 
		oi.OriginalDateReceived, 
		oi.ExpirationDate, 
		oi.Origin_ID, 
		oi.CountryProc_ID,
		oi.Package_Desc1, 
		oi.Package_Desc2, 
		oi.Package_Unit_ID,
		oi.MarkupPercent, 
		oi.MarkupCost, 
		oi.Retail_Unit_ID, 
		i.SubTeam_No,
		[ItemDescription]					=	ISNULL(ior.Item_Description, i.Item_Description),
		Identifier, 
		[ItemUnits_Per_Pallet]				=	i.Units_Per_Pallet,
		i.Retail_Sale, 
		i.Keep_Frozen, 
		i.Full_Pallet_Only,  
		i.Shipper_Item, 
		i.WFM_Item,  
		[CurrentRecord]						=	@CurrentRecord,
		[TotalRecords]						=	@TotalRecords,
		[ItemRetail_Unit_ID]				=	ISNULL(ior.Retail_Unit_ID, i.Retail_Unit_ID),
		[ItemPackage_Desc1]					=	CASE 
													WHEN oh.OrderType_ID = 3 THEN
														ISNULL(ior.Package_Desc1, i.Package_Desc1)
													ELSE
														VCA.Package_Desc1
												END,
			
		[ItemPackage_Desc2]					=	ISNULL(ior.Package_Desc2, i.Package_Desc2),
		[ItemPackage_Unit_ID]				=	ISNULL(ior.Package_Unit_ID, i.Package_Unit_ID),
		[Ordering_Unit_ID]					=	CASE 
													WHEN sr.MEGA_Store IS NULL THEN
														ISNULL(ior.Vendor_Unit_ID, i.Vendor_Unit_ID)
													ELSE 
														ISNULL(ior.Distribution_Unit_ID, i.Distribution_Unit_ID) 
												END,
			
		[Item_Cost_Unit_ID]					=	oi.CostUnit,
		[Item_Freight_Unit_ID]				=	oi.FreightUnit,
		NoDistMarkup, 
		CreditReason_ID, 
		QuantityAllocated,
		oi.Lot_No, 
		CostedByWeight						=	ISNULL(ior.CostedByWeight, i.CostedByWeight),
		i.CatchWeightRequired,
		oi.CatchWeightCostPerWeight,
		IsWeightRequired					=	CASE 
													WHEN i.CostedByWeight = 1 AND iuq.IsPackageUnit = 1 THEN 
														1 
													ELSE 
														0 
												END,
		oi.NetVendorItemDiscount,
		oi.CostAdjustmentReason_ID,
		[HandlingCharge]					=	CASE 
													WHEN ISNULL(dbo.fn_GetFacilityHandlingChargeOverride(oi.Item_Key, oh.Vendor_ID),0) = 0.00 THEN
														ISNULL(oi.HandlingCharge, 0)
													ELSE 
														dbo.fn_GetFacilityHandlingChargeOverride(oi.Item_Key, oh.Vendor_ID)
												END,		
		IsVendorDC							=	dbo.fn_IsDistributionCenter(v.Store_No),
		oi.OrderItemCool,
		oi.OrderItemBio,
		Carrier,
		SustainabilityRankingID				=	ISNULL(ior.SustainabilityRankingID, oi.SustainabilityRankingID),
		SustainabilityRankingRequired		=	ISNULL(ior.SustainabilityRankingRequired, i.SustainabilityRankingRequired), 
		oi.ReasonCodeDetailID,
		[PaidLineItemCost]					=	CASE
													--•	For a pay by agreed cost vendor when the LineItem is approved, display the amount the PO Admin selects to pay via the SPOT. Either line item received cost or line item invoice cost.
													WHEN poc.PayOrderedCostID IS NOT NULL AND oh.CloseDate IS NOT NULL AND oi.ApprovedDate IS NOT NULL AND oi.ApprovedByUserId IS NOT NULL THEN
														oi.PaidCost

													--•	For a pay by invoice, eInvoiced order, and received quantity is greater than 0, then display the line item invoice cost. 
													WHEN poc.PayOrderedCostID IS NULL AND oh.eInvoice_ID IS NOT NULL AND oh.CloseDate IS NOT NULL  AND oi.QuantityReceived > 0 THEN
														oi.InvoiceExtendedCost
														
													--•	For a pay by invoice, paper invoiced order, it will be the line item received cost.
													WHEN poc.PayOrderedCostID IS NULL AND oh.eInvoice_ID IS NULL AND oh.CloseDate IS NOT NULL THEN
														oi.ReceivedItemCost

													--•	For a pay by agreed cost vendor on which the ordered and eInvoiced costs match and the LineItem did not suspend, display the line item received cost.
													WHEN poc.PayOrderedCostID IS NOT NULL AND oh.CloseDate IS NOT NULL AND oi.ApprovedDate IS NULL AND oi.ApprovedByUserID IS NULL THEN
														oi.ReceivedItemCost
													
													--• For a pay by agreed cost vendor LineItem that suspended, this field will display the line item received cost while in suspended status.
													WHEN poc.PayOrderedCostID IS NOT NULL AND oh.CloseDate IS NOT NULL AND oi.ApprovedDate IS NULL AND oi.LineItemSuspended = 1 THEN
														oi.ReceivedItemCost
														
													ELSE
														0
												END,
		ib.Brand_Name 
	FROM 
		OrderItem						(nolock) oi
		INNER JOIN	Item				(nolock) i		ON	oi.Item_Key				=	i.Item_Key
		INNER JOIN	ItemBrand			(nolock) ib		ON	ib.Brand_ID				=	i.Brand_ID
		INNER JOIN	OrderHeader			(nolock) oh		ON	oi.OrderHeader_ID		=	oh.OrderHeader_ID
		INNER JOIN	Vendor				(nolock) v		ON	oh.Vendor_ID			=	v.Vendor_ID
		INNER JOIN	Vendor				(nolock) vr		ON	oh.ReceiveLocation_ID	=	vr.Vendor_ID
		INNER JOIN	ItemIdentifier		(nolock) ii		ON	i.Item_Key				=	ii.Item_Key 
														AND ii.Default_Identifier	=	1
		INNER JOIN	ItemUnit			(nolock) iuq	ON	oi.QuantityUnit			=	iuq.unit_ID
		INNER JOIN	Store				(nolock) sr		ON	vr.Store_No				=	sr.Store_No
		LEFT JOIN	PayOrderedCost		(nolock) poc	ON	v.Vendor_ID				=	poc.Vendor_ID
														AND	sr.Store_No				=	poc.Store_No
														AND poc.BeginDate			<=	oh.SentDate
		LEFT JOIN	Store				(nolock) sv		ON	v.Store_No 				=	sv.Store_No
		LEFT  JOIN	ItemOverride		(nolock) ior	ON	i.Item_Key				=	ior.Item_Key 
														AND	sr.StoreJurisdictionID	=	ior.StoreJurisdictionID
		LEFT  JOIN Einvoicing_Header	(nolock) eh		ON	oh.eInvoice_Id			=	eh.Einvoice_id
		OUTER APPLY
			dbo.fn_VendorCost(i.Item_Key, v.Vendor_ID, sr.Store_No, ISNULL(oh.SentDate, GETDATE()) + dbo.fn_GetLeadTimeDays(oh.Vendor_ID)) AS vca

	WHERE 
		OrderItem_ID = @OrderItem_ID

	SET NOCOUNT OFF
END
GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO