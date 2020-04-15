CREATE TABLE dbo.BulkUploadFileTypes
(
	BulkUploadFileTypeId INT NOT NULL IDENTITY(1, 1) CONSTRAINT PK_BulkUploadFileTypes_BulkUploadFileTypeId PRIMARY KEY, 
	FileType NVARCHAR(25) NOT NULL
)
