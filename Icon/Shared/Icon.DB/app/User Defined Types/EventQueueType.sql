CREATE TYPE [app].[EventQueueType] AS TABLE(
	[EventId] [int] NOT NULL,
	[EventMessage] [nvarchar](255) NULL,
	[EventReferenceId] [int] NULL,
	[RegionCode] [nvarchar](2) NULL
)
GO
