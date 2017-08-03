CREATE TABLE StoreItemExtended
(
	StoreItemExtendedID INT IDENTITY(1, 1),
	Store_No INT NOT NULL,
	Item_Key INT NOT NULL,
	ItemStatusCode INT NULL
    CONSTRAINT PK_StoreItemExtended_StoreItemExtendedID PRIMARY KEY CLUSTERED (StoreItemExtendedID ASC),
    CONSTRAINT FK_StoreItemExtended_ItemKey FOREIGN KEY ([Item_Key]) REFERENCES [dbo].[Item] ([Item_Key]),
    CONSTRAINT FK_StoreItemExtended_StoreNo FOREIGN KEY ([Store_No]) REFERENCES [dbo].[Store] ([Store_No])
)

GO
ALTER TABLE dbo.StoreItemExtended ENABLE CHANGE_TRACKING WITH (TRACK_COLUMNS_UPDATED = OFF);

GO
CREATE NONCLUSTERED INDEX [IX_StoreItemExtended_ItemKeyStoreNo]
    ON dbo.StoreItemExtended(Store_No ASC, Item_Key ASC);