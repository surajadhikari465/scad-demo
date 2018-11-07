CREATE TABLE [dbo].[CompetitorStoreItemIdentifier] (
    [CompetitorStoreID] INT          NOT NULL,
    [Identifier]        VARCHAR (50) NOT NULL,
    [Item_Key]          INT          NOT NULL,
    CONSTRAINT [PK_CompetitorItemIdentifier] PRIMARY KEY CLUSTERED ([CompetitorStoreID] ASC, [Identifier] ASC),
    CONSTRAINT [FK_CompetitorStoreItemIdentifier_CompetitorStore] FOREIGN KEY ([CompetitorStoreID]) REFERENCES [dbo].[CompetitorStore] ([CompetitorStoreID]),
    CONSTRAINT [FK_CompetitorStoreItemIdentifier_Item] FOREIGN KEY ([Item_Key]) REFERENCES [dbo].[Item] ([Item_Key])
);


GO
GRANT SELECT
    ON OBJECT::[dbo].[CompetitorStoreItemIdentifier] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[CompetitorStoreItemIdentifier] TO [IRMAReportsRole]
    AS [dbo];

