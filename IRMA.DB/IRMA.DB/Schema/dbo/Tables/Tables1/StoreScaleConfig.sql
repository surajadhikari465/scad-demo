CREATE TABLE [dbo].[StoreScaleConfig] (
    [Store_No]           INT NOT NULL,
    [ScaleFileWriterKey] INT NOT NULL,
    CONSTRAINT [FK_StoreScaleConfig_ScaleFileWriterKey] FOREIGN KEY ([ScaleFileWriterKey]) REFERENCES [dbo].[POSWriter] ([POSFileWriterKey]),
    CONSTRAINT [FK_StoreScaleConfig_Store_No] FOREIGN KEY ([Store_No]) REFERENCES [dbo].[Store] ([Store_No])
);

