 if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[UpdateStoreCompetitorStore]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[UpdateStoreCompetitorStore]
GO

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
go    