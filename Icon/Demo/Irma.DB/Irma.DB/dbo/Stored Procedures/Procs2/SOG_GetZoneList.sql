CREATE PROCEDURE dbo.SOG_GetZoneList

WITH RECOMPILE
AS

-- **************************************************************************
-- Procedure: SOG_GetZoneList()
--    Author: Billy Blackerby
--      Date: 3/13/2009
--
-- Description:
-- Utilized by StoreOrderGuide to return a list of zones for filters
--
-- Modification History:
-- Date			Init	Comment
-- 03/13/2009	BBB		Creation
-- 03/17/2009	BBB		Added in 'All' option
-- **************************************************************************	
BEGIN
    SET NOCOUNT ON
	--**************************************************************************
	--Main SQL
	--**************************************************************************
	SELECT
		[ZoneID]	= 0,
		[ZoneName]	= 'All Zones'
		
	UNION
	
	SELECT 
		[ZoneID]	= z.Zone_ID,
		[ZoneName]	= z.Zone_Name
	FROM 
		Zone (nolock) z
		
	ORDER BY
		ZoneID,
		ZoneName

    SET NOCOUNT OFF
END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[SOG_GetZoneList] TO [IRMASLIMRole]
    AS [dbo];

