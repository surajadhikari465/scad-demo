CREATE PROCEDURE [dbo].[InsertStoreCompetitorStore] 
	@Store_No int,
	@CompetitorStoreID int,
	@Priority tinyint
AS
BEGIN

INSERT INTO StoreCompetitorStore
	(Store_No,
	CompetitorStoreID,
	Priority)
	VALUES
	(@Store_No,
	@CompetitorStoreID,
	@Priority)

END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[InsertStoreCompetitorStore] TO [IRMAClientRole]
    AS [dbo];

