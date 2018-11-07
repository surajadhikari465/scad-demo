CREATE PROCEDURE [dbo].[AppLogInsertEntry]
	@logDate datetime
	,@appID [uniqueidentifier]
	,@thread varchar(255)
	,@logLevel varchar(50)
	,@logger varchar(255)
	,@message varchar(4000)
	,@exception varchar(2000)
AS 

BEGIN

	INSERT INTO AppLog ([LogDate],[ApplicationID],[HostName],[UserName],[Thread],[Level],[Logger],[Message],[Exception]) 
		VALUES (@logDate, @appID, HOST_NAME(), SUSER_NAME(), @thread, @logLevel, @logger, @message, @exception)
END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[AppLogInsertEntry] TO [IRMAAdminRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[AppLogInsertEntry] TO [IRMASupportRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[AppLogInsertEntry] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[AppLogInsertEntry] TO [IRMASchedJobsRole]
    AS [dbo];

