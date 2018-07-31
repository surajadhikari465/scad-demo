CREATE TABLE [dbo].[LocaleSubType]
(
	[localeSubTypeID] INT NOT NULL IDENTITY,
	[localeTypeID]	 INT NOT NULL,
	[localSubTypeCode] NVARCHAR(60) NOT NULL,  
	[localeSubTypeDesc] NVARCHAR(255)  NULL  
)
GO
ALTER TABLE [dbo].[LocaleSubType] ADD CONSTRAINT [LocaleSubType_PK] PRIMARY KEY CLUSTERED (
[localeSubTypeID]
)
GO
ALTER TABLE [dbo].[LocaleSubType] WITH CHECK ADD CONSTRAINT [LocaleType_LocaleSubType_FK1] FOREIGN KEY (
[localeTypeID]
)
REFERENCES [dbo].[LocaleType] (
[localeTypeID]
)