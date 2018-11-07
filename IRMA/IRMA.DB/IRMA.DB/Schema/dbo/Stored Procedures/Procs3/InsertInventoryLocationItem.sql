CREATE PROCEDURE dbo.InsertInventoryLocationItem 
@InvLocID int, 
@Item_Key int
AS 

SET NOCOUNT ON

INSERT INTO InventoryLocationItems (InvLocID, Item_Key)
VALUES (@InvLocID, @Item_Key)

SET NOCOUNT OFF
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[InsertInventoryLocationItem] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[InsertInventoryLocationItem] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[InsertInventoryLocationItem] TO [IRMAReportsRole]
    AS [dbo];

