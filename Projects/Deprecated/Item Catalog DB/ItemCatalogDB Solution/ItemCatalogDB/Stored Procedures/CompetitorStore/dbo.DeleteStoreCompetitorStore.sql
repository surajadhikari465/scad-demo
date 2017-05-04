if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[DeleteStoreCompetitorStore]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[DeleteStoreCompetitorStore]
GO

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
go    