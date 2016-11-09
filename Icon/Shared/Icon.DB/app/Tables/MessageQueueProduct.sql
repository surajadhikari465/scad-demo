﻿CREATE TABLE [app].[MessageQueueProduct] (
    [MessageQueueId]         INT            IDENTITY (1, 1) NOT NULL,
    [MessageTypeId]          INT            NOT NULL,
    [MessageStatusId]        INT            NOT NULL,
    [MessageHistoryId]       INT            NULL,
    [InsertDate]             DATETIME2 (7)  CONSTRAINT [DF_MessageQueueProduct_InsertDate] DEFAULT (sysdatetime()) NOT NULL,
    [ItemId]                 INT            NOT NULL,
    [LocaleId]               INT            NOT NULL,
    [ItemTypeCode]           NVARCHAR (3)   NOT NULL,
    [ItemTypeDesc]           NVARCHAR (255) NOT NULL,
    [ScanCodeId]             INT            NOT NULL,
    [ScanCode]               NVARCHAR (13)  NOT NULL,
    [ScanCodeTypeId]         INT            NOT NULL,
    [ScanCodeTypeDesc]       NVARCHAR (255) NOT NULL,
    [ProductDescription]     NVARCHAR (255) NOT NULL,
    [PosDescription]         NVARCHAR (255) NOT NULL,
    [PackageUnit]            NVARCHAR (255) NOT NULL,
	[RetailSize]			 NVARCHAR (255) NULL,
	[RetailUom]				 NVARCHAR (255) NULL,
    [FoodStampEligible]      NVARCHAR (255) NOT NULL,
	[ProhibitDiscount]		 BIT			NOT NULL,
    [DepartmentSale]         NVARCHAR (255) NOT NULL,
    [BrandId]                INT            NOT NULL,
    [BrandName]              NVARCHAR (255) NOT NULL,
    [BrandLevel]             INT            NOT NULL,
    [BrandParentId]          INT            NULL,
    [BrowsingClassId]        INT            NULL,
    [BrowsingClassName]      NVARCHAR (255) NULL,
    [BrowsingLevel]          INT            NULL,
    [BrowsingParentId]       INT            NULL,
    [MerchandiseClassId]     INT            NOT NULL,
    [MerchandiseClassName]   NVARCHAR (255) NOT NULL,
    [MerchandiseLevel]       INT            NOT NULL,
    [MerchandiseParentId]	 INT            NULL,
    [TaxClassId]             INT            NOT NULL,
    [TaxClassName]           NVARCHAR (255) NOT NULL,
    [TaxLevel]               INT            NOT NULL,
    [TaxParentId]            INT            NULL,
    [FinancialClassId]       NVARCHAR (32)  NOT NULL,
    [FinancialClassName]     NVARCHAR (255) NOT NULL,
    [FinancialLevel]         INT            NOT NULL,
    [FinancialParentId]      INT            NULL,
    [InProcessBy]            INT            NULL,
    [ProcessedDate]          DATETIME2 (7)  NULL,
	[AnimalWelfareRating] [nvarchar](50) NULL,
	[Biodynamic] [nvarchar](1) NULL,
	[CheeseMilkType] [nvarchar](50) NULL,
	[CheeseRaw] [nvarchar](1) NULL,
	[EcoScaleRating] [nvarchar](50) NULL,
	[GlutenFreeAgency] [nvarchar](255) NULL,
	[HealthyEatingRating] [nvarchar](50) NULL,
	[KosherAgency] [nvarchar](50) NULL, 
	[Msc] [nvarchar](1) NULL,
	[NonGmoAgency] [nvarchar](255) NULL,
	[OrganicAgency] [nvarchar](255) NULL,
	[PremiumBodyCare] [nvarchar](1) NULL,
	[SeafoodFreshOrFrozen] [nvarchar](50) NULL,
	[SeafoodCatchType] [nvarchar](50) NULL,
	[VeganAgency] [nvarchar](255) NULL,
	[Vegetarian] [nvarchar](1) NULL,
	[WholeTrade] [nvarchar](1) NULL,
	[GrassFed] [nvarchar](1) NULL,
	[PastureRaised] [nvarchar](1) NULL,
	[FreeRange] [nvarchar](1) NULL,
	[DryAged] [nvarchar](1) NULL,
	[AirChilled] [nvarchar](1) NULL,
	[MadeInHouse] [nvarchar](1) NULL,
    CONSTRAINT [PK_MessageQueueProduct] PRIMARY KEY CLUSTERED ([MessageQueueId] ASC) WITH (FILLFACTOR = 80),
    CONSTRAINT [FK_MessageQueueProduct_MessageHistoryId] FOREIGN KEY ([MessageHistoryId]) REFERENCES [app].[MessageHistory] ([MessageHistoryId]) ON DELETE CASCADE,
    CONSTRAINT [FK_MessageQueueProduct_MessageStatusId] FOREIGN KEY ([MessageStatusId]) REFERENCES [app].[MessageStatus] ([MessageStatusId]),
    CONSTRAINT [FK_MessageQueueProduct_MessageTypeId] FOREIGN KEY ([MessageTypeId]) REFERENCES [app].[MessageType] ([MessageTypeId])
);