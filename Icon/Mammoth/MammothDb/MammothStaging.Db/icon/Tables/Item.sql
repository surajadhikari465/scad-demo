﻿CREATE TABLE [icon].[Item] (
    [itemID]     INT NOT NULL,
    [itemTypeID] INT NOT NULL,
    CONSTRAINT [Item_PK] PRIMARY KEY CLUSTERED ([itemID] ASC) WITH (FILLFACTOR = 100)
);

