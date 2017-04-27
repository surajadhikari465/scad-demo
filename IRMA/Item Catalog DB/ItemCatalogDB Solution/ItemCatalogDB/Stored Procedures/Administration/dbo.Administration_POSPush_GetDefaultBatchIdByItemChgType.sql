IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[Administration_POSPush_GetDefaultBatchIdByItemChgType]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
	BEGIN
		DROP  Procedure  [dbo].[Administration_POSPush_GetDefaultBatchIdByItemChgType]
	END
GO

CREATE PROCEDURE dbo.[Administration_POSPush_GetDefaultBatchIdByItemChgType]
		@POSFileWriterKey int
AS

-- Get the default Item Change Type values for the writer
-- the query returns all ItemChgType entries, except for "All", even if 
-- the default batch id value is NULL for the writer
BEGIN
	SELECT 
		ChgType.ItemChgTypeId, ChgType.ItemChgTypeDesc,
		Writer.POSFileWriterKey, Writer.POSBatchIdDefault 
	FROM ItemChgType ChgType
	LEFT JOIN POSWriterItemChgBatchId Writer 
		ON Writer.ItemChgTypeID = ChgType.ItemChgTypeID AND
		Writer.POSFileWriterKey = @POSFileWriterKey
	WHERE ChgType.ItemChgTypeDesc <> 'All'
END
GO



 