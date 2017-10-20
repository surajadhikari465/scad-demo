CREATE TABLE [esb].[MessageType] (
    [MessageTypeId]   INT            IDENTITY (1, 1) NOT NULL,
    [MessageTypeName] NVARCHAR (255) NOT NULL,
    CONSTRAINT [PK_MessageTypeId] PRIMARY KEY CLUSTERED ([MessageTypeId] ASC) WITH (FILLFACTOR = 100)
)
GO

GRANT SELECT on [esb].[MessageType] to [TibcoRole]
GO