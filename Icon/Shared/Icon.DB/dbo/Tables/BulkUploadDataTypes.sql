CREATE TABLE dbo.BulkUploadDataTypes
(
	BulkUploadDataTypeId INT IDENTITY(1, 1) CONSTRAINT PK_BulkUploadDataTypes_BulkUploadDataTypeId PRIMARY KEY,
	DataType NVARCHAR(50) NOT NULL
)