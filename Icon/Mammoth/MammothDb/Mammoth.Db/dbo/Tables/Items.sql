CREATE TABLE [dbo].[Items] (
    [ItemID]						INT            NOT NULL,
    [ItemTypeID]					INT            NULL,
    [ScanCode]						NVARCHAR (13)  NULL,
    [HierarchyMerchandiseID]		INT            NULL,
    [HierarchyNationalClassID]		INT            NULL,
    [BrandHCID]						INT            NULL,
    [TaxClassHCID]					INT            NULL,
    [PSNumber]						INT            NULL,
    [Desc_Product]					NVARCHAR (255) NULL,
    [Desc_POS]						NVARCHAR (255) NULL,
    [PackageUnit]					NVARCHAR (255) NULL,
    [RetailSize]					NVARCHAR (255) NULL,
    [RetailUOM]						NVARCHAR (255) NULL,
    [FoodStampEligible]				BIT            NULL,
	[Desc_CustomerFriendly]			NVARCHAR(255)  NULL,
    [AddedDate]						DATETIME       DEFAULT (getdate()) NOT NULL,
    [ModifiedDate]					DATETIME       NULL,
    CONSTRAINT [PK_Items] PRIMARY KEY CLUSTERED ([ItemID] ASC) WITH (FILLFACTOR = 100),
    UNIQUE NONCLUSTERED ([ItemID] ASC) WITH (FILLFACTOR = 100)
);


GO

CREATE INDEX [IX_Items_ScanCode] ON [dbo].[Items] ([ScanCode]) INCLUDE ([ItemID])

GO

GRANT SELECT ON [dbo].[Items] TO [TibcoRole]
GO
