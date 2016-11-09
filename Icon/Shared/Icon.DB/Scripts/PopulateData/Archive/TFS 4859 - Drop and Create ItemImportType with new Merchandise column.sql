DROP TYPE [app].[ItemImportType]
GO

/****** Object:  UserDefinedTableType [app].[ItemImportType]    Script Date: 10/7/2014 10:46:09 AM ******/
CREATE TYPE [app].[ItemImportType] AS TABLE(
	[ScanCode] [nvarchar](13) NOT NULL,
	[Product Description] [nvarchar](255) NOT NULL,
	[POS Description] [nvarchar](255) NOT NULL,
	[Package Unit] [nvarchar](255) NOT NULL,
	[Food Stamp Eligible] [nvarchar](255) NOT NULL,
	[POS Scale Tare] [nvarchar](255) NOT NULL,
	[Brand ID] [nvarchar](9) NOT NULL,
	[Browsing Hierarchy ID] [nvarchar](9) NOT NULL,
	[Merchandise Hierarchy ID] [nvarchar](9) NOT NULL,
	[Tax Class ID] [nvarchar](9) NOT NULL
)
GO
