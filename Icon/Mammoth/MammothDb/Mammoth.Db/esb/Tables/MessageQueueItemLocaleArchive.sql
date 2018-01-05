﻿CREATE TABLE [esb].[MessageQueueItemLocaleArchive]
(
    [MessageQueueId]             INT            NOT NULL,
    [MessageTypeId]              INT            NOT NULL,
    [MessageStatusId]            INT            NOT NULL,
    [MessageHistoryId]           INT            NULL,
    [MessageActionId]            INT            NOT NULL,
    [InsertDate]                 DATETIME2 (7)  NOT NULL,
    [RegionCode]                 VARCHAR (4)    NOT NULL,
    [BusinessUnitId]             INT            NOT NULL,
    [ItemId]                     INT            NOT NULL,
    [ItemTypeCode]               NVARCHAR (3)   NOT NULL,
    [ItemTypeDesc]               NVARCHAR (255) NOT NULL,
    [LocaleName]                 VARCHAR (255)  NOT NULL,
    [ScanCode]                   VARCHAR (13)   NOT NULL,
	[CaseDiscount]				 BIT			NOT NULL,
	[TmDiscount]				 BIT			NOT NULL,
	[AgeRestriction]			 INT			NULL,
	[RestrictedHours]		     BIT            NOT NULL,
	[Authorized]				 BIT			NOT NULL,
	[Discontinued]				 BIT			NOT NULL,
	[LabelTypeDescription]		 NVARCHAR(255)	NULL,
	[LocalItem]					 BIT			NOT NULL,
	[ProductCode]				 NVARCHAR(255)	NULL,
	[RetailUnit]				 NVARCHAR(255)  NULL,
	[SignDescription]			 NVARCHAR(255)	NULL,
	[Locality]					 NVARCHAR(255)  NULL,
	[SignRomanceLong]			 NVARCHAR(300)  NULL,
	[SignRomanceShort]			 NVARCHAR(255)  NULL,
	[ColorAdded]				 BIT			NULL,
	[CountryOfProcessing]		 NVARCHAR(255)	NULL,
	[Origin]					 NVARCHAR(255)  NULL,
	[ElectronicShelfTag]		 BIT			NULL,
	[Exclusive]					 DATETIME2(7)   NULL,
	[NumberOfDigitsSentToScale]  INT			NULL,
	[ChicagoBaby]				 NVARCHAR(255)	NULL,
	[TagUom]					 NVARCHAR(255)	NULL,
	[LinkedItem]				 NVARCHAR(255)	NULL,
	[ScaleExtraText]			 NVARCHAR(MAX)  NULL,
    [Msrp]						 SMALLMONEY		NULL, 
    [InProcessBy]                INT            NULL,
    [ProcessedDate]              DATETIME2 (7)  NULL,
	[SupplierName]				 NVARCHAR(255)  NULL,
	[IrmaVendorKey]				 NVARCHAR(10)   NULL,
    [SupplierItemID]			 NVARCHAR(20)   NULL,
	[SupplierCaseSize]			 DECIMAL(9,4)	NULL,
	[OrderedByInfor]			 BIT            NULL,
);
go


