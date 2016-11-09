/****** Add the Statuses for Message queueing purposes ******/

DECLARE @AddOrUpdate NVARCHAR(255)
DECLARE @Delete NVARCHAR(255)

SET  @AddOrUpdate = 'AddOrUpdate'
SET	 @Delete = 'Delete'


IF (NOT EXISTS (SELECT [MessageActionName] FROM [Icon].[app].[MessageAction] WHERE [MessageActionName] = @AddOrUpdate))
BEGIN
	INSERT INTO [Icon].[app].[MessageAction] ([MessageActionName])
	VALUES (@AddOrUpdate)
END

IF (NOT EXISTS (SELECT [MessageActionName] FROM [Icon].[app].[MessageAction] WHERE [MessageActionName] = @Delete))
BEGIN
	INSERT INTO [Icon].[app].[MessageAction] ([MessageActionName])
	VALUES (@Delete)
END
