CREATE TABLE [dbo].[InventoryLocationItems] (
    [InvLocID] INT NOT NULL,
    [Item_Key] INT NOT NULL,
    CONSTRAINT [PK_InventoryLocationItems] PRIMARY KEY CLUSTERED ([InvLocID] ASC, [Item_Key] ASC) WITH (FILLFACTOR = 80),
    CONSTRAINT [FK_InventoryLocationItems_InventoryLocation] FOREIGN KEY ([InvLocID]) REFERENCES [dbo].[InventoryLocation] ([InvLoc_ID]),
    CONSTRAINT [FK_InventoryLocationItems_Item] FOREIGN KEY ([Item_Key]) REFERENCES [dbo].[Item] ([Item_Key])
);


GO
GRANT INSERT
    ON OBJECT::[dbo].[InventoryLocationItems] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[InventoryLocationItems] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT INSERT
    ON OBJECT::[dbo].[InventoryLocationItems] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[InventoryLocationItems] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT INSERT
    ON OBJECT::[dbo].[InventoryLocationItems] TO [IRMAReportsRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[InventoryLocationItems] TO [IRMAReportsRole]
    AS [dbo];

