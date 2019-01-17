CREATE TABLE [amz].[InventoryQueue]
(
	[QueueID] INT IDENTITY(1,1),
	[EventTypeID] INT NOT NULL,
	[KeyID] INT NOT NULL,
	[SecondaryKeyID] INT NULL,
	[InsertDate] DATETIME2(7) NOT NULL CONSTRAINT [DF_InventoryQueue_InsertDate] DEFAULT (SYSDATETIME()),
	[Status] NCHAR(1) NOT NULL,
	[InProcessBy] INT NULL,
	[ErrorDescription] NVARCHAR(MAX) NULL,
	[ProcessTimes] SMALLINT NULL,
	[LastProcessedTime] DATETIME2(7) NULL,
	[MessageTimestampUtc] DATETIME2(7) NOT NULL CONSTRAINT [DF_InventoryQueue_MessageTimestampUtc] DEFAULT (SYSUTCDATETIME()),
    [ResetBy] [nvarchar](255) NULL,
	CONSTRAINT [CK_InventoryQueue_Status] CHECK  ([Status]='R' OR [Status]='F' OR [Status]='P' OR [Status]='I' OR [Status]='U')
)

GO

GRANT SELECT
    ON OBJECT::[amz].[InventoryQueue] TO [MammothRole]
    AS [dbo];

GO
GRANT UPDATE
    ON OBJECT::[amz].[InventoryQueue] TO [MammothRole]
    AS [dbo];
GO

GRANT SELECT ON [amz].[InventoryQueue] TO [IRMAReports];
GO