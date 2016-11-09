CREATE PROCEDURE dbo.Administration_UserAdmin_GetAllUsers
AS
	BEGIN
		SELECT *
		FROM [Users]
		ORDER BY [UserName]
	END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[Administration_UserAdmin_GetAllUsers] TO [IRMAAdminRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[Administration_UserAdmin_GetAllUsers] TO [IRMAClientRole]
    AS [dbo];

