--PBI 26341: Add CurrencyCode Trait
DECLARE @scriptKey VARCHAR(128) = 'AddCurrencyCodeTrait'

IF(NOT EXISTS(SELECT 1 FROM app.PostDeploymentScriptHistory WHERE ScriptKey = @scriptKey))
BEGIN
	print '[' + convert(nvarchar, getdate(), 121) + '] ' + @scriptKey;	
	SET IDENTITY_INSERT dbo.Trait ON;

	DECLARE @currencyTraitId int = 167;
	DECLARE @patternCurrency nvarchar(255) = N'^[C][A][D]$|^[G][B][P]$|^[U][S][D]$';

	-- add trait for currency
	IF NOT EXISTS (SELECT 1 FROM dbo.Trait WHERE traitCode= 'CUR')
	INSERT dbo.Trait (traitID, traitGroupID, traitCode, traitDesc, traitPattern) 
			VALUES  (@currencyTraitId, 1, N'CUR', N'Currency Code', @patternCurrency);

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
		   
	-- populate currency trait for existing locales
	INSERT INTO dbo.LocaleTrait
	SELECT 
		@currencyTraitId AS traitID
		, L.localeID AS localeID
		, NULL AS uomID
		, CASE 
			WHEN C.countryCode = 'CAN' THEN 'CAD'
			WHEN C.countryCode = 'GBR' THEN 'GBP'
			ELSE 'USD'
		END AS traitValue
	FROM dbo.Locale L
		INNER JOIN dbo.LocaleType LTY ON LTY.localeTypeID = L.localeTypeID
		INNER JOIN dbo.LocaleAddress LA ON LA.localeID = L.localeID
		INNER JOIN dbo.Address A ON A.addressID = LA.addressID
		INNER JOIN dbo.PhysicalAddress PA ON PA.addressID = A.addressID
		INNER JOIN dbo.Country C ON C.countryID = PA.countryID
	WHERE L.localeID NOT IN (
		SELECT L.localeID
		FROM dbo.Locale L
			INNER JOIN dbo.LocaleType LTY ON LTY.localeTypeID = L.localeTypeID
			INNER JOIN dbo.LocaleTrait LT ON LT.localeID = L.localeID
			INNER JOIN dbo.Trait T ON T.traitID = LT.traitID
		WHERE LTY.localeTypeDesc = 'Store' AND T.traitCode = 'CUR'
	)

END
ELSE
BEGIN
	print '[' + convert(nvarchar, getdate(), 121) + '] ' + 'Pop-data already applied: ' + @scriptKey
END
go
