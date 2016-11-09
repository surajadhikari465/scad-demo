CREATE TABLE [vim].[EventQueue] (
    [QueueId]             INT            IDENTITY (1, 1) NOT NULL,
    [EventTypeId]         INT            NOT NULL,
    [EventReferenceId]    INT            NOT NULL,
    [EventMessage]        NVARCHAR (100) NULL,
    [InsertDate]          DATETIME2 (7)  CONSTRAINT [DF_InsertDate] DEFAULT (sysdatetime()) NOT NULL,
    [ProcessedFailedDate] DATETIME2 (7)  NULL,
    [InProcessBy]         INT            NULL,
    [NumberOfRetry]       INT            NULL,
    CONSTRAINT [PK__EventQueue] PRIMARY KEY CLUSTERED ([QueueId] ASC) WITH (FILLFACTOR = 80),
    CONSTRAINT [FK_EventQueue_EventType] FOREIGN KEY ([EventTypeId]) REFERENCES [vim].[EventType] ([EventTypeId])
);

