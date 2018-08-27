﻿CREATE TABLE [stage].[ItemLocale]
(
	[Region]					NVARCHAR(2)		NOT NULL,
	[BusinessUnitID]			INT				NOT NULL,
	[ScanCode]					NVARCHAR(13)	NOT NULL,
	[Discount_Case]				BIT				DEFAULT (0) NOT NULL,
	[Discount_TM]				BIT				DEFAULT (0) NOT NULL,
	[Restriction_Age]			INT				NULL,
	[Restriction_Hours]			BIT				DEFAULT (0) NOT NULL,
	[Authorized]				BIT				DEFAULT (0) NOT NULL,
	[Discontinued]				BIT				NULL,
	[LabelTypeDesc]				NVARCHAR(4)		NULL,
	[LocalItem]					BIT				NULL,
	[ScaleItem]					BIT				DEFAULT (0) NOT NULL,
	[Product_Code]				NVARCHAR(15)	NULL,
	[RetailUnit]				NVARCHAR(25)	NULL,
	[Sign_Desc]					NVARCHAR(60)	NULL,
	[Locality]					NVARCHAR(50)	NULL,
	[Sign_RomanceText_Long]		NVARCHAR(300)	NULL,
	[Sign_RomanceText_Short]	NVARCHAR(140)	NULL,
	[Msrp]						SMALLMONEY		NOT NULL,
	[OrderedByInfor]			BIT				NULL,
    [AltRetailSize]				NUMERIC(9,4)	NULL,
    [AltRetailUOM]				NVARCHAR(25)	NULL,
    [DefaultScanCode]			BIT				NULL,
    [IrmaItemKey]			    INT 			NULL,
	[Timestamp]					DATETIME		NOT NULL,
	[TransactionId]				UNIQUEIDENTIFIER NOT NULL
)
GO

CREATE CLUSTERED INDEX [IX_ItemLocale_Clustered] ON [stage].[ItemLocale]
(
	[Region],
	[BusinessUnitID],
	[ScanCode],
	[TransactionId]
)
GO