CREATE TABLE [app].[BusinessUnitRegionMapping](
	[businessUnitRegionMappingID] [int] IDENTITY(1,1) NOT NULL,
	[businessUnit] [int] NOT NULL,
	[regionCode] [varchar](2) NOT NULL,
	[insertDate] [datetime2](7) NOT NULL,
	[lastUpdateDate] [datetime2](7) NOT NULL
 CONSTRAINT [PK_BusinessUnitRegionMapping] PRIMARY KEY CLUSTERED 
(
	[BusinessUnitRegionMappingID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 80) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE [app].[BusinessUnitRegionMapping] ADD  DEFAULT (getdate()) FOR [insertDate]
GO

ALTER TABLE [app].[BusinessUnitRegionMapping] ADD  DEFAULT (getdate()) FOR [lastUpdateDate]
GO