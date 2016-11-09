/****** Add the Statuses for Message queueing purposes ******/

DECLARE @staged NVARCHAR(255)
DECLARE @ready NVARCHAR(255)
DECLARE @sent NVARCHAR(255)
DECLARE @failed NVARCHAR(255)
DECLARE @associated NVARCHAR(255)
DECLARE @consumed NVARCHAR(255)

SET  @staged = 'Staged'
SET  @ready = 'Ready'
SET  @associated = 'Associated'
SET	 @sent = 'Sent'
SET  @failed = 'Failed'
SET  @consumed = 'Consumed'

IF (NOT EXISTS (SELECT [MessageStatusName] FROM [Icon].[app].[MessageStatus] WHERE [MessageStatusName] = @staged))
BEGIN
	INSERT INTO [Icon].[app].[MessageStatus] ([MessageStatusName])
	VALUES (@staged)
END

IF (NOT EXISTS (SELECT [MessageStatusName] FROM [Icon].[app].[MessageStatus] WHERE [MessageStatusName] = @ready))
BEGIN
	INSERT INTO [Icon].[app].[MessageStatus] ([MessageStatusName])
	VALUES (@ready)
END

IF (NOT EXISTS (SELECT [MessageStatusName] FROM [Icon].[app].[MessageStatus] WHERE [MessageStatusName] = @associated))
BEGIN
	INSERT INTO [Icon].[app].[MessageStatus] ([MessageStatusName])
	VALUES (@associated)
END

IF (NOT EXISTS (SELECT [MessageStatusName] FROM [Icon].[app].[MessageStatus] WHERE [MessageStatusName] = @sent))
BEGIN
	INSERT INTO [Icon].[app].[MessageStatus] ([MessageStatusName])
	VALUES (@sent)
END

IF (NOT EXISTS (SELECT [MessageStatusName] FROM [Icon].[app].[MessageStatus] WHERE [MessageStatusName] = @failed))
BEGIN
	INSERT INTO [Icon].[app].[MessageStatus] ([MessageStatusName])
	VALUES (@failed)
END

IF (NOT EXISTS (SELECT [MessageStatusName] FROM [Icon].[app].[MessageStatus] WHERE [MessageStatusName] = @consumed))
BEGIN
	INSERT INTO [Icon].[app].[MessageStatus] ([MessageStatusName])
	VALUES (@consumed)
END