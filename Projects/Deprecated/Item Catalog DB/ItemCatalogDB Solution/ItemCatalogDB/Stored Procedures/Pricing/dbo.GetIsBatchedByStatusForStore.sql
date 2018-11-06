IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'dbo.GetIsBatchedByStatusForStore') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE dbo.GetIsBatchedByStatusForStore
GO

CREATE PROCEDURE dbo.GetIsBatchedByStatusForStore
    @StoreList varchar(8000),
    @StoreListSeparator char(1),
    @BatchStatus varchar(20),
    @IsExistingUnprocessedBatch bit OUTPUT
AS

BEGIN
	-- This returns true if there is an existing batch found for the passed in @BatchStatus
	-- for a particular store.  The stored procedure GetIsBatchedByStatus can be used to look for
	-- a particular item.
	-- NOTE: @BatchStatus can be passed in a list of statuses; [ie: 2,3,4,5 (all but Building and Processed)]
    SET NOCOUNT ON

	DECLARE @IsBatched bit

	SELECT @IsBatched = CASE WHEN EXISTS (SELECT 1 
											FROM PriceBatchDetail PBD
											LEFT JOIN PriceBatchHeader PBH
												ON PBD.PriceBatchHeaderId = PBH.PriceBatchHeaderId
											INNER JOIN
												fn_Parse_List(@StoreList, @StoreListSeparator) Store
												ON Store.Key_Value = PBD.Store_No
											WHERE PBH.PriceBatchStatusId IN (SELECT Key_value FROM dbo.fn_Parse_List(@BatchStatus, ','))	
												AND Expired = 0) THEN 1 
							ELSE 0 END

	--SET OUTPUT PARAM AS WELL AS RETURN VALUE
	SET @IsExistingUnprocessedBatch = @IsBatched

	SELECT IsBatched = @IsBatched 

    SET NOCOUNT OFF
END

GO



 