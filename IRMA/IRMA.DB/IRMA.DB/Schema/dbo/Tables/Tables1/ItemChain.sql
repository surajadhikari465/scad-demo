CREATE TABLE [dbo].[ItemChain] (
    [ItemChainId]   INT           IDENTITY (1, 1) NOT NULL,
    [ItemChainDesc] VARCHAR (100) NULL,
    CONSTRAINT [PK_ItemChain] PRIMARY KEY CLUSTERED ([ItemChainId] ASC)
);


GO
CREATE UNIQUE NONCLUSTERED INDEX [ChainName]
    ON [dbo].[ItemChain]([ItemChainDesc] ASC);


GO
GRANT SELECT
    ON OBJECT::[dbo].[ItemChain] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[ItemChain] TO [IRMAReportsRole]
    AS [dbo];

