CREATE TABLE [dbo].[ItemAttributes_Locale_SP] (
    [Region]                 NCHAR (2)      DEFAULT ('SP') NOT NULL,
    [ItemAttributeLocaleID]  INT            IDENTITY (1, 1) NOT NULL,
    [ItemID]                 INT            NOT NULL,
    [BusinessUnitID]         INT            NOT NULL,
    [Discount_Case]          BIT            DEFAULT ((0)) NOT NULL,
    [Discount_TM]            BIT            DEFAULT ((0)) NOT NULL,
    [Restriction_Age]        TINYINT        NULL,
    [Restriction_Hours]      BIT            DEFAULT ((0)) NOT NULL,
    [Authorized]             BIT            DEFAULT ((0)) NOT NULL,
    [Discontinued]           BIT            DEFAULT ((0)) NOT NULL,
    [LocalItem]              BIT            DEFAULT ((0)) NOT NULL,
    [ScaleItem]              BIT            DEFAULT ((0)) NOT NULL,
    [OrderedByInfor]         BIT            DEFAULT ((0)) NOT NULL,
    [DefaultScanCode]        BIT            DEFAULT ((0)) NOT NULL,
    [LabelTypeDesc]          NVARCHAR (255) NULL,
    [Product_Code]           NVARCHAR (255) NULL,
    [RetailUnit]             NVARCHAR (255) NULL,
    [Sign_Desc]              NVARCHAR (255) NULL,
    [Locality]               NVARCHAR (255) NULL,
    [Sign_RomanceText_Long]  NVARCHAR (MAX) NULL,
    [Sign_RomanceText_Short] NVARCHAR (255) NULL,
    [AltRetailUOM]           NVARCHAR  (25) NULL,
    [AltRetailSize]          NUMERIC  (9,4) NULL,
    [MSRP]                   SMALLMONEY     DEFAULT ((0)) NOT NULL,
    [AddedDate]              DATETIME       DEFAULT (getdate()) NOT NULL,
    [ModifiedDate]           DATETIME       NULL,
    CONSTRAINT [PK_ItemAttributes_Locale_SP] PRIMARY KEY CLUSTERED ([Region] ASC, [ItemAttributeLocaleID] ASC) WITH (FILLFACTOR = 100) ON [FG_SP],
    CONSTRAINT [CK_ItemAttributes_Locale_SP_Restriction_Age] CHECK ([Restriction_Age]=(21) OR [Restriction_Age]=(18)),
    CONSTRAINT [CK_ItemAttributes_Locale_SP_Region] CHECK ([Region] = 'SP')
) TEXTIMAGE_ON [FG_SP];
GO

CREATE INDEX [IX_ItemAttributesLocale_SP_ItemID_BusinessUnitID_Region_ItemAttributeLocaleID] ON [dbo].[ItemAttributes_Locale_SP]
(
	[ItemID] ASC,
	[BusinessUnitID] ASC,
	[Region] ASC,
	[ItemAttributeLocaleID] ASC
)
INCLUDE ([AddedDate]) ON [PRIMARY]
