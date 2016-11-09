CREATE TABLE [icon].[ItemHierarchyClass] (
    [itemID]           INT NOT NULL,
    [hierarchyClassID] INT NOT NULL,
    [localeID]         INT NULL,
    CONSTRAINT [PK_ItemHierarchyClass] PRIMARY KEY CLUSTERED ([hierarchyClassID] ASC, [itemID] ASC) WITH (FILLFACTOR = 100)
);

