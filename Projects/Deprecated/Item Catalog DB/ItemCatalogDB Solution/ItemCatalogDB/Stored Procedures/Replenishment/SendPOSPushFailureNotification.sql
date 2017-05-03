USE [ItemCatalog_Test]
GO
/****** Object:  StoredProcedure [dbo].[SendAutoPushFailureNotification]    Script Date: 11/3/2015 10:09:50 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

ALTER PROCEDURE [dbo].[SendPOSPushFailureNotification]

AS 

DECLARE @MessageBody		VARCHAR(MAX) = ''
DECLARE @SubjectLine		VARCHAR(MAX) = ''
DECLARE @PushMonitoring		BIT
DECLARE @PushDate			DATETIME
DECLARE @SentStatus			TINYINT = 5 -- Batch in Sent Status
DECLARE @StagingTableCount	INT = 0
DECLARE @PushStatus			VARCHAR(25)
DECLARE @PendingBatches		INT
DECLARE @Recipients			VARCHAR(100)
DECLARE @Region				VARCHAR(2)
DECLARE @Environment		VARCHAR(4)
DECLARE @IRMASupportEmail	VARCHAR(30) = 'IRMA.Support@wholefoods.com;IRMA.On-Call@wholefoods.com'

SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED 

SELECT @PushMonitoring = acv.Value
		FROM AppConfigValue acv INNER JOIN AppConfigEnv ace
		ON acv.EnvironmentID = ace.EnvironmentID 
		INNER JOIN AppConfigApp aca 
		ON acv.ApplicationID = aca.ApplicationID 
		INNER JOIN AppConfigKey ack
		ON acv.KeyID = ack.KeyID 
		WHERE aca.Name = 'POS PUSH JOB' AND
		ack.Name = 'PushMonitoring' and
		SUBSTRING(ace.Name,1,1) = SUBSTRING((SELECT Environment FROM Version WHERE ApplicationName = 'IRMA CLIENT'),1,1)

IF @PushMonitoring = 1 
	BEGIN
		SET @PushDate = (SELECT CONVERT(DATE,GETDATE()))
		SET @Region = (SELECT RegionCode FROM Region (NOLOCK))
		SET @Environment = (SELECT Environment FROM Version v (NOLOCK) WHERE v.ApplicationName = 'IRMA Client')
		SET @SubjectLine = 'IRMA ' + RTRIM(@Environment) + ' - POS Push Failure Notification'
		
		IF EXISTS (SELECT * FROM JobStatus (NOLOCK) WHERE ClassName = 'POSPushJob' AND [Status] = 'COMPLETE' AND StatusDescription LIKE '%COMPLETED SUCCESSFULLY%') 
			SET @PushStatus = 'COMPLETED'
		ELSE 
			SET @PushStatus = 'RUNNING OR FAILED'

		IF @PushStatus = 'COMPLETED'
			BEGIN
				SELECT 
				@PendingBatches = COUNT(*) 
				FROM PriceBatchHeader pbh (NOLOCK)
				WHERE PriceBatchStatusID = @SentStatus AND
				StartDate = @PushDate

				SET @StagingTableCount = (SELECT COUNT(*) FROM IConPOSPushStaging (NOLOCK))

				SELECT @Recipients = acv.Value
					FROM AppConfigValue acv INNER JOIN AppConfigEnv ace
					ON acv.EnvironmentID = ace.EnvironmentID 
					INNER JOIN AppConfigApp aca
					ON acv.ApplicationID = aca.ApplicationID 
					INNER JOIN AppConfigKey ack
					ON acv.KeyID = ack.KeyID 
					WHERE aca.Name = 'POS PUSH JOB' AND
					ack.Name = 'primaryErrorNotification' and
					SUBSTRING(ace.Name,1,1) = SUBSTRING((SELECT Environment FROM Version WHERE ApplicationName = 'IRMA CLIENT'),1,1)

				SET @MessageBody =	'Hi team, ' + CHAR(13) + CHAR(13) + 'POS Push failure has been detected.' + CHAR(13)

				IF @PendingBatches > 0 OR @StagingTableCount > 0 
					BEGIN
						-- SEND EMAIL			

						IF @PendingBatches > 0
							BEGIN
								IF @PendingBatches = 1
									SET @MessageBody = @MessageBody + '1. There is 1 unprocessed batch. ' + CHAR(13)
								ELSE
									SET @MessageBody = @MessageBody + '1. There are ' + RTRIM(CONVERT(VARCHAR(4), @PendingBatches)) + ' unprocessed batches. ' + CHAR(13)
							END
				
						IF @StagingTableCount > 0
							SET @MessageBody = @MessageBody + '2. There is data in the IconPOSPushStaging table.' + CHAR(13) 

						SET @MessageBody = @MessageBody + CHAR(13) + 'Additional information can be found in the debug query: http://vcmappprd.wfm.pvt:8080/debugtools/bin/debug_report.cgi?reportname=IrmaBatchesInSentStatusCurrentDate&action=query&rgn='+@Region+'&servertype='+@Environment+'&Run=Query'
				
				
						EXEC msdb.dbo.sp_send_dbmail @recipients = @Recipients, @from_address = @IRMASupportEmail, @body = @MessageBody, @importance = 'High', @copy_recipients = @IRMASupportEmail, @reply_to = @IRMASupportEmail, @subject = @SubjectLine 
					END
			END

	END
