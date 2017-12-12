CREATE TABLE [stage].[ItemStoreKeysEsl]
(
	[BusinessUnitID] INT NOT NULL,
	[ItemID] INT NOT NULL,
	[InsertDateUtc] DATETIME2(7) NOT NULL
)
GO

CREATE CLUSTERED INDEX [CX_ItemStoreKeysEsl_BusinessUnit_InsertDateUtc] ON [stage].[ItemStoreKeysEsl] ([BusinessUnitID] ASC, [InsertDateUtc] ASC)
GO

GRANT SELECT, UPDATE, INSERT ON [stage].[ItemStoreKeysEsl] TO dds_esl_role
GO