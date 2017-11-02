CREATE TABLE [app].[JobSchedule]
(	JobScheduleId int  IDENTITY (1, 1) NOT NULL,
	JobName nvarchar(50) NOT NULL,
	Region nvarchar(2) NULL,
	DestinationQueueName nvarchar(100) NULL,
	StartDateTimeUtc datetime2(7) NOT NULL,		
	LastRunDateTimeUtc datetime2(7) NULL,	
	IntervalInSeconds int NOT NULL,
	Enabled bit NOT NULL,	
	XmlObject nvarchar(max) NULL,
    CONSTRAINT [JobSchedule_PK] PRIMARY KEY CLUSTERED (JobScheduleId ASC) WITH (FILLFACTOR = 80)
)
GO
GRANT SELECT,UPDATE on [app].[JobSchedule] to [TibcoRole]
GO