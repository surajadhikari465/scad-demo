DROP TABLE [app].[MessageQueueProduct]
GO

CREATE TABLE [app].[MessageQueueProduct](
	[MessageQueueId] [int] IDENTITY(1,1) NOT NULL,
	[MessageTypeId] [int] NOT NULL,
	[MessageStatusId] [int] NOT NULL,
	[MessageHistoryId] [int] NULL,
	[InsertDate] [datetime2](7) NOT NULL CONSTRAINT [DF_MessageQueueProduct_InsertDate]  DEFAULT (sysdatetime()),
	[ItemId] [int] NOT NULL,
	[LocaleId] [int] NOT NULL,
	[ItemTypeCode] [nvarchar](3) NOT NULL,
	[ItemTypeDesc] [nvarchar](255) NOT NULL,
	[ScanCodeId] [int] NOT NULL,
	[ScanCode] [nvarchar](13) NOT NULL,
	[ScanCodeTypeId] [int] NOT NULL,
	[ScanCodeTypeDesc] [nvarchar](255) NOT NULL,
	[ProductDescription] [nvarchar](255) NOT NULL,
	[PosDescription] [nvarchar](255) NOT NULL,
	[PackageUnit] [nvarchar](255) NOT NULL,
	[RetailSize] [nvarchar](255) NULL,
	[RetailUom] [nvarchar](255) NULL,
	[FoodStampEligible] [nvarchar](255) NOT NULL,
	[ProhibitDiscount] [bit] NOT NULL,
	[DepartmentSale] [nvarchar](255) NOT NULL,
	[BrandId] [int] NOT NULL,
	[BrandName] [nvarchar](255) NOT NULL,
	[BrandLevel] [int] NOT NULL,
	[BrandParentId] [int] NULL,
	[BrowsingClassId] [int] NULL,
	[BrowsingClassName] [nvarchar](255) NULL,
	[BrowsingLevel] [int] NULL,
	[BrowsingParentId] [int] NULL,
	[MerchandiseClassId] [int] NOT NULL,
	[MerchandiseClassName] [nvarchar](255) NOT NULL,
	[MerchandiseLevel] [int] NOT NULL,
	[MerchandiseParentId] [int] NULL,
	[TaxClassId] [int] NOT NULL,
	[TaxClassName] [nvarchar](255) NOT NULL,
	[TaxLevel] [int] NOT NULL,
	[TaxParentId] [int] NULL,
	[FinancialClassId] [nvarchar](32) NOT NULL,
	[FinancialClassName] [nvarchar](255) NOT NULL,
	[FinancialLevel] [int] NOT NULL,
	[FinancialParentId] [int] NULL,
	[InProcessBy] [int] NULL,
	[ProcessedDate] [datetime2](7) NULL,
 CONSTRAINT [PK_MessageQueueProduct] PRIMARY KEY CLUSTERED 
(
	[MessageQueueId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 80) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE [app].[MessageQueueProduct]  WITH CHECK ADD  CONSTRAINT [FK_MessageQueueProduct_MessageHistoryId] FOREIGN KEY([MessageHistoryId])
REFERENCES [app].[MessageHistory] ([MessageHistoryId])
ON DELETE CASCADE
GO

ALTER TABLE [app].[MessageQueueProduct] CHECK CONSTRAINT [FK_MessageQueueProduct_MessageHistoryId]
GO

ALTER TABLE [app].[MessageQueueProduct]  WITH CHECK ADD  CONSTRAINT [FK_MessageQueueProduct_MessageStatusId] FOREIGN KEY([MessageStatusId])
REFERENCES [app].[MessageStatus] ([MessageStatusId])
GO

ALTER TABLE [app].[MessageQueueProduct] CHECK CONSTRAINT [FK_MessageQueueProduct_MessageStatusId]
GO

ALTER TABLE [app].[MessageQueueProduct]  WITH CHECK ADD  CONSTRAINT [FK_MessageQueueProduct_MessageTypeId] FOREIGN KEY([MessageTypeId])
REFERENCES [app].[MessageType] ([MessageTypeId])
GO

ALTER TABLE [app].[MessageQueueProduct] CHECK CONSTRAINT [FK_MessageQueueProduct_MessageTypeId]
GO
