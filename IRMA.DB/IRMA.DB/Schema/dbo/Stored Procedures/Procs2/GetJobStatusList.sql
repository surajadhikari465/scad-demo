CREATE PROCEDURE [dbo].[GetJobStatusList]
AS
BEGIN	
    SET NOCOUNT ON
    

	SELECT Classname as 'job name', Status, LastRun, ServerName, StatusDescription, Details FROM JobStatus
	
    SET NOCOUNT OFF
END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetJobStatusList] TO [IRMAAdminRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetJobStatusList] TO [IRMAClientRole]
    AS [dbo];

