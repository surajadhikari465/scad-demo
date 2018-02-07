CREATE TABLE [app].[JobSchedule]
(	JobScheduleId int  IDENTITY (1, 1) NOT NULL,
	JobName nvarchar(50) NOT NULL,
	Region nvarchar(2) NULL,
	DestinationQueueName nvarchar(100) NULL,
	StartDateTimeUtc datetime2(7) NOT NULL,		
	LastScheduledDateTimeUtc datetime2(7) NULL,
	LastRunEndDateTimeUtc datetime2(7) NULL,
	NextScheduledDateTimeUtc datetime2(7) NOT NULL,	
	IntervalInSeconds int NOT NULL,
	Enabled bit NOT NULL,
	Status varchar(10) NULL,  
	XmlObject nvarchar(max) NULL,
    CONSTRAINT [JobSchedule_PK] PRIMARY KEY CLUSTERED (JobScheduleId ASC) WITH (FILLFACTOR = 80)
)

GO

ALTER TABLE [app].[JobSchedule] ADD CONSTRAINT CK_JobSchedule_Status
    CHECK (Status IN ('ready', 'running', 'failed'))
GO

GRANT SELECT,UPDATE on [app].[JobSchedule] to [TibcoRole]
GO