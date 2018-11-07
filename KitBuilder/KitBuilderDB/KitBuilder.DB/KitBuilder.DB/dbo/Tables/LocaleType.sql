CREATE TABLE [dbo].[LocaleType] (
    [localeTypeId]   INT            NOT NULL,
    [localeTypeCode] NVARCHAR (3)   NULL,
    [localeTypeDesc] NVARCHAR (255) NULL,
    CONSTRAINT [PK_LocaleType] PRIMARY KEY CLUSTERED ([localeTypeId] ASC)
);




