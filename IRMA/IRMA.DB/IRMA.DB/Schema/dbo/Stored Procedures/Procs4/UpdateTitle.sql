CREATE PROCEDURE dbo.UpdateTitle
	@TitleID int,
	@TitleDesc varchar(50)
AS 
	UPDATE Title SET Title_Desc = @TitleDesc WHERE Title_ID = @TitleID
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[UpdateTitle] TO [IRMAAdminRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[UpdateTitle] TO [IRMAClientRole]
    AS [dbo];

