/****** Object:  UserDefinedTableType [dbo].[IconUpdateItemType]    Script Date: 1/8/16 9:16:26 AM ******/
CREATE TYPE [dbo].[IconUpdateItemType] AS TABLE(
	[ItemId] [int] NOT NULL,
	[ValidationDate] [nvarchar](255) NOT NULL,
	[ScanCode] [nvarchar](13) NOT NULL,
	[ScanCodeType] [nvarchar](255) NOT NULL,
	[ProductDescription] [nvarchar](255) NOT NULL,
	[PosDescription] [nvarchar](255) NOT NULL,
	[PackageUnit] [nvarchar](255) NOT NULL,
	[FoodStampEligible] [nvarchar](255) NOT NULL,
	[Tare] [nvarchar](255) NULL,
	[BrandId] [int] NOT NULL,
	[BrandName] [nvarchar](35) NOT NULL,
	[TaxClassName] [nvarchar](50) NOT NULL,
	[NationalClassCode] [nvarchar](50) NOT NULL,
	[SubTeamName] [nvarchar](255) NULL,
	[SubTeamNo] [int] NOT NULL,
	[DeptNo] [int] NOT NULL,
	[SubTeamNotAligned] [bit] NOT NULL,
	[EventTypeId] [int] NULL,
	[AnimalWelfareRating] [nvarchar](10) NULL,
	[Biodynamic] [bit] NULL,
	[CheeseMilkType] [nvarchar](40) NULL,
	[CheeseRaw] [bit] NULL,
	[EcoScaleRating] [nvarchar](30) NULL,
	[GlutenFree] [bit] NULL,
	[Kosher] [bit] NULL,
	[NonGmo] [bit] NULL,
	[Organic] [bit] NULL,
	[PremiumBodyCare] [bit] NULL,
	[FreshOrFrozen] [nvarchar](30) NULL,
	[SeafoodCatchType] [nvarchar](15) NULL,
	[Vegan] [bit] NULL,
	[Vegetarian] [bit] NULL,
	[WholeTrade] [bit] NULL,
	[Msc] [bit] NULL,
	[GrassFed] [bit] NULL,
	[PastureRaised] [bit] NULL,
	[FreeRange] [bit] NULL,			  
	[DryAged] [bit] NULL,
	[AirChilled] [bit] NULL,
	[MadeInHouse] [bit] NULL,
	[HasItemSignAttributes] [bit] NULL,
	[RetailSize] [decimal](9,4),
	[RetailUom] [varchar](5)
	PRIMARY KEY CLUSTERED 
(
	[ItemId] ASC
)WITH (IGNORE_DUP_KEY = OFF)
)
GO

-- Ensure perms are reset when this type is updated, since it must be dropped and recreated to update.
GRANT EXEC ON TYPE::IconUpdateItemType TO IConInterface
go
