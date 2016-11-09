/*
All pop-data updates for each release go here.

Do not check in separate files to the '.../Scripts/PopulateData/Release/' folder,
just add your updates directly to IconMasterData.sql or IconPopulateData.sql.

Please add prints/logging for each statement/block of code appropriately, to allow for tracking, debugging, and troublshooting;
Example/Format:
print '[' + convert(nvarchar, getdate(), 121) + '] [PopulateData] TFS ?????: PBI Desc -- Action details...'

*/

go
print '[' + convert(nvarchar, getdate(), 121) + '] [PopulateData] Bug 14106: Tax Abbreviations are allowed to be larger than 50 characters...'

update dbo.Trait 
set traitPattern = '^[\d]{7} [\w \-\\/%<>&=\+]{0,42}$'
where traitCode = 'ABR'


go
print '[' + convert(nvarchar, getdate(), 121) + '] [PopulateData] PBI 13719: Trigger PagerDuty incident when API Controllers stop processing Message Queues...'

SET IDENTITY_INSERT [app].[App] ON
IF NOT EXISTS(SELECT 1 FROM app.App WHERE AppName = 'Icon Monitoring')
	INSERT INTO app.App (AppID, AppName) VALUES (14, 'Icon Monitoring');
SET IDENTITY_INSERT [app].[App] OFF
GO

