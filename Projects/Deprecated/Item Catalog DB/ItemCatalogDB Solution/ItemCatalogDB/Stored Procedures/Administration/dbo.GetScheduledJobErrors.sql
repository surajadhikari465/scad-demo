IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[GetScheduledJobErrors]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
	BEGIN
		DROP  Procedure  [dbo].[GetScheduledJobErrors]
	END
GO

CREATE PROCEDURE dbo.[GetScheduledJobErrors]
	@Classname VARCHAR(50)
AS
BEGIN
	-- Return the error log data for the scheduled job, with the most recent error returned first.
	SELECT RunDate, ServerName, ExceptionText FROM JobErrorLog WHERE Classname=@Classname ORDER BY RunDate DESC
END  
GO