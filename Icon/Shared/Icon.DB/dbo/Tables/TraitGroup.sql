CREATE TABLE [dbo].[TraitGroup] (
[traitGroupID] INT  NOT NULL IDENTITY,
[traitGroupCode] NVARCHAR(3)  NOT NULL, 
[traitGroupDesc] NVARCHAR(255)  NULL,  
CONSTRAINT [AK_traitGroupCode_traitGroupCode] UNIQUE NONCLUSTERED ([traitGroupCode] ASC) WITH (FILLFACTOR = 80)
)
GO
ALTER TABLE [dbo].[TraitGroup] ADD CONSTRAINT [TraitGroup_PK] PRIMARY KEY CLUSTERED (
[traitGroupID]
)