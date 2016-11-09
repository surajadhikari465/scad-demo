CREATE TABLE [esb].[MessageStatus] (
    [MessageStatusId]   INT            IDENTITY (1, 1) NOT NULL,
    [MessageStatusName] NVARCHAR (255) NOT NULL,
    CONSTRAINT [PK_MessageStatusId] PRIMARY KEY CLUSTERED ([MessageStatusId] ASC) WITH (FILLFACTOR = 100)
);

