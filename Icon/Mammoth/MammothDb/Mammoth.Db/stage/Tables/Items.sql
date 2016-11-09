CREATE TABLE [stage].[Items] (
    [ItemID]          INT            NOT NULL,
    [ItemTypeID]      INT            NULL,
    [ScanCode]        NVARCHAR (13)  NULL,
    [SubBrickID]      INT            NULL,
    [NationalClassID] INT            NULL,
    [BrandHCID]       INT            NULL,
    [TaxClassHCID]    INT            NULL,
    [Desc_Product]    NVARCHAR (255) NULL,
    [Desc_POS]        NVARCHAR (255) NULL,
    [PackageUnit]     NVARCHAR (255) NULL,
    [RetailSize]      NVARCHAR (255) NULL,
    [RetailUOM]       NVARCHAR (255) NULL,
    [PSNumber]   NVARCHAR (255) NULL,
	[FoodStampEligible] BIT			 NULL,
    [Timestamp]       DATETIME       NULL,
	[TransactionId]	  UNIQUEIDENTIFIER NOT NULL
);

