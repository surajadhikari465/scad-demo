CREATE PROCEDURE dbo.DeleteItemUPCInventory
@UPC varchar(18)
AS 

BEGIN

    DELETE 
    FROM ItemUPC
    WHERE UPC = @UPC AND UPC_Changed = 1

    UPDATE ItemUPC
    SET Deleted_UPC = @UPC,
        UPC = '0',
        UPC_Changed = 1
    WHERE UPC = @UPC

END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[DeleteItemUPCInventory] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[DeleteItemUPCInventory] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[DeleteItemUPCInventory] TO [IRMAReportsRole]
    AS [dbo];

