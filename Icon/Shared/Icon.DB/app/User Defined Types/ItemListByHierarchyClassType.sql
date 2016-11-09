CREATE TYPE [app].[ItemListByHierarchyClassType] AS TABLE (
    [itemID]           INT NOT NULL,
    [hierarchyClassID] INT NOT NULL,
    [localeID]         INT NULL,
    PRIMARY KEY CLUSTERED ([itemID] ASC, [hierarchyClassID] ASC));

