CREATE TABLE [dbo].[BulkItemUpload]
(
	[BulkItemUploadId] INT NOT NULL IDENTITY(1,1)
		CONSTRAINT PK_BulkItemUpload_Id PRIMARY KEY, 
    [FileName] NVARCHAR(260) NOT NULL,
	[FileModeType] BIT NOT NULL 
		CONSTRAINT DF_BulkItemUpload_FileModeType DEFAULT (0),
	[FileUploadTime] DATETIME2 NOT NULL 
		CONSTRAINT DF_BulkItemUpload_FileUploadTime DEFAULT (getdate()),
    [UploadedBy] NVARCHAR(MAX) NOT NULL,
	[StatusId] INT NOT NULL
		CONSTRAINT DF_BulkItemUpload_StatusID DEFAULT (0)
		CONSTRAINT FK_BulkItemUpload_StatusID FOREIGN KEY REFERENCES BulkUploadStatus (Id), 
	TotalRows INT NULL,
	CurrentRow INT NULL,
    [Message] NVARCHAR(500) NULL
		
)