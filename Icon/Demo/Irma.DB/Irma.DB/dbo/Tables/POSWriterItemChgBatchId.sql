CREATE TABLE [dbo].[POSWriterItemChgBatchId] (
    [POSFileWriterKey]  INT     NOT NULL,
    [ItemChgTypeID]     TINYINT NOT NULL,
    [POSBatchIdDefault] INT     NOT NULL,
    CONSTRAINT [PK_POSWriterItemChgBatchId] PRIMARY KEY CLUSTERED ([POSFileWriterKey] ASC, [ItemChgTypeID] ASC),
    CONSTRAINT [FK_POSWriterItemChgBatchId_ItemChgTypeID] FOREIGN KEY ([ItemChgTypeID]) REFERENCES [dbo].[ItemChgType] ([ItemChgTypeID]),
    CONSTRAINT [FK_POSWriterItemChgBatchId_POSFileWriterKey] FOREIGN KEY ([POSFileWriterKey]) REFERENCES [dbo].[POSWriter] ([POSFileWriterKey])
);

