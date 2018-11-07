CREATE TABLE [dbo].[FileWriterType] (
    [FileWriterType] VARCHAR (10) NOT NULL,
    [IsConfigWriter] BIT          CONSTRAINT [DF_FileWriterType_IsConfigWriter] DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK_FileWriterType_FileWriterKey] PRIMARY KEY CLUSTERED ([FileWriterType] ASC)
);

