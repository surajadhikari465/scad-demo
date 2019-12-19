CREATE TABLE [dbo].[HierarchyClass] (
[hierarchyClassID] INT  NOT NULL IDENTITY  
, [hierarchyLevel] INT  NULL  
, [hierarchyID] INT  NOT NULL  
, [hierarchyParentClassID] INT  NULL  
, [hierarchyClassName] NVARCHAR(255)  NULL  
)
GO
ALTER TABLE [dbo].[HierarchyClass] WITH CHECK ADD CONSTRAINT [HierarchyPrototype_HierarchyClass_FK1] FOREIGN KEY (
[hierarchyID]
, [hierarchyLevel]
)
REFERENCES [dbo].[HierarchyPrototype] (
[hierarchyID]
, [hierarchyLevel]
)
GO
ALTER TABLE [dbo].[HierarchyClass] WITH CHECK ADD CONSTRAINT [Hierarchy_HierarchyClass_FK1] FOREIGN KEY (
[hierarchyID]
)
REFERENCES [dbo].[Hierarchy] (
[hierarchyID]
)
GO
ALTER TABLE [dbo].[HierarchyClass] WITH CHECK ADD CONSTRAINT [HierarchyClass_HierarchyClass_FK1] FOREIGN KEY (
[hierarchyParentClassID]
)
REFERENCES [dbo].[HierarchyClass] (
[hierarchyClassID]
)
GO
ALTER TABLE [dbo].[HierarchyClass] ADD CONSTRAINT [HierarchyClass_PK] PRIMARY KEY CLUSTERED (
[hierarchyClassID]
)
GO

CREATE NONCLUSTERED INDEX [IX_HierarchyClass_hierarchyClassId] ON [dbo].[HierarchyClass] ([hierarchyClassId])
INCLUDE ([hierarchyClassName],[hierarchyId],[hierarchyParentClassId])
GO

