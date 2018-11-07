CREATE PROCEDURE dbo.RepInventoryLocationItemsCount

	@InvLocID int

AS

SET NOCOUNT ON

SELECT
	Store.Store_Name
	,ItemIdentifier.Identifier
	,InventoryLocation.InvLoc_Name
	,Item.Item_Description,
	convert(varchar,cast(Package_Desc1 as decimal(20,2))) 
	+ '/' +
	convert(varchar,cast(Package_Desc2 as decimal(20,2)))
	+ '/' +
	cast(Unit_Name as varchar(10))AS PackSizeUnit   ,
	right('0000000000000' + cast(ItemIdentifier.identifier as varchar(20)),12) as PaddedIdentifier
FROM
	InventoryLocation
INNER JOIN 
	Store (nolock) ON Store.Store_No = InventoryLocation.Store_No
INNER JOIN
	InventoryLocationItems (nolock) ON InventoryLocation.InvLoc_ID = InventoryLocationItems.InvLocID
INNER JOIN
	Item (nolock) ON InventoryLocationItems.Item_Key = Item.Item_Key
INNER JOIN
	ItemIdentifier (nolock) ON Item.Item_Key = ItemIdentifier.Item_Key
INNER JOIN
	ItemUnit (nolock) ON Item.Package_Unit_ID = ItemUnit.Unit_ID
WHERE
	InventoryLocation.InvLoc_ID = @InvLocID
ORDER BY 
	InventoryLocation.InvLoc_Name

SET NOCOUNT OFF
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[RepInventoryLocationItemsCount] TO [IRMASupportRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[RepInventoryLocationItemsCount] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[RepInventoryLocationItemsCount] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[RepInventoryLocationItemsCount] TO [IRMAReportsRole]
    AS [dbo];

