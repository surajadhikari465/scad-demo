if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[DeleteCompetitorImportInfo]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[DeleteCompetitorImportInfo]
GO

CREATE PROCEDURE [dbo].[DeleteCompetitorImportInfo] 
	@CompetitorImportInfoID int
AS
BEGIN

DELETE 
	FROM CompetitorImportInfo
	WHERE CompetitorImportInfoID = @CompetitorImportInfoID

END 
go    