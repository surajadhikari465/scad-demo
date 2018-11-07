CREATE PROCEDURE dbo.RemoveItemUPCInventory
@UPC varchar(18)
AS 

UPDATE ItemUPC
SET Deleted_UPC = @UPC,
    UPC = '0',
    UPC_Changed = 1
WHERE UPC = @UPC
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[RemoveItemUPCInventory] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[RemoveItemUPCInventory] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[RemoveItemUPCInventory] TO [IRMAReportsRole]
    AS [dbo];

