 IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[Administration_POSPush_UpdateDefaultBatchIdByPriceChgType]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
	BEGIN
		DROP  Procedure  [dbo].[Administration_POSPush_UpdateDefaultBatchIdByPriceChgType]
	END
GO

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