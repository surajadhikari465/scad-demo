 if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[InsertStoreCompetitorStore]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[InsertStoreCompetitorStore]
GO

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
go   