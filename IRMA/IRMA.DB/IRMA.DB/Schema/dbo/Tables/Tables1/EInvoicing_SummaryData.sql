CREATE TABLE [dbo].[EInvoicing_SummaryData] (
    [EInvoice_Id]  INT           NOT NULL,
    [ElementName]  VARCHAR (255) NOT NULL,
    [ElementValue] VARCHAR (255) NULL
);


GO
CREATE NONCLUSTERED INDEX [IX_EInvoicingSummaryData_Id]
    ON [dbo].[EInvoicing_SummaryData]([EInvoice_Id] ASC);


GO
GRANT SELECT
    ON OBJECT::[dbo].[EInvoicing_SummaryData] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[EInvoicing_SummaryData] TO [IRMAReportsRole]
    AS [dbo];

