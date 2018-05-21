--PBI 26341: Add CurrencyCode Trait
DECLARE @scriptKey VARCHAR(128) = 'AddCurrencyCodeTrait'

IF(NOT EXISTS(SELECT 1 FROM app.PostDeploymentScriptHistory WHERE ScriptKey = @scriptKey))
BEGIN
	print '[' + convert(nvarchar, getdate(), 121) + '] ' + @scriptKey;	
	SET IDENTITY_INSERT dbo.Trait ON;

	--regular expressions for validation (trait patterns)
	DECLARE @patternCurrency nvarchar(255) = N'^[C][A][D]$|^[G][B][P]$|^[U][S][D]$';

	IF NOT EXISTS (SELECT 1 FROM dbo.Trait WHERE traitCode= 'CUR')
	INSERT dbo.Trait (traitID, traitGroupID, traitCode, traitDesc, traitPattern) 
			VALUES  (167, 1, N'CUR', N'Currency Code', @patternCurrency);

	INSERT INTO app.PostDeploymentScriptHistory (ScriptKey, RunTime) VALUES (@scriptKey, GETDATE())
	SET IDENTITY_INSERT dbo.Trait OFF

	--populate data in dbo.CurrencyType if it is not there	
	UPDATE dbo.CurrencyType 
		SET issuingEntity= 'UNITED STATES OF AMERICA (THE)', numericCode=840, minorUnit=2, symbol='$' 
		WHERE CurrencyTypeCode='USD'
	
	IF NOT EXISTS (SELECT 1 FROM dbo.CurrencyType WHERE CurrencyTypeCode= 'CAD')
		INSERT INTO dbo.CurrencyType
			   (currencyTypeCode, currencyTypeDesc, issuingEntity, numericCode, minorUnit, symbol)
		 VALUES
			   ('CAD', 'Canadian Dollar', 'CANADA', 124, 2, '$')

	IF NOT EXISTS (SELECT 1 FROM dbo.CurrencyType WHERE CurrencyTypeCode= 'GBP')
		INSERT INTO dbo.CurrencyType
			   (currencyTypeCode, currencyTypeDesc, issuingEntity, numericCode, minorUnit, symbol)
		 VALUES
			   ('GBP', 'Pound Sterling', 'UNITED KINGDOM OF GREAT BRITAIN AND NORTHERN IRELAND (THE)', 826, 2, '£')
		   
END
ELSE
BEGIN
	print '[' + convert(nvarchar, getdate(), 121) + '] ' + 'Pop-data already applied: ' + @scriptKey
END
go
