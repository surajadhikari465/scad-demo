CREATE TABLE [app].[MessageType] (
    [MessageTypeId]   INT            IDENTITY (1, 1) NOT NULL,
    [MessageTypeName] NVARCHAR (255) NOT NULL,
    CONSTRAINT [PK_MessageTypeId] PRIMARY KEY CLUSTERED ([MessageTypeId] ASC) WITH (FILLFACTOR = 80)
);

