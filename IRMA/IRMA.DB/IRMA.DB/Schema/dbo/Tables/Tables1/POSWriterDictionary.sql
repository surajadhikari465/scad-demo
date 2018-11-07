CREATE TABLE [dbo].[POSWriterDictionary] (
    [POSFileWriterKey] INT          NOT NULL,
    [FieldID]          VARCHAR (20) NOT NULL,
    [DataType]         VARCHAR (15) NOT NULL,
    CONSTRAINT [PK_POSWriterDictionary_POSFileWriterKey_FieldID] PRIMARY KEY CLUSTERED ([POSFileWriterKey] ASC, [FieldID] ASC),
    CONSTRAINT [FK_POSWriterDictionary_POSWriter] FOREIGN KEY ([POSFileWriterKey]) REFERENCES [dbo].[POSWriter] ([POSFileWriterKey])
);


GO
GRANT DELETE
    ON OBJECT::[dbo].[POSWriterDictionary] TO [IRMAAdminRole]
    AS [dbo];


GO
GRANT INSERT
    ON OBJECT::[dbo].[POSWriterDictionary] TO [IRMAAdminRole]
    AS [dbo];


GO
GRANT UPDATE
    ON OBJECT::[dbo].[POSWriterDictionary] TO [IRMAAdminRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[POSWriterDictionary] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[POSWriterDictionary] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[POSWriterDictionary] TO [IRMAReportsRole]
    AS [dbo];

