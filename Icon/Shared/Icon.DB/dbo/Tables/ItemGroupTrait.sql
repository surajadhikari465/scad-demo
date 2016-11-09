CREATE TABLE [dbo].[ItemGroupTrait] (
[traitID] INT  NOT NULL  
, [ItemGroupID] INT  NOT NULL  
, [uomID] int  NULL  
, [traitValue] NVARCHAR(255)  NULL  
)
GO
ALTER TABLE [dbo].[ItemGroupTrait] WITH CHECK ADD CONSTRAINT [UOM_ItemGroupTrait_FK1] FOREIGN KEY (
[uomID]
)
REFERENCES [dbo].[UOM] (
[uomID]
)
GO
ALTER TABLE [dbo].[ItemGroupTrait] WITH CHECK ADD CONSTRAINT [Trait_ItemGroupTrait_FK1] FOREIGN KEY (
[traitID]
)
REFERENCES [dbo].[Trait] (
[traitID]
)
GO
ALTER TABLE [dbo].[ItemGroupTrait] WITH CHECK ADD CONSTRAINT [ItemGroup_ItemGroupTrait_FK1] FOREIGN KEY (
[ItemGroupID]
)
REFERENCES [dbo].[ItemGroup] (
[ItemGroupID]
)
GO
ALTER TABLE [dbo].[ItemGroupTrait] ADD CONSTRAINT [ItemGroupTrait_PK] PRIMARY KEY CLUSTERED (
[traitID]
, [ItemGroupID]
)