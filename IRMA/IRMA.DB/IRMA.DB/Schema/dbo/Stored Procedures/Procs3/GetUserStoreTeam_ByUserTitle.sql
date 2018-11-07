CREATE PROCEDURE dbo.GetUserStoreTeam_ByUserTitle
	@User_ID int,
	@Title_ID int
AS
BEGIN
    SET NOCOUNT ON
    
    SELECT USTT.Team_No, USTT.Title_ID
    FROM UserStoreTeamTitle USTT
    WHERE USTT.User_ID = @User_ID
    AND ISNULL(@Title_ID, USTT.Title_ID) = USTT.Title_ID    

    SET NOCOUNT OFF
END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetUserStoreTeam_ByUserTitle] TO [IRMASupportRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetUserStoreTeam_ByUserTitle] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetUserStoreTeam_ByUserTitle] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetUserStoreTeam_ByUserTitle] TO [IRMAReportsRole]
    AS [dbo];

