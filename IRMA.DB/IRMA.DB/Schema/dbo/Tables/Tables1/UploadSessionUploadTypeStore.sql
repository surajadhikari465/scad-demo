CREATE TABLE [dbo].[UploadSessionUploadTypeStore] (
    [UploadSessionUploadTypeStore_ID] INT IDENTITY (1, 1) NOT NULL,
    [UploadSessionUploadType_ID]      INT NULL,
    [Store_No]                        INT NULL,
    CONSTRAINT [PK_UploadSessionUploadTypeStore] PRIMARY KEY CLUSTERED ([UploadSessionUploadTypeStore_ID] ASC),
    CONSTRAINT [FK_UploadSessionUploadTypeStore_Store] FOREIGN KEY ([Store_No]) REFERENCES [dbo].[Store] ([Store_No]),
    CONSTRAINT [FK_UploadSessionUploadTypeStore_UploadSessionUploadType] FOREIGN KEY ([UploadSessionUploadType_ID]) REFERENCES [dbo].[UploadSessionUploadType] ([UploadSessionUploadType_ID])
);


GO
CREATE NONCLUSTERED INDEX [_dta_IX_UploadSessionUploadTypeStore_UploadSessionUploadTypeID]
    ON [dbo].[UploadSessionUploadTypeStore]([UploadSessionUploadType_ID] ASC)
    INCLUDE([UploadSessionUploadTypeStore_ID], [Store_No]);


GO
CREATE STATISTICS [_dta_stat_UploadSessionUploadTypeStore_001]
    ON [dbo].[UploadSessionUploadTypeStore]([Store_No], [UploadSessionUploadType_ID]);


GO
GRANT SELECT
    ON OBJECT::[dbo].[UploadSessionUploadTypeStore] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[UploadSessionUploadTypeStore] TO [IRMAReportsRole]
    AS [dbo];

