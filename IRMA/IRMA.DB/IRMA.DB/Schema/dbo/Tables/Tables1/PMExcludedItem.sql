CREATE TABLE [dbo].[PMExcludedItem] (
    [Item_Key] INT NOT NULL,
    CONSTRAINT [PK_PMExcludedItem] PRIMARY KEY CLUSTERED ([Item_Key] ASC) WITH (FILLFACTOR = 80),
    CONSTRAINT [FK_PMExcludedItem_Item] FOREIGN KEY ([Item_Key]) REFERENCES [dbo].[Item] ([Item_Key])
);


GO
GRANT SELECT
    ON OBJECT::[dbo].[PMExcludedItem] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[PMExcludedItem] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[PMExcludedItem] TO [IRMAReportsRole]
    AS [dbo];

