CREATE PROCEDURE dbo.AddTitle
	@TitleDesc varchar(50)
AS 
	DECLARE @MaxId int
	SELECT @MaxId = MAX(Title_ID) + 1 FROM Title

	INSERT INTO dbo.Title (Title_ID, Title_Desc) VALUES (@MaxId, @TitleDesc)
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[AddTitle] TO [IRMAAdminRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[AddTitle] TO [IRMAClientRole]
    AS [dbo];

