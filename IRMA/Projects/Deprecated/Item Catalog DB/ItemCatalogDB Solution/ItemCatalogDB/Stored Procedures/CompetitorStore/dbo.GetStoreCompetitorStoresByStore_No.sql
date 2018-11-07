if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[GetStoreCompetitorStoresByStore_No]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[GetStoreCompetitorStoresByStore_No]
GO

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
go   