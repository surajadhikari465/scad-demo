CREATE TABLE [dbo].[UploadTypeTemplateAttribute] (
    [UploadTypeTemplateAttribute_ID] INT IDENTITY (1, 1) NOT NULL,
    [UploadTypeTemplate_ID]          INT NOT NULL,
    [UploadTypeAttribute_ID]         INT NOT NULL,
    CONSTRAINT [PK_UploadTypeTemplateAttribute] PRIMARY KEY CLUSTERED ([UploadTypeTemplateAttribute_ID] ASC),
    CONSTRAINT [FK_UploadTypeTemplateAttribute_UploadTypeAttribute] FOREIGN KEY ([UploadTypeAttribute_ID]) REFERENCES [dbo].[UploadTypeAttribute] ([UploadTypeAttribute_ID]),
    CONSTRAINT [FK_UploadTypeTemplateAttribute_UploadTypeTemplate] FOREIGN KEY ([UploadTypeTemplate_ID]) REFERENCES [dbo].[UploadTypeTemplate] ([UploadTypeTemplate_ID])
);


GO
GRANT SELECT
    ON OBJECT::[dbo].[UploadTypeTemplateAttribute] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[UploadTypeTemplateAttribute] TO [IRMAReportsRole]
    AS [dbo];

