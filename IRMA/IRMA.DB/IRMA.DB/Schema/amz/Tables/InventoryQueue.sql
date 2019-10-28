CREATE TABLE [amz].[InventoryQueue]
(
	[QueueID] INT IDENTITY(1,1),
	[EventTypeCode] NVARCHAR(25) NOT NULL,
	[MessageType] NVARCHAR(50) NOT NULL,
	[KeyID] INT NOT NULL,
	[SecondaryKeyID] INT NULL,
	[InsertDate] DATETIME2(7) NOT NULL CONSTRAINT [DF_InventoryQueue_InsertDate] DEFAULT (SYSDATETIME()),
	[MessageTimestampUtc] DATETIME2(7) NOT NULL CONSTRAINT [DF_InventoryQueue_MessageTimestampUtc] DEFAULT (SYSUTCDATETIME())
)

GO
CREATE CLUSTERED INDEX [idxInventoryQueue_InsertDateID]
    ON [amz].[InventoryQueue]([InsertDate], [QueueID]);

GO

GRANT SELECT, INSERT
    ON OBJECT::[amz].[InventoryQueue] TO [MammothRole]
    AS [dbo];

GO

GRANT SELECT ON [amz].[InventoryQueue] TO [IRMAReports] AS [dbo];
GO