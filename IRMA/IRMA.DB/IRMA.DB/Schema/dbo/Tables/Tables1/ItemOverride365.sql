CREATE TABLE [dbo].[ItemOverride365] (
    [Item_Key]      INT NOT NULL,
    [Not_Available] BIT NOT NULL,
    CONSTRAINT [PK_ItemOverride_365] PRIMARY KEY CLUSTERED ([Item_Key] ASC) WITH (FILLFACTOR = 80),
    CONSTRAINT [FK_ItemOverride_Item_Key_365] FOREIGN KEY ([Item_Key]) REFERENCES [dbo].[Item] ([Item_Key])
);


GO
GRANT DELETE
    ON OBJECT::[dbo].[ItemOverride365] TO [IRSUser]
    AS [dbo];


GO
GRANT INSERT
    ON OBJECT::[dbo].[ItemOverride365] TO [IRSUser]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[ItemOverride365] TO [IRSUser]
    AS [dbo];


GO
GRANT UPDATE
    ON OBJECT::[dbo].[ItemOverride365] TO [IRSUser]
    AS [dbo];

