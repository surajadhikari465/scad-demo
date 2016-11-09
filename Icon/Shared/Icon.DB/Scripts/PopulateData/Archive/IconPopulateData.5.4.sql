SET IDENTITY_INSERT [app].[PLURequestChangeType] ON 

insert into [app].[PLURequestChangeType] ([pluRequestChangeTypeID], [ChangeTypeDescription]) values (1, 'Notes')
insert into [app].[PLURequestChangeType]([pluRequestChangeTypeID], [ChangeTypeDescription]) values (2, 'Status Change')

SET IDENTITY_INSERT [app].[PLURequestChangeType] OFF

  if not exists (select 1 from app.RegionalSettings where [RegionId] = 1 and [SettingsId] = 3)
	insert into app.regionalsettings([RegionId],[SettingsId],[Value]) values(1, 3, 0)

 if not exists (select 1 from app.RegionalSettings where [RegionId] = 2 and [SettingsId] = 3)
	insert into app.regionalsettings([RegionId],[SettingsId],[Value]) values(2, 3, 0)

 if not exists (select 1 from app.RegionalSettings where [RegionId] = 3 and [SettingsId] = 3)
	insert into app.regionalsettings([RegionId],[SettingsId],[Value]) values(3, 3, 0)

 if not exists (select 1 from app.RegionalSettings where [RegionId] = 4 and [SettingsId] = 3)
	insert into app.regionalsettings([RegionId],[SettingsId],[Value]) values(4, 3, 0)

 if not exists (select 1 from app.RegionalSettings where [RegionId] = 5 and [SettingsId] = 3)
	insert into app.regionalsettings([RegionId],[SettingsId],[Value]) values(5, 3, 0)

 if not exists (select 1 from app.RegionalSettings where [RegionId] = 6 and [SettingsId] = 3)
	insert into app.regionalsettings([RegionId],[SettingsId],[Value]) values(6, 3, 0)

 if not exists (select 1 from app.RegionalSettings where [RegionId] = 7 and [SettingsId] = 3)
	insert into app.regionalsettings([RegionId],[SettingsId],[Value]) values(7, 3, 0)

 if not exists (select 1 from app.RegionalSettings where [RegionId] = 8 and [SettingsId] = 3)
	insert into app.regionalsettings([RegionId],[SettingsId],[Value]) values(8, 3, 0)

 if not exists (select 1 from app.RegionalSettings where [RegionId] = 9 and [SettingsId] = 3)
	insert into app.regionalsettings([RegionId],[SettingsId],[Value]) values(9, 3, 0)

 if not exists (select 1 from app.RegionalSettings where [RegionId] = 10 and [SettingsId] = 3)
	insert into app.regionalsettings([RegionId],[SettingsId],[Value]) values(10, 3, 0)

 if not exists (select 1 from app.RegionalSettings where [RegionId] = 11 and [SettingsId] = 3)
	insert into app.regionalsettings([RegionId],[SettingsId],[Value]) values(11, 3, 0)

 if not exists (select 1 from app.RegionalSettings where [RegionId] = 12 and [SettingsId] = 3)
	insert into app.regionalsettings([RegionId],[SettingsId],[Value]) values(12, 3, 0)


