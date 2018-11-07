CREATE PROCEDURE dbo.GetSubTeamName
@SubTeam_No int 
AS 

SELECT SubTeam_Name 
FROM SubTeam 
WHERE SubTeam_No = @SubTeam_No
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetSubTeamName] TO [IRMASupportRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetSubTeamName] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetSubTeamName] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetSubTeamName] TO [IRMAReportsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetSubTeamName] TO [IRMAAVCIRole]
    AS [dbo];

