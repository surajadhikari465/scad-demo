CREATE PROCEDURE [dbo].[GetCompetitorImportSessionByUser_ID] 
	@User_ID int
AS
BEGIN

SELECT
	CompetitorImportSessionID,
	User_ID,
	StartDateTime
FROM
	CompetitorImportSession 
WHERE
	User_ID = @User_ID
ORDER BY
	StartDateTime DESC

END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetCompetitorImportSessionByUser_ID] TO [IRMAClientRole]
    AS [dbo];

