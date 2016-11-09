CREATE TABLE [app].[Regions](
	[RegionId] [int] IDENTITY(1,1) NOT NULL ,
	[RegionCode] [nvarchar](2) NOT NULL unique,
	[RegionName] [nvarchar](255)
 CONSTRAINT [RegionsPK_Id] PRIMARY KEY CLUSTERED 
(
	[RegionId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 80) ON [PRIMARY]
) ON [PRIMARY]



GO

CREATE INDEX [RegionCode_Index] ON [app].[Regions] (RegionCode)
