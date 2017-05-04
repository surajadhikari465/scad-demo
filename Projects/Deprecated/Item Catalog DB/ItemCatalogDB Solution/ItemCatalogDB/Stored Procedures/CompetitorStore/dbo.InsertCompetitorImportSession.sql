if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[InsertCompetitorImportSession]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[InsertCompetitorImportSession]
GO

CREATE PROCEDURE [dbo].[InsertCompetitorImportSession] 
@User_ID int,
@CompetitorImportSessionID int OUTPUT

AS
BEGIN

INSERT INTO CompetitorImportSession (User_ID, StartDateTime) values (@User_ID, GETDATE())

SET @CompetitorImportSessionID = SCOPE_IDENTITY()

END 
go 