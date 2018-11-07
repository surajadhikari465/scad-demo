CREATE TABLE [dbo].[JDA_ItemIdentifierSync] (
    [JDA_ItemIdentifierSync_ID] BIGINT       IDENTITY (1, 1) NOT NULL,
    [ActionCode]                CHAR (1)     NOT NULL,
    [ApplyDate]                 DATETIME     NOT NULL,
    [Item_Key]                  INT          NOT NULL,
    [Identifier]                VARCHAR (13) NOT NULL,
    [National_Identifier]       TINYINT      NULL,
    [ItemType_ID]               INT          NULL,
    [SyncState]                 TINYINT      DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK_JDA_ItemIdentifierSync] PRIMARY KEY CLUSTERED ([JDA_ItemIdentifierSync_ID] ASC)
);

