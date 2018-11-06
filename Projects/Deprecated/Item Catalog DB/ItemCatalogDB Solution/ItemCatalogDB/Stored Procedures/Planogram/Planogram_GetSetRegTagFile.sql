IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Planogram_GetSetRegTagFile]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[Planogram_GetSetRegTagFile]
GO

CREATE PROCEDURE [dbo].[Planogram_GetSetRegTagFile]
    @ItemList varchar(max),
    @ItemListSeparator char(1),
	@Store_No int ,
	@StartDate DateTime,
	@POSFileWriterKey AS INT
AS
BEGIN
-- DaveStacey - 20080121 - Created this sproc to switch different filewriters into seperate queries
-- For now, using a text compare, but ultimately a new table should be added with master id's for each
DECLARE @POSFileWriterClass varchar(50), @POSFileWriterCode varchar(20)
BEGIN TRY
-- Based on Filewriter selection, channel to the appropriate query

SELECT @POSFileWriterClass = POSFileWriterClass,
       @POSFileWriterCode  = POSFileWriterCode
  FROM dbo.PosWriter WHERE POSFileWriterKey = @POSFileWriterKey

IF @POSFileWriterClass like '%PrintLab%' --PrintLab
	BEGIN
		--PRINT 'PRINTLAB ' + @POSFileWriterCode
		EXEC dbo.Planogram_GetPrintLabSetRegTagFile @ItemList, @ItemListSeparator, @Store_No, @StartDate
	END
ELSE IF  @POSFileWriterClass like '%FileMaker%' --FileMaker
	BEGIN
		--PRINT 'FileMaker ' + @POSFileWriterCode
		EXEC dbo.Planogram_GetFileMakerSetRegTagFile @ItemList, @ItemListSeparator, @Store_No, @StartDate
	END
ELSE IF  @POSFileWriterClass like '%FX%' --FileMaker
	BEGIN
		--PRINT 'FileMaker ' + @POSFileWriterCode
		EXEC dbo.Planogram_GetFXSetRegTagFile @ItemList, @ItemListSeparator, @Store_No, @StartDate
	END
ELSE IF @POSFileWriterClass = 'TagWriter' AND @POSFileWriterCode LIKE '%AccessVia%' -- AccessVia
	BEGIN
		--PRINT 'AccessVia ' + @POSFileWriterCode
		IF @POSFileWriterCode LIKE '%AccessViaExt%' -- AccessViaExt
				BEGIN
					EXEC dbo.Planogram_GetAccessViaExtendedSetRegTagFile @ItemList, @ItemListSeparator, @Store_No, @StartDate
				END
			ELSE
				BEGIN
		            EXEC dbo.Planogram_GetAccessViaSetRegTagFile @ItemList, @ItemListSeparator, @Store_No, @StartDate
				END
	END


	END TRY
	BEGIN CATCH
		DECLARE @ErrorMessage NVARCHAR(4000);
		DECLARE @ErrorSeverity INT;
		DECLARE @ErrorState INT;

		SELECT @ErrorMessage = ERROR_MESSAGE(),
			   @ErrorSeverity = ERROR_SEVERITY(),
			   @ErrorState = ERROR_STATE();

		RAISERROR ('Planogram_GetSetRegTagFile failed with  @ErrorMessage' , -- Message text.
				   @ErrorSeverity, -- Severity.
				   @ErrorState -- State.
				   );
	END CATCH
END

GO
