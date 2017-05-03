CREATE PROCEDURE dbo.GetAllStores 
AS 
BEGIN
    SET NOCOUNT ON
    
    SELECT Zone.Zone_id, Zone_Name, Store_Name, Mega_Store, WFM_Store, Region_id, Store.Store_No 
    FROM Zone (nolock) INNER JOIN Store (nolock) on Zone.Zone_Id = Store.Zone_Id
    GROUP BY Zone.Zone_Id, Zone_Name, Store.Store_No, Store_Name, Region_Id, Mega_Store, WFM_Store
    ORDER BY Zone.Zone_Id, Store.Store_No, Zone_Name, Store_Name, Region_Id, Mega_Store, WFM_Store
    
    SET NOCOUNT OFF
END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetAllStores] TO [IRMAAdminRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetAllStores] TO [IRMASupportRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetAllStores] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetAllStores] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetAllStores] TO [IRMAReportsRole]
    AS [dbo];

