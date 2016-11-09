CREATE TABLE [dbo].[Einvoicing_Invoices_Old] (
    [EInvoice_Id]     INT           IDENTITY (1, 1) NOT NULL,
    [Invoice_Num]     VARCHAR (255) NOT NULL,
    [PSVendor_Id]     VARCHAR (255) NOT NULL,
    [Store_No]        INT           NOT NULL,
    [po_num]          VARCHAR (255) NOT NULL,
    [InvoiceDate]     SMALLDATETIME NOT NULL,
    [InvoiceXML]      XML           NOT NULL,
    [Status]          VARCHAR (255) NULL,
    [ErrorCode_id]    INT           NULL,
    [ImportDate]      DATETIME      CONSTRAINT [ImportDateDefault] DEFAULT (getdate()) NOT NULL,
    [Archived]        BIT           NULL,
    [ArchivedDate]    DATETIME      NULL,
    [Original_PO_Num] VARCHAR (255) NULL,
    [EditedBy]        INT           NULL,
    [EditedDate]      DATETIME      NULL,
    CONSTRAINT [EInvoicingInvoices_PK] PRIMARY KEY CLUSTERED ([EInvoice_Id] ASC)
);


GO
CREATE NONCLUSTERED INDEX [IX_EInvoicingInvoices_VendorId]
    ON [dbo].[Einvoicing_Invoices_Old]([PSVendor_Id] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_EInvoicingInvoices_InvoiceNum]
    ON [dbo].[Einvoicing_Invoices_Old]([Invoice_Num] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_EInvoicingInvoices_InvoiceDate]
    ON [dbo].[Einvoicing_Invoices_Old]([InvoiceDate] ASC);


GO
GRANT DELETE
    ON OBJECT::[dbo].[Einvoicing_Invoices_Old] TO [IRMAAdminRole]
    AS [dbo];


GO
GRANT INSERT
    ON OBJECT::[dbo].[Einvoicing_Invoices_Old] TO [IRMAAdminRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[Einvoicing_Invoices_Old] TO [IRMAAdminRole]
    AS [dbo];


GO
GRANT UPDATE
    ON OBJECT::[dbo].[Einvoicing_Invoices_Old] TO [IRMAAdminRole]
    AS [dbo];


GO
GRANT DELETE
    ON OBJECT::[dbo].[Einvoicing_Invoices_Old] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT INSERT
    ON OBJECT::[dbo].[Einvoicing_Invoices_Old] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[Einvoicing_Invoices_Old] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT UPDATE
    ON OBJECT::[dbo].[Einvoicing_Invoices_Old] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[Einvoicing_Invoices_Old] TO [IRMAReportsRole]
    AS [dbo];

