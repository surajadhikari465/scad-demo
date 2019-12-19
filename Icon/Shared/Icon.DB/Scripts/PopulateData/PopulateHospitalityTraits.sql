DECLARE @populateHospitalityScriptKey VARCHAR(128) = 'PopulateHospitalityTraits'

IF(NOT EXISTS(SELECT 1 FROM app.PostDeploymentScriptHistory WHERE ScriptKey = @populateHospitalityScriptKey))
BEGIN
	PRINT '[' + convert(nvarchar, getdate(), 121) + '] ' + @populateHospitalityScriptKey;	

	
	set identity_insert trait on

	if not exists (select * from trait where traitcode = 'HI')
		insert into trait (traitCode, traitDesc, traitGroupID, traitPattern, traitID) values ('HI', 'Hospitality Item', 1, '',216)

	if not exists (select * from trait where traitcode = 'KI')
		insert into trait (traitCode, traitDesc, traitGroupID, traitPattern, traitID) values ('KI', 'Kitchen Item', 1, '',217)

	if not exists (select * from trait where traitcode = 'KD')
		insert into trait (traitCode, traitDesc, traitGroupID, traitPattern, traitID) values ('KD', 'Kitchen Description', 1, '',218)

	if not exists (select * from trait where traitcode = 'URL')
		insert into trait (traitCode, traitDesc, traitGroupID, traitPattern, traitID) values ('URL', 'Image Url', 1, '',219)

	set identity_insert trait off



	INSERT INTO app.PostDeploymentScriptHistory (ScriptKey, RunTime) values (@populateHospitalityScriptKey, GETDATE())

END
ELSE
BEGIN
	PRINT '[' + convert(nvarchar, getdate(), 121) + '] ' + 'Pop-data already applied: ' + @populateHospitalityScriptKey
END
GO
