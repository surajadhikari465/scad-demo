CREATE TYPE [app].[CertificationAgencyImportType] AS TABLE(
	[AgencyId] [int] NOT NULL,
	[AgencyName] [nvarchar](255) NOT NULL,
	[GlutenFree] [nvarchar](1) NOT NULL,
	[Kosher] [nvarchar](1) NOT NULL,
	[NonGMO] [nvarchar](1) NOT NULL,
	[Organic] [nvarchar](1) NOT NULL,
	[Vegan] [nvarchar](1) NOT NULL
)