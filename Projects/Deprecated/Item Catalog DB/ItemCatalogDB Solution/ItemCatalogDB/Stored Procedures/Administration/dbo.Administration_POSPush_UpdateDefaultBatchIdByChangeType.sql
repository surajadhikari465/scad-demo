IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[Administration_POSPush_UpdateDefaultBatchIdByChangeType]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
	BEGIN
		DROP  Procedure  [dbo].[Administration_POSPush_UpdateDefaultBatchIdByChangeType]
	END
GO

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