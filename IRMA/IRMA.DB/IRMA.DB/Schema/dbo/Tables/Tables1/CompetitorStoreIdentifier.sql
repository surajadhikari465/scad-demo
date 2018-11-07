CREATE TABLE [dbo].[CompetitorStoreIdentifier] (
    [CompetitorStoreIdentifierID] INT          IDENTITY (1, 1) NOT NULL,
    [CompetitorStoreID]           INT          NOT NULL,
    [Identifier]                  VARCHAR (50) NOT NULL,
    CONSTRAINT [PK_CompetitorStoreIdentifier] PRIMARY KEY CLUSTERED ([CompetitorStoreIdentifierID] ASC),
    CONSTRAINT [FK_CompetitorStoreIdentifier_CompetitorStore] FOREIGN KEY ([CompetitorStoreID]) REFERENCES [dbo].[CompetitorStore] ([CompetitorStoreID])
);


GO
CREATE NONCLUSTERED INDEX [IX_CompetitorStoreIdentifier_Identifier]
    ON [dbo].[CompetitorStoreIdentifier]([Identifier] ASC);


GO
CREATE UNIQUE NONCLUSTERED INDEX [UX_CompetitorStoreIdentifier]
    ON [dbo].[CompetitorStoreIdentifier]([CompetitorStoreID] ASC, [Identifier] ASC);


GO
GRANT SELECT
    ON OBJECT::[dbo].[CompetitorStoreIdentifier] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[CompetitorStoreIdentifier] TO [IRMAReportsRole]
    AS [dbo];

