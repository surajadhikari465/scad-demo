DROP TABLE [app].[MessageQueueLocale]
GO

CREATE TABLE [app].[MessageQueueLocale](
	[MessageQueueId] [int] IDENTITY(1,1) NOT NULL,
	[MessageTypeId] [int] NOT NULL,
	[MessageStatusId] [int] NOT NULL,
	[MessageHistoryId] [int] NULL,
	[InsertDate] [datetime2](7) NOT NULL,
	[LocaleId] [int] NOT NULL,
	[OwnerOrgPartyId] [int] NOT NULL,
	[StoreAbbreviation] [nvarchar](3) NULL,
	[LocaleName] [nvarchar](255) NOT NULL,
	[LocaleOpenDate] [date] NULL,
	[LocaleCloseDate] [date] NULL,
	[LocaleTypeId] [int] NOT NULL,
	[ParentLocaleId] [int] NULL,
	[BusinessUnitId] [nvarchar](255) NOT NULL,
	[AddressId] [int] NULL,
	[AddressUsageCode] [nvarchar](3) NULL,
	[CountryName] [nvarchar](255) NULL,
	[CountryCode] [nvarchar](3) NULL,
	[TerritoryName] [nvarchar](255) NULL,
	[TerritoryCode] [nvarchar](3) NULL,
	[CityName] [nvarchar](255) NULL,
	[PostalCode] [nvarchar](15) NULL,
	[Latitude] [nvarchar](255) NULL,
	[Longitude] [nvarchar](255) NULL,
	[AddressLine1] [nvarchar](255) NULL,
	[AddressLine2] [nvarchar](255) NULL,
	[AddressLine3] [nvarchar](255) NULL,
	[TimezoneCode] [nvarchar](5) NOT NULL,
	[TimezoneName] [nvarchar](100) NOT NULL,
	[PhoneNumber] [nvarchar](50) NOT NULL,
	[InProcessBy] [int] NULL,
	[ProcessedDate] [datetime2](7) NULL,
 CONSTRAINT [PK_app.MessageQueueLocale] PRIMARY KEY CLUSTERED 
(
	[MessageQueueId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 80) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE [app].[MessageQueueLocale]  WITH NOCHECK ADD  CONSTRAINT [FK_app.MessageQueueLocale_MessageHistory] FOREIGN KEY([MessageHistoryId])
REFERENCES [app].[MessageHistory] ([MessageHistoryId])
ON DELETE CASCADE
GO

ALTER TABLE [app].[MessageQueueLocale] CHECK CONSTRAINT [FK_app.MessageQueueLocale_MessageHistory]
GO

ALTER TABLE [app].[MessageQueueLocale]  WITH CHECK ADD  CONSTRAINT [FK_app.MessageQueueLocale_MessageStatus] FOREIGN KEY([MessageStatusId])
REFERENCES [app].[MessageStatus] ([MessageStatusId])
GO

ALTER TABLE [app].[MessageQueueLocale] CHECK CONSTRAINT [FK_app.MessageQueueLocale_MessageStatus]
GO

ALTER TABLE [app].[MessageQueueLocale]  WITH CHECK ADD  CONSTRAINT [FK_app.MessageQueueLocale_MessageType] FOREIGN KEY([MessageTypeId])
REFERENCES [app].[MessageType] ([MessageTypeId])
GO

ALTER TABLE [app].[MessageQueueLocale] CHECK CONSTRAINT [FK_app.MessageQueueLocale_MessageType]
GO
