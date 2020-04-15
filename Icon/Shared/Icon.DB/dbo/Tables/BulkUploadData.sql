CREATE TABLE dbo.BulkUploadData
(
	BulkUploadDataId INT IDENTITY(1, 1) CONSTRAINT PK_BulkUploadData_BulkUploadDataId PRIMARY KEY, 
    BulkUploadId INT NOT NULL, 
    FileContent VARBINARY(MAX) NOT NULL
)
GO
ALTER TABLE dbo.BulkUploadData WITH CHECK ADD CONSTRAINT BulkUpload_BulkUploadData_FK1 FOREIGN KEY (BulkUploadId)
REFERENCES dbo.BulkUpload (BulkUploadId)
