SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[GetOrderItemItemInfo]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[GetOrderItemItemInfo]
GO


CREATE PROCEDURE [dbo].[GetOrderItemItemInfo]
	@OrderItem_ID int,
	@OrderHeader_ID int
AS 
	-- **************************************************************************
	-- Procedure: GetOrderItemItemInfo()
	--    Author: n/a
	--      Date: n/a
	--
	-- Modification History:
	-- Date        Init	Comment
	-- 01/24/2011  TTL  TFS 759, Added @CostDate var and updated to include any vendor lead-time in the date used to pull vendor cost attributes.
	--                  
	-- **************************************************************************

	DECLARE
		@StoreNo As int
		,@ReceiveLoc As int
		,@CostDate smalldatetime

	SELECT
		@ReceiveLoc = ReceiveLocation_ID 
		,@CostDate = GETDATE() + dbo.fn_GetLeadTimeDays(OrderHeader.Vendor_ID)
	FROM OrderHeader (NOLOCK) 
	WHERE OrderHeader_ID = @OrderHeader_ID

	SELECT @StoreNo = (SELECT Store_No FROM Vendor WHERE Vendor_ID = @ReceiveLoc)

SELECT 
    ISNULL(ItemOverride.Item_Description, Item.Item_Description) As Item_Description, 
	Identifier, 
	Item.Units_Per_Pallet AS Units_Per_Pallet, 
	Item.Retail_Sale, 
	Item.Keep_Frozen, 
	Item.Full_Pallet_Only, 
	Item.Shipper_Item, 
	Item.WFM_Item,  
	ISNULL(ItemOverride.Retail_Unit_ID, Item.Retail_Unit_ID) AS Retail_Unit_ID, 
	VCA.Package_Desc1 AS Package_Desc1,
	ISNULL(ItemOverride.Package_Desc2, Item.Package_Desc2) As Package_Desc2, 
	ISNULL(ItemOverride.Package_Unit_ID, Item.Package_Unit_ID) As Package_Unit_ID,
	CASE WHEN Store.MEGA_Store IS NULL THEN ISNULL(ItemOverride.Vendor_Unit_ID, Item.Vendor_Unit_ID) ELSE ISNULL(ItemOverride.Distribution_Unit_ID, Item.Distribution_Unit_ID) END AS Ordering_Unit_ID,
    VCA.CostUnit_ID AS Item_Cost_Unit_ID, 
    VCA.FreightUnit_ID AS Item_Freight_Unit_ID 
FROM 
	Store (NOLOCK) RIGHT JOIN (
		ItemIdentifier (NOLOCK) INNER JOIN (
			Vendor ReceivingVendor (nolock) INNER JOIN (
				Vendor (NOLOCK) INNER JOIN (
					OrderHeader INNER JOIN (
						OrderItem INNER JOIN Item ON (OrderItem.Item_Key = Item.Item_Key)
					) ON (OrderHeader.OrderHeader_ID = OrderItem.OrderHeader_ID)
				) ON (Vendor.Vendor_ID = OrderHeader.Vendor_ID)
			) ON (ReceivingVendor.Vendor_ID = Orderheader.ReceiveLocation_ID)
		) ON (ItemIdentifier.Item_Key = Item.Item_Key AND ItemIdentifier.Default_Identifier = 1)
	) ON (Store.Store_No = Vendor.Store_No)
	LEFT JOIN ItemOverride (nolock) ON 
		ItemOverride.Item_Key = Item.Item_Key AND 
		ItemOverride.StoreJurisdictionID = (SELECT SN.StoreJurisdictionID FROM Store SN WHERE SN.Store_No = @StoreNo)
	LEFT JOIN dbo.fn_VendorCostAll(@CostDate) VCA ON
		VCA.Item_Key = Item.Item_Key AND
		VCA.Store_No = @StoreNo	AND 
		VCA.Vendor_ID = OrderHeader.Vendor_ID 
WHERE OrderItem_ID = @OrderItem_ID
GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO


