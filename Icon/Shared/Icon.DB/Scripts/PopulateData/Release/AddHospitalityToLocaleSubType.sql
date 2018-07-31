DECLARE @scriptKey VARCHAR(128) = 'AddHospitalityToLocaleSubType'

IF(NOT EXISTS(SELECT 1 FROM app.PostDeploymentScriptHistory WHERE ScriptKey = @scriptKey))
BEGIN
	print '[' + convert(nvarchar, getdate(), 121) + '] ' + @scriptKey;	
	DECLARE @VenueLocaleTypeID int = (SELECT TOP 1 LocaleTypeID FROM LocaleType WHERE localeTypeCode = 'VE')
								  
	SET IDENTITY_INSERT [dbo].LocaleSubType ON 

	INSERT INTO LocaleSubType (localeSubTypeID,localeTypeID,localSubTypeCode, localeSubTypeDesc)
	Values (1, @VenueLocaleTypeID, 'HS', 'Hospitality')

	SET IDENTITY_INSERT [dbo].LocaleSubType OFF 

END
ELSE
BEGIN
	print '[' + convert(nvarchar, getdate(), 121) + '] ' + 'Pop-data already applied: ' + @scriptKey
END
GO