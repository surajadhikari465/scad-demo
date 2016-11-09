DROP TABLE [app].[MessageQueueItemLocale]
GO

CREATE TABLE [app].[MessageQueueItemLocale](
	[MessageQueueId] [int] IDENTITY(1,1) NOT NULL,
	[MessageTypeId] [int] NOT NULL,
	[MessageStatusId] [int] NOT NULL,
	[MessageHistoryId] [int] NULL,
	[MessageActionId] [int] NOT NULL,
	[IRMAPushID] [int] NOT NULL,
	[InsertDate] [datetime2](7) NOT NULL,
	[RegionCode] [varchar](4) NOT NULL,
	[BusinessUnit_ID] [int] NOT NULL,
	[ItemId] [int] NOT NULL,
	[ItemTypeCode] [nvarchar](3) NOT NULL,
	[ItemTypeDesc] [nvarchar](255) NOT NULL,
	[LocaleId] [int] NOT NULL,
	[LocaleName] [varchar](255) NOT NULL,
	[ScanCodeId] [int] NOT NULL,
	[ScanCode] [varchar](13) NOT NULL,
	[ScanCodeTypeId] [int] NOT NULL,
	[ScanCodeTypeDesc] [nvarchar](255) NOT NULL,
	[ChangeType] [varchar](32) NOT NULL,
	[LockedForSale] [bit] NOT NULL,
	[Recall] [bit] NOT NULL,
	[TMDiscountEligible] [bit] NOT NULL,
	[Case_Discount] [bit] NOT NULL,
	[AgeCode] [int] NULL,
	[Restricted_Hours] [bit] NOT NULL,
	[Sold_By_Weight] [bit] NOT NULL,
	[ScaleForcedTare] [bit] NOT NULL,
	[Quantity_Required] [bit] NOT NULL,
	[Price_Required] [bit] NOT NULL,
	[QtyProhibit] [bit] NOT NULL,
	[VisualVerify] [bit] NOT NULL,
	[LinkedItemScanCode] [nvarchar](13) NULL,
	[PosScaleTare] [int] NULL,
	[InProcessBy] [int] NULL,
	[ProcessedDate] [datetime2](7) NULL,
 CONSTRAINT [PK_MessageQueueItemLocale] PRIMARY KEY CLUSTERED 
(
	[MessageQueueId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 80) ON [PRIMARY]
) ON [PRIMARY]

GO

SET ANSI_PADDING ON
GO

ALTER TABLE [app].[MessageQueueItemLocale] ADD  CONSTRAINT [DF_MessageQueueItemLocale_InsertDate]  DEFAULT (sysdatetime()) FOR [InsertDate]
GO

ALTER TABLE [app].[MessageQueueItemLocale]  WITH NOCHECK ADD  CONSTRAINT [FK_MessageQueueItemLocale_IRMAPushID] FOREIGN KEY([IRMAPushID])
REFERENCES [app].[IRMAPush] ([IRMAPushID])
ON DELETE CASCADE
GO

ALTER TABLE [app].[MessageQueueItemLocale] CHECK CONSTRAINT [FK_MessageQueueItemLocale_IRMAPushID]
GO

ALTER TABLE [app].[MessageQueueItemLocale]  WITH CHECK ADD  CONSTRAINT [FK_MessageQueueItemLocale_MessageActionId] FOREIGN KEY([MessageActionId])
REFERENCES [app].[MessageAction] ([MessageActionId])
GO

ALTER TABLE [app].[MessageQueueItemLocale] CHECK CONSTRAINT [FK_MessageQueueItemLocale_MessageActionId]
GO

ALTER TABLE [app].[MessageQueueItemLocale]  WITH NOCHECK ADD  CONSTRAINT [FK_MessageQueueItemLocale_MessageHistoryId] FOREIGN KEY([MessageHistoryId])
REFERENCES [app].[MessageHistory] ([MessageHistoryId])
ON DELETE CASCADE
GO

ALTER TABLE [app].[MessageQueueItemLocale] CHECK CONSTRAINT [FK_MessageQueueItemLocale_MessageHistoryId]
GO

ALTER TABLE [app].[MessageQueueItemLocale]  WITH CHECK ADD  CONSTRAINT [FK_MessageQueueItemLocale_MessageStatusId] FOREIGN KEY([MessageStatusId])
REFERENCES [app].[MessageStatus] ([MessageStatusId])
GO

ALTER TABLE [app].[MessageQueueItemLocale] CHECK CONSTRAINT [FK_MessageQueueItemLocale_MessageStatusId]
GO

ALTER TABLE [app].[MessageQueueItemLocale]  WITH CHECK ADD  CONSTRAINT [FK_MessageQueueItemLocale_MessageTypeId] FOREIGN KEY([MessageTypeId])
REFERENCES [app].[MessageType] ([MessageTypeId])
GO

ALTER TABLE [app].[MessageQueueItemLocale] CHECK CONSTRAINT [FK_MessageQueueItemLocale_MessageTypeId]
GO
