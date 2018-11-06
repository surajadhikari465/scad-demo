CREATE TABLE [dbo].[POSWriterBatchIds] (
    [POSFileWriterKey]  INT NOT NULL,
    [POSChangeTypeKey]  INT NOT NULL,
    [POSBatchIdDefault] INT CONSTRAINT [DF_POSBatchHeaderConfig_POSBatchIdDefault] DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK_POSWriterBatchIds] PRIMARY KEY CLUSTERED ([POSFileWriterKey] ASC, [POSChangeTypeKey] ASC),
    CONSTRAINT [FK_POSWriterBatchIds_POSChangeType] FOREIGN KEY ([POSChangeTypeKey]) REFERENCES [dbo].[POSChangeType] ([POSChangeTypeKey]),
    CONSTRAINT [FK_POSWriterBatchIds_POSFileWriterKey] FOREIGN KEY ([POSFileWriterKey]) REFERENCES [dbo].[POSWriter] ([POSFileWriterKey])
);

