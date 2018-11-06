CREATE TABLE [dbo].[ScaleWriterType] (
    [ScaleWriterTypeKey]  INT           IDENTITY (1, 1) NOT NULL,
    [ScaleWriterTypeDesc] VARCHAR (100) NOT NULL,
    CONSTRAINT [PK_ScaleWriterType] PRIMARY KEY CLUSTERED ([ScaleWriterTypeKey] ASC)
);

