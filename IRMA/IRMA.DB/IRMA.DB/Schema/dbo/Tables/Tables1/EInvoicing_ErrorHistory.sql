CREATE TABLE [dbo].[EInvoicing_ErrorHistory] (
    [ErrorHistoryId]   INT           IDENTITY (1, 1) NOT NULL,
    [EInvoiceId]       INT           NOT NULL,
    [Timestamp]        DATETIME      DEFAULT (getdate()) NOT NULL,
    [ErrorInformation] VARCHAR (MAX) NULL,
    PRIMARY KEY CLUSTERED ([ErrorHistoryId] ASC)
);


GO
GRANT DELETE
    ON OBJECT::[dbo].[EInvoicing_ErrorHistory] TO [IRMAAdminRole]
    AS [dbo];


GO
GRANT INSERT
    ON OBJECT::[dbo].[EInvoicing_ErrorHistory] TO [IRMAAdminRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[EInvoicing_ErrorHistory] TO [IRMAAdminRole]
    AS [dbo];


GO
GRANT UPDATE
    ON OBJECT::[dbo].[EInvoicing_ErrorHistory] TO [IRMAAdminRole]
    AS [dbo];


GO
GRANT DELETE
    ON OBJECT::[dbo].[EInvoicing_ErrorHistory] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT INSERT
    ON OBJECT::[dbo].[EInvoicing_ErrorHistory] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[EInvoicing_ErrorHistory] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT UPDATE
    ON OBJECT::[dbo].[EInvoicing_ErrorHistory] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[EInvoicing_ErrorHistory] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[EInvoicing_ErrorHistory] TO [IRMAReportsRole]
    AS [dbo];

