CREATE TABLE [dbo].[ItemAttributes_Kit]
(
	[ItemAttributeId] INT IDENTITY (1,1) NOT NULL,
	ItemId	INT	NOT NULL, 
	[KitchenItem]					BIT			   NULL,
	[HospitalityItem]				BIT				NULL, 
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



CREATE INDEX [IX_ItemAttributes_Kit_ItemId] ON [dbo].[ItemAttributes_Kit] (ItemId)
