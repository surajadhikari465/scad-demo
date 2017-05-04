CREATE PROCEDURE [dbo].[GetStoreCompetitorStoresByStore_No] 
	@Store_No int
AS
BEGIN

SELECT
	Store_No,
	CompetitorStoreID,
	Priority
FROM
	StoreCompetitorStore
WHERE
	@Store_No = Store_No
ORDER BY
	Priority

END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetStoreCompetitorStoresByStore_No] TO [IRMAClientRole]
    AS [dbo];

