CREATE TABLE [dbo].[NameType] (
[nameTypeCode] INT  NOT NULL IDENTITY  
, [nameTypeDesc] NVARCHAR(255)  NULL  
)
GO
ALTER TABLE [dbo].[NameType] ADD CONSTRAINT [NameType_PK] PRIMARY KEY CLUSTERED (
[nameTypeCode]
)