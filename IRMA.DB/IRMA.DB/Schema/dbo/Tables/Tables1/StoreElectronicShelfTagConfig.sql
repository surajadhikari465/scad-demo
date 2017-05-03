CREATE TABLE [dbo].[StoreElectronicShelfTagConfig] (
    [Store_No]         INT          NOT NULL,
    [POSFileWriterKey] INT          NOT NULL,
    [ConfigType]       VARCHAR (20) NOT NULL,
    CONSTRAINT [PK_StoreElectronicShelfTagConfig] PRIMARY KEY CLUSTERED ([Store_No] ASC, [POSFileWriterKey] ASC) WITH (FILLFACTOR = 90)
);

