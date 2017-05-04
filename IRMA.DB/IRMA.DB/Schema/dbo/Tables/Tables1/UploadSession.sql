CREATE TABLE [dbo].[UploadSession] (
    [UploadSession_ID]        INT           IDENTITY (1, 1) NOT NULL,
    [Name]                    VARCHAR (100) NULL,
    [IsUploaded]              BIT           DEFAULT ((0)) NOT NULL,
    [ItemsProcessedCount]     INT           NULL,
    [ItemsLoadedCount]        INT           NULL,
    [ErrorsCount]             INT           NULL,
    [EmailToAddress]          VARCHAR (255) NOT NULL,
    [CreatedByUserID]         INT           NOT NULL,
    [CreatedDateTime]         DATETIME      NOT NULL,
    [ModifiedByUserID]        INT           NULL,
    [ModifiedDateTime]        DATETIME      NULL,
    [IsNewItemSessionFlag]    BIT           DEFAULT ((0)) NULL,
    [IsFromSLIM]              BIT           DEFAULT ((0)) NOT NULL,
    [IsDeleteItemSessionFlag] BIT           DEFAULT ((0)) NULL,
    CONSTRAINT [PK_UploadSession] PRIMARY KEY CLUSTERED ([UploadSession_ID] ASC)
);


GO
CREATE STATISTICS [_dta_stat_UploadSession_001]
    ON [dbo].[UploadSession]([CreatedByUserID], [ModifiedByUserID]);


GO
CREATE STATISTICS [_dta_stat_UploadSession_002]
    ON [dbo].[UploadSession]([IsUploaded], [UploadSession_ID]);


GO
CREATE STATISTICS [_dta_stat_UploadSession_003]
    ON [dbo].[UploadSession]([ModifiedByUserID], [CreatedByUserID]);


GO
CREATE STATISTICS [_dta_stat_UploadSession_004]
    ON [dbo].[UploadSession]([ModifiedByUserID], [UploadSession_ID], [CreatedByUserID]);


GO
CREATE STATISTICS [_dta_stat_UploadSession_005]
    ON [dbo].[UploadSession]([UploadSession_ID], [CreatedByUserID], [ModifiedByUserID]);


GO
GRANT SELECT
    ON OBJECT::[dbo].[UploadSession] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[UploadSession] TO [IRMAReportsRole]
    AS [dbo];

