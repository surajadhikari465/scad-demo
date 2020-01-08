CREATE TABLE [dbo].[BulkContactUpload]
(
	[BulkContactUploadId] INT NOT NULL IDENTITY(1,1)
		CONSTRAINT PK_BulkContactUpload_Id PRIMARY KEY, 
    [FileName] NVARCHAR(260) NOT NULL,
	[FileUploadTime] DATETIME2 NOT NULL 
		CONSTRAINT DF_BulkContactUpload_FileUploadTime DEFAULT (getdate()),
    [UploadedBy] NVARCHAR(MAX) NOT NULL,
	[StatusId] INT NOT NULL
		CONSTRAINT DF_BulkContactUpload_StatusID DEFAULT (0)
		CONSTRAINT FK_BulkContactUpload_StatusID FOREIGN KEY REFERENCES BulkUploadStatus (Id), 
	TotalRows INT NULL,
	CurrentRow INT NULL,
    [Message] NVARCHAR(500) NULL
		
)