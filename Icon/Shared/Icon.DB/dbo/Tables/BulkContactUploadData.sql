CREATE TABLE [dbo].[BulkContactUploadData]
(
	[BulkContactUploadDataId] INT NOT NULL PRIMARY KEY IDENTITY, 
    [BulkContactUploadId] INT NOT NULL, 
    [FileContent] VARBINARY(MAX) NOT NULL
)
GO
ALTER TABLE [dbo].[BulkContactUploadData] WITH CHECK ADD CONSTRAINT [BulkContactUpload_BulkItemUploadData_FK1] FOREIGN KEY ([BulkContactUploadId])
REFERENCES [dbo].[BulkContactUpload] ([BulkContactUploadId])
