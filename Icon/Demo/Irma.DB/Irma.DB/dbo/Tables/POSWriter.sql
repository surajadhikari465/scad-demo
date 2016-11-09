CREATE TABLE [dbo].[POSWriter] (
    [POSFileWriterKey]    INT           IDENTITY (1, 2) NOT NULL,
    [POSFileWriterCode]   VARCHAR (20)  NULL,
    [POSFileWriterClass]  VARCHAR (100) NOT NULL,
    [DelimChar]           VARCHAR (10)  NULL,
    [FixedWidth]          BIT           CONSTRAINT [DF_POSWriter_FixedWidth] DEFAULT ((0)) NOT NULL,
    [LeadingDelim]        BIT           CONSTRAINT [DF_POSWriter_LeadingDelim] DEFAULT ((0)) NULL,
    [TrailingDelim]       BIT           CONSTRAINT [DF_POSWriter_TrailingDelim] DEFAULT ((0)) NULL,
    [EnforceDictionary]   BIT           CONSTRAINT [DF_POSWriter_EnforceDictionary] DEFAULT ((0)) NOT NULL,
    [TaxFlagTrueChar]     CHAR (1)      CONSTRAINT [DF_POSWriter_TaxFlagTrueChar] DEFAULT ('Y') NULL,
    [TaxFlagFalseChar]    CHAR (1)      CONSTRAINT [DF_POSWriter_TaxFlagFalseChar] DEFAULT ('N') NULL,
    [Disabled]            BIT           CONSTRAINT [DF_POSWriter_Disabled] DEFAULT ((0)) NOT NULL,
    [AppendToFile]        BIT           NULL,
    [FileWriterType]      VARCHAR (10)  NULL,
    [ScaleWriterType]     INT           NULL,
    [FieldIdDelim]        BIT           NULL,
    [OutputByIrmaBatches] BIT           CONSTRAINT [DF_POSWriter_OutputByIrmaBatches] DEFAULT ((0)) NULL,
    [BatchIdMin]          INT           NULL,
    [BatchIdMax]          INT           NULL,
    CONSTRAINT [PK_POSWriter_POSFileWriterKey] PRIMARY KEY CLUSTERED ([POSFileWriterKey] ASC),
    CONSTRAINT [FK_POSWriter_FileWriterClass] FOREIGN KEY ([POSFileWriterClass]) REFERENCES [dbo].[FileWriterClass] ([FileWriterClass]),
    CONSTRAINT [FK_POSWriter_FileWriterType] FOREIGN KEY ([FileWriterType]) REFERENCES [dbo].[FileWriterType] ([FileWriterType]),
    CONSTRAINT [FK_POSWriter_ScaleWriterType] FOREIGN KEY ([ScaleWriterType]) REFERENCES [dbo].[ScaleWriterType] ([ScaleWriterTypeKey])
);


GO
GRANT SELECT
    ON OBJECT::[dbo].[POSWriter] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[POSWriter] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[POSWriter] TO [IRMAReportsRole]
    AS [dbo];

