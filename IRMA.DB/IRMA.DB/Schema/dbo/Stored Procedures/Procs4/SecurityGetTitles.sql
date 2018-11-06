CREATE PROCEDURE dbo.SecurityGetTitles
AS
BEGIN
    SET NOCOUNT ON

	SELECT Title_ID, Title_Desc 
	FROM Title 
	ORDER BY Title_Desc
  
    SET NOCOUNT OFF
END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[SecurityGetTitles] TO [IRMAAdminRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[SecurityGetTitles] TO [IRMAClientRole]
    AS [dbo];

