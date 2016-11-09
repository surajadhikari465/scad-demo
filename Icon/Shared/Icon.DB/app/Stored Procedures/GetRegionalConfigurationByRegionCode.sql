--Kasthuri
-- Retruns region specific configuration

create procedure [app].[GetRegionalConfigurationByRegionCode]
 @regionCode nvarchar(2)	
as		

select rs.[RegionalSettingsId] as RegionalSettingsId, r.RegionCode as RegionCode, sc.SectionName as SectionName, s.KeyName as KeyName, s.Description as Description, rs.Value as Value
from app.regionalsettings rs
join app.Settings s on rs.SettingsId = s.SettingsId
join app.SettingSection sc on s.SettingSectionId = sc.SettingSectionId
join app.regions r on rs.RegionId = r.RegionId and r.RegionCode = @regionCode--'RM'

order by sc.SectionName, s.KeyName


GO
