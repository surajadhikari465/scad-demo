if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[DeleteCompetitorImportSession]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[DeleteCompetitorImportSession]
GO

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
go   