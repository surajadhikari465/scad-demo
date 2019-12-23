DECLARE @scriptKey VARCHAR(128) = 'PopulateContactType_24426'

IF (NOT EXISTS (SELECT 1 FROM app.PostDeploymentScriptHistory WHERE ScriptKey = @scriptKey))
BEGIN
    INSERT INTO dbo.ContactType(ContactTypeName)
        VALUES('AR/AP'),
              ('Chief Executive'),
              ('Marketing'),
              ('Operations Management'),
              ('Product Safety Emergency Contact'),
              ('QA'),
              ('Sales Management'),
              ('WFM Account Manager (Primary)')
END
ELSE
BEGIN
	PRINT '[' + convert(NVARCHAR, getdate(), 121) + '] ' + 'Pop-data already applied: ' + @scriptKey
END
GO