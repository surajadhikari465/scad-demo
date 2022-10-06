create procedure [dbo].[WriteAppLog]
	@AppRef       NVARCHAR (128), -- String used to find the application name in the App table, so should be unique enough, but does not need to be exact; is not case-sensitive.
	@Level        NVARCHAR (100),
	@Logger       NVARCHAR (255), -- Top-level object or class for the application
	@LogDate      DATETIME2,
	@Thread       NVARCHAR (100), -- Thread or process identifier.
	@Message      NVARCHAR (max),
	@CallSite     NVARCHAR (max), -- Specific class or object or script needing to log something.
	@StackTrace   NVARCHAR (max)
as
begin
/*
Author: Tom Lux
Date: Sept, 2022

When this was written, it was checked into Azure repo here:
https://dev.azure.com/wholefoods/Supply%20Chain%20Application%20Development/_git/SCAD?path=/OutOfStock/DEV/OOSDatabase/dbo/Stored%20Procedures/WriteAppLog.sql&version=GBmaster

Main tech doc(s) here: https://dev.azure.com/wholefoods/Supply%20Chain%20Application%20Development/_git/SCAD?path=/OutOfStock/DEV/OOSDatabase/_documentation/
*/

	insert into AppLog
	select
		AppID = coalesce((select min(AppID) from App where AppName like ('%' + @AppRef + '%')), dbo.fnGetUnknownAppID()),
		[Level] = @Level,
		Logger = @Logger,
		UserName = suser_name(),
		MachineName = host_name(),
		InsertDate = sysdatetime(),
		LogDate = @LogDate,
		Thread = @Thread,
		[Message] = @Message,
		CallSite = @CallSite,
		StackTrace = @StackTrace
end;
go

