create TYPE [dbo].[IconItemWithSubteamType] AS TABLE(
	[ItemId] [int] NOT NULL,
	[ValidationDate] [nvarchar](255) NULL,
	[ScanCode] [nvarchar](13) NOT NULL,
	[ScanCodeType] [nvarchar](255) NULL,
	[ProductDescription] [nvarchar](255) NULL,
	[PosDescription] [nvarchar](255) NULL,
	[PackageUnit] [nvarchar](255) NULL,
	[FoodStampEligible] [nvarchar](255) NULL,
	[Tare] [nvarchar](255) NULL,
	[BrandId] [int] NULL,
	[BrandName] [nvarchar](35) NULL,
	[TaxClassName] [nvarchar](50) NULL,
	[SubTeamName] [nvarchar](255) NULL,
	[SubTeamNo] [int] NOT NULL,
	[DeptNo] [int] NOT NULL,
	[SubTeamNotAligned] [bit] NOT NULL
	PRIMARY KEY CLUSTERED 
(
	[ItemId] ASC
)WITH (IGNORE_DUP_KEY = OFF)
)
GO


