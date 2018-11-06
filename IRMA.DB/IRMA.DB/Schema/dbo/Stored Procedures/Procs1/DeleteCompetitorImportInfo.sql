CREATE PROCEDURE [dbo].[DeleteCompetitorImportInfo] 
	@CompetitorImportInfoID int
AS
BEGIN

DELETE 
	FROM CompetitorImportInfo
	WHERE CompetitorImportInfoID = @CompetitorImportInfoID

END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[DeleteCompetitorImportInfo] TO [IRMAClientRole]
    AS [dbo];

