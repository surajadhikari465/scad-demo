CREATE TABLE [stage].[ItemStoreKeysEPlum]
(
	[BusinessUnitID] INT NOT NULL,
	[ItemID] INT NOT NULL,
	[InsertDateUtc] DATETIME2(7) NOT NULL
)
GO

CREATE CLUSTERED INDEX [CX_ItemStoreKeysEPlum_BusinessUnit_InsertDateUtc] ON [stage].[ItemStoreKeysEPlum] ([BusinessUnitID] ASC, [InsertDateUtc] ASC)
GO

GRANT SELECT, UPDATE, INSERT ON [stage].[ItemStoreKeysEPlum] TO dds_eplum_role
GO

GRANT SELECT, INSERT ON [stage].[ItemStoreKeysEPlum] TO TibcoRole
GO