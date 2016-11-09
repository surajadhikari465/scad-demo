
CREATE PROCEDURE [dbo].[AutomaticOrderItemInfoPackSize]  
    @Item_Key				int,   
    @OrderHeader_ID			int,   
    @Package_Desc1			decimal(9,4),
	@VendorCostHistoryId	int
AS   
	-- ****************************************************************************************************************************************************
	-- Procedure: AutomaticOrderItemInfoPackSize()
	--    Author: n/a
	--      Date: n/a
	--
	-- Description:
	-- This procedure is called from Orders.vb to add items to an order.
	--
	-- Modification History:
	-- Date			Init	TFS		Comment
	-- 01.24.2011	TTL		759		Updated @CurrDate var to include any vendor lead-time in the date used to pull vendor cost attributes.
	--								Moved set of @CurrDate up to select that gets order-header info so we can use vendor ID from OH for lead-time FN.
	-- 12.14.2011	BBB		3744	coding standards;
	-- ****************************************************************************************************************************************************
BEGIN  
    SET NOCOUNT ON 
	--**************************************************************************
	--Declare variables
	--**************************************************************************
    DECLARE @VendorNetDiscount				decimal(10,4),   
			@Vendor_ID						int, 
			@VendStore_No					int, 
			@RecvStore_No					int,  
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
            @VendorCost						money, 
            @VendorExtCost					money, 
            @VendorPackage_Desc1			decimal(9,4),  
            @Units_Per_Pallet				int, 
            @Full_Pallet_Only				bit,   
            @Retail_Unit_ID					int, 
            @Ordering_Unit_ID				int, 
            @Cost_Unit_ID					int, 
            @Freight_Unit_ID				int,  
            @CurrDate						datetime, 
            @Identifier						varchar(255),
            @HandlingCharge					smallmoney, 
            @IsStoreDistributionCenter		bit  
	--**************************************************************************
	--Populate variables
	--**************************************************************************
	
	SELECT
		@Vendor_ID						= oh.Vendor_ID,     
		@RecvStore_No					= vr.Store_No,    
		@VendStore_No					= v.Store_No,    
		@IsVendorInternalManufacturer	= CASE WHEN ISNULL(sv.Manufacturer, 0) = 1 AND sv.BusinessUnit_ID IS NOT NULL THEN 1 ELSE 0 END,    
		@VendBusinessUnit_ID			= sv.BusinessUnit_ID,    
		@RecvBusinessUnit_ID			= s.BusinessUnit_ID,                
		@Transfer_SubTeam				= oh.Transfer_SubTeam,    
		@Transfer_To_SubTeam			= oh.Transfer_To_SubTeam,    
		@OrderType_ID					= oh.OrderType_ID,
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
		@CurrDate						= CONVERT(datetime, CONVERT(varchar(255), GETDATE(), 101)) + dbo.fn_GetLeadTimeDays(oh.Vendor_ID),
		@VendorCost						= vca.UnitCost,
		@VendorExtCost					= ISNULL(vca.UnitFreight, 0) + vca.UnitCost,     
		@Freight_Unit_ID				= vca.FreightUnit_ID,    
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
		OUTER APPLY (	
					SELECT * 
					FROM
						dbo.fn_VendorCostAllOrderItemInfoPackSizes(CONVERT(datetime, CONVERT(varchar(255), GETDATE(), 101)) + dbo.fn_GetLeadTimeDays(oh.Vendor_ID))
					WHERE 
						Item_Key				= i.Item_Key 
						AND Vendor_ID			= v.Vendor_ID 
						AND Store_No			= s.Store_No
						AND VendorCostHistoryID = ISNULL(@VendorCostHistoryId, VendorCostHistoryID))
			AS vca
	WHERE
		oh.OrderHeader_ID = @OrderHeader_ID     
		
	--**************************************************************************
	-- set cost and freight units to retail unit (via @Order_Unit_ID) for transfer orders    
	--**************************************************************************
	BEGIN    
		IF @OrderType_ID = 3    
		BEGIN    
			SET @Cost_Unit_ID		= @Ordering_Unit_ID    
			SET @Freight_Unit_ID	= @Cost_Unit_ID
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
    ON OBJECT::[dbo].[AutomaticOrderItemInfoPackSize] TO [IRMAClientRole]
    AS [dbo];

