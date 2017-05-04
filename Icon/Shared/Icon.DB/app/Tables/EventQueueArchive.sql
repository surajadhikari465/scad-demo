CREATE TABLE app.EventQueueArchive
(
	EventQueueArchiveId INT NOT NULL PRIMARY KEY IDENTITY(1, 1),
	QueueId INT NOT NULL,
	EventId INT NOT NULL, 
    EventMessage NVARCHAR(255) NULL, 
    EventReferenceId INT NULL, 
    RegionCode NVARCHAR(2) NULL,
	EventQueueInsertDate DATETIME2 NOT NULL,
	Context NVARCHAR(MAX) NULL,
	ErrorCode NVARCHAR(100) NULL,
	ErrorDetails NVARCHAR(MAX) NULL,
    InsertDate DATETIME2 NOT NULL CONSTRAINT [DF_EventQueueHistory_InsertDate] DEFAULT(SYSDATETIME())
)
