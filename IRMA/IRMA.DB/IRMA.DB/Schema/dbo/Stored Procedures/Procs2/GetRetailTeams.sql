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
GRANT EXECUTE
    ON OBJECT::[dbo].[GetRetailTeams] TO [IRMAAdminRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetRetailTeams] TO [IRMASupportRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetRetailTeams] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetRetailTeams] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetRetailTeams] TO [IRMAReportsRole]
    AS [dbo];

