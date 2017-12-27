CREATE TABLE [stage].[ItemLocaleSupplier]
(
	[Region]			NVARCHAR(2)		NOT NULL,
	[ScanCode]			NVARCHAR(13)	NOT NULL,
	[BusinessUnitId]	INT				NOT NULL,
	[SupplierName]		NVARCHAR(255)	NOT NULL,
	[SupplierItemId]	NVARCHAR(20)	NULL,
	[SupplierCaseSize]	DECIMAL(9,4)	NULL,
	[IrmaVendorKey]		NVARCHAR(10)	NULL,
	[Timestamp]			DATETIME		NOT NULL,
	[TransactionId]		UNIQUEIDENTIFIER NOT NULL
)
GO

CREATE CLUSTERED INDEX [IX_ItemLocaleSupplier_Clustered] ON [stage].[ItemLocaleSupplier]
(
	[Region] ASC,
	[BusinessUnitId] ASC,
	[ScanCode] ASC,
	[TransactionId] ASC
)
GO