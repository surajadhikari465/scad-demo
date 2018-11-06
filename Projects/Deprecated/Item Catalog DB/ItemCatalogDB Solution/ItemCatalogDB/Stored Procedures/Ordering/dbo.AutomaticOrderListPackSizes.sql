SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[AutomaticOrderListPackSizes]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[AutomaticOrderListPackSizes]
GO

CREATE PROCEDURE dbo.AutomaticOrderListPackSizes
	@OrderHeader_ID		int,  
	@SearchSubTeam_No	int,  
	@DistSubTeam_No		int,   
	@Not_Available		tinyint,
	@ProductType_ID		int    

AS  
 
	-- **************************************************************************
	-- Procedure: AutomaticOrderListPackSizes()
	--    Author: n/a
	--      Date: n/a
	--
	-- Modification History:
	-- Date			Init	TFS		Comment
	-- 01/24/2011	TTL		759		Updated @Now var to include any vendor lead-time in the date used to pull vendor cost attributes.
	--								Updated to include lead-time for @OrderType_ID = 1 and @OrderType_ID = 2 because these sections of code use the same cost dates.
	-- 09/20/2011	MD		3005    Updated the call to fn_vendorcostall to get the cost data instead of using fn_VendorCostAllPackSizes. This fixes
	--								the issues with duplicate records returned to List View Screen (more info attached to the bug 3005) 
	-- 12.26.2011	BJL		3751	Added CurrentVendorCost as part of the returned values. Borrowed SQL from GetOrderItemInfo
	-- 02/08/12		BJL		4681	Added ISNULL check on VCA.UnitCost to return 0 for NULL UnitCost.
	-- 2013/01/03	KM		9251	Check ItemOverride for new 4.8 override values (Not_Available, Not_AvailableNote, Brand_Name);
	-- 2013/06/28   BJL		12952	Renamed the DiscontinueItem column to be Discontinue_Item
	-- **************************************************************************

BEGIN  
	SET NOCOUNT ON  
	  
	DECLARE 
		@Vendor_ID                    int,   
		@VendStore_No                 int,   
		@CustStore_No                 int,   
		@Transfer_From_SubTeam        int,   
		@Transfer_To_SubTeam          int,   
		@WFM_Store                    int,   
		@HFM_Store                    int,   
		@Distribution_Center          int,   
		@Manufacturer                 int,   
		@OrderType_ID                 int,   
		@Internal                     tinyint,   
		@CustBusinessUnit_ID          int,   
		@IsVendorInternalManufacturer bit,   
		@Now                          smalldatetime,
		@ReturnRows					  int ,
		@CloseDate					  datetime
  
	SELECT	
		@Transfer_From_SubTeam			= Transfer_SubTeam,   
		@Transfer_To_SubTeam			= Transfer_To_SubTeam,  
		@Vendor_ID						= OrderHeader.Vendor_ID,   
		@VendStore_No					= Vendor.Store_No,  
		@CustStore_No					= Cust.Store_No,  
		@CustBusinessUnit_ID			= CustStore.BusinessUnit_ID,  
		@OrderType_ID					= OrderHeader.OrderType_ID,   
		@WFM_Store						= CustStore.WFM_Store,   
		@HFM_Store						= CustStore.Mega_Store,   
		@Distribution_Center			= CustStore.Distribution_Center,   
		@Manufacturer					= CustStore.Manufacturer,  
		@Internal						= CustStore.Internal,  
		@IsVendorInternalManufacturer	=	CASE 
												WHEN (VendStore.Manufacturer = 1) AND (VendStore.BusinessUnit_ID IS NOT NULL) THEN 1   
												ELSE 0   
											END,  
		@Now							= GETDATE() + dbo.fn_GetLeadTimeDays(OrderHeader.Vendor_ID),
		@CloseDate						= OrderHeader.CloseDate
	
	FROM	
		Vendor						(NOLOCK)   
		INNER JOIN	OrderHeader		(NOLOCK)    ON (Vendor.Vendor_ID   = OrderHeader.Vendor_ID)  
		INNER JOIN	Vendor Cust		(nolock)    ON (Cust.Vendor_ID     = OrderHeader.ReceiveLocation_ID)  
		INNER JOIN	Store CustStore	(nolock)	ON (CustStore.Store_No = Cust.Store_No)  
		LEFT JOIN	Store VendStore (nolock)	ON (Vendor.Store_No    = VendStore.Store_No)  
	
	WHERE 
		OrderHeader_ID = @OrderHeader_ID 

	DECLARE @SubTeamList TABLE (SubTeam_No int)
	
	IF @ProductType_ID = 1 AND @Distribution_Center = 0
			INSERT @SubTeamList                       
				SELECT SubTeam_No FROM SubTeam (nolock) WHERE SubTeamType_ID IN (1,2,3,4)
	IF @ProductType_ID = 1 AND @Distribution_Center = 1
			INSERT @SubTeamList                       
				SELECT SubTeam_No FROM SubTeam (nolock) WHERE SubTeamType_ID IN (1,2,3,4,5,6)
	IF @ProductType_ID = 2
			INSERT @SubTeamList
					SELECT SubTeam_No FROM SubTeam (nolock) WHERE SubTeamType_ID = 5
	IF @ProductType_ID = 3
			INSERT @SubTeamList
					SELECT SubTeam_No FROM SubTeam (nolock) WHERE SubTeamType_ID = 6
					
	IF @SearchSubTeam_No = NULL
		SET @ReturnRows = 1000
	ELSE
		SET @ReturnRows = 0
  
	--------- PURCHASE ORDER ---------------------------------------------------------------------------  
	IF @OrderType_ID = 1 OR @OrderType_ID = 4 -- Purchase or Flowthru
		SET ROWCOUNT @ReturnRows
		
		SELECT	
			Item.Item_Key,   
			VCA.Package_Desc1 AS Package_Desc1,  
			VCA.VendorCostHistoryId AS VendorCostHistoryId,  
			ISNULL(ItemOverride.Package_Desc2, Item.Package_Desc2) As Package_Desc2,   
			IUnit.Unit_Abbreviation AS Package_Unit,  
			ItemIdentifier.Identifier,   
			ISNULL(ItemOverride.Item_Description, Item.Item_Description) As Item_Description,  
			Item.Pre_Order,  
			ISNULL(ItemOverride.Not_Available, Item.Not_Available) AS Not_Available,  
			Item.Category_ID,   
			ItemCategory.Category_Name,   
			MAX(OrderItem.OrderItem_ID) AS OrderItem_ID,   
			MAX(OrderItem.QuantityOrdered) AS QuantityOrdered,   
			MAX(ISNULL(QuantityUnit, ISNULL(ItemOverride.Vendor_Unit_Id, Item.Vendor_Unit_Id))) AS QuantityUnit,   
			ISNULL(Price.ExceptionSubteam_No, Item.SubTeam_No) AS SubTeam_No,  
			MAX(OrderItem.CreditReason_ID) As CreditReason_ID,  
			ISNULL(ItemOverride.Not_AvailableNote, Item.Not_AvailableNote) AS Not_AvailableNote,  
			EXEDistributed,   
			Item.DistSubTeam_No,   
			[Discontinue_Item] = SIV.DiscontinueItem,  
			MAX(ItemUnit.Unit_Name) AS Unit_Name,   
			SUM(OrderItem.QuantityReceived) AS QuantityReceived,   
			COUNT(*) AS OrderCount,  
			VU.Unit_Name AS Vendor_Unit,  
			VU.Unit_ID AS Vendor_Unit_Id,  
			MAX(CreditReason) As CreditReason,
			IV.Item_ID as VendorItemID,
			ISNULL(IBO.Brand_Name, IB.Brand_Name) as Brand,
			VCA.NetCost as Cost,
			ISNULL(VCA.UnitCost,0) as [CurrentVendorCost],	
			VIS.StatusCode as VendorItemStatus,
			VIS.StatusName as VendorItemStatusFull,
			CASE WHEN VIS.StatusCode Is Null THEN 'ZZZZZ-NULL' ELSE VIS.StatusCode END AS [VendorItemStatusSort]

		FROM  
			   CreditReasons (NOLOCK) RIGHT JOIN (  
				ItemUnit VU (NOLOCK) RIGHT JOIN (  
				 ItemUnit (NOLOCK) RIGHT JOIN (   
				  OrderItem (NOLOCK) RIGHT JOIN (   
				   ItemUnit IUnit (NOLOCK) RIGHT JOIN (   
					ItemCategory (NOLOCK) RIGHT JOIN (  
					 StoreItem (NOLOCK) SI INNER JOIN (  
					  ItemIdentifier (NOLOCK) INNER JOIN (  
					   Item (NOLOCK) INNER JOIN StoreItemVendor SIV (NOLOCK)   
						ON   
						 (SIV.Item_Key = Item.Item_Key   
						 AND   
						 SIV.Store_No = @CustStore_No   
						 AND   
						 SIV.Vendor_ID = @Vendor_ID   
						 AND   
						 (SIV.DeleteDate IS NULL OR SIV.DeleteDate > GETDATE()))    
						INNER JOIN ItemVendor IV (NOLOCK)
						ON IV.Item_Key = Item.Item_Key
						AND IV.Vendor_Id = @Vendor_ID  
						LEFT JOIN VendorItemStatuses VIS
						ON IV.VendorItemStatus = VIS.StatusId
						INNER JOIN ItemBrand IB
						ON IB.Brand_Id = Item.Brand_Id
					  ) ON (ItemIdentifier.Item_Key = Item.Item_Key AND ItemIdentifier.Default_Identifier = 1)  
					 ) ON SI.Item_Key = SIV.Item_Key AND SI.Store_No = SIV.Store_No  
					) ON (ItemCategory.Category_ID = Item.Category_ID)  
				   ) ON (IUnit.Unit_ID = Item.Package_Unit_ID)   
				  ) ON (OrderItem.Item_Key = Item.Item_Key AND OrderItem.OrderHeader_ID = @OrderHeader_ID)  
				 ) ON (ItemUnit.Unit_ID = OrderItem.QuantityUnit)  
				) ON (VU.Unit_ID = Item.Vendor_Unit_ID)  
			   ) ON (CreditReasons.CreditReason_ID = OrderItem.CreditReason_ID)  
		
		LEFT JOIN	Price			(NOLOCK)	ON	SI.Item_Key		= Price.Item_Key 
												AND SI.Store_No		= Price.Store_No  
		LEFT JOIN	ItemOverride	(NOLOCK)	ON	Item.Item_Key	= ItemOverride.Item_Key 
												AND ItemOverride.StoreJurisdictionID = (SELECT StoreJurisdictionID FROM Store WHERE Store_No = @CustStore_No)  
		LEFT JOIN	ItemBrand IBO	(NOLOCK)	ON	ItemOverride.Brand_ID = IBO.Brand_ID
		LEFT JOIN	dbo.fn_VendorCostAll(@Now) VCA	ON	VCA.Item_Key		= Item.Item_Key 
													AND	VCA.Store_No		= @CustStore_No 
													AND VCA.Vendor_ID		= @Vendor_ID 
													AND	VCA.Package_desc1	= ISNULL(OrderItem.Package_Desc1, VCA.Package_desc1)
	  
	   WHERE   
			Item.WFM_Item = CASE WHEN @WFM_Store = 1 THEN 1 ELSE Item.WFM_Item END  
			AND Item.HFM_Item = CASE WHEN @HFM_Store = 1 THEN 1 ELSE Item.HFM_Item END  
			AND (ISNULL(@Not_Available, ISNULL(ItemOverride.Not_Available, Item.Not_Available)) = ISNULL(ItemOverride.Not_Available, Item.Not_Available))   
			AND (Item.SubTeam_No = ISNULL(@SearchSubTeam_No, Item.SubTeam_No))  
			AND (ISNULL(Item.DistSubTeam_No, 0) = ISNULL(@DistSubTeam_No, ISNULL(Item.DistSubTeam_No, 0)))   
			AND Item.Deleted_Item = 0   
			AND Item.Remove_Item = 0   
			AND SI.Authorized = case when @wfm_store=1 then 1 else si.authorized end
			AND Item.SubTeam_No IN(SELECT SubTeam_No FROM @SubTeamList)
		
		GROUP BY 
			Item.Item_Key,   
			VCA.Package_Desc1,   
			VCA.VendorCostHistoryId,
			VCA.NetCost,
			VCA.UnitCost, 
			ISNULL(ItemOverride.Package_Desc2, Item.Package_Desc2),   
			IUnit.Unit_Abbreviation,   
			ItemIdentifier.Identifier,   
			ISNULL(ItemOverride.Item_Description, Item.Item_Description),   
			ISNULL(ItemOverride.Vendor_Unit_ID, Item.Vendor_Unit_ID),   
			Item.Category_ID,   
			ItemCategory.Category_Name,   
			Item.Pre_Order,   
			ISNULL(ItemOverride.Not_Available, Item.Not_Available),   
			ISNULL(Price.ExceptionSubteam_No, Item.SubTeam_No),  
			ISNULL(ItemOverride.Not_AvailableNote, Item.Not_AvailableNote),   
			EXEDistributed,   
			Item.DistSubTeam_No,   
			SIV.DiscontinueItem,  
			VU.Unit_Name, 
			VU.Unit_ID, 
			IV.Item_Id, 
			ISNULL(IBO.Brand_Name, IB.Brand_Name),
			VIS.StatusCode,
			VIS.StatusName  

		ORDER BY 
			OrderCount DESC,
			QuantityReceived DESC   
  

	--------- DISTRIBUTION ORDER ---------------------------------------------------------------------------  
	IF @OrderType_ID = 2 -- Distribution or Flowthrough
		SET ROWCOUNT @ReturnRows
		SELECT 
			Item.Item_Key,   
			VCA.Package_Desc1 AS Package_Desc1,  
			VCA.VendorCostHistoryId AS VendorCostHistoryId,  
			ISNULL(ItemOverride.Package_Desc2, Item.Package_Desc2) As Package_Desc2,   
			IUnit.Unit_Abbreviation AS Package_Unit,  
			ItemIdentifier.Identifier,   
			ISNULL(ItemOverride.Item_Description, Item.Item_Description) As Item_Description,   
			Item.Pre_Order,  
			ISNULL(ItemOverride.Not_Available, Item.Not_Available) AS Not_Available,  
			Item.Category_ID,   
			ItemCategory.Category_Name,   
			MAX(OrderItem.OrderItem_ID) AS OrderItem_ID,   
			MAX(OrderItem.QuantityOrdered) AS QuantityOrdered,   
			MAX(ISNULL(QuantityUnit, ISNULL(ItemOverride.Distribution_Unit_ID, Item.Distribution_Unit_ID))) AS QuantityUnit,   
			ISNULL(Price.ExceptionSubteam_No, Item.SubTeam_No) AS SubTeam_No,  
			MAX(OrderItem.CreditReason_ID) As CreditReason_ID,  
			ISNULL(ItemOverride.Not_AvailableNote, Item.Not_AvailableNote) AS Not_AvailableNote,  
			EXEDistributed,   
			Item.DistSubTeam_No,   
			[Discontinue_Item] = SIV.DiscontinueItem,  
			MAX(ItemUnit.Unit_Name) AS Unit_Name,   
			SUM(OrderItem.QuantityReceived) AS QuantityReceived,   
			COUNT(*) AS OrderCount,  
			VU.Unit_Name AS Vendor_Unit,  
			VU.Unit_ID AS Vendor_Unit_Id,  
			MAX(CreditReason) As CreditReason,
			IV.Item_ID as VendorItemID,
			ISNULL(IBO.Brand_Name, IB.Brand_Name) as Brand,
			VCA.NetCost as Cost,
			[CurrentVendorCost]		=	CASE 
											WHEN @Distribution_Center = 1 THEN 
												ISNULL((dbo.fn_AvgCostHistory(Item.Item_Key, @VendStore_No,  @Transfer_From_SubTeam, ISNULL(@CloseDate,@Now)) * vca.Package_Desc1), 0)
											ELSE 
												ISNULL(VCA.UnitCost, 0)
										END,
			VIS.StatusCode as VendorItemStatus,
			VIS.StatusName as VendorItemStatusFull,
			CASE WHEN VIS.StatusCode Is Null THEN 'ZZZZZ-NULL' ELSE VIS.StatusCode END AS [VendorItemStatusSort]

		FROM   
			   CreditReasons (NOLOCK) RIGHT JOIN (  
				ItemUnit VU (NOLOCK) RIGHT JOIN (  
				 ItemUnit (NOLOCK) RIGHT JOIN (   
				  OrderItem (NOLOCK) RIGHT JOIN (   
				   ItemUnit IUnit (NOLOCK) RIGHT JOIN (   
					ItemCategory (NOLOCK) RIGHT JOIN (  
					 StoreItem (NOLOCK) SI INNER JOIN (  
					  ItemIdentifier (NOLOCK) INNER JOIN (  
					   Item (NOLOCK) INNER JOIN StoreItemVendor SIV (NOLOCK)   
						ON   
						 (SIV.Item_Key = Item.Item_Key   
						 AND   
						 SIV.Store_No = @CustStore_No   
						 AND   
						 SIV.Vendor_ID = @Vendor_ID   
						 AND   
						 (SIV.DeleteDate IS NULL OR SIV.DeleteDate > GETDATE()))   
						 AND   
						 ISNULL(Item.DistSubTeam_No, Item.SubTeam_No) = @Transfer_From_SubTeam    
						INNER JOIN ItemVendor IV (NOLOCK)
						ON IV.Item_Key = Item.Item_Key
						AND IV.Vendor_Id = @Vendor_ID  
						LEFT JOIN VendorItemStatuses VIS
						ON IV.VendorItemStatus = VIS.StatusID
						INNER JOIN ItemBrand IB
						ON IB.Brand_Id = Item.Brand_Id
					  ) ON (ItemIdentifier.Item_Key = Item.Item_Key AND ItemIdentifier.Default_Identifier = 1)  
					 ) ON SI.Item_Key = SIV.Item_Key AND SI.Store_No = SIV.Store_No  
					) ON (ItemCategory.Category_ID = Item.Category_ID)  
				   ) ON (IUnit.Unit_ID = Item.Package_Unit_ID)   
				  ) ON (OrderItem.Item_Key = Item.Item_Key AND OrderItem.OrderHeader_ID = @OrderHeader_ID)  
				 ) ON (ItemUnit.Unit_ID = OrderItem.QuantityUnit)  
				) ON (VU.Unit_ID = Item.Distribution_Unit_ID)  
			   ) ON (CreditReasons.CreditReason_ID = OrderItem.CreditReason_ID)  
		
		LEFT JOIN Price				(NOLOCK)		ON	SI.Item_Key				= Price.Item_Key 
													AND SI.Store_No				= Price.Store_No  
		LEFT JOIN ItemOverride		(NOLOCK)		ON  Item.Item_Key			= ItemOverride.Item_Key 
													AND ItemOverride.StoreJurisdictionID = (SELECT StoreJurisdictionID FROM Store WHERE Store_No = @CustStore_No)  
		LEFT JOIN ItemBrand IBO		(NOLOCK)		ON	ItemOverride.Brand_ID	= IBO.Brand_ID
		LEFT JOIN dbo.fn_VendorCostAll(@Now) VCA	ON	VCA.Item_Key	= Item.Item_Key 
													AND VCA.Store_No	= @CustStore_No 
													AND VCA.Vendor_ID	= @Vendor_ID 
	WHERE   
		Item.WFM_Item = CASE WHEN @WFM_Store = 1 THEN 1 ELSE Item.WFM_Item END  
		AND Item.HFM_Item = CASE WHEN @HFM_Store = 1 THEN 1 ELSE Item.HFM_Item END  
		AND	(ISNULL(@Not_Available, ISNULL(ItemOverride.Not_Available, Item.Not_Available)) = ISNULL(ItemOverride.Not_Available, Item.Not_Available))   
		AND	(ISNULL(Price.ExceptionSubteam_No, Item.SubTeam_No) = ISNULL(@SearchSubTeam_No, Item.SubTeam_No))  
		AND	(ISNULL(Item.DistSubTeam_No, 0) = ISNULL(@DistSubTeam_No, ISNULL(Item.DistSubTeam_No, 0)))  
		AND	ISNULL(Item.DistSubTeam_No, Item.SubTeam_No) = @Transfer_From_SubTeam  
		AND	Item.Deleted_Item = 0   
		AND	Item.Remove_Item = 0   
		AND	SI.Authorized = case when @wfm_store=1 then 1 else si.authorized end  
		AND Item.SubTeam_No IN(SELECT SubTeam_No FROM @SubTeamList)
	
	GROUP BY 
		Item.Item_Key,   
		VCA.Package_Desc1, 
		VCA.VendorCostHistoryId,  
		VCA.NetCost,
		ISNULL(ItemOverride.Package_Desc2, Item.Package_Desc2),   
		IUnit.Unit_Abbreviation,   
		ItemIdentifier.Identifier,   
		ISNULL(ItemOverride.Item_Description, Item.Item_Description),   
		ISNULL(ItemOverride.Distribution_Unit_ID, Item.Distribution_Unit_ID),   
		Item.Category_ID,   
		ItemCategory.Category_Name,   
		Item.Pre_Order,   
		ISNULL(ItemOverride.Not_Available, Item.Not_Available),   
		ISNULL(Price.ExceptionSubteam_No, Item.SubTeam_No),   
		ISNULL(ItemOverride.Not_AvailableNote, Item.Not_AvailableNote),   
		EXEDistributed,   
		Item.DistSubTeam_No,   
		SIV.DiscontinueItem,  
		VU.Unit_Name,   
		VU.Unit_ID,
		IV.Item_Id, 
		ISNULL(IBO.Brand_Name, IB.Brand_Name),
		VIS.StatusCode,
		VIS.StatusName,
		VCA.UnitCost 
	
	ORDER BY 
		OrderCount DESC,   
		QuantityReceived DESC  
  
	SET NOCOUNT OFF  
END
GO