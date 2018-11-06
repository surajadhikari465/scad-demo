CREATE PROCEDURE dbo.GetUsersWithTitle
	@TitleID int
AS 
	SELECT * FROM Users WHERE Title = @TitleID
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetUsersWithTitle] TO [IRMAAdminRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetUsersWithTitle] TO [IRMAClientRole]
    AS [dbo];

