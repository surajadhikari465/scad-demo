CREATE TABLE [dbo].[OrderInvoice] (
    [OrderHeader_ID]   INT        NOT NULL,
    [SubTeam_No]       INT        NOT NULL,
    [InvoiceCost]      SMALLMONEY CONSTRAINT [DF_OrderInvoice_InvoiceCost] DEFAULT ((0)) NULL,
    [InvoiceFreight]   SMALLMONEY CONSTRAINT [DF_OrderInvoice_InvoiceFreight] DEFAULT ((0)) NULL,
    [InvoiceTotalCost] MONEY      NULL,
    CONSTRAINT [PK_OrderInvoice_OrderHeader_ID_SubTeam_No] PRIMARY KEY NONCLUSTERED ([OrderHeader_ID] ASC, [SubTeam_No] ASC) WITH (FILLFACTOR = 80),
    CONSTRAINT [FK__OrderInvo__Order__239A86E6] FOREIGN KEY ([OrderHeader_ID]) REFERENCES [dbo].[OrderHeader] ([OrderHeader_ID]),
    CONSTRAINT [FK_From_OrderInvoice_SubTeam_SubTeam_No] FOREIGN KEY ([SubTeam_No]) REFERENCES [dbo].[SubTeam] ([SubTeam_No])
);





GO
ALTER TABLE [dbo].[OrderInvoice] ENABLE CHANGE_TRACKING WITH (TRACK_COLUMNS_UPDATED = OFF);


GO
CREATE CLUSTERED INDEX [_dta_IX_OrderInvoice_OrderHeader_ID]
    ON [dbo].[OrderInvoice]([OrderHeader_ID] ASC);


GO
CREATE STATISTICS [_dta_stat_OrderInvoice_InvoiceCost_OrderHeader_ID]
    ON [dbo].[OrderInvoice]([InvoiceCost], [OrderHeader_ID]);


GO
CREATE STATISTICS [_dta_stat_OrderInvoice_OrderHeader_ID_InvoiceFreight]
    ON [dbo].[OrderInvoice]([OrderHeader_ID], [InvoiceFreight]);


GO
GRANT VIEW CHANGE TRACKING
    ON OBJECT::[dbo].[OrderInvoice] TO [IRMAAdminRole]
    AS [dbo];


GO
GRANT VIEW CHANGE TRACKING
    ON OBJECT::[dbo].[OrderInvoice] TO [IRMASupportRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[OrderInvoice] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[OrderInvoice] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT VIEW CHANGE TRACKING
    ON OBJECT::[dbo].[OrderInvoice] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[OrderInvoice] TO [IRMAReportsRole]
    AS [dbo];


GO
GRANT VIEW CHANGE TRACKING
    ON OBJECT::[dbo].[OrderInvoice] TO [IRMAReportsRole]
    AS [dbo];


GO
GRANT VIEW CHANGE TRACKING
    ON OBJECT::[dbo].[OrderInvoice] TO [IRMA_Teradata]
    AS [dbo];


GO
GRANT VIEW CHANGE TRACKING
    ON OBJECT::[dbo].[OrderInvoice] TO [IConInterface]
    AS [dbo];


GO
GRANT VIEW CHANGE TRACKING
    ON OBJECT::[dbo].[OrderInvoice] TO [iCONReportingRole]
    AS [dbo];

