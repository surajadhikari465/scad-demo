CREATE TABLE [dbo].[Item] (
	[itemID] INT NOT NULL IDENTITY,
	[itemTypeID] INT NOT NULL,
	[productKey] INT NOT NULL CONSTRAINT DF_Item_productKey DEFAULT app.fn_GetNextProductKey()
)
GO
ALTER TABLE [dbo].[Item] WITH CHECK ADD CONSTRAINT [ItemType_Item_FK1] FOREIGN KEY (
[itemTypeID]
)
REFERENCES [dbo].[ItemType] (
[itemTypeID]
)
GO
ALTER TABLE [dbo].[Item] ADD CONSTRAINT [Item_PK] PRIMARY KEY CLUSTERED (
[itemID]
)
GO
CREATE NONCLUSTERED INDEX IX_Item_productKey on [dbo].[Item] (productKey)
GO
