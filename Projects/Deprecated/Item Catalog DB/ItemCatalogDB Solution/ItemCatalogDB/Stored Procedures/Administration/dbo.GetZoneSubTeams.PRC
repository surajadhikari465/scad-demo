SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[GetZoneSubTeams]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[GetZoneSubTeams]
GO


CREATE PROCEDURE dbo.GetZoneSubTeams 
AS 

SELECT Zone.Zone_ID, Zone.Zone_Name, SubTeam.SubTeam_No, SubTeam.SubTeam_Name, Supplier_Store_No, Store.Store_Name
FROM Store RIGHT JOIN (
       ZoneSubTeam RIGHT JOIN (
         Zone FULL JOIN SubTeam ON (1 = 1)
       ) ON (ZoneSubTeam.SubTeam_No = SubTeam.SubTeam_No AND
             ZoneSubTeam.Zone_ID = Zone.Zone_ID)
     ) ON (Store.Store_No = ZoneSubTeam.Supplier_Store_No)
WHERE EXISTS (SELECT * FROM Store S (nolock) WHERE S.Zone_ID = Zone.Zone_ID)
ORDER BY Zone.Zone_Name, SubTeam.SubTeam_Name
GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

