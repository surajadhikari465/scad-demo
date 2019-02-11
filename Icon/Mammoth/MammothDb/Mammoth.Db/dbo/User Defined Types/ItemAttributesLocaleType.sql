CREATE TYPE [dbo].[ItemAttributesLocaleType] AS TABLE(
	[ScanCode] [nvarchar](13) NULL,
	[BusinessUnitID] [int] NOT NULL,
	[Discount_Case] [bit] NOT NULL,
	[Discount_TM] [bit] NOT NULL,
	[Restriction_Age] [tinyint] NULL,
	[Restriction_Hours] [bit] NOT NULL,
	[Authorized] [bit] NOT NULL,
	[Discontinued] [bit] NOT NULL,
	[LocalItem] [bit] NOT NULL,
	[ScaleItem] [bit] NOT NULL,
	[OrderedByInfor] [bit] NOT NULL,
	[DefaultScanCode] [bit] NOT NULL,
	[LabelTypeDesc] [nvarchar](255) NULL,
	[Product_Code] [nvarchar](255) NULL,
	[RetailUnit] [nvarchar](255) NULL,
	[Sign_Desc] [nvarchar](255) NULL,
	[Locality] [nvarchar](255) NULL,
	[Sign_RomanceText_Long] [nvarchar](max) NULL,
	[Sign_RomanceText_Short] [nvarchar](255) NULL,
	[AltRetailUOM] [nvarchar](25) NULL,
	[AltRetailSize] [numeric](9, 4) NULL,
	[MSRP] [smallmoney] NOT NULL,
	[IrmaItemKey] [int] NULL);
GO

GRANT EXEC ON type::dbo.ItemAttributesExtType TO MammothRole;
GO