DECLARE @SubteamController NVARCHAR(255);
SET  @SubteamController = 'Subteam Controller';

IF (NOT EXISTS (SELECT [AppName] FROM [Icon].[app].[App] WHERE [AppName] = @SubteamController))
BEGIN
	INSERT INTO [Icon].[app].[App] ([AppName])
	VALUES (@SubteamController)
END