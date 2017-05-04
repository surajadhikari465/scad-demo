CREATE PROCEDURE dbo.GetItemName 
@Item_Key int 
AS 


SELECT Item.Item_Key, Item.Item_Description, Identifier
FROM Item INNER JOIN ItemIdentifier ON (ItemIdentifier.Item_Key = Item.Item_Key AND ItemIdentifier.Default_Identifier = 1)
WHERE Item.Item_Key = @Item_Key
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetItemName] TO [IRMASupportRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetItemName] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetItemName] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetItemName] TO [IRMAReportsRole]
    AS [dbo];

