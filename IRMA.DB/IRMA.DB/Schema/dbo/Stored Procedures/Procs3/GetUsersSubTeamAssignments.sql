CREATE PROCEDURE [dbo].[GetUsersSubTeamAssignments]
    @User_ID int
AS
BEGIN
    SET NOCOUNT ON
    
    SELECT SubTeam.SubTeam_No, SubTeam_Name, ISNULL(Regional_Coordinator, 0) AS Regional_Coordinator
    FROM SubTeam
    INNER JOIN
        UsersSubTeam
        ON UsersSubTeam.SubTeam_No = SubTeam.SubTeam_No
    WHERE UsersSubTeam.User_ID = @User_ID
        
    SET NOCOUNT OFF
END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetUsersSubTeamAssignments] TO [IRMAAdminRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetUsersSubTeamAssignments] TO [IRMAClientRole]
    AS [dbo];

