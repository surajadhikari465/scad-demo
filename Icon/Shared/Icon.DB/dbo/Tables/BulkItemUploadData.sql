CREATE TABLE [dbo].[BulkItemUploadData]
(
	[BulkItemUploadDataId] INT NOT NULL PRIMARY KEY IDENTITY, 
    [BulkItemUploadId] INT NOT NULL, 
    [FileContent] VARBINARY(MAX) NOT NULL
)
GO
ALTER TABLE [dbo].[BulkItemUploadData] WITH CHECK ADD CONSTRAINT [BulkItemUpload_BulkItemUploadData_FK1] FOREIGN KEY ([BulkItemUploadId])
REFERENCES [dbo].[BulkItemUpload] ([BulkItemUploadId])
