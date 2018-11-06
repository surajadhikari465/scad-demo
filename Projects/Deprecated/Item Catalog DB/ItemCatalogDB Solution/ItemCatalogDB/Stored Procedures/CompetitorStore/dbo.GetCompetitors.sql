  if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[GetCompetitors]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[GetCompetitors]
GO

CREATE PROCEDURE [dbo].[GetCompetitors] 
AS
BEGIN

SELECT
	CompetitorID,
	Name
FROM
	Competitor
ORDER BY
	Name

END 
go  