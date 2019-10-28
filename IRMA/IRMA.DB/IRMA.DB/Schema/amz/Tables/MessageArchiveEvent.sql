CREATE TABLE [amz].[MessageArchiveEvent]
(
    [MessageArchiveEventID] BIGINT         IDENTITY (1, 1) NOT NULL,
    [KeyID]                 INT            NOT NULL,
	[SecondaryKeyID]	    INT			   NULL,
    [MessageType]           NVARCHAR (50)  NOT NULL,
	[EventTypeCode]			NVARCHAR (25)  NOT NULL,
	[InsertDate]		    DATETIME2(7)   NOT NULL,
	[MessageTimestampUtc]   DATETIME2(7)   NOT NULL,
    [MessageID]             NVARCHAR (50)  NULL,
    [MessageHeaders]        NVARCHAR (MAX) NULL,
    [Message]               XML            NULL,
    [ErrorCode]             NVARCHAR (100) NULL,
    [ErrorDetails]          NVARCHAR (MAX) NULL,
    [ArchiveInsertDateUtc]  DATETIME2 (7)  NOT NULL CONSTRAINT DF_MessageArchiveEvent_InsertDateUtc DEFAULT SYSUTCDATETIME(),
	CONSTRAINT [PK_MessageArchiveID] PRIMARY KEY CLUSTERED ([MessageArchiveEventID])
)
GO

GRANT INSERT ON [amz].[MessageArchiveEvent] TO [TibcoDataWriter] AS [dbo];
GO

GRANT SELECT ON [amz].[MessageArchiveEvent] TO [MammothRole] AS [dbo];
GO
