CREATE PROCEDURE [dbo].[GetCompetitorStoreSearch] 
	@CompetitorID int = NULL,
	@CompetitorLocationID int = NULL
AS
BEGIN

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
	(@CompetitorID IS NULL OR CS.CompetitorID = @CompetitorID)
	AND
	(@CompetitorLocationID IS NULL OR CS.CompetitorLocationID = @CompetitorLocationID)
ORDER BY
	C.Name,
	CL.Name,
	CS.Name

END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetCompetitorStoreSearch] TO [IRMAClientRole]
    AS [dbo];

