CREATE TABLE [dbo].[HierarchyPrototype] (
[hierarchyID] INT  NOT NULL  
, [hierarchyLevel] INT  NOT NULL  
, [hierarchyLevelName] NVARCHAR(255)  NULL  
, [itemsAttached] BIT  NULL  
)
GO
ALTER TABLE [dbo].[HierarchyPrototype] WITH CHECK ADD CONSTRAINT [Hierarchy_HierarchyPrototype_FK1] FOREIGN KEY (
[hierarchyID]
)
REFERENCES [dbo].[Hierarchy] (
[hierarchyID]
)
GO
ALTER TABLE [dbo].[HierarchyPrototype] ADD CONSTRAINT [HierarchyPrototype_PK] PRIMARY KEY CLUSTERED (
[hierarchyID]
, [hierarchyLevel]
)