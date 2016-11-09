declare @scriptKey varchar(128)

set @scriptKey = 'PopulateCurrency'

IF(NOT exists(Select * from app.PostDeploymentScriptHistory where ScriptKey = @scriptKey))
BEGIN
	Print 'running script ' + @scriptKey 

	IF NOT EXISTS (SELECT 1 FROM Currency WHERE CurrencyCode = 'USD')
	BEGIN
		INSERT INTO Currency (CurrencyCode, CurrencyDesc)
		VALUES ('USD', 'US Dollar')
	END
	IF NOT EXISTS (SELECT 1 FROM Currency WHERE CurrencyCode = 'CAD')
	BEGIN
		INSERT INTO Currency (CurrencyCode, CurrencyDesc)
		VALUES ('CAD', 'Canadian Dollar')
	END
	IF NOT EXISTS (SELECT 1 FROM Currency WHERE CurrencyCode = 'GBP')
	BEGIN
		INSERT INTO Currency (CurrencyCode, CurrencyDesc)
		VALUES ('GBP', 'Pound Sterling')
	END

	insert into app.PostDeploymentScriptHistory values(@scriptKey, getdate())
END
ELSE
BEGIN
	print 'Skipping script ' + @scriptKey
END
GO
	