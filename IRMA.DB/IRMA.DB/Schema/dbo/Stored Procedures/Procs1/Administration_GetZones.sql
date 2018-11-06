CREATE PROCEDURE [dbo].[Administration_GetZones]
AS 

SELECT Zone.Zone_id, Zone_Name, ISNULL(GLMarketingExpenseAcct, '') AS GLMarketingExpenseAcct, Region_id, LastUpdate,
		(SELECT COUNT(1) 
	 	 FROM dbo.Store (NOLOCK)
	 	 WHERE Store.Zone_ID = Zone.Zone_ID) As StoreCount
FROM dbo.Zone (NOLOCK) 
GROUP BY Zone.Zone_Id, Zone_Name, GLMarketingExpenseAcct, Region_id, LastUpdate
ORDER BY Zone_Name, GLMarketingExpenseAcct, Zone.Zone_Id, Region_id, LastUpdate
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[Administration_GetZones] TO [IRMAAdminRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[Administration_GetZones] TO [IRMAClientRole]
    AS [dbo];

