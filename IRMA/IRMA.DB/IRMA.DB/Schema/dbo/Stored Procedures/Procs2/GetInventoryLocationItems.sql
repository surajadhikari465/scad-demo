CREATE PROCEDURE dbo.GetInventoryLocationItems
	@InvLocID int
	,@Identifier varchar(15)
	,@ItemName varchar (60)

AS

SET NOCOUNT ON

SELECT 
	Item.Item_Key
	,ItemIdentifier.Identifier
	,Item.Item_Description 
FROM 
	InventoryLocationItems InvLocItems (nolock)
INNER JOIN 
	Item (nolock) ON InvLocItems.Item_Key = Item.Item_Key
INNER JOIN
	ItemIdentifier (nolock) ON Item.Item_Key = ItemIdentifier.Item_Key and ItemIdentifier.Default_Identifier = 1
WHERE
	InvLocItems.InvLocID = @InvLocID 
	and Item.Item_Description LIKE isnull('%' + @ItemName + '%', Item.Item_Description)
	and ItemIdentifier.Identifier LIKE isnull('%' + @Identifier + '%', ItemIdentifier.Identifier)

ORDER BY 
	Item.Item_Description, ItemIdentifier.Identifier

SET NOCOUNT OFF
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetInventoryLocationItems] TO [IRMASupportRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetInventoryLocationItems] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetInventoryLocationItems] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetInventoryLocationItems] TO [IRMAReportsRole]
    AS [dbo];

