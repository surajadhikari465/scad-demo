CREATE TABLE [app].[MessageQueueProductSelectionGroup](
	[MessageQueueId] [int] IDENTITY(1,1) NOT NULL,
	[InsertDate] [datetime2](7) NOT NULL CONSTRAINT [DF_MessageQueueProductSelectionGroup_InsertDate]  DEFAULT (sysdatetime()),
	[MessageTypeId] [int] NOT NULL,
	[MessageStatusId] [int] NOT NULL,
	[MessageHistoryId] [int] NULL,
	[MessageActionId] [int] NOT NULL,
	[ProductSelectionGroupId] [int] NOT NULL,
	[ProductSelectionGroupName] [nvarchar](255) NOT NULL,
	[ProductSelectionGroupTypeId] [int] NOT NULL,
	[ProductSelectionGroupTypeName] [nvarchar](255) NOT NULL,
	[InProcessBy] [int] NULL,
	[ProcessedDate] [datetime2](7) NULL,
 CONSTRAINT [PK_MessageQueueProductSelectionGroup] PRIMARY KEY CLUSTERED 
(
	[MessageQueueId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 80) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE [app].[MessageQueueProductSelectionGroup]  WITH CHECK ADD  CONSTRAINT [FK_MessageQueueProductSelectionGroup_MessageAction] FOREIGN KEY([MessageActionId])
REFERENCES [app].[MessageAction] ([MessageActionId])
GO

ALTER TABLE [app].[MessageQueueProductSelectionGroup] CHECK CONSTRAINT [FK_MessageQueueProductSelectionGroup_MessageAction]
GO

ALTER TABLE [app].[MessageQueueProductSelectionGroup]  WITH CHECK ADD  CONSTRAINT [FK_MessageQueueProductSelectionGroup_MessageHistory] FOREIGN KEY([MessageHistoryId])
REFERENCES [app].[MessageHistory] ([MessageHistoryId]) ON DELETE CASCADE
GO

ALTER TABLE [app].[MessageQueueProductSelectionGroup] CHECK CONSTRAINT [FK_MessageQueueProductSelectionGroup_MessageHistory]
GO

ALTER TABLE [app].[MessageQueueProductSelectionGroup]  WITH CHECK ADD  CONSTRAINT [FK_MessageQueueProductSelectionGroup_MessageStatus] FOREIGN KEY([MessageStatusId])
REFERENCES [app].[MessageStatus] ([MessageStatusId])
GO

ALTER TABLE [app].[MessageQueueProductSelectionGroup] CHECK CONSTRAINT [FK_MessageQueueProductSelectionGroup_MessageStatus]
GO

ALTER TABLE [app].[MessageQueueProductSelectionGroup]  WITH CHECK ADD  CONSTRAINT [FK_MessageQueueProductSelectionGroup_MessageType] FOREIGN KEY([MessageTypeId])
REFERENCES [app].[MessageType] ([MessageTypeId])
GO

ALTER TABLE [app].[MessageQueueProductSelectionGroup] CHECK CONSTRAINT [FK_MessageQueueProductSelectionGroup_MessageType]
GO

ALTER TABLE [app].[MessageQueueProductSelectionGroup]  WITH CHECK ADD  CONSTRAINT [FK_MessageQueueProductSelectionGroup_ProductSelectionGroupType] FOREIGN KEY([ProductSelectionGroupTypeId])
REFERENCES [app].[ProductSelectionGroupType] ([ProductSelectionGroupTypeId])
GO

ALTER TABLE [app].[MessageQueueProductSelectionGroup] CHECK CONSTRAINT [FK_MessageQueueProductSelectionGroup_ProductSelectionGroupType]
GO
