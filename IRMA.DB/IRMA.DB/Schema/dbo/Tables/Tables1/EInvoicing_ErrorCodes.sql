CREATE TABLE [dbo].[EInvoicing_ErrorCodes] (
    [ErrorCode_Id] INT            IDENTITY (1, 1) NOT NULL,
    [ErrorMessage] VARCHAR (2048) NOT NULL,
    [Description]  VARCHAR (2048) NULL,
    CONSTRAINT [EInvoicing_ErrorCodes_PK] PRIMARY KEY CLUSTERED ([ErrorCode_Id] ASC)
);


GO
GRANT DELETE
    ON OBJECT::[dbo].[EInvoicing_ErrorCodes] TO [IRMAAdminRole]
    AS [dbo];


GO
GRANT INSERT
    ON OBJECT::[dbo].[EInvoicing_ErrorCodes] TO [IRMAAdminRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[EInvoicing_ErrorCodes] TO [IRMAAdminRole]
    AS [dbo];


GO
GRANT UPDATE
    ON OBJECT::[dbo].[EInvoicing_ErrorCodes] TO [IRMAAdminRole]
    AS [dbo];


GO
GRANT DELETE
    ON OBJECT::[dbo].[EInvoicing_ErrorCodes] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT INSERT
    ON OBJECT::[dbo].[EInvoicing_ErrorCodes] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[EInvoicing_ErrorCodes] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT UPDATE
    ON OBJECT::[dbo].[EInvoicing_ErrorCodes] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[EInvoicing_ErrorCodes] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[EInvoicing_ErrorCodes] TO [IRMAReportsRole]
    AS [dbo];

