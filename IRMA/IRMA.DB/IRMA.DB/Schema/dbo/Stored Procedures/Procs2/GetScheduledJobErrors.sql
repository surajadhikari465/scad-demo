CREATE PROCEDURE dbo.[GetScheduledJobErrors]
	@Classname VARCHAR(50)
AS
BEGIN
	-- Return the error log data for the scheduled job, with the most recent error returned first.
	SELECT RunDate, ServerName, ExceptionText FROM JobErrorLog WHERE Classname=@Classname ORDER BY RunDate DESC
END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetScheduledJobErrors] TO [IRMAAdminRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetScheduledJobErrors] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetScheduledJobErrors] TO [IRMASchedJobsRole]
    AS [dbo];

