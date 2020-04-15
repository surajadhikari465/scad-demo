CREATE TABLE dbo.BulkUpload
(
	BulkUploadId INT NOT NULL IDENTITY(1,1)
		CONSTRAINT PK_BulkUpload_Id PRIMARY KEY, 
    FileName NVARCHAR(260) NOT NULL,
	BulkUploadDataTypeId INT NOT NULL
		CONSTRAINT FK_BulkUpload_BulkUploadDataTypeId FOREIGN KEY REFERENCES BulkUploadDataTypes(BulkUploadDataTypeId),
	FileModeTypeId INT NOT NULL 
		CONSTRAINT DF_BulkUpload_FileModeType DEFAULT (1)
		CONSTRAINT FK_BulkUpload_FileModeTypeId FOREIGN KEY REFERENCES BulkUploadFileTypes(BulkUploadFileTypeId), 
	FileUploadTime DATETIME2 NOT NULL 
		CONSTRAINT DF_BulkUpload_FileUploadTime DEFAULT (getdate()),
    UploadedBy NVARCHAR(MAX) NOT NULL,
	StatusId INT NOT NULL
		CONSTRAINT DF_BulkUpload_StatusID DEFAULT (1)
		CONSTRAINT FK_BulkUpload_StatusID FOREIGN KEY REFERENCES BulkUploadStatus (Id), 
	TotalRows INT NULL,
	CurrentRow INT NULL,
    Message NVARCHAR(500) NULL,
	PercentageProcessed  INT NULL DEFAULT 0	 
)