CREATE TABLE [dbo].[HierarchyClass] (
    [HierarchyClassID]   INT            NOT NULL,
    [HierarchyID]        INT            NULL,
    [HierarchyClassName] NVARCHAR (255) NULL,
    [AddedDate]          DATETIME       DEFAULT (getdate()) NOT NULL,
    [ModifiedDate]       DATETIME       NULL,
    CONSTRAINT [PK_HierarchyClass] PRIMARY KEY CLUSTERED ([HierarchyClassID] ASC) WITH (FILLFACTOR = 100),
    CONSTRAINT [FK_HierarchyClass_HierarchyID] FOREIGN KEY ([HierarchyID]) REFERENCES [dbo].[Hierarchy] ([hierarchyID])
);







