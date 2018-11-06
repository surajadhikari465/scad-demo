CREATE TABLE [dbo].[EInvoicing_SACTypes] (
    [SACType_Id] INT           IDENTITY (1, 1) NOT NULL,
    [SACType]    VARCHAR (100) NOT NULL,
    CONSTRAINT [EInvoicing_SACTypes_PK] PRIMARY KEY CLUSTERED ([SACType_Id] ASC)
);


GO
GRANT INSERT
    ON OBJECT::[dbo].[EInvoicing_SACTypes] TO [IRMAAdminRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[EInvoicing_SACTypes] TO [IRMAAdminRole]
    AS [dbo];


GO
GRANT UPDATE
    ON OBJECT::[dbo].[EInvoicing_SACTypes] TO [IRMAAdminRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[EInvoicing_SACTypes] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[EInvoicing_SACTypes] TO [IRMAReportsRole]
    AS [dbo];

