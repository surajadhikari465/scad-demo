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
GRANT EXECUTE
    ON OBJECT::[dbo].[GetAdminSubTeamList] TO [IRMAAdminRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetAdminSubTeamList] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetAdminSubTeamList] TO [IRMAReportsRole]
    AS [dbo];

