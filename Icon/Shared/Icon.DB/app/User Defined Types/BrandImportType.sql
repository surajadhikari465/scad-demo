CREATE TYPE [app].[BrandImportType] AS TABLE(
	[BrandId] [int] NOT NULL,
	[BrandName] [nvarchar](255) NOT NULL,
	[BrandAbbreviation] [nvarchar](10) NOT NULL
)