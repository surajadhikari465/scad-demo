CREATE TABLE [dbo].[ScanCodeType] (
[scanCodeTypeID] INT  NOT NULL IDENTITY  
, [scanCodeTypeDesc] NVARCHAR(255)  NULL  
)
GO
ALTER TABLE [dbo].[ScanCodeType] ADD CONSTRAINT [ScanCodeType_PK] PRIMARY KEY CLUSTERED (
[scanCodeTypeID]
)