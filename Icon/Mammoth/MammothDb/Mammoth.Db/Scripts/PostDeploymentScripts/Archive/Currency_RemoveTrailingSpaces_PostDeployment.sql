/* Script to seed some of the other scripts so they don't run */


declare @scriptKey varchar(128)

set @scriptKey = 'Currency_RemoveTrailingSpaces_PostDeployment'

IF(NOT exists(Select * from app.PostDeploymentScriptHistory where ScriptKey = @scriptKey))
BEGIN
	Print 'running script ' + @scriptKey
	UPDATE Currency
	SET CurrencyCode = RTRIM(CurrencyCode),
	CurrencyDesc = RTRIM(CurrencyDesc)
END
ELSE
BEGIN
	print 'Skipping script ' + @scriptKey
END
GO