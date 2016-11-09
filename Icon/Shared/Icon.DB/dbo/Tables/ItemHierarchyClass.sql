CREATE TABLE [dbo].[ItemHierarchyClass] (
    [itemID]           INT NOT NULL,
    [hierarchyClassID] INT NOT NULL,
    [localeID]         INT NULL,
    CONSTRAINT [ItemHierarchyClass_PK] PRIMARY KEY CLUSTERED ([hierarchyClassID] ASC, [itemID] ASC) WITH (FILLFACTOR = 80),
    CONSTRAINT [HierarchyClass_ItemHierarchyClass_FK1] FOREIGN KEY ([hierarchyClassID]) REFERENCES [dbo].[HierarchyClass] ([hierarchyClassID]),
    CONSTRAINT [Item_ItemHierarchyClass_FK1] FOREIGN KEY ([itemID]) REFERENCES [dbo].[Item] ([itemID]),
    CONSTRAINT [Locale_ItemHierarchyClass_FK1] FOREIGN KEY ([localeID]) REFERENCES [dbo].[Locale] ([localeID])
);


GO

GO

GO

GO
CREATE NONCLUSTERED INDEX [ItemHierarchyClass_itemID]
    ON [dbo].[ItemHierarchyClass]([itemID] ASC)
    INCLUDE([hierarchyClassID]) WITH (FILLFACTOR = 80);

