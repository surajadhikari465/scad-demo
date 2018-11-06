if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[GetJobStatusList]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[GetJobStatusList]
GO

CREATE PROCEDURE [dbo].[GetJobStatusList]
AS
BEGIN	
    SET NOCOUNT ON
    

	SELECT Classname as 'job name', Status, LastRun, ServerName, StatusDescription, Details FROM JobStatus
	
    SET NOCOUNT OFF
END

GO

