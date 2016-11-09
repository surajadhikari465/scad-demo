--===============================================================
-- Date:		10/27/2014
-- Description: Adds the controller names to the app.App table
--===============================================================

DECLARE @PushController NVARCHAR(255);
DECLARE @GlobalController NVARCHAR(255);
DECLARE @RegionalController NVARCHAR(255);

SET  @PushController = 'POS Push Controller';
SET  @GlobalController = 'Global Controller';
SET  @RegionalController = 'Regional Controller';

IF (NOT EXISTS (SELECT [AppName] FROM [Icon].[app].[App] WHERE [AppName] = @PushController))
BEGIN
	INSERT INTO [Icon].[app].[App] ([AppName])
	VALUES (@PushController)
END

IF (NOT EXISTS (SELECT [AppName] FROM [Icon].[app].[App] WHERE [AppName] = @GlobalController))
BEGIN
	INSERT INTO [Icon].[app].[App] ([AppName])
	VALUES (@GlobalController)
END

IF (NOT EXISTS (SELECT [AppName] FROM [Icon].[app].[App] WHERE [AppName] = @RegionalController))
BEGIN
	INSERT INTO [Icon].[app].[App] ([AppName])
	VALUES (@RegionalController)
END