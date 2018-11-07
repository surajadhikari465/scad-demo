CREATE TABLE [dbo].[EInvoicing_Logging] (
    [Id]         INT            IDENTITY (1, 1) NOT NULL,
    [LogDate]    DATETIME       DEFAULT (getdate()) NOT NULL,
    [LogLevel]   VARCHAR (255)  NOT NULL,
    [LogMessage] VARCHAR (4000) NULL,
    CONSTRAINT [EInvoicing_Logging_PK] PRIMARY KEY CLUSTERED ([Id] ASC)
);


GO
CREATE NONCLUSTERED INDEX [IX_EInvoicingLogging_Date]
    ON [dbo].[EInvoicing_Logging]([LogDate] ASC);


GO
GRANT DELETE
    ON OBJECT::[dbo].[EInvoicing_Logging] TO [IRMAAdminRole]
    AS [dbo];


GO
GRANT INSERT
    ON OBJECT::[dbo].[EInvoicing_Logging] TO [IRMAAdminRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[EInvoicing_Logging] TO [IRMAAdminRole]
    AS [dbo];


GO
GRANT UPDATE
    ON OBJECT::[dbo].[EInvoicing_Logging] TO [IRMAAdminRole]
    AS [dbo];


GO
GRANT DELETE
    ON OBJECT::[dbo].[EInvoicing_Logging] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT INSERT
    ON OBJECT::[dbo].[EInvoicing_Logging] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[EInvoicing_Logging] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT UPDATE
    ON OBJECT::[dbo].[EInvoicing_Logging] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[EInvoicing_Logging] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[EInvoicing_Logging] TO [IRMAReportsRole]
    AS [dbo];

