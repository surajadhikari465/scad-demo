CREATE TABLE [stage].[ItemLocaleSupplierDelete]
(
	[Region]					NVARCHAR(2)		NOT NULL,
	[BusinessUnitID]			INT				NOT NULL,
	[ScanCode]					NVARCHAR(13)	NOT NULL,
	[Timestamp]					DATETIME		NOT NULL,
	[TransactionId]				UNIQUEIDENTIFIER NOT NULL
)
GO

CREATE CLUSTERED INDEX [IX_ItemLocaleSupplierDelete_Clustered] ON [stage].[ItemLocaleSupplierDelete]
(
	[Region],
	[BusinessUnitID],
	[ScanCode],
	[TransactionId]
)
GO