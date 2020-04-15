CREATE TABLE dbo.BulkUploadErrors
(
	BulkUploadErrorId INT IDENTITY (1,1) CONSTRAINT PK_BulkUploadErrors_BulkUploadErrorId PRIMARY KEY, 
	BulkUploadId INT NOT NULL CONSTRAINT FK_BulkUpload_BulkUploadErrors  FOREIGN KEY (BulkUploadId) references dbo.BulkUpload (BulkUploadId) , 
	RowId INT, 
	Message NVARCHAR(500)
)
