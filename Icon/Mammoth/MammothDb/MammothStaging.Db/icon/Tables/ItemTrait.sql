CREATE TABLE [icon].[ItemTrait] (
    [traitID]    INT            NOT NULL,
    [itemID]     INT            NOT NULL,
    [uomID]      NVARCHAR (5)   NULL,
    [traitValue] NVARCHAR (255) NULL,
    [localeID]   INT            NOT NULL,
    CONSTRAINT [PK_ItemTrait] PRIMARY KEY CLUSTERED ([traitID] ASC, [itemID] ASC, [localeID] ASC) WITH (FILLFACTOR = 100)
);

