/****** Add applications to the App table ******/

DECLARE @WebApp NVARCHAR(255);
DECLARE @InterfaceController NVARCHAR(255);
DECLARE @EsbSubscriber NVARCHAR(255);
DECLARE @IconService NVARCHAR(255);
DECLARE @ApiController NVARCHAR(255);
DECLARE @PushController NVARCHAR(255);
DECLARE @GlobalController NVARCHAR(255);
DECLARE @RegionalController NVARCHAR(255);

SET  @WebApp = 'Web App';
SET  @InterfaceController = 'Interface Controller';
SET  @EsbSubscriber = 'ESB Subscriber';
SET  @IconService = 'Icon Service';
SET  @ApiController = 'API Controller';
SET  @PushController = 'POS Push Controller';
SET  @GlobalController = 'Global Controller';
SET  @RegionalController = 'Regional Controller';

IF (NOT EXISTS (SELECT [AppName] FROM [Icon].[app].[App] WHERE [AppName] = @WebApp))
BEGIN
	INSERT INTO [Icon].[app].[App] ([AppName])
	VALUES (@WebApp)
END

IF (NOT EXISTS (SELECT [AppName] FROM [Icon].[app].[App] WHERE [AppName] = @InterfaceController))
BEGIN
	INSERT INTO [Icon].[app].[App] ([AppName])
	VALUES (@InterfaceController)
END

IF (NOT EXISTS (SELECT [AppName] FROM [Icon].[app].[App] WHERE [AppName] = @EsbSubscriber))
BEGIN
	INSERT INTO [Icon].[app].[App] ([AppName])
	VALUES (@EsbSubscriber)
END

IF (NOT EXISTS (SELECT [AppName] FROM [Icon].[app].[App] WHERE [AppName] = @IconService))
BEGIN
	INSERT INTO [Icon].[app].[App] ([AppName])
	VALUES (@IconService)
END

IF (NOT EXISTS (SELECT [AppName] FROM [Icon].[app].[App] WHERE [AppName] = @ApiController))
BEGIN
	INSERT INTO [Icon].[app].[App] ([AppName])
	VALUES (@ApiController)
END

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