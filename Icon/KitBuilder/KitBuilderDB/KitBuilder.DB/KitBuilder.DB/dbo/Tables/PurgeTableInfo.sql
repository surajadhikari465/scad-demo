create table PurgeTableInfo(
	ID                    int identity constraint PK_PurgeTableInfo primary key clustered,
	SchemaName            nvarchar(50) not null,
	TableName             nvarchar(128) not null,
	ReferenceColumn       nvarchar(50) not null,
	DaysToKeep            smallint not null default 10,
	TimeToStart           tinyint not null,
	TimeToEnd             tinyint not null,
	IsDailyPurge          bit not null,
	IsDailyPurgeCompleted bit not null,
	PurgeJobName          nvarchar(50) not null,
	LastPurgedDate        datetime,

  index ix_SchemaTable unique(SchemaName, TableName)
) on [Primary]