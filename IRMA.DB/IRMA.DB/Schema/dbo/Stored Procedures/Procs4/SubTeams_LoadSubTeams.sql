CREATE PROCEDURE dbo.[SubTeams_LoadSubTeams]
AS
BEGIN
	SET NOCOUNT ON;

	SELECT	*,
			CASE SubTeamType_ID
				WHEN 1 THEN (RTRIM(SubTeam_Name) + ' (RET)')
				WHEN 2 THEN	(RTRIM(SubTeam_Name) + ' (MFG)')
				WHEN 3 THEN (RTRIM(SubTeam_Name) + ' (RET/MFG)')
				WHEN 4 THEN	(RTRIM(SubTeam_Name) + ' (EXP)')
				WHEN 5 THEN (RTRIM(SubTeam_Name) + ' (PKG)')
				WHEN 6 THEN (RTRIM(SubTeam_Name) + ' (SUP)')
			END AS SubTeam_Description
	FROM	SubTeam
	Order By SubTeam_Name


END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[SubTeams_LoadSubTeams] TO [IRMAAdminRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[SubTeams_LoadSubTeams] TO [IRMASupportRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[SubTeams_LoadSubTeams] TO [IRMAClientRole]
    AS [dbo];

