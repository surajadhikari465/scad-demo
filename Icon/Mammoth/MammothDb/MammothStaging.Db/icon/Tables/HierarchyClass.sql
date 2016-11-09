CREATE TABLE [icon].[HierarchyClass] (
    [hierarchyClassID]       INT            NOT NULL,
    [hierarchyLevel]         INT            NULL,
    [hierarchyID]            INT            NOT NULL,
    [hierarchyParentClassID] INT            NULL,
    [hierarchyClassName]     NVARCHAR (255) NULL
);

