/*
All pop-data updates for each release go here.

Do not check in separate files to the '.../Scripts/PopulateData/Release/' folder,
just add your updates directly to IconMasterData.sql or IconPopulateData.sql.
*/

go

IF EXISTS (SELECT 1 FROM app.Settings WHERE KeyName = 'SendItemSubTeamupdatesToIRMA')
BEGIN
	DELETE rs 
	FROM app.RegionalSettings rs
	JOIN app.Settings s ON rs.SettingsId = s.SettingsId
	WHERE s.KeyName = 'SendItemSubTeamupdatesToIRMA'

	DELETE FROM app.Settings WHERE KeyName = 'SendItemSubTeamupdatesToIRMA'
END
go

--Add vim locale event types
IF NOT EXISTS (SELECT 1 FROM vim.EventType where Name = 'LocaleAdd')
BEGIN
	INSERT INTO vim.EventType(Name)
	VALUES ('Locale Add')
END
GO

IF NOT EXISTS (SELECT 1 FROM vim.EventType where Name = 'LocaleUpdate')
BEGIN
	INSERT INTO vim.EventType(Name)
	VALUES ('Locale Update')
END
GO

--Update Brand Abbreviation trait pattern to allow ampersands
update Trait
set traitPattern = '^[a-zA-Z0-9 &]{1,10}$'
where traitDesc = 'Brand Abbreviation'

GO

