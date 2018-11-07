CREATE PROCEDURE dbo.[Administration_POSPush_UpdateDefaultBatchIdByItemChgType]
	@POSFileWriterKey int,
	@ItemChgTypeID int,
	@POSBatchIdDefault int
AS

BEGIN
	-- Delete the existing value
	DELETE FROM POSWriterItemChgBatchId 
	WHERE POSFileWriterKey = @POSFileWriterKey AND
		  ItemChgTypeID = @ItemChgTypeID
	
	-- Insert the current value
	IF @POSBatchIdDefault IS NOT NULL
	BEGIN
		INSERT INTO POSWriterItemChgBatchId
			(POSFileWriterKey, ItemChgTypeID, POSBatchIdDefault)
		VALUES
			(@POSFileWriterKey, @ItemChgTypeID, @POSBatchIdDefault)
	END
		
END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[Administration_POSPush_UpdateDefaultBatchIdByItemChgType] TO [IRMAAdminRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[Administration_POSPush_UpdateDefaultBatchIdByItemChgType] TO [IRMAClientRole]
    AS [dbo];

