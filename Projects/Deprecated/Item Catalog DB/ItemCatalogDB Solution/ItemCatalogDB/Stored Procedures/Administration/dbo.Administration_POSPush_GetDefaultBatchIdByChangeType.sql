IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[Administration_POSPush_GetDefaultBatchIdByChangeType]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
	BEGIN
		DROP  Procedure  [dbo].[Administration_POSPush_GetDefaultBatchIdByChangeType]
	END
GO

CREATE PROCEDURE dbo.[Administration_POSPush_GetDefaultBatchIdByChangeType]
	@POSFileWriterKey int
AS

BEGIN
	DECLARE @FileWriterType As varchar(10)
	SELECT @FileWriterType = FileWriterType FROM POSWriter WHERE POSFileWriterKey=@POSFileWriterKey
	
	IF @FileWriterType = 'POS' 
	BEGIN
		-- Get the default Change Type values for the writer
		-- the query returns all POSChangeType entries that apply
		-- to POS writers, even if the default batch id value is 
		-- NULL for the writer
		SELECT 
			ChgType.POSChangeTypeKey, ChgType.ChangeTypeDesc,
			Writer.POSFileWriterKey, Writer.POSBatchIdDefault
		FROM POSChangeType ChgType
		LEFT JOIN POSWriterBatchIds Writer
			ON Writer.POSChangeTypeKey = ChgType.POSChangeTypeKey AND
			Writer.POSFileWriterKey = @POSFileWriterKey
		WHERE ChgType.POSChangeTypeKey IN (1, 2, 3, 4, 5, 6)
	END
	ELSE IF @FileWriterType = 'SCALE'
	BEGIN
		-- Get the default Change Type values for the writer
		-- the query returns all POSChangeType entries that apply
		-- to SCALE writers, even if the default batch id value is 
		-- NULL for the writer
		SELECT 
			ChgType.POSChangeTypeKey, ChgType.ChangeTypeDesc,
			Writer.POSFileWriterKey, Writer.POSBatchIdDefault
		FROM POSChangeType ChgType
		LEFT JOIN POSWriterBatchIds Writer
			ON Writer.POSChangeTypeKey = ChgType.POSChangeTypeKey AND
			Writer.POSFileWriterKey = @POSFileWriterKey
		WHERE ChgType.POSChangeTypeKey IN (7, 8, 9, 10, 11)
	END
END
GO



 