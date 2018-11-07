CREATE PROCEDURE dbo.[Administration_POSPush_GetDefaultBatchIdByItemChgType]
		@POSFileWriterKey int
AS

-- Get the default Item Change Type values for the writer
-- the query returns all ItemChgType entries, except for "All", even if 
-- the default batch id value is NULL for the writer
BEGIN
	SELECT 
		ChgType.ItemChgTypeId, ChgType.ItemChgTypeDesc,
		Writer.POSFileWriterKey, Writer.POSBatchIdDefault 
	FROM ItemChgType ChgType
	LEFT JOIN POSWriterItemChgBatchId Writer 
		ON Writer.ItemChgTypeID = ChgType.ItemChgTypeID AND
		Writer.POSFileWriterKey = @POSFileWriterKey
	WHERE ChgType.ItemChgTypeDesc <> 'All'
END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[Administration_POSPush_GetDefaultBatchIdByItemChgType] TO [IRMAAdminRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[Administration_POSPush_GetDefaultBatchIdByItemChgType] TO [IRMAClientRole]
    AS [dbo];

