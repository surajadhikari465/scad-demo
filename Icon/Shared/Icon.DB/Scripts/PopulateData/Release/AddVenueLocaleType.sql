DECLARE @scriptKey VARCHAR(128) = 'AddVenueToLocaleType'

IF(NOT EXISTS(SELECT 1 FROM app.PostDeploymentScriptHistory WHERE ScriptKey = @scriptKey))
BEGIN
	print '[' + convert(nvarchar, getdate(), 121) + '] ' + @scriptKey;	

	SET IDENTITY_INSERT [dbo].[LocaleType] ON 

	INSERT INTO LocaleType (localeTypeID,localeTypeCode,localeTypeDesc)
	Values (5, 'VE','Venue')

	SET IDENTITY_INSERT [dbo].[LocaleType] OFF 
	
END
ELSE
BEGIN
	print '[' + convert(nvarchar, getdate(), 121) + '] ' + 'Pop-data already applied: ' + @scriptKey
END
GO