CREATE TABLE [dbo].[Einvoicing_Invoices] (
    [EInvoice_Id]        INT           IDENTITY (1, 1) NOT NULL,
    [Invoice_Num]        VARCHAR (255) NOT NULL,
    [PSVendor_Id]        VARCHAR (255) NOT NULL,
    [Store_No]           INT           NOT NULL,
    [po_num]             VARCHAR (255) NULL,
    [InvoiceDate]        SMALLDATETIME NOT NULL,
    [InvoiceXML]         XML           NOT NULL,
    [Status]             VARCHAR (255) NULL,
    [ErrorCode_id]       INT           NULL,
    [ImportDate]         DATETIME      CONSTRAINT [ImportDateDefault_Temp] DEFAULT (getdate()) NOT NULL,
    [Archived]           BIT           NULL,
    [ArchivedDate]       DATETIME      NULL,
    [Original_PO_Num]    VARCHAR (255) NULL,
    [EditedBy]           INT           NULL,
    [EditedDate]         DATETIME      NULL,
    [ErrorEmailSentDate] DATETIME      NULL,
    [psvendor_id_padded] AS            (right('0000000000'+CONVERT([varchar](255),[psvendor_id],(0)),(10))) PERSISTED,
    [po_num_clean]       AS            ([dbo].[udfStripCharacters]([po_num])) PERSISTED,
    CONSTRAINT [EInvoicingInvoices_Temp_PK] PRIMARY KEY CLUSTERED ([EInvoice_Id] ASC) WITH (FILLFACTOR = 80)
);


GO
CREATE NONCLUSTERED INDEX [IX_EI_InvoiceNum]
    ON [dbo].[Einvoicing_Invoices]([Invoice_Num] ASC) WITH (FILLFACTOR = 80);


GO
CREATE NONCLUSTERED INDEX [IX_EI_VendorId]
    ON [dbo].[Einvoicing_Invoices]([PSVendor_Id] ASC) WITH (FILLFACTOR = 80);


GO
CREATE NONCLUSTERED INDEX [IX_EI_InvoiceDate]
    ON [dbo].[Einvoicing_Invoices]([InvoiceDate] ASC) WITH (FILLFACTOR = 80);


GO
CREATE NONCLUSTERED INDEX [IX_EI_Invoices_PONum]
    ON [dbo].[Einvoicing_Invoices]([po_num] ASC) WITH (FILLFACTOR = 80);


GO
GRANT DELETE
    ON OBJECT::[dbo].[Einvoicing_Invoices] TO [IRMAAdminRole]
    AS [dbo];


GO
GRANT INSERT
    ON OBJECT::[dbo].[Einvoicing_Invoices] TO [IRMAAdminRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[Einvoicing_Invoices] TO [IRMAAdminRole]
    AS [dbo];


GO
GRANT UPDATE
    ON OBJECT::[dbo].[Einvoicing_Invoices] TO [IRMAAdminRole]
    AS [dbo];


GO
GRANT DELETE
    ON OBJECT::[dbo].[Einvoicing_Invoices] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT INSERT
    ON OBJECT::[dbo].[Einvoicing_Invoices] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[Einvoicing_Invoices] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT UPDATE
    ON OBJECT::[dbo].[Einvoicing_Invoices] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT DELETE
    ON OBJECT::[dbo].[Einvoicing_Invoices] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT INSERT
    ON OBJECT::[dbo].[Einvoicing_Invoices] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[Einvoicing_Invoices] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT UPDATE
    ON OBJECT::[dbo].[Einvoicing_Invoices] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT DELETE
    ON OBJECT::[dbo].[Einvoicing_Invoices] TO [IRMAReportsRole]
    AS [dbo];


GO
GRANT INSERT
    ON OBJECT::[dbo].[Einvoicing_Invoices] TO [IRMAReportsRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[Einvoicing_Invoices] TO [IRMAReportsRole]
    AS [dbo];


GO
GRANT UPDATE
    ON OBJECT::[dbo].[Einvoicing_Invoices] TO [IRMAReportsRole]
    AS [dbo];

