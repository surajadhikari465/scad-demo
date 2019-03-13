-- PBI 30780: As part of Honeycrisp we are archiving events sent to EMS Topic
-- We need to make sure the event archive table doesn't get too large

INSERT INTO dbo.RetentionPolicy 
(
	[Schema],
	[Table],
	ReferenceColumn,
	DaysToKeep,
	TimeToStart,
	TimeToEnd,
	IncludedInDailyPurge,
	DailyPurgeCompleted,
	PurgeJobName
)
VALUES 
(
	'amz',
	'MessageArchiveEvent',
	'InsertDate',
	10,
	21,
	23,
	1,
	0,
	'StraightPurge'
)

GO


