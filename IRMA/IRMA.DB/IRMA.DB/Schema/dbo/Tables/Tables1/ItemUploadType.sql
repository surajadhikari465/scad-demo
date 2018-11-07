CREATE TABLE [dbo].[ItemUploadType] (
    [ItemUploadType_ID] INT          IDENTITY (1, 1) NOT NULL,
    [Description]       VARCHAR (50) NOT NULL,
    CONSTRAINT [PK_ItemUploadType] PRIMARY KEY CLUSTERED ([ItemUploadType_ID] ASC)
);

