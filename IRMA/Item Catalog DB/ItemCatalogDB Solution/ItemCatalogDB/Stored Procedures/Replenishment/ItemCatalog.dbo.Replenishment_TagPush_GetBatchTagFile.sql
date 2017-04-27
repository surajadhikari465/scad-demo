
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Replenishment_TagPush_GetBatchTagFile]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[Replenishment_TagPush_GetBatchTagFile]
GO

CREATE PROCEDURE [dbo].[Replenishment_TagPush_GetBatchTagFile]
    @ItemList varchar(max),
    @ItemListSeparator char(1),
    @PriceBatchHeaderID int,
	@StartLabelPosition AS INT,
	@POSFileWriterKey AS INT
AS
BEGIN
-- MD - 2/11/2011 - Added logic to call the "No Tag" logic Extended stored procedure for AccessViaExt
-- Tom Lux - 20080818 - Fixed @ErrorMessage in catch to actually concatenate the error.
DECLARE
	@POSFileWriterCode varchar(20),
	@POSFileWriterClass varchar(100)

BEGIN TRY
-- Based on Filewriter selection, channel to the appropriate query
	SELECT 
		@POSFileWriterCode = POSFileWriterCode,
		@POSFileWriterClass = POSFileWriterClass
	FROM dbo.PosWriter
	WHERE POSFileWriterKey = @POSFileWriterKey

IF @POSFileWriterClass like '%PrintLab%' --PrintLab
	BEGIN
		--PRINT 'PRINTLAB ' + @POSFileWriterCode
		EXEC dbo.Replenishment_TagPush_GetPrintLabBatchTagFile @ItemList, @ItemListSeparator, @PriceBatchHeaderID, @StartLabelPosition
	END
ELSE IF  @POSFileWriterClass like '%FileMaker%' --FileMaker
	BEGIN
		--PRINT 'FileMaker ' + @POSFileWriterCode
		EXEC dbo.Replenishment_TagPush_GetFileMakerBatchTagFile @ItemList, @ItemListSeparator, @PriceBatchHeaderID, @StartLabelPosition
	END
ELSE IF  @POSFileWriterClass like '%FX%' --FileMaker
	BEGIN
		--PRINT 'FileMaker ' + @POSFileWriterCode
		EXEC dbo.Replenishment_TagPush_GetFXBatchTagFile @ItemList, @ItemListSeparator, @PriceBatchHeaderID, @StartLabelPosition
	END
	ELSE IF @POSFileWriterClass = 'TagWriter' AND @POSFileWriterCode LIKE '%AccessVia%' -- AccessVia
		BEGIN
		--PRINT 'ACCESSVIA ' + @POSFileWriterCode
			IF @POSFileWriterCode LIKE '%AccessViaExt%' -- AccessViaExt
				BEGIN
					EXEC dbo.Replenishment_TagPush_GetAccessViaExtendedBatchTagFile @ItemList, @ItemListSeparator, @PriceBatchHeaderID, @StartLabelPosition
				END
			ELSE
				BEGIN
					EXEC dbo.Replenishment_TagPush_GetAccessViaBatchTagFile @ItemList, @ItemListSeparator, @PriceBatchHeaderID, @StartLabelPosition
				END
		END
	END TRY
	BEGIN CATCH
		DECLARE @ErrorMessage NVARCHAR(4000);
		DECLARE @ErrorSeverity INT;
		DECLARE @ErrorState INT;

		SELECT @ErrorMessage = 'Replenishment_TagPush_GetBatchTagFile failed with error: ' + ERROR_MESSAGE(),
			   @ErrorSeverity = ERROR_SEVERITY(),
			   @ErrorState = ERROR_STATE();

		RAISERROR (@ErrorMessage, -- Message text.
				   @ErrorSeverity, -- Severity.
				   @ErrorState -- State.
				   );
	END CATCH
END

GO