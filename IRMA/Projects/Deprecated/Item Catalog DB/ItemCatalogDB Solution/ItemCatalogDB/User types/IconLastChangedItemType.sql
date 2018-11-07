CREATE TYPE [dbo].[IconLastChangedItemType] AS TABLE(
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
	[AreNutriFactsUpdated] [bit] NULL,
	[SubTeamName] [nvarchar](255) NULL,
	[SubTeamNo] [int] NOT NULL,
	[DeptNo] [int] NOT NULL,
	[SubTeamNotAligned] [bit] NOT NULL,
	[RetailUom] [nvarchar](4) NOT NULL,
	[RetailSize] [decimal](9, 4) NOT NULL,
	PRIMARY KEY CLUSTERED 
(
	[ItemId] ASC
)WITH (IGNORE_DUP_KEY = OFF)
)
GO

-- Ensure perms are reset when this type is updated, since it must be dropped and recreated to update.
GRANT EXEC ON TYPE::dbo.IconLastChangedItemType TO IConInterface
go

