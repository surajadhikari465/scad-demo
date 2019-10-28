CREATE TABLE [amz].[OrderQueue]
(
	[QueueID] INT IDENTITY(1,1),
	[EventTypeCode] NVARCHAR(25) NOT NULL,
	[MessageType] NVARCHAR(50) NOT NULL,
	[KeyID] INT NOT NULL,
	[SecondaryKeyID] INT NULL,
	[InsertDate] DATETIME2(7) NOT NULL CONSTRAINT [DF_OrderQueue_InsertDate] DEFAULT (SYSDATETIME()),
	[MessageTimestampUtc] DATETIME2(7) NOT NULL CONSTRAINT [DF_OrderQueue_MessageTimestampUtc] DEFAULT (SYSUTCDATETIME())
)
GO
CREATE CLUSTERED INDEX [idxOrderQueue_InsertDateID]
    ON [amz].[OrderQueue]([InsertDate], [QueueID]);

GO

GRANT SELECT, INSERT
    ON OBJECT::[amz].[OrderQueue] TO [MammothRole]
    AS [dbo];

GO

GRANT SELECT ON [amz].[OrderQueue] TO [IRMAReports] AS [dbo];
GO