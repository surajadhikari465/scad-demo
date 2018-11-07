CREATE PROCEDURE dbo.GetBatchesInSentState
	@Item_Key int
AS

BEGIN
	SELECT DISTINCT pbh.PriceBatchHeaderID, pbh.BatchDescription, pbd.StartDate, pbd.Store_No
	FROM ItemIdentifier ii
	JOIN PriceBatchDetail pbd ON pbd.Item_Key = ii.Item_Key
	JOIN PriceBatchHeader pbh ON pbh.PriceBatchHeaderId = pbd.PriceBatchHeaderId
	WHERE (pbh.PriceBatchStatusId > 2 AND pbh.PriceBatchStatusId < 6) AND ii.Item_key = @Item_Key
	ORDER BY Store_No
END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetBatchesInSentState] TO [IRMAClientRole]
    AS [dbo];

