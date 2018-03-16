CREATE TABLE [dbo].[ItemAttributes_Ext] (
    [ItemAttributeID] INT            IDENTITY (1, 1) NOT NULL,
    [ItemID]          INT            NOT NULL,
    [AttributeID]     INT            NOT NULL,
    [AttributeValue]  NVARCHAR (300) NULL,
    [AddedDate]       DATETIME       DEFAULT (getdate()) NOT NULL,
    [ModifiedDate]    DATETIME       NULL,
    CONSTRAINT [PK_ItemAttributes_Ext] PRIMARY KEY CLUSTERED ([ItemAttributeID] ASC) WITH (FILLFACTOR = 100),
    CONSTRAINT [FK_ItemAttributes_Ext_ItemID] FOREIGN KEY ([ItemID]) REFERENCES [dbo].[Items] ([ItemID])
);
GO

CREATE INDEX [IX_ItemAttributes_Ext_ItemID_AttributeID] ON [dbo].[ItemAttributes_Ext]
(
	[ItemID] ASC,
	[AttributeID] ASC
)
INCLUDE ([AttributeValue]) ON [PRIMARY]

GO

GRANT SELECT, UPDATE, INSERT, DELETE ON dbo.ItemAttributes_Ext TO MammothRole
GO
