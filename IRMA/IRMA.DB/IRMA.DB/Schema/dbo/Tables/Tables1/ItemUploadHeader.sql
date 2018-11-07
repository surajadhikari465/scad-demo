CREATE TABLE [dbo].[ItemUploadHeader] (
    [ItemUploadHeader_ID] INT            IDENTITY (1, 1) NOT NULL,
    [ItemUploadType_ID]   INT            NOT NULL,
    [ItemsProcessedCount] INT            NULL,
    [ItemsLoadedCount]    INT            NULL,
    [ErrorsCount]         INT            NULL,
    [EmailToAddress]      VARCHAR (2000) NOT NULL,
    [User_ID]             INT            NOT NULL,
    [UploadDateTime]      DATETIME       CONSTRAINT [DF_ItemUploadHeader_UploadDateTime] DEFAULT (getdate()) NOT NULL,
    CONSTRAINT [PK_ItemUploadHeader] PRIMARY KEY CLUSTERED ([ItemUploadHeader_ID] ASC),
    CONSTRAINT [FK_ItemUploadHeader_ItemUploadType] FOREIGN KEY ([ItemUploadType_ID]) REFERENCES [dbo].[ItemUploadType] ([ItemUploadType_ID]),
    CONSTRAINT [FK_ItemUploadHeader_Users] FOREIGN KEY ([User_ID]) REFERENCES [dbo].[Users] ([User_ID])
);

