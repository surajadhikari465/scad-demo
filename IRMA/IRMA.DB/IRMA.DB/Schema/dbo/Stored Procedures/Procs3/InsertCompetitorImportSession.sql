CREATE PROCEDURE [dbo].[InsertCompetitorImportSession] 
@User_ID int,
@CompetitorImportSessionID int OUTPUT

AS
BEGIN

INSERT INTO CompetitorImportSession (User_ID, StartDateTime) values (@User_ID, GETDATE())

SET @CompetitorImportSessionID = SCOPE_IDENTITY()

END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[InsertCompetitorImportSession] TO [IRMAClientRole]
    AS [dbo];

