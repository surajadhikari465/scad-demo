SET IDENTITY_INSERT [app].[EventType] ON 
if not exists (select 1 from app.eventtype where EventName = 'Nutrition Update')
	insert into [app].[EventType] ([EventId], [EventName]) values (9, 'Nutrition Update')
if not exists (select 1 from app.eventtype where EventName = 'Nutrition Add')
	insert into [app].[EventType] ([EventId], [EventName]) values (10, 'Nutrition Add')
SET IDENTITY_INSERT [app].[EventType] OFF

SET IDENTITY_INSERT [app].[Settings] ON
if not exists (select 1 from app.[Settings] where [SettingSectionId] = 1 and [KeyName] = 'SendItemNutritionUpdatesToIRMA')
	insert into [app].[Settings]([SettingsId],[SettingSectionId],[KeyName],[Description]) 
	values( 3, 1, 'SendItemNutritionUpdatesToIRMA', 'Enables item nutrifact updates from ICON to IRMA')
SET IDENTITY_INSERT [app].[Settings] OFF

SET IDENTITY_INSERT [app].App ON 
if not exists (select 1 from app.App where AppID = 12)
insert into app.App (AppID, AppName) values (12, 'Nutrition Web API')

SET IDENTITY_INSERT [app].App OFF