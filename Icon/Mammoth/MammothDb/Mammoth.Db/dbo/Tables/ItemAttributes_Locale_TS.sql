CREATE TABLE [dbo].[ItemAttributes_Locale_TS](
	[Region]				[nchar](2)		DEFAULT ('TS')		NOT NULL,
	[ItemAttributeLocaleID] [int]			IDENTITY(1,1)		NOT NULL,
	[ItemID]				[int]								NOT NULL,
	[BusinessUnitID]		[int]								NOT NULL,
	[Discount_Case]			[bit]			DEFAULT ((0))		NOT NULL,
	[Discount_TM]			[bit]			DEFAULT ((0))		NOT NULL,
	[Restriction_Age]		[tinyint]							NULL,
	[Restriction_Hours]		[bit]			DEFAULT ((0))		NOT NULL,
	[Authorized]			[bit]			DEFAULT ((0))		NOT NULL,
	[Discontinued]			[bit]			DEFAULT ((0))		NOT NULL,
	[LocalItem]				[bit]			DEFAULT ((0))		NOT NULL,
	[LabelTypeDesc]			[nvarchar](255)						NULL,
	[Product_Code]			[nvarchar](255)						NULL,
	[RetailUnit]			[nvarchar](255)						NULL,
	[Sign_Desc]				[nvarchar](255)						NULL,
	[Locality]				[nvarchar](255)						NULL,
	[Sign_RomanceText_Long]	[nvarchar](max)						NULL,
	[Sign_RomanceText_Short] [nvarchar](255)					NULL,
	[MSRP]					[smallmoney]	DEFAULT ((0))		NOT NULL,
	[AddedDate]				[datetime]		DEFAULT (getdate())	NOT NULL,
	[ModifiedDate]			[datetime]							NULL,
 CONSTRAINT [PK_ItemAttributes_Locale_TS] PRIMARY KEY CLUSTERED ([Region] ASC,	[ItemAttributeLocaleID] ASC)
 WITH (FILLFACTOR = 100) ON [FG_RM],
 CONSTRAINT [CK_ItemAttributes_Locale_TS_Restriction_Age] CHECK  ([Restriction_Age]=(21) OR [Restriction_Age]=(18))
)TEXTIMAGE_ON [FG_RM];
GO

CREATE INDEX [IX_ItemAttributesLocale_TS_ItemID_BusinessUnitID_Region_ItemAttributeLocaleID] ON [dbo].[ItemAttributes_Locale_TS]
(
	[ItemID] ASC,
	[BusinessUnitID] ASC,
	[Region] ASC,
	[ItemAttributeLocaleID] ASC
)
INCLUDE ([AddedDate]) ON [PRIMARY]


