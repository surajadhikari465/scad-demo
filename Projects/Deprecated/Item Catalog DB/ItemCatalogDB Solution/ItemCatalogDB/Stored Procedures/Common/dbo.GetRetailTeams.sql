SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[GetRetailTeams]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[GetRetailTeams]
GO


CREATE PROCEDURE [dbo].[GetRetailTeams]
AS
BEGIN
    SET NOCOUNT ON
    
	SELECT DISTINCT Team.Team_No, Team.Team_Name
	FROM Team
		INNER JOIN SubTeam ON Team.Team_No = SubTeam.Team_No
	WHERE SubTeam.SubTeamType_ID IN(1,3)
	ORDER BY Team.Team_Name
    
    SET NOCOUNT OFF
END

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

