CREATE TABLE [dbo].[ItemTrait] (
    [traitID]    INT            NOT NULL,
    [itemID]     INT            NOT NULL,
    [uomID]      NVARCHAR (5)   NULL,
    [traitValue] NVARCHAR (300) NULL,
    [localeID]   INT            NOT NULL,
    CONSTRAINT [ItemTrait_PK] PRIMARY KEY CLUSTERED ([traitID] ASC, [itemID] ASC, [localeID] ASC) WITH (FILLFACTOR = 80),
    CONSTRAINT [Item_ItemTrait_FK1] FOREIGN KEY ([itemID]) REFERENCES [dbo].[Item] ([itemID]),
    CONSTRAINT [Locale_ItemTrait_FK1] FOREIGN KEY ([localeID]) REFERENCES [dbo].[Locale] ([localeID]),
    CONSTRAINT [Trait_ItemTrait_FK1] FOREIGN KEY ([traitID]) REFERENCES [dbo].[Trait] ([traitID])
);

GO

CREATE NONCLUSTERED INDEX IX_ItemTrait_itemID_localeID ON dbo.ItemTrait (itemID, localeID) INCLUDE (traitValue);
GO