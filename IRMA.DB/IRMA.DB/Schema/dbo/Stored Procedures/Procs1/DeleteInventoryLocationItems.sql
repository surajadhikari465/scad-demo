CREATE PROCEDURE dbo.DeleteInventoryLocationItems
@InvLocID int,
@Item_Key int = null
AS 

SET NOCOUNT ON

DELETE 
FROM InventoryLocationItems
WHERE InvLocID = @InvLocID and Item_Key = ISNULL(@Item_Key, Item_Key)

SET NOCOUNT OFF
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[DeleteInventoryLocationItems] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[DeleteInventoryLocationItems] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[DeleteInventoryLocationItems] TO [IRMAReportsRole]
    AS [dbo];

