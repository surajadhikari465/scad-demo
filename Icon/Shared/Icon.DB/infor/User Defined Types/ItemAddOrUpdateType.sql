CREATE TYPE [infor].[ItemAddOrUpdateType] AS TABLE(
	[ItemId] [int] NOT NULL,
	[ItemTypeId] [int] NOT NULL,
	[ScanCode] [nvarchar](13) NOT NULL,
	[ScanCodeTypeId] [int] NOT NULL
)
GO