CREATE TABLE [dbo].[BulkItemUploadErrors]
(
		[BulkItemUploadErrorId] INT NOT NULL PRIMARY KEY IDENTITY (1,1), 
		[BulkItemUploadId] INT NOT NULL
		CONSTRAINT [FK_BulkItemUpload_BulkItemUploadErrors]  FOREIGN KEY ([BulkItemUploadId]) references [dbo].[BulkItemUpload] ([BulkItemUploadId]) , 
		RowId int, 
		Message varchar(500)
)
