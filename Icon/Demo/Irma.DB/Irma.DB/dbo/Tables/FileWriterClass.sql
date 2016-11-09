CREATE TABLE [dbo].[FileWriterClass] (
    [FileWriterClass] VARCHAR (100) NOT NULL,
    [FileWriterType]  VARCHAR (10)  NOT NULL,
    CONSTRAINT [PK_FileWriterClass] PRIMARY KEY CLUSTERED ([FileWriterClass] ASC),
    CONSTRAINT [FK_FileWriterClass_FileWriterType] FOREIGN KEY ([FileWriterType]) REFERENCES [dbo].[FileWriterType] ([FileWriterType])
);

