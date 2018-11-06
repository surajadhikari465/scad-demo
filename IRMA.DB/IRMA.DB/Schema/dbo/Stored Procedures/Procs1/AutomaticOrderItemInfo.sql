
CREATE PROCEDURE [dbo].[AutomaticOrderItemInfo]
	@Item_Key		int, 
	@OrderHeader_ID int, 
	@Package_Desc1	decimal(9,4)

AS 

	-- ****************************************************************************************************************************************************
	-- Procedure: AutomaticOrderItemInfo()
	--    Author: n/a
	--      Date: n/a
	--
	-- Description:
	-- This procedure is called from Orders.vb to add items to an order.
	--
	-- Modification History:
	-- Date			Init	TFS		Comment
	-- 11/17/2009	BBB				update existing SP that will correctly identify Non-Regional
	--								WFM Facilities and return utilize the vendor on the order
	--								to retrieve cost as opposed to the store placing the order;
	--								reformatted for readability;
	-- 07.14.2009	BS		10321	added columns;
	-- 04/06/2010	BBB				Removed lookup needed for GL Enhancements and leave values 
	--								set in initial query as is
	-- 04/15/2010	BSR				Subtracting @VendorNetDiscount from @VendorCost to match logic
	--								used when closing a PO.
	-- 04.27.2010	DBS		12604	Replaced @Cost variable with @VendorCost due to it being the original cost intended
	-- 07/05/2010	BSR		12793	Effective Cost Ordering modifications
	-- 08.02.2010	RDS		13179	Update transfer logic to utilize avg cost flag and last po item cost when appropriate from 3.6.7
	--								Only use avg cost if turned on. Otherwise, use "last PO cost" from most recent closed PO.
	-- 01.22.2010	TTL		11405	Updated the logic below to match what the UpdateOrderRefreshCosts (UORC) SP does for Avg-Cost DC orders.
	--								I had to split out the OrderType=3 piece, as this is not included in UORC.
	-- 01.24.2011	TTL		759		Updated @CostDate var to include any vendor lead-time in the date used to pull vendor cost attributes.
	-- 01.14.2011	DBS		12793	Effective Cost Ordering modifications
	-- 04.12.2011	DBS		1795	Divide invoice cost by quantity for transfers due to change in einvoice matching
	-- 10.04.2011	MD		2140	Fixed the transfers issue
	-- 12.14.2011	BBB		3744	coding standards;
	-- 11.13.2012	DN		7403	Replace OrigReceivedItemCost with new cost logic for transfer orders i.e. @OrderType_ID = 3
	-- 12.17.2012   MZ      9470	Modified the logic to set the @cost on a transfer PO to the paid unit cost on the last received PO by the store/subteam with the item on it. 
    -- 12.20.2012   KM      9251    Check ItemOverride for new 4.8 override values (CostedByWeight, PackageDesc); 
    -- 01.17.2013   MZ      9819    Modified the logic to set the @cost on a transfer PO to add logic to handle costed-by-weight items.
	-- ****************************************************************************************************************************************************

BEGIN    
	SET NOCOUNT ON    
	
	--**************************************************************************
	--Declare variables
	--**************************************************************************
	DECLARE 
		@WFM							int,
		@Customer						int,
		@VendorNetDiscount				decimal(10,4),
		@Vendor_ID						int, 
		@VendStore_No					int, 
		@RecvStore_No					int, 
		@RecvVend_ID					int,   
		@VendBusinessUnit_ID			bigint, 
		@RecvBusinessUnit_ID			bigint, 
		@IsVendorInternalManufacturer	bit,    
		@Transfer_SubTeam				int, 
		@Transfer_To_SubTeam			int, 
		@OrderType_ID					tinyint,    
		@Package_Desc2					decimal(9,4), 
		@Package_Unit_ID				int, 
		@NoDistMarkup					bit,    
		@Cost							money, 
		@ExtCost						money, 
		@MarkupPercent					decimal(18,4),    
		@OriginalCostUnitName			varchar(50),
		@VendorCost						money, 
		@VendorExtCost					money, 
		@VendorPackage_Desc1			decimal(9,4),    
		@Units_Per_Pallet				int, 
		@Full_Pallet_Only				bit,     
		@Retail_Unit_ID					int,
		@Ordering_Unit_ID				int, 
		@Cost_Unit_ID					int, 
		@Freight_Unit_ID				int,    
		@CostDate						datetime, 
		@Identifier						varchar(255),
		@HandlingCharge					smallmoney, 
		@IsStoreDistributionCenter		bit,
		@IsCostedByWeight				bit,
		@UnitID							int,
		@PoundID						int,
		@UseAvgCost						bit
			
	--**************************************************************************
	--Populate variables
	--**************************************************************************
	SELECT @UnitID = Unit_ID FROM ItemUnit (nolock) WHERE lower(UnitSysCode) = 'unit'      
	SELECT @PoundID = Unit_ID FROM ItemUnit (nolock) WHERE lower(UnitSysCode) = 'lbs'  
	
	SELECT
		@Vendor_ID						= oh.Vendor_ID,     
		@RecvStore_No					= vr.Store_No,    
		@RecvVend_ID					= vr.Vendor_ID, 
		@VendStore_No					= v.Store_No,    
		@IsVendorInternalManufacturer	= CASE WHEN ISNULL(sv.Manufacturer, 0) = 1 AND sv.BusinessUnit_ID IS NOT NULL THEN 1 ELSE 0 END,    
		@VendBusinessUnit_ID			= sv.BusinessUnit_ID,    
		@RecvBusinessUnit_ID			= s.BusinessUnit_ID,                
		@Transfer_SubTeam				= oh.Transfer_SubTeam,    
		@Transfer_To_SubTeam			= oh.Transfer_To_SubTeam,    
		@OrderType_ID					= oh.OrderType_ID,
		@CostDate						= ISNULL(oh.SentDate, GetDate()) + dbo.fn_GetLeadTimeDays(oh.Vendor_ID),
		@Transfer_SubTeam				= ISNULL(@Transfer_SubTeam, i.SubTeam_No),     
		@Retail_Unit_ID					= ISNULL(ior.Retail_Unit_ID, i.Retail_Unit_ID),    
		@Package_Desc1					= ISNULL(@Package_Desc1, ISNULL(ior.Package_Desc1, i.Package_Desc1)),    
		@Package_Desc2					= ISNULL(ior.Package_Desc2, i.Package_Desc2),     
		@Package_Unit_ID				= ISNULL(ior.Package_Unit_ID, i.Package_Unit_ID),     
		@Units_Per_Pallet				= i.Units_Per_Pallet,    
		@Full_Pallet_Only				= Full_Pallet_Only,    
		@Ordering_Unit_ID				=	CASE
												WHEN (@IsVendorInternalManufacturer = 1) AND (ISNULL(@VendBusinessUnit_ID, -1) = ISNULL(@RecvBusinessUnit_ID, -2)) AND ISNULL(ior.Manufacturing_Unit_ID, i.Manufacturing_Unit_ID) IS NOT NULL THEN 
													ISNULL(ior.Manufacturing_Unit_ID, i.Manufacturing_Unit_ID)
												ELSE 
													CASE @OrderType_ID     
														WHEN 1 THEN ISNULL(ior.Vendor_Unit_Id, i.Vendor_Unit_ID)
														WHEN 2 THEN ISNULL(ior.Distribution_Unit_ID, i.Distribution_Unit_ID)
														WHEN 3 THEN ISNULL(ior.Retail_Unit_ID, i.Retail_Unit_ID)
														WHEN 4 THEN ISNULL(ior.Vendor_Unit_Id, i.Vendor_Unit_ID)
													END    
											END,    
		@NoDistMarkup					= NoDistMarkup,    
		@Identifier						= ii.Identifier,
		@IsCostedByWeight				= ISNULL(ior.CostedByWeight, i.CostedByWeight),
		@MarkupPercent					= ISNULL(
											CASE
												WHEN oh.OrderType_ID = 2 AND NoDistMarkup = 0 AND oh.Transfer_SubTeam = 2800 AND LEFT(ii.Identifier, 3) NOT IN ('260','261') THEN
													0
												WHEN zs.Distribution_Markup IS NOT NULL THEN
													zs.Distribution_Markup
												ELSE
													st.Transfer_To_Markup
											END, 0),
		@HandlingCharge					=	CASE
												WHEN oh.OrderType_ID = 2 THEN
													dbo.fn_GetCurrentHandlingCharge(i.Item_Key, oh.Vendor_ID)
												ELSE
													0
											END,
		@VendorCost						= vca.UnitCost,
		@VendorExtCost					= ISNULL(vca.UnitFreight, 0) + vca.UnitCost,     
		@Freight_Unit_ID				= vca.CostUnit_ID,    
		@VendorPackage_Desc1			= vca.Package_Desc1,    
		@Cost_Unit_ID					= vca.CostUnit_ID,    
		@VendorNetDiscount				= ISNULL(dbo.fn_ItemNetDiscount(s.Store_No, i.Item_Key, v.Vendor_ID, ISNULL(vca.UnitCost, 0), ISNULL(oh.SentDate, GetDate()) + dbo.fn_GetLeadTimeDays(oh.Vendor_ID)),0),
		@Cost							= CASE 
											WHEN vca.UnitCost IS NOT NULL THEN
												@VendorCost - @VendorNetDiscount
										  END,
		@ExtCost						= CASE 
											WHEN vca.UnitCost IS NOT NULL THEN
												@VendorExtCost - @VendorNetDiscount
										  END,
		@Package_Desc1					= CASE 
											WHEN vca.UnitCost IS NOT NULL THEN
												@VendorPackage_Desc1 
										  END,
		@IsStoreDistributionCenter		= sv.Distribution_Center
	
	FROM     
		Vendor						(nolock) v
		INNER JOIN	OrderHeader		(nolock) oh			ON	v.Vendor_ID				= oh.Vendor_ID
		INNER JOIN	Vendor			(nolock) vr			ON	oh.ReceiveLocation_ID	= vr.Vendor_ID
		INNER JOIN	Item			(nolock) i			ON	i.Item_Key				= @Item_Key
		INNER JOIN	ItemIdentifier	(nolock) ii			ON  i.Item_Key				= ii.Item_Key
														AND	ii.Default_Identifier	= 1
		INNER JOIN	SubTeam			(nolock) st			ON	oh.Transfer_To_SubTeam	= st.SubTeam_No
		LEFT JOIN	Store			(nolock) s			ON	vr.Store_No				= s.Store_No
		LEFT JOIN	Store			(nolock) sv			ON	v.Store_No				= sv.Store_No
		LEFT JOIN	ItemOverride	(nolock) ior		ON	i.Item_Key				= ior.Item_Key 
														AND	ior.StoreJurisdictionID = s.StoreJurisdictionID
		LEFT JOIN	ZoneSupply		(nolock) zs			ON	zs.SubTeam_No			= ISNULL(@Transfer_SubTeam, i.SubTeam_No)
														AND	v.Store_No				= zs.FromZone_ID
														AND s.Store_No				= zs.ToZone_ID
		OUTER APPLY 
			dbo.fn_VendorCost(i.Item_Key, v.Vendor_ID, s.Store_No, ISNULL(oh.SentDate, GETDATE()) + dbo.fn_GetLeadTimeDays(oh.Vendor_ID)) AS vca
	
	WHERE
		oh.OrderHeader_ID = @OrderHeader_ID     
	
	--**************************************************************************
	-- If there is no vendor cost and this is a distribution, get the distributing store's AvgCost
	--**************************************************************************
	IF (@OrderType_ID = 2 and @IsStoreDistributionCenter = 1) or @OrderType_ID = 3
	BEGIN
		IF @OrderType_ID = 3
			BEGIN
				DECLARE @SourcePONumber int
				SELECT @UseAvgCost = ISNULL(UseAvgCostHistory,0) FROM Store WHERE Store_No = @VendStore_No
				
				IF @UseAvgCost = 1
					BEGIN
					
						SET @SourcePONumber = ISNULL(dbo.fn_GetLastItemPOID(@Item_Key, @Vendor_ID), 0)
						IF @SourcePONumber > 0											
							SELECT 
								@Package_Desc1		=	Package_Desc1,
								@Package_Desc2		=	Package_Desc2,
								@Cost_Unit_ID		=	CostUnit,
								@Package_Unit_ID	=	Package_Unit_ID
							FROM 
								OrderItem
							WHERE 
								OrderHeader_ID		= @SourcePONumber
							AND	
								Item_Key			= @Item_Key
			
			
							SELECT @Cost = dbo.fn_AvgCostHistory(@Item_Key, @VendStore_No, @Transfer_SubTeam, GETDATE())  
							
							SELECT @Cost = dbo.fn_CostConversion(@Cost, CASE WHEN @IsCostedByWeight = 1 THEN @PoundID ELSE @UnitID END, @Retail_Unit_ID, @Package_Desc1, @Package_Desc2, @Package_Unit_ID)
					END
					  
				ELSE
					BEGIN				
						SET @SourcePONumber = ISNULL(dbo.fn_GetLastItemPOID(@Item_Key, @Vendor_ID), 0)

						IF @SourcePONumber > 0
											
							SELECT 
								-- The @Cost is the LineItem's paid cost which was copied from the calculation of [PaidLineItemCost] in GetOrderItemInfo SP
								@Cost               =	CASE
														--•	For a pay by agreed cost vendor when the LineItem is approved, display the amount the PO Admin selects to pay via the SPOT. Either line item received cost or line item invoice cost.
														WHEN poc.PayOrderedCostID IS NOT NULL AND oh.CloseDate IS NOT NULL AND oi.ApprovedDate IS NOT NULL AND oi.ApprovedByUserId IS NOT NULL THEN
															oi.PaidCost / CASE WHEN @IsCostedByWeight = 1 AND oi.Total_Weight > 0 THEN oi.Total_Weight ELSE oi.QuantityReceived END 
														--•	For a pay by invoice, eInvoiced order, and received quantity is greater than 0, then display the line item invoice cost. 
														WHEN poc.PayOrderedCostID IS NULL AND oh.eInvoice_ID IS NOT NULL AND oh.CloseDate IS NOT NULL  AND oi.QuantityReceived > 0 THEN
															oi.InvoiceExtendedCost / CASE WHEN @IsCostedByWeight = 1 AND oi.Total_Weight > 0 THEN oi.Total_Weight ELSE oi.QuantityReceived END
														--•	For a pay by invoice, paper invoiced order, it will be the line item received cost.
														WHEN poc.PayOrderedCostID IS NULL AND oh.eInvoice_ID IS NULL AND oh.CloseDate IS NOT NULL THEN
															oi.ReceivedItemCost / CASE WHEN @IsCostedByWeight = 1 AND oi.Total_Weight > 0 THEN oi.Total_Weight ELSE oi.QuantityReceived END
														--•	For a pay by agreed cost vendor on which the ordered and eInvoiced costs match and the LineItem did not suspend, display the line item received cost.
														WHEN poc.PayOrderedCostID IS NOT NULL AND oh.CloseDate IS NOT NULL AND oi.ApprovedDate IS NULL AND oi.ApprovedByUserID IS NULL THEN
															oi.ReceivedItemCost	/ CASE WHEN @IsCostedByWeight = 1 AND oi.Total_Weight > 0 THEN oi.Total_Weight ELSE oi.QuantityReceived END		
														--• For a pay by agreed cost vendor LineItem that suspended, this field will display the line item received cost while in suspended status.
														WHEN poc.PayOrderedCostID IS NOT NULL AND oh.CloseDate IS NOT NULL AND oi.ApprovedDate IS NULL AND oi.LineItemSuspended = 1 THEN
															oi.ReceivedItemCost	/ CASE WHEN @IsCostedByWeight = 1 AND oi.Total_Weight > 0 THEN oi.Total_Weight ELSE oi.QuantityReceived END													
														ELSE
															0
														END,
								@Package_Desc1		=	Package_Desc1,
								@Package_Desc2		=	Package_Desc2,
								@Cost_Unit_ID		=	CostUnit,
								@Package_Unit_ID	=	Package_Unit_ID
							FROM 
								OrderItem						(nolock) oi
								INNER JOIN	OrderHeader			(nolock) oh		ON	oi.OrderHeader_ID		=	oh.OrderHeader_ID
								INNER JOIN	Vendor				(nolock) v		ON	oh.Vendor_ID			=	v.Vendor_ID
								INNER JOIN	Vendor				(nolock) vr		ON	oh.ReceiveLocation_ID	=	vr.Vendor_ID
								INNER JOIN	Store				(nolock) sr		ON	vr.Store_No				=	sr.Store_No
								LEFT JOIN	PayOrderedCost		(nolock) poc	ON	oh.Vendor_ID			=	poc.Vendor_ID
																				AND	sr.Store_No				=	poc.Store_No
																				AND poc.BeginDate			<=	oh.SentDate
							WHERE 
								oi.OrderHeader_ID	= @SourcePONumber
							AND	
								oi.Item_Key			= @Item_Key
			
							--For Costed-by-Weight items, the unit cost is alwasy set by POUND. This will need to be changed for CA and UK.
							If @IsCostedByWeight = 0
								-- now convert cost to Unit UOM
								SELECT @Cost = dbo.fn_CostConversion(@Cost, @Cost_Unit_ID, CASE WHEN @IsCostedByWeight = 1 THEN @PoundID ELSE @UnitID END, @Package_Desc1, @Package_Desc2, @Package_Unit_ID)
					END
			END
		ELSE
			BEGIN
				-- This was copied from the UpdateOrderRefreshCosts SP, as it should be the correct logic for AvgCost DC orders.
				SELECT @Cost = dbo.fn_AvgCostHistory(@Item_Key, @VendStore_No, @Transfer_SubTeam, getdate())
				-- Add retrieval of the costed-by-weight flag value for the item in an above item-data query.    
				SELECT @Cost = dbo.fn_CostConversion(@Cost, CASE WHEN @IsCostedByWeight = 1 THEN @PoundID ELSE @UnitID END, @Cost_Unit_ID, @Package_Desc1, @Package_Desc2, @Package_Unit_ID)
			END
		
		-- We're keeping (not modifying) this bit of logic because it's unique to this SP.
		SET @ExtCost = @Cost    
	END      

	--**************************************************************************
	-- set cost and freight units to retail unit (via @Order_Unit_ID) for transfer orders    
	--**************************************************************************
	BEGIN    
		IF @OrderType_ID = 3    
		BEGIN    
			SET @Cost_Unit_ID		= @Ordering_Unit_ID    
			SET @Freight_Unit_ID	= @Cost_Unit_ID
			SET @VendorCost			= @Cost
		END    
	END    
		
	--**************************************************************************
	-- Output
	--**************************************************************************
	SELECT 
		[Units_Per_Pallet]		= @Units_Per_Pallet,     
		[QuantityUnit]			= @Ordering_Unit_ID,     
		[QuantityOrdered]		= 1,     
		[Cost]					= ISNULL(@Cost, 0),
		[CostUnit]				= @Cost_Unit_ID,
		[OriginalCost]			= @VendorCost,		
		[OriginalCostUnit]		= @Cost_Unit_ID,		
		[OriginalCostUnitName]	= (SELECT Unit_Name FROM ItemUnit WHERE ItemUnit.Unit_ID = @Cost_Unit_ID),
		[Handling]				= 0,
		[HandlingUnit]			= @Cost_Unit_ID,
		[Freight]				= (ISNULL(@ExtCost, 0) - ISNULL(@Cost, 0)),
		[FreightUnit]			= @Freight_Unit_ID,     
		[AdjustedCost]			= 0,     
		[QuantityDiscount]		= 0,     
		[DiscountType]			= 0,     
		[Package_Desc1]			= ISNULL(@Package_Desc1, 1),     
		[Package_Desc2]			= ISNULL(@Package_Desc2, 1),     
		[Package_Unit_ID]		= @Package_Unit_ID,    
		[MarkupPercent]			= @MarkupPercent,     
		[Retail_Unit_ID]		= @Retail_Unit_ID,    
		[VendorNetDiscount]		= @VendorNetDiscount,    
		[HandlingCharge]		= @HandlingCharge
		
	SET NOCOUNT OFF            
END

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[AutomaticOrderItemInfo] TO [IRMASupportRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[AutomaticOrderItemInfo] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[AutomaticOrderItemInfo] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[AutomaticOrderItemInfo] TO [IRMAReportsRole]
    AS [dbo];

