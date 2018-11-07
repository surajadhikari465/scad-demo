CREATE TABLE [dbo].[ItemUploadHeaderVendor] (
    [ItemUploadHeader_ID] INT NOT NULL,
    [Vendor_ID]           INT NOT NULL,
    CONSTRAINT [FK_ItemUploadHeaderVendor_ItemUploadHeader] FOREIGN KEY ([ItemUploadHeader_ID]) REFERENCES [dbo].[ItemUploadHeader] ([ItemUploadHeader_ID]),
    CONSTRAINT [FK_ItemUploadHeaderVendor_Vendor] FOREIGN KEY ([Vendor_ID]) REFERENCES [dbo].[Vendor] ([Vendor_ID])
);

