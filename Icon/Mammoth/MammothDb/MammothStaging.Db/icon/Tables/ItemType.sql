CREATE TABLE [icon].[ItemType] (
    [itemTypeID]   INT            NOT NULL,
    [itemTypeCode] NVARCHAR (3)   NOT NULL,
    [itemTypeDesc] NVARCHAR (255) NULL,
    CONSTRAINT [PK_ItemType] PRIMARY KEY CLUSTERED ([itemTypeID] ASC) WITH (FILLFACTOR = 100)
);

