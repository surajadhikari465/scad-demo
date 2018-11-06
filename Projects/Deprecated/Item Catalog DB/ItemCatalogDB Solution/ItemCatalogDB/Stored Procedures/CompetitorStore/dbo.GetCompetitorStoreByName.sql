if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[GetCompetitorStoreByName]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[GetCompetitorStoreByName]
GO

CREATE  PROCEDURE [dbo].[GetCompetitorStoreByName] 
	@CompetitorName varchar(50),
	@CompetitorLocationName varchar(50),
	@CompetitorStoreName varchar(50)
AS
BEGIN

SELECT
	CompetitorID,
	Name
FROM
	Competitor
WHERE
	Name = @CompetitorName

SELECT
	CompetitorLocationID,
	Name
FROM
	CompetitorLocation
WHERE
	Name = @CompetitorLocationName

SELECT
	CS.CompetitorStoreID,
	CS.CompetitorID,
	CS.CompetitorLocationID,
	CS.Name,
	CS.UpdateUserID,
	CS.UpdateDateTime
FROM
	CompetitorStore CS
	INNER JOIN Competitor C ON CS.CompetitorID = C.CompetitorID
	INNER JOIN CompetitorLocation CL ON CS.CompetitorLocationID = CL.CompetitorLocationID
WHERE
	C.Name = @CompetitorName
	AND
	CL.Name = @CompetitorLocationName
	AND
	CS.Name = @CompetitorStoreName

END 
go    