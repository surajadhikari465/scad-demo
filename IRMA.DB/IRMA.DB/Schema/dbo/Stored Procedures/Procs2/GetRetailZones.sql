CREATE PROCEDURE dbo.GetRetailZones
@Region_ID int
AS 
BEGIN
    SET NOCOUNT ON
    
    SELECT DISTINCT
           zone.zone_name,
           zone.Zone_ID
    FROM store 
        INNER JOIN
            zone 
            ON zone.zone_id = store.zone_id
    WHERE (store.Mega_Store = 1 or store.WFM_Store = 1)
          AND isnull(@Region_ID, Zone.Region_ID) = Zone.Region_ID            
    ORDER BY zone_name    
    SET NOCOUNT OFF        
END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetRetailZones] TO [IRMAClientRole]
    AS [dbo];

