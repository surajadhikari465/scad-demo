CREATE TABLE [dbo].[ItemHistoryUpload] (
    [ItemHistoryId]        INT      NOT NULL,
    [AccountingUploadDate] DATETIME NULL,
    CONSTRAINT [FK_ItemHistoryUpload_ID] FOREIGN KEY ([ItemHistoryId]) REFERENCES [dbo].[ItemHistory] ([ItemHistoryID])
);

