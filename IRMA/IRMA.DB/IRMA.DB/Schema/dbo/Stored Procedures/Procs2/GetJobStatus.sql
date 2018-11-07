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
GRANT EXECUTE
    ON OBJECT::[dbo].[GetJobStatus] TO [IRMAAdminRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetJobStatus] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetJobStatus] TO [IRMASchedJobsRole]
    AS [dbo];

