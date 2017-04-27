SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[GetRetailSubteamsByTeamNo]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[GetRetailSubteamsByTeamNo]
GO

CREATE PROCEDURE [dbo].[GetRetailSubteamsByTeamNo] 

	@Team_No int
AS 
BEGIN
    SET NOCOUNT ON
    
    SELECT 
		SubTeam_No, SubTeam_Name 
    FROM 
		SubTeam
    WHERE 
		Team_No = @Team_No
		AND SubTeamType_ID IN(1,3)
    ORDER BY 
		SubTeam_Name
    
    SET NOCOUNT OFF
END
GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

