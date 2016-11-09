CREATE PROCEDURE app.AddLogEvent (
	@AppName NVARCHAR(128)
	, @UserName NVARCHAR(10)
	, @LogDate DATETIME2(3)
	, @Level NVARCHAR(16)
	, @Source NVARCHAR(255)
	, @Message NVARCHAR(60)
	, @MachineName NVARCHAR(20)
) AS 
INSERT INTO app.AppLog(AppID, UserName, LogDate, Level, Logger, Message, MachineName) 
SELECT AppID, @UserName, COALESCE(@LogDate, GETDATE()), @Level, @Source, @Message, COALESCE(@MachineName, HOST_NAME())
FROM app.App
WHERE AppName = @AppName