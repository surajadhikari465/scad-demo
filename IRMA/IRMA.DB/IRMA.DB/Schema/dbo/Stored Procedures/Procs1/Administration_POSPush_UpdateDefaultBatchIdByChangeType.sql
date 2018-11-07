CREATE PROCEDURE dbo.[Administration_POSPush_UpdateDefaultBatchIdByChangeType]
	@POSFileWriterKey int,
	@POSChangeTypeKey int,
	@POSBatchIdDefault int
AS

BEGIN
	-- Delete the existing value
	DELETE FROM POSWriterBatchIds
	WHERE POSFileWriterKey = @POSFileWriterKey AND
		  POSChangeTypeKey = @POSChangeTypeKey
	
	-- Insert the current value
	IF @POSBatchIdDefault IS NOT NULL
	BEGIN
		INSERT INTO POSWriterBatchIds
			(POSFileWriterKey, POSChangeTypeKey, POSBatchIdDefault)
		VALUES
			(@POSFileWriterKey, @POSChangeTypeKey, @POSBatchIdDefault)
	END
		
END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[Administration_POSPush_UpdateDefaultBatchIdByChangeType] TO [IRMAAdminRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[Administration_POSPush_UpdateDefaultBatchIdByChangeType] TO [IRMAClientRole]
    AS [dbo];

