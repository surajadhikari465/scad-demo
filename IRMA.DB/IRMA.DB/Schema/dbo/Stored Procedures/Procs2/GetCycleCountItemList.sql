CREATE PROCEDURE dbo.GetCycleCountItemList 
	@CycleCountID as int	
	,@Identifier as varchar (50) = null
	,@ItemDesc as varchar(50) = null
	,@Vendor as varchar(50) = null
	,@VendorID as int = null
AS

SET NOCOUNT ON

SELECT 
	CycleCountItems.CycleCountItemID
	,Item.Item_Key
	,Identifier
	,Item_Description 
	,Item.CostedByWeight
    ,Item.Package_Desc1
    ,Item.Package_Desc2
    ,Item.Package_Unit_ID
       	,SUM(ISNULL(CycleCountHistory.Count,0)) AS CountTotal
      	,SUM(ISNULL(CycleCountHistory.Weight,0)) AS WeightTotal

FROM 
	CycleCountItems (NOLOCK)
	INNER JOIN Item (NOLOCK) ON (CycleCountItems.Item_Key = Item.Item_Key)
	INNER JOIN ItemIdentifier (NOLOCK) ON (ItemIdentifier.Item_Key = Item.Item_Key AND ItemIdentifier.Default_Identifier = 1)
	LEFT JOIN CycleCountHistory (NOLOCK) ON (CycleCountItems.CycleCountItemID = CycleCountHistory.CycleCountItemID) 


WHERE 
	CycleCountItems.CycleCountID = @CycleCountID

	AND Item_Description LIKE ISNULL('%' + @ItemDesc + '%', Item_Description)  		

	AND Identifier LIKE ISNULL('%' + @Identifier + '%', Identifier)

	AND EXISTS (
			-- If the @Vendor is null, return a row.  This means to return all items, not limited by Vendor Name.
			SELECT SubItem.Item_Key
			FROM Item SubItem(NOLOCK)
			WHERE Item.Item_Key = SubItem.Item_Key
	                  		AND @Vendor = null
		UNION
			-- If the @Vendor was supplied, then only return a row if there is a match on the Vendor Name.  No match, no row.
			SELECT SubItem.Item_Key 
			FROM Item SubItem (NOLOCK)
			INNER JOIN ItemVendor (NOLOCK) ON SubItem.Item_Key = ItemVendor.Item_Key
			INNER JOIN Vendor (NOLOCK)  ON ItemVendor.Vendor_ID = Vendor.Vendor_ID 
			WHERE Item.Item_Key = SubItem.Item_Key
				AND Vendor.CompanyName LIKE ISNULL('%' + @Vendor + '%', Vendor.CompanyName))

	AND EXISTS (
			-- If the @VendorID is null, return a row.  This means to return all items, not limited by VendorID.
			SELECT SubItem.Item_Key
			FROM Item SubItem(NOLOCK)
			WHERE Item.Item_Key = SubItem.Item_Key
	                  		AND @VendorID = null
		UNION
			-- If the @VendorID was supplied, then only return a row if there is a match on the VendorID.  No match, no row.
			SELECT SubItem.Item_Key 
			FROM Item SubItem (NOLOCK)
			JOIN ItemVendor (NOLOCK) ON SubItem.Item_Key = ItemVendor.Item_Key
			WHERE Item.Item_Key = SubItem.Item_Key
				 AND ISNULL(@VendorID, ItemVendor.Vendor_ID) = ItemVendor.Vendor_ID)
	
GROUP BY 
	CycleCountItems.CycleCountItemID , Item.Item_Key, Identifier, Item_Description, Item.CostedByWeight, Item.Package_Desc1, Item.Package_Desc2, Item.Package_Unit_ID

ORDER BY 
	Item_Description

SET NOCOUNT OFF
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetCycleCountItemList] TO [IRMASupportRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetCycleCountItemList] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetCycleCountItemList] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetCycleCountItemList] TO [IRMAReportsRole]
    AS [dbo];

