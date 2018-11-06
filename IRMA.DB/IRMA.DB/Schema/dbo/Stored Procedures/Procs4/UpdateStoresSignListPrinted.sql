CREATE PROCEDURE dbo.UpdateStoresSignListPrinted
@StoreSignListID int
AS

UPDATE StoresSignList 
SET Sign_Printed = 1 
WHERE StoreSignListID = @StoreSignListID
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[UpdateStoresSignListPrinted] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[UpdateStoresSignListPrinted] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[UpdateStoresSignListPrinted] TO [IRMAReportsRole]
    AS [dbo];

