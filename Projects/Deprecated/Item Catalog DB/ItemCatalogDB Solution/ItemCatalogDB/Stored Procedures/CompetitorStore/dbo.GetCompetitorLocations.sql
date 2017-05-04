  if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[GetCompetitorLocations]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[GetCompetitorLocations]
GO

CREATE PROCEDURE [dbo].[GetCompetitorLocations] 
AS
BEGIN

SELECT
	CompetitorLocationID,
	Name
FROM
	CompetitorLocation
ORDER BY
	Name

END 
go   