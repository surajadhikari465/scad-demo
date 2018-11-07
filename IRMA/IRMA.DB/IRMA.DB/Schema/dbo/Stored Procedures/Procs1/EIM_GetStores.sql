CREATE PROCEDURE dbo.EIM_GetStores 
AS 
BEGIN
    SET NOCOUNT ON
    
    SELECT 
		Store.Store_No
		,Store_Name		
    FROM Store (nolock)
    WHERE Mega_Store = 1 OR WFM_Store = 1
    ORDER BY Store_Name
    
    SET NOCOUNT OFF
END