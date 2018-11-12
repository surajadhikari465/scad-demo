CREATE TABLE [amz].[MessageArchive](
	[MessageArchiveID] [int] IDENTITY(1,1) NOT NULL,
	[BusinessUnitID] [int] NOT NULL,
	[QueueID] [int] NULL,
	[KeyID] [int] NOT NULL,
	[SecondaryKeyID] [int] NULL,
	[InsertDate] [datetime2](7) NOT NULL,
	[Status] [nchar](1) NOT NULL,
	[ErrorDescription] [nvarchar](max) NULL,
	[LastProcessedTime] [datetime2](7) NULL,
	[MessageNumber] [bigint] NOT NULL,
	[Message] [xml] NOT NULL,
	[LastReprocess] [datetime2](7) NULL,
	[LastReprocessID] [nchar](13) NULL,
	[ProcessTimes] [smallint] NOT NULL,
	[EventType] [nvarchar](25) NOT NULL
) 

GO

ALTER TABLE [amz].[MessageArchive] ADD  DEFAULT ((0)) FOR [ProcessTimes]
GO