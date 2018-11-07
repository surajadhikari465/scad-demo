CREATE PROCEDURE [dbo].[UpdateStoreCompetitorStore] 
	@Store_No int,
	@CompetitorStoreID int,
	@Priority tinyint
AS
BEGIN

UPDATE StoreCompetitorStore
SET
	Priority = @Priority
WHERE
	@Store_No = Store_No
	AND
	@CompetitorStoreID = CompetitorStoreID

END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[UpdateStoreCompetitorStore] TO [IRMAClientRole]
    AS [dbo];

