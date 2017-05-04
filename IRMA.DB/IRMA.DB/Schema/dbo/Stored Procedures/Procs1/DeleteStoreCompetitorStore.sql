CREATE PROCEDURE [dbo].[DeleteStoreCompetitorStore] 
	@Store_No int,
	@CompetitorStoreID int
AS
BEGIN

DELETE FROM
	StoreCompetitorStore
WHERE
	@Store_No = Store_No
	AND
	@CompetitorStoreID = CompetitorStoreID

END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[DeleteStoreCompetitorStore] TO [IRMAClientRole]
    AS [dbo];

