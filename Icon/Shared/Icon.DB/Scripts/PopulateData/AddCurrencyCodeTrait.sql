--PBI 26341: Add CurrencyCode Trait
DECLARE @scriptKey VARCHAR(128) = 'AddCurrencyCodeTrait'

IF(NOT EXISTS(SELECT 1 FROM app.PostDeploymentScriptHistory WHERE ScriptKey = @scriptKey))
BEGIN
	print '[' + convert(nvarchar, getdate(), 121) + '] ' + @scriptKey;	
	SET IDENTITY_INSERT dbo.Trait ON;

	--regular expressions for validation (trait patterns)
	DECLARE @patternCurrency nvarchar(255) = N'^[C][A][D]$|^[G][B][P]$|^[U][S][D]$';

	--remove enum-like validation pattern for FairTradeCertified (FTC)
	UPDATE dbo.Trait SET traitPattern = @patternAnyText255 WHERE traitCode = 'CUR'

	IF NOT EXISTS (SELECT 1 FROM dbo.Trait WHERE traitCode= 'CUR')
	INSERT dbo.Trait (traitID, traitGroupID, traitCode, traitDesc, traitPattern) 
			VALUES  (167, 1, N'CUR', N'Currency Code', @patternCurrency);

	INSERT INTO app.PostDeploymentScriptHistory (ScriptKey, RunTime) VALUES (@scriptKey, GETDATE())
	SET IDENTITY_INSERT dbo.Trait OFF
END
ELSE
BEGIN
	print '[' + convert(nvarchar, getdate(), 121) + '] ' + 'Pop-data already applied: ' + @scriptKey
END
go
