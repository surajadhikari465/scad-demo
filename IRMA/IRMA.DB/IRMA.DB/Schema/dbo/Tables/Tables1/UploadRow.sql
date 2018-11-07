CREATE TABLE [dbo].[UploadRow] (
    [UploadRow_ID]     INT          IDENTITY (1, 1) NOT NULL,
    [Item_Key]         INT          NULL,
    [UploadSession_ID] INT          NOT NULL,
    [Identifier]       VARCHAR (13) NULL,
    [ValidationLevel]  INT          DEFAULT ((0)) NOT NULL,
    [ItemRequest_ID]   INT          NULL,
    CONSTRAINT [PK_UploadDataRow] PRIMARY KEY CLUSTERED ([UploadRow_ID] ASC),
    CONSTRAINT [FK_UploadDataRow_UploadSession] FOREIGN KEY ([UploadSession_ID]) REFERENCES [dbo].[UploadSession] ([UploadSession_ID])
);


GO
CREATE NONCLUSTERED INDEX [_dta_IX_UploadRow_UploadSessionID]
    ON [dbo].[UploadRow]([UploadSession_ID] ASC)
    INCLUDE([UploadRow_ID], [Item_Key], [Identifier], [ValidationLevel]);


GO
GRANT SELECT
    ON OBJECT::[dbo].[UploadRow] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[UploadRow] TO [IRMAReportsRole]
    AS [dbo];

