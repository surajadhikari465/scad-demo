CREATE TABLE [dbo].[BulkUploadStatus]
(
	[Id] int not null identity(1,1)
		constraint PK_BulkUploadStatus_Id primary key,
	[Status] varchar(100) not null
)
