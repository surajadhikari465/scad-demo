CREATE PROCEDURE dbo.GetDistManZones
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
    WHERE (store.Manufacturer = 1 or store.Distribution_Center = 1)
          AND isnull(@Region_ID, Zone.Region_ID) = Zone.Region_ID            
    ORDER BY zone_name    
    SET NOCOUNT OFF        
END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetDistManZones] TO [IRMASupportRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetDistManZones] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetDistManZones] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetDistManZones] TO [IRMAReportsRole]
    AS [dbo];

