CREATE TABLE [icon].[Trait] (
    [traitID]      INT            NOT NULL,
    [traitCode]    NVARCHAR (3)   NOT NULL,
    [traitPattern] NVARCHAR (255) NOT NULL,
    [traitDesc]    NVARCHAR (255) NULL,
    [traitGroupID] INT            NULL,
    CONSTRAINT [PK_Trait] PRIMARY KEY CLUSTERED ([traitID] ASC) WITH (FILLFACTOR = 100)
);

