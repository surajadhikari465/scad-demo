IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[GetJobStatus]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[GetJobStatus]
GO

CREATE PROCEDURE dbo.GetJobStatus
	@Classname VARCHAR(50)
AS
BEGIN	
    SET NOCOUNT ON
    
    -- Read the current job status for the given classname from the DB status table.
	SELECT Status, LastRun, ServerName FROM JobStatus WHERE Classname=@Classname
	
    SET NOCOUNT OFF
END
GO

 