--Kasthuri
-- Returns configuration list for given key name

create procedure [app].[GetRegionalConfigurationForSettingName]

 @settingSectionKeyName nvarchar(255)	
as

select rs.[RegionalSettingsId] as RegionalSettingsId, r.RegionCode as RegionCode, sc.SectionName as SectionName, s.KeyName as KeyName, s.Description as Description, rs.Value as Value
from app.regionalsettings rs
join app.Settings s on rs.SettingsId = s.SettingsId and s.KeyName = @settingSectionKeyName
join app.SettingSection sc on s.SettingSectionId = sc.SettingSectionId
join app.regions r on rs.RegionId = r.RegionId

order by r.RegionCode

GO
