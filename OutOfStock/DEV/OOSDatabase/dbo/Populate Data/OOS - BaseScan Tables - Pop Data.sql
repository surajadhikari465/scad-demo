/*
POP-DATA
*/

use oos;

insert into app values
('Unknown - Miscellaneous - Default'),
('Raw Scan Copy To Base Scan'),
('Table Data Maintenance')

select 'desc'='inserted App table entry: ', * from app

--------------------------------------------------------------------------------------------------------------------
--------------------------------------------------------------------------------------------------------------------

/*
We'll postpone config entries for purge (table maint) until after base-scan solution is running in prod (so it has time to run for over a week and biz confirm).
*/

declare @tableDataMaintAppID int = (select appid from app where appname like '%table data maint%')
declare @rawScanCopyAppID int = (select appid from app where appname like '%raw scan copy%')
insert into appconfig values
/*
(@tableDataMaintAppID, 'BaseScanDetail.Purge Enabled', '1'),
(@tableDataMaintAppID, 'BaseScanDetail.Use Date Limit', '1'),
(@tableDataMaintAppID, 'BaseScanDetail.Days To Keep', '600'),

(@tableDataMaintAppID, 'BaseScanDetail.Use Row Limit', '1'),
(@tableDataMaintAppID, 'BaseScanDetail.Rows To Keep', '100000'),

(@tableDataMaintAppID, 'BaseScanDetail.Each Pass Purge Limit', '1000')
*/
(@rawScanCopyAppID, 'Catch Block Processing Enabled', '1')

select 'desc'='inserted AppConfig table entry: ', a.appname, ac.[key], ac.value from appconfig ac join app a on ac.AppID=a.AppID order by a.appname, ac.[key]


go

