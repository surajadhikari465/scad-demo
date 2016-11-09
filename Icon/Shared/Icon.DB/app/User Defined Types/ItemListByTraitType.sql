CREATE TYPE [app].[ItemListByTraitType] AS TABLE (
    [itemID]     INT            NULL,
    [traitID]  INT            NULL,
    [traitDesc]  NVARCHAR (255) NULL,
    [traitValue] NVARCHAR (255) NULL);

