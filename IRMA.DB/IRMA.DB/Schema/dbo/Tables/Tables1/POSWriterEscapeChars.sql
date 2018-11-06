CREATE TABLE [dbo].[POSWriterEscapeChars] (
    [POSFileWriterKey]      INT          NOT NULL,
    [EscapeCharValue]       VARCHAR (10) NOT NULL,
    [EscapeCharReplacement] VARCHAR (10) NULL,
    CONSTRAINT [PK_POSWriterEscapeChars] PRIMARY KEY CLUSTERED ([POSFileWriterKey] ASC, [EscapeCharValue] ASC),
    CONSTRAINT [FK_POSWriterEscapeChars_POSWriter] FOREIGN KEY ([POSFileWriterKey]) REFERENCES [dbo].[POSWriter] ([POSFileWriterKey])
);


GO
GRANT DELETE
    ON OBJECT::[dbo].[POSWriterEscapeChars] TO [IRMAAdminRole]
    AS [dbo];


GO
GRANT INSERT
    ON OBJECT::[dbo].[POSWriterEscapeChars] TO [IRMAAdminRole]
    AS [dbo];


GO
GRANT UPDATE
    ON OBJECT::[dbo].[POSWriterEscapeChars] TO [IRMAAdminRole]
    AS [dbo];


GO
GRANT DELETE
    ON OBJECT::[dbo].[POSWriterEscapeChars] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT INSERT
    ON OBJECT::[dbo].[POSWriterEscapeChars] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[POSWriterEscapeChars] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT UPDATE
    ON OBJECT::[dbo].[POSWriterEscapeChars] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[POSWriterEscapeChars] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[POSWriterEscapeChars] TO [IRMAReportsRole]
    AS [dbo];

