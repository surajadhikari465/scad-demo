CREATE PROCEDURE dbo.RepInventoryLocationItems

	@InvLocID int

AS
SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED
SET NOCOUNT ON

SELECT
	Store.Store_Name
	,ItemIdentifier.Identifier
	,InventoryLocation.InvLoc_Name
	,Item.Item_Description
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
WHERE
	InventoryLocation.InvLoc_ID = @InvLocID
ORDER BY 
	InventoryLocation.InvLoc_Name

SET NOCOUNT OFF
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[RepInventoryLocationItems] TO [IRMASupportRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[RepInventoryLocationItems] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[RepInventoryLocationItems] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[RepInventoryLocationItems] TO [IRMAReportsRole]
    AS [dbo];

