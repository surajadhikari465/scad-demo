CREATE PROCEDURE dbo.GetInventoryStores
AS 

SELECT Store_Name, Store_No, Mega_Store, WFM_Store, Distribution_Center, Manufacturer, Zone_ID
FROM Store 
WHERE Mega_Store = 1 OR WFM_Store = 1 OR Distribution_Center = 1 OR Manufacturer = 1
ORDER BY Mega_Store DESC, WFM_Store DESC, Distribution_Center DESC, Manufacturer DESC, Store_Name
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetInventoryStores] TO [IRMASupportRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetInventoryStores] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetInventoryStores] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetInventoryStores] TO [IRMAReportsRole]
    AS [dbo];

