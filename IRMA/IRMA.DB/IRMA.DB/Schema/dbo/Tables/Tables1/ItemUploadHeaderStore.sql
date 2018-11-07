CREATE TABLE [dbo].[ItemUploadHeaderStore] (
    [ItemUploadHeader_ID] INT NOT NULL,
    [Store_No]            INT NOT NULL,
    CONSTRAINT [FK_ItemUploadHeaderStore_ItemUploadHeader] FOREIGN KEY ([ItemUploadHeader_ID]) REFERENCES [dbo].[ItemUploadHeader] ([ItemUploadHeader_ID]),
    CONSTRAINT [FK_ItemUploadHeaderStore_Store] FOREIGN KEY ([Store_No]) REFERENCES [dbo].[Store] ([Store_No])
);

