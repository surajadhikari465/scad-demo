CREATE TABLE [dbo].[OrderInvoice_InvoiceType] (
    [OrderInvoice_InvoiceType_ID]   INT          NOT NULL,
    [OrderInvoice_InvoiceType_Desc] VARCHAR (60) NOT NULL,
    CONSTRAINT [PK_OrderInvoice_InvoiceType] PRIMARY KEY CLUSTERED ([OrderInvoice_InvoiceType_ID] ASC)
);

