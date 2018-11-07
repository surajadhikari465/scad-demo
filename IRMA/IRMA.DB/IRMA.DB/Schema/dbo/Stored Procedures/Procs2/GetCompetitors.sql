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
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetCompetitors] TO [IRMAClientRole]
    AS [dbo];

