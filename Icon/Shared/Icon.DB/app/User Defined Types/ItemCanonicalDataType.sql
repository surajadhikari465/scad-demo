create TYPE [app].[ItemCanonicalDataType] AS TABLE(
	[ScanCode] [nvarchar](13) NOT NULL,
	[Product Description] [nvarchar](255) NOT NULL,
	[POS Description] [nvarchar](255) NOT NULL,
	[Package Unit] [nvarchar](255) NOT NULL,
	[Food Stamp Eligible] [nvarchar](255) NOT NULL,
	[POS Scale Tare] [nvarchar](255) NOT NULL,
	[Retail Size] [nvarchar](10) NULL,
	[Retail Uom] [nvarchar](15) NULL,
	[Brand ID] [nvarchar](9) NOT NULL,
	[Browsing Hierarchy ID] [nvarchar](9) NULL,
	[Merchandise Hierarchy ID] [nvarchar](9) NULL,
	[Tax Class ID] [nvarchar](9) NULL,
	[National Class ID] [nvarchar](9) NULL
)
GO