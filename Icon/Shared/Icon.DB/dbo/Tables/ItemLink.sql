CREATE TABLE [dbo].[ItemLink] (
[parentItemID] INT NOT NULL  
, [childItemID] INT NOT NULL  
, [localeID] INT NOT NULL  
)
GO
ALTER TABLE [dbo].[ItemLink] WITH CHECK ADD CONSTRAINT [Item_ItemLink_FK1] FOREIGN KEY (
[parentItemID]
)
REFERENCES [dbo].[Item] (
[itemID]
)
GO
ALTER TABLE [dbo].[ItemLink] WITH CHECK ADD CONSTRAINT [Locale_ItemLink_FK1] FOREIGN KEY (
[localeID]
)
REFERENCES [dbo].[Locale] (
[localeID]
)
GO
ALTER TABLE [dbo].[ItemLink] WITH CHECK ADD CONSTRAINT [Item_ItemLink_FK2] FOREIGN KEY (
[childItemID]
)
REFERENCES [dbo].[Item] (
[itemID]
)
GO
ALTER TABLE [dbo].[ItemLink] ADD CONSTRAINT [ItemLink_PK] PRIMARY KEY CLUSTERED (
[parentItemID]
, [childItemID]
, [localeID]
)
GO
CREATE NONCLUSTERED INDEX IX_ItemLink_childItemID_localeID ON dbo.ItemLink (childItemID, localeID) INCLUDE (parentItemID)
GO