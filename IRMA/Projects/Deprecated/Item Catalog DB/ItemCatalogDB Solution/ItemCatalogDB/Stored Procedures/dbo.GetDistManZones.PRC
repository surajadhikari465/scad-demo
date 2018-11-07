SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[GetDistManZones]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[GetDistManZones]
GO


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
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO


