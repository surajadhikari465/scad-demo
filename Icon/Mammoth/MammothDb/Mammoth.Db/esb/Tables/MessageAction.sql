CREATE TABLE [esb].[MessageAction] (
    [MessageActionId]   INT            IDENTITY (1, 1) NOT NULL,
    [MessageActionName] NVARCHAR (255) NOT NULL,
    CONSTRAINT [PK_MessageActionId] PRIMARY KEY CLUSTERED ([MessageActionId] ASC) WITH (FILLFACTOR = 100)
);

