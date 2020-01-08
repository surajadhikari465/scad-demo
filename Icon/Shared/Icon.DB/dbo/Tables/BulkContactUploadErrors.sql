CREATE TABLE [dbo].[BulkContactUploadErrors]
(
		[BulkContactUploadErrorId] INT NOT NULL PRIMARY KEY IDENTITY (1,1), 
		[BulkContactUploadId] INT NOT NULL
		CONSTRAINT [FK_BulkContactUpload_BulkContactUploadErrors]  FOREIGN KEY ([BulkContactUploadId]) references [dbo].[BulkContactUpload] ([BulkContactUploadId]) , 
		RowId int, 
		Message varchar(500)
)