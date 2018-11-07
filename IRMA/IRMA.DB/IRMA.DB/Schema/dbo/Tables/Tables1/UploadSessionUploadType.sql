CREATE TABLE [dbo].[UploadSessionUploadType] (
    [UploadSessionUploadType_ID] INT          IDENTITY (1, 1) NOT NULL,
    [UploadSession_ID]           INT          NOT NULL,
    [UploadType_Code]            VARCHAR (50) NOT NULL,
    [UploadTypeTemplate_ID]      INT          NULL,
    [StoreSelectionType]         VARCHAR (50) NULL,
    [Zone_ID]                    INT          NULL,
    [State]                      VARCHAR (50) NULL,
    CONSTRAINT [PK_UploadSessionUploadType] PRIMARY KEY CLUSTERED ([UploadSessionUploadType_ID] ASC),
    CONSTRAINT [FK_UploadSessionUploadType_UploadSession] FOREIGN KEY ([UploadSession_ID]) REFERENCES [dbo].[UploadSession] ([UploadSession_ID]),
    CONSTRAINT [FK_UploadSessionUploadType_UploadType] FOREIGN KEY ([UploadType_Code]) REFERENCES [dbo].[UploadType] ([UploadType_Code]),
    CONSTRAINT [FK_UploadSessionUploadType_Zone] FOREIGN KEY ([Zone_ID]) REFERENCES [dbo].[Zone] ([Zone_ID])
);


GO
CREATE NONCLUSTERED INDEX [_dta_IX_UploadSessionUploadType_UploadSessionID]
    ON [dbo].[UploadSessionUploadType]([UploadSession_ID] ASC)
    INCLUDE([UploadType_Code]);


GO
CREATE STATISTICS [_dta_stat_UploadSessionUploadType_001]
    ON [dbo].[UploadSessionUploadType]([UploadSession_ID], [UploadTypeTemplate_ID], [Zone_ID]);


GO
GRANT SELECT
    ON OBJECT::[dbo].[UploadSessionUploadType] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[UploadSessionUploadType] TO [IRMAReportsRole]
    AS [dbo];

