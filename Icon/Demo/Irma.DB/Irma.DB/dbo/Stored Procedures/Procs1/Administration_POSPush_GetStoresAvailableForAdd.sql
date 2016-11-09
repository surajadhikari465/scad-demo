CREATE PROCEDURE dbo.Administration_POSPush_GetStoresAvailableForAdd
AS 
-- Queries the Store and StorePOSConfig tables to retrieve the
-- list of stores that do not have a StorePOSConfig entry.
-- Skips the Distribution_Center and Manufacturer records.

BEGIN
SELECT 
Store_No, Store_Name
FROM Store ST 
WHERE 
	ST.Store_No not in (SELECT Store_No FROM StorePOSConfig)
	and ST.Distribution_Center = 0 
	and ST.Manufacturer = 0
ORDER BY ST.Store_Name
END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[Administration_POSPush_GetStoresAvailableForAdd] TO [IRMAAdminRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[Administration_POSPush_GetStoresAvailableForAdd] TO [IRMASupportRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[Administration_POSPush_GetStoresAvailableForAdd] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[Administration_POSPush_GetStoresAvailableForAdd] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[Administration_POSPush_GetStoresAvailableForAdd] TO [IRMAReportsRole]
    AS [dbo];

