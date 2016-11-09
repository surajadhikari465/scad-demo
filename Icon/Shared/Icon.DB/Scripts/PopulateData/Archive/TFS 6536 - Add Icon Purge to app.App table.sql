DECLARE @appName nvarchar(128) = 'Icon Data Purge'

IF(NOT EXISTS (SELECT * FROM app.App where AppName = @appName))
BEGIN
	INSERT INTO app.App(AppName)
	VALUES (@appName)
END