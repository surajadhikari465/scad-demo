CREATE PROCEDURE dbo.GetAllSubTeams
AS
BEGIN
    SET NOCOUNT ON
    
    SELECT SubTeam_Name, SubTeam_No 
    FROM SubTeam (NoLock)
    ORDER BY SubTeam_Name
    
    SET NOCOUNT OFF
END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetAllSubTeams] TO [IRMASupportRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetAllSubTeams] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetAllSubTeams] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetAllSubTeams] TO [IRMAReportsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetAllSubTeams] TO [IRMASLIMRole]
    AS [dbo];

