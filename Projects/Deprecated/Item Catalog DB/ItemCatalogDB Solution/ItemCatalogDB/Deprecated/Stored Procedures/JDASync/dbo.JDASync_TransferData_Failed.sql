 IF EXISTS (SELECT * FROM sysobjects WHERE type = 'P' AND name = 'JDASync_TransferData_Failed')
	BEGIN
		DROP  Procedure  dbo.JDASync_TransferData_Failed
	END

GO

CREATE Procedure dbo.JDASync_TransferData_Failed
AS
	-- ======================================================
	-- Purpose: This procedure logs and sends email notifications
	-- if the scheduled job running dbo.JDASync_TransferData fails.
	--
	-- Author: David Marine; dmarine@athensgroup.com
	-- Created: 05.07.07
	-- ======================================================


		DECLARE @ErrorMessage varchar(4000)
		
		SELECT @ErrorMessage = 'The JDA Sync Job failed to be executed. ' +
			'The error is also logged in the JDA_SyncJobLog table.'

	
		INSERT INTO dbo.JDA_SyncJobLog 
		(
			JobName,
			IsFailed,
			RunDate,
			ErrorMessage
		)
		VALUES
		(
			'SYNC_JOB',
			1,
			GetDate(),
			@ErrorMessage
		)
		
		
	EXEC dbo.JDASync_Notify
		@EventKey = 'SYNC_JOB_FAILED',
		@AdditionalBodyText = @ErrorMessage
		
	GO