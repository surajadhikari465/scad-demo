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
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetCompetitorLocations] TO [IRMAClientRole]
    AS [dbo];

