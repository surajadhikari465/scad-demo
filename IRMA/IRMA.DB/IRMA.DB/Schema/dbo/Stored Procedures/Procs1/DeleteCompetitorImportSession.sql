CREATE PROCEDURE [dbo].[DeleteCompetitorImportSession] 
	@CompetitorImportSessionID int
AS
BEGIN

-- Clear child data
DELETE 
	FROM CompetitorImportInfo
	WHERE CompetitorImportSessionID = @CompetitorImportSessionID

DELETE 
	FROM CompetitorImportSession
	WHERE CompetitorImportSessionID = @CompetitorImportSessionID

END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[DeleteCompetitorImportSession] TO [IRMAClientRole]
    AS [dbo];

