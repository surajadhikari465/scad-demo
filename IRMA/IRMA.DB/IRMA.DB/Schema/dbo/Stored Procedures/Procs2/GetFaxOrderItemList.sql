CREATE PROCEDURE dbo.GetFaxOrderItemList 
    @OrderHeader_ID		int,
    @Item_ID			bit

AS
 
	-- *************************************************************************************************************************
	-- Procedure: GetFaxOrderItemList()
	--    Author: n/a
	--      Date: n/a
	--
	-- Description:
	-- This procedure is utilized by the SendOrdersDAO.vb to select a list of order
	-- items specific to this order type.
	--
	-- Modification History:
	-- Date			Init	Comment
	-- 07/14/2009	BBB		Added left joins to ItemOverride table from OrderItem in
	--						all SQL calls; added IsNull on column values that should
	--						pull from override table if value available
	-- 2013/01/04	KM		TFS 9251: Undo the nested JOINs; Check ItemOverride for new 4.8 override values (Brand, Origin);
	-- ***************************************************************************************************************************

BEGIN
    SET NOCOUNT ON
    
    SELECT 
		Item.Item_Key,
		OrderItem.OrderItem_ID, 
        CASE WHEN ISNULL(ItemVendor.Item_ID,'') > '' THEN ItemVendor.Item_ID ELSE Identifier END AS Identifier, 
        ISNULL(ItemOverride.Item_Description, Item.Item_Description) AS Item_Description,
        OrderItem.QuantityOrdered, 
        OrderItem.QuantityReceived,
        OrderItem.Total_Weight, 
        OrderItem.QuantityDiscount, 
        OrderItem.DiscountType, 
        OrderItem.LineItemCost, 
        OrderItem.LineItemFreight, 
        OrderItem.LineItemHandling, 
        ItemUnit.Unit_Name,
        OrderItem.Package_Desc1,
        OrderItem.Package_Desc2,
        ISNULL(I.Unit_Name, 'Unit') AS Package_Unit,
        OrderItem.Cost,
        SubTeam.SubTeam_Name,
        Item.SubTeam_No,
        Category_Name, 
		Brand_Name, 
		Origin_Name
    
	FROM
		OrderHeader
		INNER JOIN	OrderItem					ON	OrderHeader.OrderHeader_ID						= OrderItem.OrderHeader_ID
		INNER JOIN	ItemUnit					ON	OrderItem.QuantityUnit							= ItemUnit.Unit_ID
		INNER JOIN	Item			(NOLOCK)	ON	OrderItem.Item_Key								= Item.Item_Key
		INNER JOIN	ItemIdentifier	(NOLOCK)	ON	Item.Item_Key									= ItemIdentifier.Item_Key
												AND	ItemIdentifier.Default_Identifier				= 1
		INNER JOIN	Subteam			(NOLOCK)	ON	Subteam.SubTeam_No								= CASE WHEN Transfer_SubTeam IS NOT NULL THEN ISNULL(Transfer_To_SubTeam, Transfer_SubTeam) ELSE ISNULL(Transfer_To_SubTeam, Item.SubTeam_No) END
		LEFT JOIN	ItemUnit I		(NOLOCK)	ON	OrderItem.Package_Unit_ID						= I.Unit_ID
		LEFT JOIN	ItemVendor		(NOLOCK)	ON	@Item_ID										= 1
												AND	OrderItem.Item_Key								= ItemVendor.Item_Key
												AND	OrderHeader.Vendor_ID							= ItemVendor.Vendor_ID
		LEFT JOIN	ItemCategory	(NOLOCK)	ON	Item.Category_ID								= ItemCategory.Category_ID
		LEFT JOIN	ItemOverride	(NOLOCK)	ON	Item.Item_Key									= ItemOverride.Item_Key
												AND	ItemOverride.StoreJurisdictionID				= (SELECT StoreJurisdictionID FROM Store JOIN Vendor ON Store.Store_No = Vendor.Store_No WHERE Vendor.Vendor_ID = OrderHeader.PurchaseLocation_ID)
		LEFT JOIN	ItemBrand		(NOLOCK)	ON	ISNULL(ItemOverride.Brand_ID, Item.Brand_ID)	= ItemBrand.Brand_ID
		LEFT JOIN	ItemOrigin		(NOLOCK)	ON	ISNULL(OrderItem.Origin_ID, ISNULL(ItemOverride.Origin_ID, Item.Origin_ID))	= ItemOrigin.Origin_ID
	
	WHERE 
		OrderHeader.OrderHeader_ID = @OrderHeader_ID
    
	ORDER BY 
		OrderItem.OrderItem_ID
    
    SET NOCOUNT OFF
END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetFaxOrderItemList] TO [IRMAAdminRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetFaxOrderItemList] TO [IRMASupportRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetFaxOrderItemList] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetFaxOrderItemList] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetFaxOrderItemList] TO [IRMAReportsRole]
    AS [dbo];

