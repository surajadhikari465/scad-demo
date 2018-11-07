CREATE PROCEDURE dbo.InsertJobStatus
	@Classname VARCHAR(50),
    @Status	VARCHAR(50)
AS
BEGIN
    SET NOCOUNT ON
	
	-- Create a new entry in the job status table for the given classname.
	INSERT INTO JobStatus (Classname, Status, LastRun, ServerName)
	VALUES (@Classname, @Status, GetDate(), HOST_NAME())
    
    SET NOCOUNT OFF
END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[InsertJobStatus] TO [IRMAAdminRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[InsertJobStatus] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[InsertJobStatus] TO [IRMASchedJobsRole]
    AS [dbo];

