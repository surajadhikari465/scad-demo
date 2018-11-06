if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[GetCompetitorImportSessionByUser_ID]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[GetCompetitorImportSessionByUser_ID]
GO

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
go  