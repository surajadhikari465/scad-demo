CREATE PROCEDURE dbo.GetUsersSubTeamList
    @User_ID int
AS
BEGIN
    SET NOCOUNT ON
    
    IF EXISTS (SELECT * FROM Users WHERE User_ID = @User_ID AND SuperUser = 1)
        EXEC GetAllSubTeams
    ELSE
        SELECT SubTeam.SubTeam_No, SubTeam_Name
        FROM SubTeam
        INNER JOIN
            UsersSubTeam
            ON UsersSubTeam.SubTeam_No = SubTeam.SubTeam_No
        WHERE UsersSubTeam.User_ID = @User_ID
        
    SET NOCOUNT OFF
END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetUsersSubTeamList] TO [IRMASupportRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetUsersSubTeamList] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetUsersSubTeamList] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetUsersSubTeamList] TO [IRMAReportsRole]
    AS [dbo];

