CREATE TABLE [dbo].[AttributeMetaData] (
    [AttributeMetaDataID] INT            IDENTITY (1, 1) NOT NULL,
    [TraitCode]           NCHAR (3)      NULL,
    [TraitGroupCode]      NCHAR (3)      NULL,
    [TraitDesc]           NVARCHAR (100) NULL,
    [TraitGroup]          INT            NULL,
    CONSTRAINT [PK_AttributeMetaData] PRIMARY KEY CLUSTERED ([AttributeMetaDataID] ASC) WITH (FILLFACTOR = 100)
);

