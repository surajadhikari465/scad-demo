CREATE TABLE [dbo].[ItemTypes] (
    [itemTypeID]   INT            NOT NULL IDENTITY(1,1),
    [itemTypeCode] NVARCHAR (3)   NOT NULL,
    [itemTypeDesc] NVARCHAR (255) NULL,
    [AddedDate]    DATETIME       DEFAULT (getdate()) NOT NULL,
    [ModifiedDate] DATETIME       NULL,
    CONSTRAINT [PK_ItemTypes] PRIMARY KEY CLUSTERED ([itemTypeID] ASC) WITH (FILLFACTOR = 100)
);
