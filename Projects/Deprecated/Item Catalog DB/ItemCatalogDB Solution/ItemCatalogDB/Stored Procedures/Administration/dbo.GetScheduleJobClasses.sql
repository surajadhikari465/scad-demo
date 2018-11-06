IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[GetScheduledJobClasses]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
	BEGIN
		DROP  Procedure  [dbo].[GetScheduledJobClasses]
	END
GO

CREATE PROCEDURE dbo.[GetScheduledJobClasses]
AS
BEGIN
	-- Read all of the scheduled job classes that currently exist in the database.
	SELECT Classname, Status, LastRun, ServerName, StatusDescription, Details FROM JobStatus
END 
GO