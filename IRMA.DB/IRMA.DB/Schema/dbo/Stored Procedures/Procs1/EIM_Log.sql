CREATE Procedure dbo.EIM_Log
	(
		@LoggingLevel varchar(10),
		@EntryType varchar(10),
		@UploadSession_ID int,
		@UploadRow_ID int,
		@RetryCount int,
		@Item_Key int,
		@Identifier varchar(13),
		@LogText varchar(max)
	)

AS

	IF XACT_STATE() != -1 AND((@LoggingLevel = 'TRACE' AND (@EntryType = 'TRACE' OR @EntryType = 'ERROR'))
		OR (@LoggingLevel = 'ERROR' AND @EntryType = 'ERROR'))
	BEGIN

		INSERT INTO dbo.UploadLog
		(
			EntryType,
			UploadSession_ID,
			UploadRow_ID,
			RetryCount,
			Item_Key,
			Identifier,
			[Timestamp],
			LogText
		)
		Values
		(
			@EntryType,
			@UploadSession_ID,
			@UploadRow_ID,
			@RetryCount,
			@Item_Key,
			@Identifier,
			GETDATE(),
			@LogText
		)

	END