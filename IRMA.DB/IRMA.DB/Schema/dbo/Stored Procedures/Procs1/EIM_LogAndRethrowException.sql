CREATE Procedure dbo.EIM_LogAndRethrowException
	(
		@UploadSession_ID int,
		@UploadRow_ID int,
		@RetryCount int,
		@Item_Key int,
		@Identifier varchar(13),
		@LogText varchar(max)
	)

AS

	-- capture the error data
	DECLARE @ErrorNumber INT,
		@ErrorMessage VARCHAR(8000),
		@Severity INT

	SELECT @ErrorNumber = ERROR_NUMBER()

	SELECT @ErrorMessage =
		@LogText + '| Error Number: ' + CAST(ERROR_NUMBER() AS VARCHAR(100)) + ' | ' +
		'Proc: ' + CAST(ERROR_PROCEDURE() AS VARCHAR(100)) + ' | ' +
		'Line No: ' + CAST(ERROR_LINE() AS VARCHAR(100)) + ' | ' +
		ERROR_MESSAGE()

	-- log the error
	EXEC dbo.EIM_Log
		'ERROR',
		'ERROR',
		@UploadSession_ID,
		@UploadRow_ID,
		@RetryCount,
		@Item_Key,
		@Identifier,
		@ErrorMessage

	-- rethrow the error
	RAISERROR (@ErrorMessage, 16, 1, @ErrorNumber)