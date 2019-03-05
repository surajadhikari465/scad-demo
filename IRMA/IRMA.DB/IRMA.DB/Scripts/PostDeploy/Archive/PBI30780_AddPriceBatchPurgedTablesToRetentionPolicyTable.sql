-- PBI 30780: Update Price-Batch purge process to support 'purged' reference tables for DW Team
--    DW team only needs records for last 7 days to be kept in the Purged_ PBH/PBD tables

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
	'dbo',
	'Purged_PriceBatchDetail',
	'InsertDate',
	7,
	21,
	24,
	1,
	0,
	'StraightPurge'
),
(
	'dbo',
	'Purged_PriceBatchHeader',
	'InsertDate',
	7,
	21,
	24,
	1,
	0,
	'StraightPurge'
)

GO


