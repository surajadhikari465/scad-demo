﻿CREATE TABLE [amz].[MessageArchive](
	[MessageArchiveID] [int] IDENTITY(1,1) NOT NULL,
	[BusinessUnitID] [int] NOT NULL,
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
	[ProcessTimes] [smallint] NOT NULL CONSTRAINT [DF_Amz_MessageArchive_ProcessTimes] DEFAULT ((0)),
	[EventType] [nvarchar](25) NOT NULL,
  [ResetBy] [nvarchar](255) NULL
)
GO
CREATE CLUSTERED INDEX [idxMessageArchive_InsertDateID]
    ON [amz].[MessageArchive]([InsertDate], [MessageArchiveID]);

GO

GRANT SELECT, UPDATE
    ON OBJECT::[amz].[MessageArchive] TO [MammothRole], [TibcoDataWriter]
    AS [dbo];

GO

GRANT SELECT ON [amz].[MessageArchive] TO [IRMAReports];
GO