CREATE TABLE [amz].[ReceiptQueue]
(
	[QueueID] INT IDENTITY(1,1),
	[EventTypeCode] NVARCHAR(25) NOT NULL,
	[MessageType] NVARCHAR(50) NOT NULL,
	[KeyID] INT NOT NULL,
	[SecondaryKeyID] INT NULL,
	[InsertDate] DATETIME2(7) NOT NULL CONSTRAINT [DF_ReceiptQueue_InsertDate] DEFAULT (SYSDATETIME()),
	[MessageTimestampUtc] DATETIME2(7) NOT NULL CONSTRAINT [DF_ReceiptQueue_MessageTimestampUtc] DEFAULT (SYSUTCDATETIME())
)

GO
CREATE CLUSTERED INDEX [idxReceiptQueue_InsertDateID]
    ON [amz].[ReceiptQueue]([InsertDate], [QueueID]);

GO
GRANT SELECT, INSERT
    ON OBJECT::[amz].[ReceiptQueue] TO [MammothRole]
    AS [dbo];

GO

GRANT SELECT ON [amz].[ReceiptQueue] TO [IRMAReports];
GO