CREATE PROCEDURE dbo.ResetFailedScheduledJob
	@Classname VARCHAR(50),
    @ValidationCode int OUTPUT
AS

BEGIN
    SET NOCOUNT ON
 	-- Initialize the ValidationCode to SUCCESS
	SET @ValidationCode = 0

   -- Validate that the job is in the FAILED status before allowing the reset
	IF lower(ISNULL((SELECT Status FROM JobStatus (nolock) WHERE Classname=@Classname),'')) not in ('failed', 'running')
	BEGIN
		SET @ValidationCode = 300
	END

    -- Reset the job to COMPLETE, which will allow for it to be executed again
    IF @ValidationCode = 0
    BEGIN
		UPDATE JobStatus SET Status='COMPLETE', StatusDescription = '', Details = '' WHERE Classname=@Classname
    END
     
	SET NOCOUNT OFF
END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[ResetFailedScheduledJob] TO [IRMAAdminRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[ResetFailedScheduledJob] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[ResetFailedScheduledJob] TO [IRMASchedJobsRole]
    AS [dbo];

