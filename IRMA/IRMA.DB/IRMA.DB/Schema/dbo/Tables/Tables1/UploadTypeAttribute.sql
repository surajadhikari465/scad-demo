CREATE TABLE [dbo].[UploadTypeAttribute] (
    [UploadTypeAttribute_ID]                  INT           IDENTITY (1, 1) NOT NULL,
    [UploadType_Code]                         VARCHAR (50)  NULL,
    [UploadAttribute_ID]                      INT           NULL,
    [IsRequiredForUploadTypeForExistingItems] BIT           DEFAULT ((0)) NOT NULL,
    [IsReadOnlyForExistingItems]              BIT           DEFAULT ((0)) NOT NULL,
    [IsHidden]                                BIT           DEFAULT ((0)) NOT NULL,
    [GridPosition]                            INT           DEFAULT ((0)) NOT NULL,
    [IsRequiredForUploadTypeForNewItems]      BIT           DEFAULT ((0)) NULL,
    [IsReadOnlyForNewItems]                   BIT           DEFAULT ((0)) NULL,
    [GroupName]                               VARCHAR (100) NULL,
    CONSTRAINT [PK_UploadTypeAttribute] PRIMARY KEY CLUSTERED ([UploadTypeAttribute_ID] ASC),
    CONSTRAINT [FK_UploadAttribute_UploadType] FOREIGN KEY ([UploadType_Code]) REFERENCES [dbo].[UploadType] ([UploadType_Code]),
    CONSTRAINT [FK_UploadTypeAttribute_UploadAttribute] FOREIGN KEY ([UploadAttribute_ID]) REFERENCES [dbo].[UploadAttribute] ([UploadAttribute_ID])
);


GO
GRANT SELECT
    ON OBJECT::[dbo].[UploadTypeAttribute] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[UploadTypeAttribute] TO [IRMAReportsRole]
    AS [dbo];

