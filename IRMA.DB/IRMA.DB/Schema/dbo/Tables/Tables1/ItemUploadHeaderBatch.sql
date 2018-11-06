CREATE TABLE [dbo].[ItemUploadHeaderBatch] (
    [ItemUploadHeader_ID] INT NOT NULL,
    [PriceBatchHeaderID]  INT NOT NULL,
    CONSTRAINT [FK_ItemUploadHeaderBatch_ItemUploadHeader] FOREIGN KEY ([ItemUploadHeader_ID]) REFERENCES [dbo].[ItemUploadHeader] ([ItemUploadHeader_ID]),
    CONSTRAINT [FK_ItemUploadHeaderBatch_PriceBatchHeader] FOREIGN KEY ([PriceBatchHeaderID]) REFERENCES [dbo].[PriceBatchHeader] ([PriceBatchHeaderID])
);

