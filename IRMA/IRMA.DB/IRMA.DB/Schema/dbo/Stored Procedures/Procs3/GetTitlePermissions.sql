CREATE PROCEDURE dbo.GetTitlePermissions
	@TitleID int
AS 
	SELECT * FROM TitleDefaultPermission WHERE TitleId = @TitleID
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetTitlePermissions] TO [IRMAAdminRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetTitlePermissions] TO [IRMAClientRole]
    AS [dbo];

