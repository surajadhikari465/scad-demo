CREATE TABLE [dbo].[HierarchyClassTrait] (
[traitID] INT  NOT NULL  
, [HierarchyClassID] INT  NOT NULL  
, [uomID] int  NULL  
, [traitValue] NVARCHAR(255)  NULL  
)
GO
ALTER TABLE [dbo].[HierarchyClassTrait] WITH CHECK ADD CONSTRAINT [UOM_HierarchyClassTrait_FK1] FOREIGN KEY (
[uomID]
)
REFERENCES [dbo].[UOM] (
[uomID]
)
GO
ALTER TABLE [dbo].[HierarchyClassTrait] WITH CHECK ADD CONSTRAINT [Trait_HierarchyClassTrait_FK1] FOREIGN KEY (
[traitID]
)
REFERENCES [dbo].[Trait] (
[traitID]
)
GO
ALTER TABLE [dbo].[HierarchyClassTrait] WITH CHECK ADD CONSTRAINT [HierarchyClass_HierarchyClassTrait_FK1] FOREIGN KEY (
[HierarchyClassID]
)
REFERENCES [dbo].[HierarchyClass] (
[HierarchyClassID]
)
GO
ALTER TABLE [dbo].[HierarchyClassTrait] ADD CONSTRAINT [HierarchyClassTrait_PK] PRIMARY KEY CLUSTERED (
[traitID]
, [HierarchyClassID]
)
GO

CREATE NONCLUSTERED INDEX IX_HierarchyClassTrait_HierarchyClassId ON dbo.HierarchyClassTrait(HierarchyClassId)
INCLUDE(traitValue)
GO