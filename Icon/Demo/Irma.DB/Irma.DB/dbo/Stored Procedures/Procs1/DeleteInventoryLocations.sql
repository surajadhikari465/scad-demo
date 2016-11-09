CREATE PROCEDURE dbo.DeleteInventoryLocations
@InvLoc_ID int 
AS

SET NOCOUNT ON

DELETE 
FROM InventoryLocationItems
WHERE InvLocID = @InvLoc_ID


DELETE 
FROM InventoryLocation
WHERE InvLoc_ID = @InvLoc_ID

SET NOCOUNT OFF
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[DeleteInventoryLocations] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[DeleteInventoryLocations] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[DeleteInventoryLocations] TO [IRMAReportsRole]
    AS [dbo];

