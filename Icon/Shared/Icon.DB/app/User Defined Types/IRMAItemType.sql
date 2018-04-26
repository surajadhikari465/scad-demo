CREATE TYPE [app].[IRMAItemType] AS TABLE(
	[RegionCode] [varchar](2) NOT NULL,
	[Identifier] [varchar](13) NOT NULL,
	[DefaultIdentifier] [bit] NOT NULL,
	[BrandName] [varchar](25) NOT NULL,
	[ItemDescription] [varchar](60) NOT NULL,
	[PosDescription] [varchar](60) NOT NULL,
	[PackageUnit] [int] NOT NULL,
	[RetailSize] [decimal](9, 4) NOT NULL,
	[RetailUom] [varchar](100) NULL,
	[FoodStamp] [bit] NOT NULL,
	[PosScaleTare] [decimal](18, 0) NOT NULL,
	[DepartmentSale] [bit] NOT NULL,
	[GiftCard] [bit] NULL,
	[TaxClassID] [int] NULL,
	[MerchandiseClassID] [int] NULL,
	[IrmaSubTeamName] [varchar](100) NULL,
	[NationalClassID] [int] NULL
)
GO