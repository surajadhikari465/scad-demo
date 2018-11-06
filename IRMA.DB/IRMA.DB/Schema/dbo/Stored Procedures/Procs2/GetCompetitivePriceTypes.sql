CREATE PROCEDURE [dbo].[GetCompetitivePriceTypes] 
AS
BEGIN

SELECT
	CompetitivePriceTypeID,
	Description
FROM
	CompetitivePriceType
ORDER BY
	Description

END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetCompetitivePriceTypes] TO [IRMAClientRole]
    AS [dbo];

