DECLARE @scriptKey VARCHAR(128) = '32221_UpdatePrimeNowMerchantIDEncryptedToAllow50Characters'

IF (
		NOT EXISTS (
			SELECT 1
			FROM app.PostDeploymentScriptHistory
			WHERE ScriptKey = @scriptKey
			)
		)
BEGIN
	PRINT '[' + convert(NVARCHAR, getdate(), 121) + '] ' + @scriptKey;

	UPDATE dbo.Trait
	SET traitPattern = '^[a-zA-Z0-9]{1,50}$'
	WHERE traitCode = 'MIE'

	INSERT INTO app.PostDeploymentScriptHistory (
		ScriptKey
		,RunTime
		)
	VALUES (
		@scriptKey
		,GETDATE()
		)
END
ELSE
BEGIN
	PRINT '[' + convert(NVARCHAR, getdate(), 121) + '] ' + 'Pop-data already applied: ' + @scriptKey
END
GO