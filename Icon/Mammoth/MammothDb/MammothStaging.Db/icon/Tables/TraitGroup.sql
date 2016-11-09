CREATE TABLE [icon].[TraitGroup] (
    [traitGroupID]   INT            NOT NULL,
    [traitGroupCode] NVARCHAR (3)   NOT NULL,
    [traitGroupDesc] NVARCHAR (255) NULL,
    CONSTRAINT [TraitGroup_PK] PRIMARY KEY CLUSTERED ([traitGroupID] ASC) WITH (FILLFACTOR = 100)
);

