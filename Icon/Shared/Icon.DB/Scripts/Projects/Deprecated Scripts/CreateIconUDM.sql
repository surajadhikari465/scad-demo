ALTER TABLE [dbo].[Trait] DROP CONSTRAINT [FK_Trait_TraitGroup]
GO
ALTER TABLE [dbo].[ScanCode] DROP CONSTRAINT [FK_ScanCode_ScanCodeType]
GO
ALTER TABLE [dbo].[ScanCode] DROP CONSTRAINT [FK_ScanCode_Item]
GO
ALTER TABLE [dbo].[LocaleItemPrice] DROP CONSTRAINT [FK_LocaleItemPrice_ItemPrice]
GO
ALTER TABLE [dbo].[Locale] DROP CONSTRAINT [FK_Locale_Locale]
GO
ALTER TABLE [dbo].[ItemTrait] DROP CONSTRAINT [FK_ItemTrait_UOM]
GO
ALTER TABLE [dbo].[ItemTrait] DROP CONSTRAINT [FK_ItemTrait_Trait]
GO
ALTER TABLE [dbo].[ItemTrait] DROP CONSTRAINT [FK_ItemTrait_Item]
GO
ALTER TABLE [dbo].[ItemPricing] DROP CONSTRAINT [FK_ItemPricing_UOM]
GO
ALTER TABLE [dbo].[ItemPricing] DROP CONSTRAINT [FK_ItemPricing_Locale]
GO
ALTER TABLE [dbo].[ItemPricing] DROP CONSTRAINT [FK_ItemPricing_ItemPricingType]
GO
ALTER TABLE [dbo].[ItemPricing] DROP CONSTRAINT [FK_ItemPricing_Item]
GO
ALTER TABLE [dbo].[ItemPrice] DROP CONSTRAINT [FK_ItemPrice_ItemPricing]
GO
ALTER TABLE [dbo].[ItemPrice] DROP CONSTRAINT [FK_ItemPrice_CurrencyType]
GO
ALTER TABLE [dbo].[ItemLink] DROP CONSTRAINT [FK_ItemLink_Item_Parent]
GO
ALTER TABLE [dbo].[ItemLink] DROP CONSTRAINT [FK_ItemLink_Item_Child]
GO
ALTER TABLE [dbo].[ItemHierarchyClass] DROP CONSTRAINT [FK_ItemHierarchyClass_Item]
GO
ALTER TABLE [dbo].[ItemHierarchyClass] DROP CONSTRAINT [FK_ItemHierarchyClass_HierarchyClass1]
GO
ALTER TABLE [dbo].[ItemGroupMember] DROP CONSTRAINT [FK_ItemGroupMember_ItemGroup]
GO
ALTER TABLE [dbo].[ItemGroupMember] DROP CONSTRAINT [FK_ItemGroupMember_Item]
GO
ALTER TABLE [dbo].[Item] DROP CONSTRAINT [FK_Item_Locale]
GO
ALTER TABLE [dbo].[Item] DROP CONSTRAINT [FK_Item_ItemType]
GO
ALTER TABLE [dbo].[HierarchyClass] DROP CONSTRAINT [FK_HierarchyClass_HierarchyClass]
GO
ALTER TABLE [dbo].[HierarchyClass] DROP CONSTRAINT [FK_HierarchyClass_Hierarchy]
GO
/****** Object:  Table [dbo].[UOM]    Script Date: 1/9/2014 11:27:29 AM ******/
DROP TABLE [dbo].[UOM]
GO
/****** Object:  Table [dbo].[TraitGroup]    Script Date: 1/9/2014 11:27:29 AM ******/
DROP TABLE [dbo].[TraitGroup]
GO
/****** Object:  Table [dbo].[Trait]    Script Date: 1/9/2014 11:27:29 AM ******/
DROP TABLE [dbo].[Trait]
GO
/****** Object:  Table [dbo].[ScanCodeType]    Script Date: 1/9/2014 11:27:29 AM ******/
DROP TABLE [dbo].[ScanCodeType]
GO
/****** Object:  Table [dbo].[ScanCode]    Script Date: 1/9/2014 11:27:29 AM ******/
DROP TABLE [dbo].[ScanCode]
GO
/****** Object:  Table [dbo].[LocaleItemPrice]    Script Date: 1/9/2014 11:27:29 AM ******/
DROP TABLE [dbo].[LocaleItemPrice]
GO
/****** Object:  Table [dbo].[Locale]    Script Date: 1/9/2014 11:27:29 AM ******/
DROP TABLE [dbo].[Locale]
GO
/****** Object:  Table [dbo].[ItemType]    Script Date: 1/9/2014 11:27:29 AM ******/
DROP TABLE [dbo].[ItemType]
GO
/****** Object:  Table [dbo].[ItemTrait]    Script Date: 1/9/2014 11:27:29 AM ******/
DROP TABLE [dbo].[ItemTrait]
GO
/****** Object:  Table [dbo].[ItemPricingType]    Script Date: 1/9/2014 11:27:29 AM ******/
DROP TABLE [dbo].[ItemPricingType]
GO
/****** Object:  Table [dbo].[ItemPricing]    Script Date: 1/9/2014 11:27:29 AM ******/
DROP TABLE [dbo].[ItemPricing]
GO
/****** Object:  Table [dbo].[ItemPrice]    Script Date: 1/9/2014 11:27:29 AM ******/
DROP TABLE [dbo].[ItemPrice]
GO
/****** Object:  Table [dbo].[ItemLink]    Script Date: 1/9/2014 11:27:29 AM ******/
DROP TABLE [dbo].[ItemLink]
GO
/****** Object:  Table [dbo].[ItemHierarchyClass]    Script Date: 1/9/2014 11:27:29 AM ******/
DROP TABLE [dbo].[ItemHierarchyClass]
GO
/****** Object:  Table [dbo].[ItemGroupMember]    Script Date: 1/9/2014 11:27:29 AM ******/
DROP TABLE [dbo].[ItemGroupMember]
GO
/****** Object:  Table [dbo].[ItemGroup]    Script Date: 1/9/2014 11:27:29 AM ******/
DROP TABLE [dbo].[ItemGroup]
GO
/****** Object:  Table [dbo].[Item]    Script Date: 1/9/2014 11:27:29 AM ******/
DROP TABLE [dbo].[Item]
GO
/****** Object:  Table [dbo].[HierarchyClass]    Script Date: 1/9/2014 11:27:29 AM ******/
DROP TABLE [dbo].[HierarchyClass]
GO
/****** Object:  Table [dbo].[Hierarchy]    Script Date: 1/9/2014 11:27:29 AM ******/
DROP TABLE [dbo].[Hierarchy]
GO
/****** Object:  Table [dbo].[CurrencyType]    Script Date: 1/9/2014 11:27:29 AM ******/
DROP TABLE [dbo].[CurrencyType]
GO
/****** Object:  Table [dbo].[CurrencyType]    Script Date: 1/9/2014 11:27:29 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[CurrencyType](
	[currencyTypeID] [int] IDENTITY(1,1) NOT NULL,
	[currencyTypeDesc] [varchar](50) NOT NULL,
 CONSTRAINT [PK_CurrencyType] PRIMARY KEY CLUSTERED 
(
	[currencyTypeID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 80) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Hierarchy]    Script Date: 1/9/2014 11:27:29 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Hierarchy](
	[hierarchyID] [int] IDENTITY(1,1) NOT NULL,
	[hierarchyName] [varchar](100) NOT NULL,
 CONSTRAINT [PK_Hierarchy] PRIMARY KEY CLUSTERED 
(
	[hierarchyID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 80) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[HierarchyClass]    Script Date: 1/9/2014 11:27:29 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[HierarchyClass](
	[hierarchyClassID] [int] NOT NULL,
	[hierarchyID] [int] NOT NULL,
	[parentHierarchyClassID] [int] NULL,
	[hierarchyClassName] [varchar](100) NOT NULL,
 CONSTRAINT [PK_HierarchyClass] PRIMARY KEY CLUSTERED 
(
	[hierarchyClassID] ASC,
	[hierarchyID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 80) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Item]    Script Date: 1/9/2014 11:27:29 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Item](
	[itemID] [int] identity NOT NULL,
	[localeID] [int] NOT NULL,
	[itemTypeCode] [int] NULL,
	[itemDesc] [varchar](100) NOT NULL,
 CONSTRAINT [PK_Item] PRIMARY KEY CLUSTERED 
(
	[itemID] ASC,
	[localeID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 80) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[ItemGroup]    Script Date: 1/9/2014 11:27:29 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[ItemGroup](
	[itemGroupID] [int] IDENTITY(1,1) NOT NULL,
	[itemGroupDesc] [varchar](100) NOT NULL,
 CONSTRAINT [PK_ItemGroup] PRIMARY KEY CLUSTERED 
(
	[itemGroupID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 80) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[ItemGroupMember]    Script Date: 1/9/2014 11:27:29 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ItemGroupMember](
	[itemID] [int] NOT NULL,
	[localeID] [int] NOT NULL,
	[itemGroupID] [int] NOT NULL,
 CONSTRAINT [PK_ItemGroupMember] PRIMARY KEY CLUSTERED 
(
	[itemID] ASC,
	[localeID] ASC,
	[itemGroupID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 80) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[ItemHierarchyClass]    Script Date: 1/9/2014 11:27:29 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ItemHierarchyClass](
	[itemID] [int] NOT NULL,
	[localeID] [int] NOT NULL,
	[hierarchyClassID] [int] NOT NULL,
	[hierarchyID] [int] NOT NULL,
 CONSTRAINT [PK_ItemHierarchyClass] PRIMARY KEY CLUSTERED 
(
	[itemID] ASC,
	[localeID] ASC,
	[hierarchyClassID] ASC,
	[hierarchyID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 80) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[ItemLink]    Script Date: 1/9/2014 11:27:29 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ItemLink](
	[parentItemID] [int] NOT NULL,
	[parentLocaleID] [int] NOT NULL,
	[childItemID] [int] NOT NULL,
	[childLocaleID] [int] NOT NULL,
 CONSTRAINT [PK_ItemLink] PRIMARY KEY CLUSTERED 
(
	[parentItemID] ASC,
	[parentLocaleID] ASC,
	[childItemID] ASC,
	[childLocaleID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 80) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[ItemPrice]    Script Date: 1/9/2014 11:27:29 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ItemPrice](
	[itemPricingID] [int] NOT NULL,
	[itemID] [int] NOT NULL,
	[localeID] [int] NOT NULL,
	[itemPricingUOM] [int] NOT NULL,
	[itemPriceAmt] [smallmoney] NOT NULL,
	[breakPointStartQty] [int] NOT NULL,
	[breakPointEndQty] [int] NOT NULL,
	[effectiveDate] [datetime] NOT NULL,
	[currencyTypeID] [int] NULL,
 CONSTRAINT [PK_ItemPrice] PRIMARY KEY CLUSTERED 
(
	[itemPricingID] ASC,
	[itemID] ASC,
	[localeID] ASC,
	[itemPricingUOM] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 80) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[ItemPricing]    Script Date: 1/9/2014 11:27:29 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ItemPricing](
	[itemPricingID] [int] NOT NULL,
	[itemID] [int] NOT NULL,
	[localeID] [int] NOT NULL,
	[itemPricingUOM] [int] NOT NULL,
	[itemPricingTypeCode] [int] NULL,
	[itemPriceStartDate] [datetime] NOT NULL,
 CONSTRAINT [PK_ItemPricing] PRIMARY KEY CLUSTERED 
(
	[itemPricingID] ASC,
	[itemID] ASC,
	[localeID] ASC,
	[itemPricingUOM] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 80) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[ItemPricingType]    Script Date: 1/9/2014 11:27:29 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[ItemPricingType](
	[itemPricingTypeCode] [int] NOT NULL,
	[itemPricingTypeDesc] [varchar](50) NOT NULL,
 CONSTRAINT [PK_ItemPricingType] PRIMARY KEY CLUSTERED 
(
	[itemPricingTypeCode] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 80) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[ItemTrait]    Script Date: 1/9/2014 11:27:29 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[ItemTrait](
	[traitCode] [int] NOT NULL,
	[itemID] [int] NOT NULL,
	[localeID] [int] NOT NULL,
	[uomID] [int] NULL,
	[traitValue] [varchar](100) NOT NULL,
 CONSTRAINT [PK_ItemTrait] PRIMARY KEY CLUSTERED 
(
	[traitCode] ASC,
	[itemID] ASC,
	[localeID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 80) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[ItemType]    Script Date: 1/9/2014 11:27:29 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[ItemType](
	[itemTypeCode] [int] NOT NULL,
	[itemTypeDesc] [varchar](100) NOT NULL,
 CONSTRAINT [PK_ItemType] PRIMARY KEY CLUSTERED 
(
	[itemTypeCode] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 80) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Locale]    Script Date: 1/9/2014 11:27:29 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Locale](
	[localeID] [int] identity NOT NULL,
	[ownerOrgPartyID] [int] NOT NULL,
	[localeName] [varchar](100) NOT NULL,
	[localeOpenDate] [date] NULL,
	[localeCloseDate] [date] NULL,
	[localeTypeCode] [int] NOT NULL,
	[parentLocaleID] [int] NULL,
 CONSTRAINT [PK_Locale] PRIMARY KEY CLUSTERED 
(
	[localeID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 80) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[LocaleItemPrice]    Script Date: 1/9/2014 11:27:29 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[LocaleItemPrice](
	[itemPricingID] [int] NOT NULL,
	[itemID] [int] NOT NULL,
	[itemLocaleID] [int] NOT NULL,
	[itemPricingUOM] [int] NOT NULL,
	[pricingLocaleID] [int] NOT NULL,
 CONSTRAINT [PK_LocaleItemPrice] PRIMARY KEY CLUSTERED 
(
	[itemPricingID] ASC,
	[itemID] ASC,
	[itemLocaleID] ASC,
	[itemPricingUOM] ASC,
	[pricingLocaleID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 80) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[ScanCode]    Script Date: 1/9/2014 11:27:29 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ScanCode](
	[itemID] [int] NOT NULL,
	[localeID] [int] NOT NULL,
	[scanCode] [int] NOT NULL,
	[scanCodeTypeID] [int] NULL,
 CONSTRAINT [PK_ScanCode] PRIMARY KEY CLUSTERED 
(
	[itemID] ASC,
	[localeID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 80) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[ScanCodeType]    Script Date: 1/9/2014 11:27:29 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[ScanCodeType](
	[scanCodeTypeID] [int] IDENTITY(1,1) NOT NULL,
	[scanCodeTypeDesc] [varchar](50) NOT NULL,
 CONSTRAINT [PK_ScanCodeType] PRIMARY KEY CLUSTERED 
(
	[scanCodeTypeID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 80) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Trait]    Script Date: 1/9/2014 11:27:29 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Trait](
	[traitCode] [int] identity NOT NULL,
	[traitPattern] [varchar](100) NOT NULL,
	[traitDesc] [varchar](300) NOT NULL,
	[traitGroupCode] [int] NULL,
 CONSTRAINT [PK_Trait] PRIMARY KEY CLUSTERED 
(
	[traitCode] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 80) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[TraitGroup]    Script Date: 1/9/2014 11:27:29 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[TraitGroup](
	[traitGroupCode] [int] identity NOT NULL,
	[traitGroupDesc] [varchar](100) NOT NULL,
 CONSTRAINT [PK_TraitGroup] PRIMARY KEY CLUSTERED 
(
	[traitGroupCode] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 80) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[UOM]    Script Date: 1/9/2014 11:27:29 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[UOM](
	[uomID] [int] NOT NULL,
	[uomName] [varchar](50) NOT NULL,
 CONSTRAINT [PK_UOM] PRIMARY KEY CLUSTERED 
(
	[uomID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 80) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
ALTER TABLE [dbo].[HierarchyClass]  WITH CHECK ADD  CONSTRAINT [FK_HierarchyClass_Hierarchy] FOREIGN KEY([hierarchyID])
REFERENCES [dbo].[Hierarchy] ([hierarchyID])
GO
ALTER TABLE [dbo].[HierarchyClass] CHECK CONSTRAINT [FK_HierarchyClass_Hierarchy]
GO
ALTER TABLE [dbo].[HierarchyClass]  WITH CHECK ADD  CONSTRAINT [FK_HierarchyClass_HierarchyClass] FOREIGN KEY([hierarchyClassID], [hierarchyID])
REFERENCES [dbo].[HierarchyClass] ([hierarchyClassID], [hierarchyID])
GO
ALTER TABLE [dbo].[HierarchyClass] CHECK CONSTRAINT [FK_HierarchyClass_HierarchyClass]
GO
ALTER TABLE [dbo].[Item]  WITH CHECK ADD  CONSTRAINT [FK_Item_ItemType] FOREIGN KEY([itemTypeCode])
REFERENCES [dbo].[ItemType] ([itemTypeCode])
ON UPDATE SET NULL
ON DELETE SET NULL
GO
ALTER TABLE [dbo].[Item] CHECK CONSTRAINT [FK_Item_ItemType]
GO
ALTER TABLE [dbo].[Item]  WITH CHECK ADD  CONSTRAINT [FK_Item_Locale] FOREIGN KEY([localeID])
REFERENCES [dbo].[Locale] ([localeID])
GO
ALTER TABLE [dbo].[Item] CHECK CONSTRAINT [FK_Item_Locale]
GO
ALTER TABLE [dbo].[ItemGroupMember]  WITH CHECK ADD  CONSTRAINT [FK_ItemGroupMember_Item] FOREIGN KEY([itemID], [localeID])
REFERENCES [dbo].[Item] ([itemID], [localeID])
GO
ALTER TABLE [dbo].[ItemGroupMember] CHECK CONSTRAINT [FK_ItemGroupMember_Item]
GO
ALTER TABLE [dbo].[ItemGroupMember]  WITH CHECK ADD  CONSTRAINT [FK_ItemGroupMember_ItemGroup] FOREIGN KEY([itemGroupID])
REFERENCES [dbo].[ItemGroup] ([itemGroupID])
GO
ALTER TABLE [dbo].[ItemGroupMember] CHECK CONSTRAINT [FK_ItemGroupMember_ItemGroup]
GO
ALTER TABLE [dbo].[ItemHierarchyClass]  WITH CHECK ADD  CONSTRAINT [FK_ItemHierarchyClass_HierarchyClass1] FOREIGN KEY([hierarchyClassID], [hierarchyID])
REFERENCES [dbo].[HierarchyClass] ([hierarchyClassID], [hierarchyID])
GO
ALTER TABLE [dbo].[ItemHierarchyClass] CHECK CONSTRAINT [FK_ItemHierarchyClass_HierarchyClass1]
GO
ALTER TABLE [dbo].[ItemHierarchyClass]  WITH CHECK ADD  CONSTRAINT [FK_ItemHierarchyClass_Item] FOREIGN KEY([itemID], [localeID])
REFERENCES [dbo].[Item] ([itemID], [localeID])
GO
ALTER TABLE [dbo].[ItemHierarchyClass] CHECK CONSTRAINT [FK_ItemHierarchyClass_Item]
GO
ALTER TABLE [dbo].[ItemLink]  WITH CHECK ADD  CONSTRAINT [FK_ItemLink_Item_Child] FOREIGN KEY([childItemID], [childLocaleID])
REFERENCES [dbo].[Item] ([itemID], [localeID])
GO
ALTER TABLE [dbo].[ItemLink] CHECK CONSTRAINT [FK_ItemLink_Item_Child]
GO
ALTER TABLE [dbo].[ItemLink]  WITH CHECK ADD  CONSTRAINT [FK_ItemLink_Item_Parent] FOREIGN KEY([parentItemID], [parentLocaleID])
REFERENCES [dbo].[Item] ([itemID], [localeID])
GO
ALTER TABLE [dbo].[ItemLink] CHECK CONSTRAINT [FK_ItemLink_Item_Parent]
GO
ALTER TABLE [dbo].[ItemPrice]  WITH CHECK ADD  CONSTRAINT [FK_ItemPrice_CurrencyType] FOREIGN KEY([currencyTypeID])
REFERENCES [dbo].[CurrencyType] ([currencyTypeID])
ON UPDATE SET NULL
ON DELETE SET NULL
GO
ALTER TABLE [dbo].[ItemPrice] CHECK CONSTRAINT [FK_ItemPrice_CurrencyType]
GO
ALTER TABLE [dbo].[ItemPrice]  WITH CHECK ADD  CONSTRAINT [FK_ItemPrice_ItemPricing] FOREIGN KEY([itemPricingID], [itemID], [localeID], [itemPricingUOM])
REFERENCES [dbo].[ItemPricing] ([itemPricingID], [itemID], [localeID], [itemPricingUOM])
GO
ALTER TABLE [dbo].[ItemPrice] CHECK CONSTRAINT [FK_ItemPrice_ItemPricing]
GO
ALTER TABLE [dbo].[ItemPricing]  WITH CHECK ADD  CONSTRAINT [FK_ItemPricing_Item] FOREIGN KEY([itemID], [localeID])
REFERENCES [dbo].[Item] ([itemID], [localeID])
GO
ALTER TABLE [dbo].[ItemPricing] CHECK CONSTRAINT [FK_ItemPricing_Item]
GO
ALTER TABLE [dbo].[ItemPricing]  WITH CHECK ADD  CONSTRAINT [FK_ItemPricing_ItemPricingType] FOREIGN KEY([itemPricingTypeCode])
REFERENCES [dbo].[ItemPricingType] ([itemPricingTypeCode])
ON UPDATE SET NULL
ON DELETE SET NULL
GO
ALTER TABLE [dbo].[ItemPricing] CHECK CONSTRAINT [FK_ItemPricing_ItemPricingType]
GO
ALTER TABLE [dbo].[ItemPricing]  WITH CHECK ADD  CONSTRAINT [FK_ItemPricing_Locale] FOREIGN KEY([localeID])
REFERENCES [dbo].[Locale] ([localeID])
GO
ALTER TABLE [dbo].[ItemPricing] CHECK CONSTRAINT [FK_ItemPricing_Locale]
GO
ALTER TABLE [dbo].[ItemPricing]  WITH CHECK ADD  CONSTRAINT [FK_ItemPricing_UOM] FOREIGN KEY([itemPricingUOM])
REFERENCES [dbo].[UOM] ([uomID])
GO
ALTER TABLE [dbo].[ItemPricing] CHECK CONSTRAINT [FK_ItemPricing_UOM]
GO
ALTER TABLE [dbo].[ItemTrait]  WITH CHECK ADD  CONSTRAINT [FK_ItemTrait_Item] FOREIGN KEY([itemID], [localeID])
REFERENCES [dbo].[Item] ([itemID], [localeID])
GO
ALTER TABLE [dbo].[ItemTrait] CHECK CONSTRAINT [FK_ItemTrait_Item]
GO
ALTER TABLE [dbo].[ItemTrait]  WITH CHECK ADD  CONSTRAINT [FK_ItemTrait_Trait] FOREIGN KEY([traitCode])
REFERENCES [dbo].[Trait] ([traitCode])
GO
ALTER TABLE [dbo].[ItemTrait] CHECK CONSTRAINT [FK_ItemTrait_Trait]
GO
ALTER TABLE [dbo].[ItemTrait]  WITH CHECK ADD  CONSTRAINT [FK_ItemTrait_UOM] FOREIGN KEY([uomID])
REFERENCES [dbo].[UOM] ([uomID])
ON UPDATE SET NULL
ON DELETE SET NULL
GO
ALTER TABLE [dbo].[ItemTrait] CHECK CONSTRAINT [FK_ItemTrait_UOM]
GO
ALTER TABLE [dbo].[Locale]  WITH CHECK ADD  CONSTRAINT [FK_Locale_Locale] FOREIGN KEY([parentLocaleID])
REFERENCES [dbo].[Locale] ([localeID])
GO
ALTER TABLE [dbo].[Locale] CHECK CONSTRAINT [FK_Locale_Locale]
GO
ALTER TABLE [dbo].[LocaleItemPrice]  WITH CHECK ADD  CONSTRAINT [FK_LocaleItemPrice_ItemPrice] FOREIGN KEY([pricingLocaleID])
REFERENCES [dbo].[Locale] ([localeID])
GO
ALTER TABLE [dbo].[LocaleItemPrice] CHECK CONSTRAINT [FK_LocaleItemPrice_ItemPrice]
GO
ALTER TABLE [dbo].[ScanCode]  WITH CHECK ADD  CONSTRAINT [FK_ScanCode_Item] FOREIGN KEY([itemID], [localeID])
REFERENCES [dbo].[Item] ([itemID], [localeID])
GO
ALTER TABLE [dbo].[ScanCode] CHECK CONSTRAINT [FK_ScanCode_Item]
GO
ALTER TABLE [dbo].[ScanCode]  WITH CHECK ADD  CONSTRAINT [FK_ScanCode_ScanCodeType] FOREIGN KEY([scanCodeTypeID])
REFERENCES [dbo].[ScanCodeType] ([scanCodeTypeID])
ON UPDATE SET NULL
ON DELETE SET NULL
GO
ALTER TABLE [dbo].[ScanCode] CHECK CONSTRAINT [FK_ScanCode_ScanCodeType]
GO
ALTER TABLE [dbo].[Trait]  WITH CHECK ADD  CONSTRAINT [FK_Trait_TraitGroup] FOREIGN KEY([traitGroupCode])
REFERENCES [dbo].[TraitGroup] ([traitGroupCode])
ON UPDATE SET NULL
ON DELETE SET NULL
GO
ALTER TABLE [dbo].[Trait] CHECK CONSTRAINT [FK_Trait_TraitGroup]
GO