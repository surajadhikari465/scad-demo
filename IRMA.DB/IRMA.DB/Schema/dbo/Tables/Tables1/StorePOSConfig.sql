CREATE TABLE [dbo].[StorePOSConfig] (
    [Store_No]         INT          NOT NULL,
    [POSFileWriterKey] INT          NOT NULL,
    [ConfigType]       VARCHAR (20) NOT NULL,
    CONSTRAINT [CK_ConfigType] CHECK ([ConfigType]='sent' OR [ConfigType]='direct'),
    CONSTRAINT [FK_StorePOSConfig_POSFileWriterKey] FOREIGN KEY ([POSFileWriterKey]) REFERENCES [dbo].[POSWriter] ([POSFileWriterKey]),
    CONSTRAINT [FK_StorePOSConfig_Store_No] FOREIGN KEY ([Store_No]) REFERENCES [dbo].[Store] ([Store_No])
);


GO
GRANT SELECT
    ON OBJECT::[dbo].[StorePOSConfig] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[StorePOSConfig] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[StorePOSConfig] TO [IRMAReportsRole]
    AS [dbo];

