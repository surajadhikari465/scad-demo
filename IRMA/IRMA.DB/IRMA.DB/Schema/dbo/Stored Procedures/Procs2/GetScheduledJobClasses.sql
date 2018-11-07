CREATE PROCEDURE dbo.[GetScheduledJobClasses]
AS
BEGIN
	-- Read all of the scheduled job classes that currently exist in the database.
	SELECT Classname, Status, LastRun, ServerName, StatusDescription, Details FROM JobStatus
END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetScheduledJobClasses] TO [IRMAAdminRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetScheduledJobClasses] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetScheduledJobClasses] TO [IRMASchedJobsRole]
    AS [dbo];

