CREATE PROCEDURE dbo.[GetDefaultPOSWriterBatchId] 
	@POSFileWriterKey as int,
    @POSChangeTypeKey as int
AS 
BEGIN
	-- Read the default POS Batch Id assigned to the change type for this POS Writer
	SELECT POSBatchIdDefault FROM POSWriterBatchIds
	WHERE POSFileWriterKey = @POSFileWriterKey AND
		  POSChangeTypeKey = @POSChangeTypeKey
END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetDefaultPOSWriterBatchId] TO [IRMAAdminRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetDefaultPOSWriterBatchId] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetDefaultPOSWriterBatchId] TO [IRMASchedJobsRole]
    AS [dbo];

