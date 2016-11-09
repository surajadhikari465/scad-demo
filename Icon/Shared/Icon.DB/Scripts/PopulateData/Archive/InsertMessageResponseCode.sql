DECLARE @ok NVARCHAR(255)
DECLARE @rejected NVARCHAR(255)
DECLARE @partial NVARCHAR(255)

SET @ok = 'OK'
SET @rejected = 'Rejected'
SET @partial = 'Partial'

IF (NOT EXISTS (SELECT [MessageResponseCodeName] FROM [Icon].[app].[MessageResponseCode] WHERE [MessageResponseCodeName] = @ok))
BEGIN
	INSERT INTO [Icon].[app].[MessageResponseCode] ([MessageResponseCodeName])
	VALUES (@ok)
END

IF (NOT EXISTS (SELECT [MessageResponseCodeName] FROM [Icon].[app].[MessageResponseCode] WHERE [MessageResponseCodeName] = @rejected))
BEGIN
	INSERT INTO [Icon].[app].[MessageResponseCode] ([MessageResponseCodeName])
	VALUES (@rejected)
END

IF (NOT EXISTS (SELECT [MessageResponseCodeName] FROM [Icon].[app].[MessageResponseCode] WHERE [MessageResponseCodeName] = @partial))
BEGIN
	INSERT INTO [Icon].[app].[MessageResponseCode] ([MessageResponseCodeName])
	VALUES (@partial)
END