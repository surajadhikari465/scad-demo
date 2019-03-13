CREATE TABLE [amz].[TransferQueue]
(
	[QueueID] INT IDENTITY(1,1),
	[EventTypeCode] NVARCHAR(25) NOT NULL,
	[MessageType] NVARCHAR(50) NOT NULL,
	[KeyID] INT NOT NULL,
	[SecondaryKeyID] INT NULL,
	[InsertDate] DATETIME2(7) NOT NULL CONSTRAINT [DF_TransferQueue_InsertDate] DEFAULT (SYSDATETIME()),
	[MessageTimestampUtc] DATETIME2(7) NOT NULL CONSTRAINT [DF_TransferQueue_MessageTimestampUtc] DEFAULT (SYSUTCDATETIME())
)
GO
CREATE CLUSTERED INDEX [idxTransferQueue_InsertDateID]
    ON [amz].[TransferQueue]([InsertDate], [QueueID]);

GO

GRANT SELECT, INSERT
    ON OBJECT::[amz].[TransferQueue] TO [MammothRole]
    AS [dbo];

GO

GRANT SELECT ON [amz].[TransferQueue] TO [IRMAReports];
GO