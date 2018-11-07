CREATE TABLE [dbo].[StoreFTPConfig] (
    [Store_No]         INT           NOT NULL,
    [FileWriterType]   VARCHAR (10)  NOT NULL,
    [IP_Address]       VARCHAR (15)  NOT NULL,
    [FTP_User]         VARCHAR (25)  NOT NULL,
    [FTP_Password]     VARCHAR (25)  NOT NULL,
    [ChangeDirectory]  VARCHAR (100) NULL,
    [Port]             INT           NULL,
    [IsSecureTransfer] BIT           CONSTRAINT [DF_StoreFTPConfig_IsSecureTransfer] DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK_StoreFTPConfig_StoreNo_FileWriterType] PRIMARY KEY CLUSTERED ([Store_No] ASC, [FileWriterType] ASC),
    CONSTRAINT [FK_StoreFTPConfig_FileWriterType] FOREIGN KEY ([FileWriterType]) REFERENCES [dbo].[FileWriterType] ([FileWriterType]),
    CONSTRAINT [FK_StoreFTPConfig_StoreNo] FOREIGN KEY ([Store_No]) REFERENCES [dbo].[Store] ([Store_No])
);


GO
GRANT DELETE
    ON OBJECT::[dbo].[StoreFTPConfig] TO [IRMAAdminRole]
    AS [dbo];


GO
GRANT INSERT
    ON OBJECT::[dbo].[StoreFTPConfig] TO [IRMAAdminRole]
    AS [dbo];


GO
GRANT UPDATE
    ON OBJECT::[dbo].[StoreFTPConfig] TO [IRMAAdminRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[StoreFTPConfig] TO [IRMAReportsRole]
    AS [dbo];

