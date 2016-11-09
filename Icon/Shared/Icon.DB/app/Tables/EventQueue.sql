CREATE TABLE [app].[EventQueue]
( 
	[QueueId] INT NOT NULL IDENTITY (1,1), 
    [EventId] INT NOT NULL, 
    [EventMessage] NVARCHAR(255) NULL, 
    [EventReferenceId] INT NULL, 
    [RegionCode] NVARCHAR(2) NULL, 
    [InsertDate] DATETIME2 NOT NULL CONSTRAINT [DF_InsertDate] DEFAULT(SYSDATETIME()),
    [ProcessFailedDate] DATETIME2 NULL, 
    [InProcessBy] NVARCHAR(255) NULL, 
    CONSTRAINT [FK_EventQueue_EventType] FOREIGN KEY ([EventId]) REFERENCES [app].[EventType]([EventId]), 
    CONSTRAINT [PK_EventQueue] PRIMARY KEY ([QueueId])
)
