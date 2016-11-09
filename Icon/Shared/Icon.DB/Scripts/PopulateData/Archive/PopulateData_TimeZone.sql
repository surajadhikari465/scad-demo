-- Populate dbo.Timezone table
-- 7/11/2014
-- TFS 3790, Sprint 18

IF NOT EXISTS (select * from dbo.Timezone tz where tz.timezoneCode = 'CST')
BEGIN
	INSERT INTO dbo.Timezone (timezoneName, timezoneCode, gmtOffset)
	VALUES
		('(UTC-10:00) Hawaii','HST',-10),
		('(UTC-09:00) Alaska','AKST',-9),
		('(UTC-08:00) Pacific Time (US & Canada)','PST',-8),
		('(UTC-07:00) Mountain Time (US & Canada)','MST',-7),
		('(UTC-06:00) Central Time (US & Canada)','CST',-6),
		('(UTC-05:00) Eastern Time (US & Canada)','EST',-5),
		('(UTC-04:00) Atlantic Time (Canada)','ADT',-4),
		('(UTC-03:30) Newfoundland Time (Canada)','NST',-3.5),
		('(UTC) Dublin, Edinburgh, Lisbon, London','GMT',0)
END