SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[GetZoneSupply]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[GetZoneSupply]
GO


CREATE PROCEDURE dbo.GetZoneSupply
@FromZone_ID int, 
@ToZone_ID int, 
@SubTeam_No int
AS

SELECT FromZone.Zone_ID FromZone_ID, FromZone.Zone_Name FromZone_Name, 
       ToZone.Zone_ID ToZone_ID, ToZone.Zone_Name ToZone_Name, 
       SubTeam.SubTeam_No, SubTeam.SubTeam_Name,
       Distribution_Markup,
       CrossDock_Markup
FROM ZoneSupply RIGHT JOIN (
       Zone FromZone INNER JOIN (
         Zone ToZone INNER JOIN SubTeam ON (1 = 1)
       ) ON (1 = 1)
     ) ON (ZoneSupply.FromZone_ID = FromZone.Zone_ID AND
           ZoneSupply.ToZone_ID = ToZone.Zone_ID AND
           ZoneSupply.SubTeam_No = SubTeam.SubTeam_No)
WHERE ISNULL(@FromZone_ID, FromZone.Zone_ID) = FromZone.Zone_ID AND
      ISNULL(@ToZone_ID, ToZone.Zone_ID) = ToZone.Zone_ID AND
      ISNULL(@SubTeam_No, SubTeam.SubTeam_No) = SubTeam.SubTeam_No
ORDER BY FromZone.Zone_Name, ToZone.Zone_Name, SubTeam.SubTeam_Name

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO


