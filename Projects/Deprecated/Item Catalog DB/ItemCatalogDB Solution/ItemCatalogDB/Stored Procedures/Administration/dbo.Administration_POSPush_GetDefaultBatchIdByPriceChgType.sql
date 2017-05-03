IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[Administration_POSPush_GetDefaultBatchIdByPriceChgType]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
	BEGIN
		DROP  Procedure  [dbo].[Administration_POSPush_GetDefaultBatchIdByPriceChgType]
	END
GO

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



 