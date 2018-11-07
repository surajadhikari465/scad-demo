CREATE TABLE [dbo].[CatalogStore] (
    [CatalogStoreID] INT           IDENTITY (1, 1) NOT NULL,
    [CatalogID]      INT           NOT NULL,
    [StoreNo]        INT           NULL,
    [InsertDate]     SMALLDATETIME CONSTRAINT [DF_CatalogStore_InsertDate] DEFAULT (getdate()) NOT NULL,
    [InsertUser]     VARCHAR (50)  NULL,
    CONSTRAINT [PK_CatalogStore] PRIMARY KEY CLUSTERED ([CatalogStoreID] ASC),
    CONSTRAINT [FK_Catalog_CatalogStore] FOREIGN KEY ([CatalogID]) REFERENCES [dbo].[Catalog] ([CatalogID])
);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'PK', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'CatalogStore', @level2type = N'COLUMN', @level2name = N'CatalogStoreID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'FK - StoreOrderGuide.Catalog used to relate Store to Catalog', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'CatalogStore', @level2type = N'COLUMN', @level2name = N'CatalogID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'FK - ItemCatalog.Store used to retrieve store information', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'CatalogStore', @level2type = N'COLUMN', @level2name = N'StoreNo';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'INSERT date', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'CatalogStore', @level2type = N'COLUMN', @level2name = N'InsertDate';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'INSERT user', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'CatalogStore', @level2type = N'COLUMN', @level2name = N'InsertUser';

