CREATE TABLE [dbo].[POSWriterPriceChgBatchId] (
    [POSFileWriterKey]  INT     NOT NULL,
    [PriceChgTypeID]    TINYINT NOT NULL,
    [POSBatchIdDefault] INT     NOT NULL,
    CONSTRAINT [PK_POSWriterPriceChgBatchId] PRIMARY KEY CLUSTERED ([POSFileWriterKey] ASC, [PriceChgTypeID] ASC),
    CONSTRAINT [FK_POSWriterPriceChgBatchId_POSFileWriterKey] FOREIGN KEY ([POSFileWriterKey]) REFERENCES [dbo].[POSWriter] ([POSFileWriterKey]),
    CONSTRAINT [FK_POSWriterPriceChgBatchId_PriceChgTypeID] FOREIGN KEY ([PriceChgTypeID]) REFERENCES [dbo].[PriceChgType] ([PriceChgTypeID])
);

