CREATE PROCEDURE dbo.UpdateTitleConflicts
	@TitleId int,
	@UserId int
AS 
	UPDATE Users SET Title = @TitleId WHERE User_Id = @UserId
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[UpdateTitleConflicts] TO [IRMAAdminRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[UpdateTitleConflicts] TO [IRMAClientRole]
    AS [dbo];

