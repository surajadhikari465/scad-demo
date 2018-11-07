CREATE TABLE [dbo].[StoreCompetitorStore] (
    [Store_No]          INT     NOT NULL,
    [CompetitorStoreID] INT     NOT NULL,
    [Priority]          TINYINT NOT NULL,
    CONSTRAINT [PK_StoreCompetitorStore] PRIMARY KEY CLUSTERED ([Store_No] ASC, [CompetitorStoreID] ASC),
    CONSTRAINT [FK_StoreCompetitorStore_CompetitorStore] FOREIGN KEY ([CompetitorStoreID]) REFERENCES [dbo].[CompetitorStore] ([CompetitorStoreID]),
    CONSTRAINT [FK_StoreCompetitorStore_Store] FOREIGN KEY ([Store_No]) REFERENCES [dbo].[Store] ([Store_No])
);


GO
GRANT SELECT
    ON OBJECT::[dbo].[StoreCompetitorStore] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[StoreCompetitorStore] TO [IRMAReportsRole]
    AS [dbo];

