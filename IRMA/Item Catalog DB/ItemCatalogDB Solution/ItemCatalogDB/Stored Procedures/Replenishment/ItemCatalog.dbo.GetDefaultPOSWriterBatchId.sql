IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetDefaultPOSWriterBatchId]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[GetDefaultPOSWriterBatchId]
GO

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
 