CREATE PROCEDURE [dbo].[AppLogPurgeHistory]
    @applicationID [uniqueidentifier],
    @daysToKeep int
AS 

BEGIN

	if @daysToKeep > 0
	begin
		delete from applog
		where ApplicationID = @applicationID
		and logdate < getdate() - @daysToKeep
	end

END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[AppLogPurgeHistory] TO [IRMAAdminRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[AppLogPurgeHistory] TO [IRMASupportRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[AppLogPurgeHistory] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[AppLogPurgeHistory] TO [IRMASchedJobsRole]
    AS [dbo];

