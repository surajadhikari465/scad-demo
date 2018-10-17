CREATE TABLE [dbo].[ItemAttributes_Kit]
(
	[ItemAttributeId] INT IDENTITY (1,1) NOT NULL,
	ItemId	INT	NOT NULL, 
	[KitchenItem]					BIT			   NOT NULL
		CONSTRAINT DF_Items_KitchenItem DEFAULT (0),
	[HospitalityItem]				BIT				NOT NULL
		CONSTRAINT DF_Items_HospitalityItem DEFAULT (0), 
	[Desc_Kitchen]					NVARCHAR(15)	NULL,
	[ImageUrl]						NVARCHAR(255)	NULL,
	[InsertUTCDate] DATETIME2 NOT NULL
	CONSTRAINT	DF_ItemAttributesKit_InsertUTCDate DEFAULT	(SYSUTCDATETIME()), 
    [ModifiedUTCDate] DATETIME2 NULL, 
    CONSTRAINT PK_ItemAttributesKit_ItemAttributeId PRIMARY KEY CLUSTERED (ItemAttributeId ASC) WITH (FILLFACTOR = 100),
	UNIQUE NONCLUSTERED ([ItemID] ASC) WITH (FILLFACTOR = 100)
)
GO 
GRANT SELECT, UPDATE, INSERT, DELETE ON dbo.ItemAttributes_Kit TO MammothRole
GO

