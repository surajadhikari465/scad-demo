CREATE PROCEDURE dbo.GetTitleConflicts
	@TitleID int
AS 
	SELECT User_ID, Username, Fullname FROM Users WHERE Title = @TitleID
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetTitleConflicts] TO [IRMAAdminRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetTitleConflicts] TO [IRMAClientRole]
    AS [dbo];

