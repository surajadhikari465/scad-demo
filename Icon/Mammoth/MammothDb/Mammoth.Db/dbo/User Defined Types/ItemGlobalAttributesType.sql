CREATE TYPE [dbo].ItemGlobalAttributesType AS TABLE
(
	[ItemID]						INT				NOT NULL,
    [ItemTypeID]					INT				NULL,
    [ScanCode]						NVARCHAR (13)	NULL,
    [SubBrickID]					INT				NULL,
    [NationalClassID]				INT				NULL,
    [BrandHCID]						INT				NULL,
    [TaxClassHCID]					INT				NULL,
    [Desc_Product]					NVARCHAR (255)	NULL,
    [Desc_POS]						NVARCHAR (255)	NULL,
    [PackageUnit]					NVARCHAR (255)	NULL,
    [RetailSize]					NVARCHAR (255)	NULL,
    [RetailUOM]						NVARCHAR (255)	NULL,
    [PSNumber]						NVARCHAR (255)	NULL,
	[FoodStampEligible]				BIT				NULL,
	[Desc_CustomerFriendly]			NVARCHAR(255)  NULL
)
GO

GRANT EXEC ON type::dbo.ItemGlobalAttributesType TO MammothRole
GO