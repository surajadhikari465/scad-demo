CREATE TABLE [dbo].[StoreShelfTagConfig] (
    [Store_No]         INT          NOT NULL,
    [POSFileWriterKey] INT          NOT NULL,
    [ConfigType]       VARCHAR (20) NOT NULL,
    CONSTRAINT [PK_StoreShelfTagConfig] PRIMARY KEY CLUSTERED ([Store_No] ASC, [POSFileWriterKey] ASC) WITH (FILLFACTOR = 90),
    CONSTRAINT [CK_ShelfTag_ConfigType] CHECK ([ConfigType]='sent' OR [ConfigType]='direct'),
    CONSTRAINT [FK_StoreShelfTagConfig_POSFileWriterKey] FOREIGN KEY ([POSFileWriterKey]) REFERENCES [dbo].[POSWriter] ([POSFileWriterKey]),
    CONSTRAINT [FK_StoreShelfTagConfig_Store_No] FOREIGN KEY ([Store_No]) REFERENCES [dbo].[Store] ([Store_No])
);


GO
GRANT SELECT
    ON OBJECT::[dbo].[StoreShelfTagConfig] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[StoreShelfTagConfig] TO [IRMAReportsRole]
    AS [dbo];

