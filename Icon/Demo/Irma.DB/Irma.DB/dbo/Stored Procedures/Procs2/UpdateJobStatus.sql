CREATE PROCEDURE dbo.UpdateJobStatus
	@Classname VARCHAR(50),
    @Status	VARCHAR(50)
AS
BEGIN
    SET NOCOUNT ON
    
    DECLARE @CentralTimeZoneOffset int
	
	SELECT @CentralTimeZoneOffset = CentralTimeZoneOffset FROM Region
	
	-- Update an entry in the job status table for the given classname.
	UPDATE JobStatus SET
		Status = @Status, 
		LastRun = DATEADD(hour, @CentralTimeZoneOffset, GETDATE()),
		ServerName = HOST_NAME()
	WHERE
		Classname = @Classname
    
    SET NOCOUNT OFF
END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[UpdateJobStatus] TO [IRMAAdminRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[UpdateJobStatus] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[UpdateJobStatus] TO [IRMASchedJobsRole]
    AS [dbo];

