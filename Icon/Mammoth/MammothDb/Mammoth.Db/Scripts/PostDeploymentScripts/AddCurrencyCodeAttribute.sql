DECLARE @scriptKey VARCHAR(128) = 'AddCurrencyCodeAttribute';

IF(NOT EXISTS(SELECT * FROM app.PostDeploymentScriptHistory WHERE ScriptKey = @scriptKey))
BEGIN
    PRINT 'running script ' + @scriptKey

	IF NOT EXISTS (SELECT 1 FROM Attributes WHERE AttributeCode = 'CUR')
		INSERT INTO Attributes (AttributeGroupID, AttributeCode, AttributeDesc,AddedDate) 
						VALUES (1, 'CUR', 'Currency Code', GETDATE());

    INSERT INTO app.PostDeploymentScriptHistory VALUES (@scriptKey, GETDATE())
	
	--populate new column data in dbo.Currency 
	UPDATE dbo.Currency
		SET issuingEntity= 'UNITED STATES OF AMERICA (THE)', numericCode=840, minorUnit=2, symbol='$' 
		WHERE CurrencyCode='USD'	
	UPDATE dbo.Currency 
		SET issuingEntity= 'CANADA', numericCode=124, minorUnit=2, symbol='$' 
		WHERE CurrencyCode='CAD'
	UPDATE dbo.Currency 
		SET issuingEntity= 'UNITED KINGDOM OF GREAT BRITAIN AND NORTHERN IRELAND (THE)', numericCode=826, minorUnit=2, symbol='£' 
		WHERE CurrencyCode='GBP'
		  
END
ELSE
BEGIN
    PRINT 'Skipping script ' + @scriptKey
END
GO

