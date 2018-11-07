CREATE PROCEDURE dbo.[Administration_POSPush_UpdateDefaultBatchIdByPriceChgType]
	@POSFileWriterKey int,
	@PriceChgTypeID int,
	@POSBatchIdDefault int
AS

BEGIN
	-- Delete the existing value
	DELETE FROM POSWriterPriceChgBatchId 
	WHERE POSFileWriterKey = @POSFileWriterKey AND
		  PriceChgTypeID = @PriceChgTypeID
	
	-- Insert the current value
	IF @POSBatchIdDefault IS NOT NULL
	BEGIN
		INSERT INTO POSWriterPriceChgBatchId
			(POSFileWriterKey, PriceChgTypeID, POSBatchIdDefault)
		VALUES
			(@POSFileWriterKey, @PriceChgTypeID, @POSBatchIdDefault)
	END	
END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[Administration_POSPush_UpdateDefaultBatchIdByPriceChgType] TO [IRMAAdminRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[Administration_POSPush_UpdateDefaultBatchIdByPriceChgType] TO [IRMAClientRole]
    AS [dbo];

