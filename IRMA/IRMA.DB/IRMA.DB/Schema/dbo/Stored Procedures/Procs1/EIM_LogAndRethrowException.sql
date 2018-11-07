CREATE Procedure [dbo].[EIM_LogAndRethrowException]
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

	declare @str as varchar(max), @errorNum as varchar(100), @errorProc as varchar(100), @errorLineNo as varchar(100), @errorMsg as varchar(max)
	select @errorNum = case when ERROR_NUMBER() is Null then 'NULL'
	            else CAST(ERROR_NUMBER() AS VARCHAR(100))
			End

	select @errorProc = case when ERROR_PROCEDURE() is Null then 'NULL'
	            else CAST(ERROR_PROCEDURE() AS VARCHAR(100))
			End

	select @errorLineNo = case when ERROR_LINE() is Null then 'NULL'
	            else CAST(ERROR_LINE() AS VARCHAR(100))
			End

	select @errorMsg = case when ERROR_MESSAGE() is Null then 'NULL'
	            else ERROR_MESSAGE()
			End

	SELECT @ErrorMessage =
		@LogText + '| Error Number: ' + @errorNum + ' | ' +
		'Proc: ' + @errorProc + ' | ' +
		'Line No: ' + @errorLineNo + ' | ' +
		@errorMsg

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