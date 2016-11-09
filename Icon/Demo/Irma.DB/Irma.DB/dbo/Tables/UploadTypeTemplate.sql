CREATE TABLE [dbo].[UploadTypeTemplate] (
    [UploadTypeTemplate_ID] INT          IDENTITY (11, 1) NOT NULL,
    [UploadType_Code]       VARCHAR (50) NOT NULL,
    [Name]                  VARCHAR (50) NOT NULL,
    [CreatedByUserID]       INT          NOT NULL,
    [CreatedDateTime]       DATETIME     NOT NULL,
    [ModifiedByUserID]      INT          NULL,
    [ModifiedDateTime]      DATETIME     NULL,
    CONSTRAINT [PK_UploadTypeTemplate] PRIMARY KEY CLUSTERED ([UploadTypeTemplate_ID] ASC),
    CONSTRAINT [FK_UploadTypeTemplate_UploadType] FOREIGN KEY ([UploadType_Code]) REFERENCES [dbo].[UploadType] ([UploadType_Code])
);


GO
GRANT SELECT
    ON OBJECT::[dbo].[UploadTypeTemplate] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[UploadTypeTemplate] TO [IRMAReportsRole]
    AS [dbo];

