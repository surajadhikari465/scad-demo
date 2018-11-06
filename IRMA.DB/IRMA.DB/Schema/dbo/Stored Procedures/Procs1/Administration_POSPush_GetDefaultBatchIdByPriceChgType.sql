CREATE PROCEDURE dbo.[Administration_POSPush_GetDefaultBatchIdByPriceChgType]
		@POSFileWriterKey int
AS

-- Get the default Price Change Type values for the writer
-- the query returns all PriceChgType entries, even if 
-- the default batch id value is NULL for the writer
BEGIN
	SELECT 
		ChgType.PriceChgTypeId, ChgType.PriceChgTypeDesc,
		Writer.POSFileWriterKey, Writer.POSBatchIdDefault 
	FROM PriceChgType ChgType
	LEFT JOIN POSWriterPriceChgBatchId Writer 
		ON Writer.PriceChgTypeID = ChgType.PriceChgTypeID AND
		Writer.POSFileWriterKey = @POSFileWriterKey
END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[Administration_POSPush_GetDefaultBatchIdByPriceChgType] TO [IRMAAdminRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[Administration_POSPush_GetDefaultBatchIdByPriceChgType] TO [IRMAClientRole]
    AS [dbo];

