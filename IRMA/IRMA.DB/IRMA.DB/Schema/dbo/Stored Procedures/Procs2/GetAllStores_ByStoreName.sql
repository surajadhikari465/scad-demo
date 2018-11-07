CREATE PROCEDURE dbo.[GetAllStores_ByStoreName] 
AS 
BEGIN
    SET NOCOUNT ON
    
    SELECT Zone.Zone_id, Zone_Name, Store_Name, Mega_Store, WFM_Store, Region_id, Store.Store_No 
    FROM Zone INNER JOIN Store on Zone.Zone_Id = Store.Zone_Id
    GROUP BY Zone.Zone_Id, Zone_Name, Store.Store_No, Store_Name, Region_Id, Mega_Store, WFM_Store
    ORDER BY Store_Name
    
    SET NOCOUNT OFF
END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetAllStores_ByStoreName] TO [IRMASupportRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetAllStores_ByStoreName] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetAllStores_ByStoreName] TO [IRMAReportsRole]
    AS [dbo];

