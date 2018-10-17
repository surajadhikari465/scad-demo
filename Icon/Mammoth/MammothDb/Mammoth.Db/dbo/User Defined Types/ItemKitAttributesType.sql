CREATE TYPE [dbo].[ItemKitAttributesType] AS TABLE
(
	[ItemID]						INT				NOT NULL,
	[KitchenItem]					BIT				NOT NULL DEFAULT 0,		
	[HospitalityItem]				BIT				NOT NULL DEFAULT 0,
	[ImageUrl]						NVARCHAR(255)	NULL,
	[KitchenDescription]			NVARCHAR(15)	NULL
)
GO

GRANT EXEC ON type::dbo.ItemKitAttributesType TO MammothRole
GO

