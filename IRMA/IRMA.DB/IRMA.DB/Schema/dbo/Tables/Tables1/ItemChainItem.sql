CREATE TABLE [dbo].[ItemChainItem] (
    [ItemChainId] INT NOT NULL,
    [Item_Key]    INT NOT NULL,
    CONSTRAINT [FK_ItemChainItem_Item] FOREIGN KEY ([Item_Key]) REFERENCES [dbo].[Item] ([Item_Key]),
    CONSTRAINT [FK_ItemChainItem_ItemChain] FOREIGN KEY ([ItemChainId]) REFERENCES [dbo].[ItemChain] ([ItemChainId])
);


GO
GRANT SELECT
    ON OBJECT::[dbo].[ItemChainItem] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[ItemChainItem] TO [IRMAReportsRole]
    AS [dbo];

