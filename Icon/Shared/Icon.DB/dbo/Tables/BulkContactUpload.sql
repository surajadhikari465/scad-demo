CREATE TABLE [dbo].[BulkContactUpload]
(
    BulkContactUploadId INT NOT NULL IDENTITY(1,1) CONSTRAINT PK_BulkContactUpload_Id PRIMARY KEY, 
    FileName NVARCHAR(1000) NOT NULL,
    FileUploadTime DATETIME2 NOT NULL CONSTRAINT DF_BulkContactUpload_FileUploadTime DEFAULT (getdate()),
    UploadedBy NVARCHAR(1000) NOT NULL,
    TotalRows INT NULL,
    FileContent VARBINARY(MAX) NULL
)