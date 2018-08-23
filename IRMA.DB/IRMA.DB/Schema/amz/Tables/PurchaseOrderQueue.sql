CREATE TABLE [amz].[PurchaseOrderQueue]
(
	[QueueID] INT IDENTITY(1,1),
	[EventTypeID] INT NOT NULL,
	[KeyID] INT NOT NULL,
	[SecondaryKeyID] INT NULL,
	[InsertDate] DATETIME2(7) NOT NULL CONSTRAINT [DF_PurchaseOrderQueue_InsertDate] DEFAULT (SYSDATETIME()),
	[Status] NCHAR(1) NOT NULL,
	[InProcessBy] INT NULL,
	[ErrorDescription] NVARCHAR(MAX) NULL,
	[ProcessTimes] SMALLINT NULL,
	[LastProcessedTime] DATETIME2(7) NULL,
	[MessageTimestampUtc] DATETIME2(7) NOT NULL CONSTRAINT [DF_PurchaseOrderQueue_MessageTimestampUtc] DEFAULT (SYSUTCDATETIME()),
	CONSTRAINT [CK_PurchaseOrderQueue_Status] CHECK ([Status] IN ('U', 'I', 'P', 'F'))
)