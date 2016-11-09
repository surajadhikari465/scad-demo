CREATE TABLE [dbo].[ItemAttributes_Ext] (
    [ItemAttributeID] INT            IDENTITY (1, 1) NOT NULL,
    [ItemID]          INT            NULL,
    [AttributeID]     INT            NULL,
    [AttributeValue]  NVARCHAR (255) NULL,
    [AddedDate]       DATETIME       DEFAULT (getdate()) NOT NULL,
    [ModifiedDate]    DATETIME       NULL,
    CONSTRAINT [PK_ItemAttributes_Ext] PRIMARY KEY CLUSTERED ([ItemAttributeID] ASC) WITH (FILLFACTOR = 100),
    CONSTRAINT [FK_ItemAttributes_Ext_ItemID] FOREIGN KEY ([ItemID]) REFERENCES [dbo].[Items] ([ItemID])
);

