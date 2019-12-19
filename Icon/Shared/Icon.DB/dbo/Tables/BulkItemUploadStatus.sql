CREATE TABLE [dbo].[BulkItemUploadStatus]
(
	[Id] int not null identity(1,1)
		constraint PK_BulkItemUploadStatus_Id primary key,
	[Status] varchar(100) not null
)
