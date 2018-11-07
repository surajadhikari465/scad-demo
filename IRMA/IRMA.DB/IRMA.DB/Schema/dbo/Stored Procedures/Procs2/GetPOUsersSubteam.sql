CREATE PROCEDURE dbo.GetPOUsersSubteam

AS

BEGIN
    SET NOCOUNT ON

SELECT Users.User_ID, Users.UserName, EMail, SubTeam_No
FROM UsersSubTeam 
INNER JOIN Users ON Users.User_ID = UsersSubTeam.User_ID
WHERE Regional_Coordinator = 1

SET NOCOUNT OFF
    
    
    
END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetPOUsersSubteam] TO [IRMASupportRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetPOUsersSubteam] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetPOUsersSubteam] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetPOUsersSubteam] TO [IRMAReportsRole]
    AS [dbo];

