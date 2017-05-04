IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[Administration_POSPush_UpdateDefaultBatchIdByItemChgType]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
	BEGIN
		DROP  Procedure  [dbo].[Administration_POSPush_UpdateDefaultBatchIdByItemChgType]
	END
GO

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