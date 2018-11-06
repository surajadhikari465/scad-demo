CREATE PROCEDURE dbo.GetReceivingStores
AS 

SELECT Store_Name, Store_No
FROM Store 
WHERE (Mega_Store = 1 OR WFM_Store = 1 OR Distribution_Center = 1 OR Manufacturer = 1)
AND LastRecvLogDate IS NOT NULL
ORDER BY Mega_Store DESC, WFM_Store DESC, Store_Name
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetReceivingStores] TO [IRMASupportRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetReceivingStores] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetReceivingStores] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetReceivingStores] TO [IRMAReportsRole]
    AS [dbo];

