CREATE TABLE [irma].[ValidatedScanCode] (
    [Region]     NCHAR (2)    NOT NULL,
    [Id]         INT          NOT NULL,
    [ScanCode]   VARCHAR (13) NOT NULL,
    [InsertDate] DATETIME     NOT NULL,
    CONSTRAINT [PK_ValidatedScanCode] PRIMARY KEY CLUSTERED ([Region] ASC, [Id] ASC) WITH (FILLFACTOR = 100)
);

