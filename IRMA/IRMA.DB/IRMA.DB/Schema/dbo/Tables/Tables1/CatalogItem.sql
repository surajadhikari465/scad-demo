CREATE TABLE [dbo].[CatalogItem] (
    [CatalogItemID] INT           IDENTITY (1, 1) NOT NULL,
    [CatalogID]     INT           NOT NULL,
    [ItemKey]       INT           NOT NULL,
    [ItemNotes]     VARCHAR (MAX) NULL,
    [InsertDate]    SMALLDATETIME CONSTRAINT [DF_CatalogItem_InsertDate] DEFAULT (getdate()) NOT NULL,
    [InsertUser]    VARCHAR (50)  NULL,
    CONSTRAINT [PK_CatalogItem] PRIMARY KEY CLUSTERED ([CatalogItemID] ASC),
    CONSTRAINT [FK_Catalog_CatalogItem] FOREIGN KEY ([CatalogID]) REFERENCES [dbo].[Catalog] ([CatalogID])
);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'PK', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'CatalogItem', @level2type = N'COLUMN', @level2name = N'CatalogItemID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'FK - StoreOrderGuide.Catalog used to relate Item to Catalog', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'CatalogItem', @level2type = N'COLUMN', @level2name = N'CatalogID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'FK - ItemCatalog.Item used to retrieve item information', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'CatalogItem', @level2type = N'COLUMN', @level2name = N'ItemKey';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Catalog specific description of item', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'CatalogItem', @level2type = N'COLUMN', @level2name = N'ItemNotes';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'INSERT date', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'CatalogItem', @level2type = N'COLUMN', @level2name = N'InsertDate';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'INSERT user', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'CatalogItem', @level2type = N'COLUMN', @level2name = N'InsertUser';

