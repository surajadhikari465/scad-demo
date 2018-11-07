CREATE PROCEDURE dbo.DeleteTitle
	@TitleID int
AS 
	DELETE FROM RoleConflictReason WHERE Title_Id = @TitleID
	DELETE FROM TitleDefaultPermission WHERE TitleId = @TitleID
	DELETE FROM Title WHERE Title_ID = @TitleID
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[DeleteTitle] TO [IRMAAdminRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[DeleteTitle] TO [IRMAClientRole]
    AS [dbo];

