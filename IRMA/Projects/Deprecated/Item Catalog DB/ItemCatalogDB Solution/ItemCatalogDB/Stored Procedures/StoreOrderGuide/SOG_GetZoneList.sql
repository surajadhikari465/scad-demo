SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

IF EXISTS (SELECT * FROM sysobjects WHERE type = 'P' AND name = 'SOG_GetZoneList')
	BEGIN
		DROP Procedure [dbo].SOG_GetZoneList
	END
GO

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
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO