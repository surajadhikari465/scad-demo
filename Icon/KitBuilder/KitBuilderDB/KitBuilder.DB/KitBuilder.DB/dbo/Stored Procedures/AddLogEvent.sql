create proc [dbo].[AddLogEvent] (
	@AppName nvarchar(128) = null,
	@UserName nvarchar(255),
	@LogDate datetime2(3) = null,
	@Level nvarchar(100),
	@Source nvarchar(255),
	@Message nvarchar(max),
	@MachineName nvarchar(255) = null)
as

insert into AppLog(AppID, UserName, LogDate, Level, Logger, Message, MachineName) 
  values(1, @UserName, IsNull(@LogDate, GetDate()), @Level, @Source, @Message, IsNull(@MachineName, HOST_NAME()));