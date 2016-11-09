CREATE TYPE [app].[ScanCodesByBusinessUnitType] AS TABLE(
	[ScanCode] [nvarchar](13) NOT NULL,
	[BusinessUnitId] [nvarchar](5) NOT NULL
)
