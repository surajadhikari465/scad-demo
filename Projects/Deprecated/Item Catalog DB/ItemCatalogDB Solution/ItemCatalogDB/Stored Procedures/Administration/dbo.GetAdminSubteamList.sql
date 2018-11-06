SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[GetAdminSubTeamList]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[GetAdminSubTeamList]
GO

CREATE PROCEDURE dbo.GetAdminSubTeamList
AS 
BEGIN
    SET NOCOUNT ON
    
		SELECT 
			SubTeam_No,
			SubTeam_Name,
			CASE SubTeamType_ID
				WHEN 1 THEN (RTRIM(SubTeam_Name) + ' (RET)')
				WHEN 2 THEN	(RTRIM(SubTeam_Name) + ' (MFG)')
				WHEN 3 THEN (RTRIM(SubTeam_Name) + ' (RET/MFG)')
				WHEN 4 THEN	(RTRIM(SubTeam_Name) + ' (EXP)')
				WHEN 5 THEN (RTRIM(SubTeam_Name) + ' (PKG)')
				WHEN 6 THEN (RTRIM(SubTeam_Name) + ' (SUP)')
			END AS SubTeam_Description
		FROM
			SubTeam
		ORDER BY
			RTRIM(SubTeam_Name)
    
    SET NOCOUNT OFF
END
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO
