CREATE TABLE [stage].[ItemLocaleExtended]
(
	[Region]			NVARCHAR(2)		NOT NULL,
	[ScanCode]			NVARCHAR(13)	NOT NULL,
	[BusinessUnitId]	INT				NOT NULL,
	[AttributeId]		INT				NOT NULL,
	[AttributeValue]	NVARCHAR(max)	NULL,
	[Timestamp]			DATETIME		NOT NULL,
	[TransactionId]		UNIQUEIDENTIFIER NOT NULL
)
GO

CREATE CLUSTERED INDEX [IX_ItemLocaleExtended_Clustered] ON [stage].[ItemLocaleExtended]
(
	[Region] ASC,
	[BusinessUnitId] ASC,
	[ScanCode] ASC,
	[TransactionId] ASC
)
GO

CREATE NONCLUSTERED INDEX [IX_ItemLocaleExtended_AttributeID] ON [stage].[ItemLocaleExtended]
(
	[TransactionId] ASC,
	[BusinessUnitId] ASC,
	[Region] ASC,
	[AttributeId] ASC
)
INCLUDE([AttributeValue],[ScanCode])
GO