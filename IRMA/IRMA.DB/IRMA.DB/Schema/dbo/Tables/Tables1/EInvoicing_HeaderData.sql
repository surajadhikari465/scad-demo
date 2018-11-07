CREATE TABLE [dbo].[EInvoicing_HeaderData] (
    [EInvoice_Id]  INT           NOT NULL,
    [ElementName]  VARCHAR (255) NOT NULL,
    [ElementValue] VARCHAR (255) NULL
);


GO
CREATE NONCLUSTERED INDEX [IX_EInvoicingHeaderData_Id]
    ON [dbo].[EInvoicing_HeaderData]([EInvoice_Id] ASC);


GO
GRANT SELECT
    ON OBJECT::[dbo].[EInvoicing_HeaderData] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[EInvoicing_HeaderData] TO [IRMAReportsRole]
    AS [dbo];

