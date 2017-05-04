IF EXISTS (SELECT * FROM sysobjects WHERE type = 'P' AND name = 'JDASync_Notify')
	BEGIN
		DROP  Procedure  dbo.JDASync_Notify
	END

GO

CREATE Procedure dbo.JDASync_Notify
	(
		@EventKey varchar(100),
		@AdditionalBodyText varchar(2000)
	)

AS

    DECLARE @SyncJDA bit
    
    SELECT @SyncJDA = dbo.fn_InstanceDataValue('SyncJDA', NULL)

	-- only if the instance data flag is set
	If @SyncJDA = 1
	BEGIN

		DECLARE 
			@RecipientEmailAddresses varchar(2000)
			,@EmailSubject varchar(2000)
			,@EmailBody varchar(8000)

		DECLARE @BodyText varchar(8000)

		declare RecipientsCursor cursor for
			SELECT 
				[RecipientEmailAddresses]
				,[EmailSubject]
				,[EmailBody]
			FROM [dbo].[JDA_SyncNotification]
			WHERE EventKey = @EventKey

		OPEN RecipientsCursor
		
		FETCH NEXT FROM RecipientsCursor
			into @RecipientEmailAddresses, @EmailSubject, @EmailBody
			
		IF @@FETCH_STATUS <> 0
		BEGIN
			
			DECLARE @NotificationConfigErrorSubject varchar(500)
			
			SET @NotificationConfigErrorSubject = 'JDA Sync - No Notification Data found for Event: ' + @EventKey
			
			EXEC msdb.dbo.sp_send_dbmail
				@profile_name = 'IRMA JDA Sync',
				@recipients = 'dmarine@athensgroup.com',
				@subject = @NotificationConfigErrorSubject,
				@body = ''
		END
		
		WHILE @@FETCH_STATUS = 0
		BEGIN
		
		SELECT @BodyText = @EmailBody + CHAR(10) + CHAR(10) + IsNull(@AdditionalBodyText, '')

			-- send an email to the recipients for the event key
			-- to be updated
			EXEC msdb.dbo.sp_send_dbmail
				@profile_name = 'IRMA JDA Sync',
				@recipients = @RecipientEmailAddresses,
				@subject = @EmailSubject,
				@body = @BodyText

			FETCH NEXT FROM RecipientsCursor
				into @RecipientEmailAddresses, @EmailSubject, @EmailBody
		END
		
		CLOSE RecipientsCursor
		DEALLOCATE RecipientsCursor

	END
	
GO


