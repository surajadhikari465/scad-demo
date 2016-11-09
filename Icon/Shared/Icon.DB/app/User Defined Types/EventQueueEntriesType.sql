CREATE TYPE [app].[EventQueueEntriesType] AS TABLE(
	[EventMessage] [nvarchar](255) NULL,
	[EventReferenceId] [int] NULL
)