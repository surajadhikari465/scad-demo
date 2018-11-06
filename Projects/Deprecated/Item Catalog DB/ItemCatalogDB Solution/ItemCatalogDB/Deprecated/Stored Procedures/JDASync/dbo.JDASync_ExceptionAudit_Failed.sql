  IF EXISTS (SELECT * FROM sysobjects WHERE type = 'P' AND name = 'JDASync_ExceptionAudit_Failed')
	BEGIN
		DROP  Procedure  dbo.JDASync_ExceptionAudit_Failed
	END

GO

CREATE Procedure dbo.JDASync_ExceptionAudit_Failed
AS
	-- ======================================================
	-- Purpose: This procedure logs and sends email notifications
	-- if the scheduled job running dbo.JDASync_TransferData fails.
	--
	-- Author: David Marine; dmarine@athensgroup.com
	-- Created: 05.07.07
	-- ======================================================		
		
	EXEC dbo.JDASync_Notify
		@EventKey = 'SYNC_AUDIT_JOB_FAILED',
		@AdditionalBodyText = NULL
		
	GO