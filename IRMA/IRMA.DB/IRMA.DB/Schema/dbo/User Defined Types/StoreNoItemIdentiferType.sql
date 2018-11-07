CREATE TYPE [dbo].[StoreNoItemIdentiferType] AS TABLE (
    [StoreNo] INT NULL,
    [ItemIdentifier] VARCHAR(13) NULL);

GO
GRANT EXECUTE
    ON TYPE::[dbo].[StoreNoItemIdentiferType] TO [IRMAClientRole];
GO