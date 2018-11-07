CREATE PROCEDURE dbo.GetItemData
@Item_Key int
AS 

SELECT Item_Description, Identifier, Deleted_Item
FROM ItemIdentifier INNER JOIN Item ON (ItemIdentifier.Item_Key = Item.Item_Key AND ItemIdentifier.Default_Identifier = 1)
WHERE Item.Item_Key = @Item_Key
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetItemData] TO [IRMASupportRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetItemData] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetItemData] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetItemData] TO [IRMAReportsRole]
    AS [dbo];

