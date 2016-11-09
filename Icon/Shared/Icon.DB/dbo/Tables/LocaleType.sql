CREATE TABLE [dbo].[LocaleType] (
[localeTypeID] INT  NOT NULL IDENTITY,
[localeTypeCode] NVARCHAR(3)  NOT NULL,
[localeTypeDesc] NVARCHAR(255)  NULL,
CONSTRAINT [AK_localeTypeCode_localeTypeCode] UNIQUE NONCLUSTERED ([localeTypeCode] ASC) WITH (FILLFACTOR = 80)
)
GO
ALTER TABLE [dbo].[LocaleType] ADD CONSTRAINT [LocaleType_PK] PRIMARY KEY CLUSTERED (
[localeTypeID]
)