CREATE TABLE [dbo].[ItemTypes_Audit] (
    [itemTypeID]   INT            NOT NULL,
    [itemTypeCode] NVARCHAR (3)   NOT NULL,
    [itemTypeDesc] NVARCHAR (255) NULL,
    [AddedDate]    DATETIME       DEFAULT (getdate()) NOT NULL,
    [ModifiedDate] DATETIME       NULL,
    CONSTRAINT [PK_ItemTypes_Audit] PRIMARY KEY CLUSTERED ([itemTypeID] ASC) WITH (FILLFACTOR = 100)
);

GO