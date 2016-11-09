DECLARE @tlogControllerName nvarchar(128) = 'TLog Controller'

IF(NOT EXISTS (SELECT * FROM app.App where AppName = @tlogControllerName))
BEGIN
	INSERT INTO app.App(AppName)
	VALUES (@tlogControllerName)
END