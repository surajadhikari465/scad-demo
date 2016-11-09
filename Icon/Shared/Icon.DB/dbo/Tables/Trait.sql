CREATE TABLE [dbo].[Trait] (
[traitID] INT  NOT NULL IDENTITY,
[traitCode] NVARCHAR(3)  NOT NULL,
[traitPattern] NVARCHAR(255)  NOT NULL,
[traitDesc] NVARCHAR(255)  NULL,
[traitGroupID] INT  NULL,
CONSTRAINT [AK_traitCode_traitCode] UNIQUE NONCLUSTERED ([traitCode] ASC) WITH (FILLFACTOR = 80)
 
)
GO
ALTER TABLE [dbo].[Trait] WITH CHECK ADD CONSTRAINT [TraitGroup_Trait_FK1] FOREIGN KEY (
[traitGroupID]
)
REFERENCES [dbo].[TraitGroup] (
[traitGroupID]
)
GO
ALTER TABLE [dbo].[Trait] ADD CONSTRAINT [Trait_PK] PRIMARY KEY CLUSTERED (
[traitID]
)