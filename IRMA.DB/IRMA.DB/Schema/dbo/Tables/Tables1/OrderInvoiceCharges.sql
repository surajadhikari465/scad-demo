CREATE TABLE [dbo].[OrderInvoiceCharges] (
    [Charge_ID]      INT           IDENTITY (1, 1) NOT NULL,
    [OrderHeader_Id] INT           NOT NULL,
    [SACType_ID]     INT           NOT NULL,
    [OrderItem_ID]   INT           NULL,
    [SubTeam_No]     INT           NULL,
    [Value]          MONEY         NULL,
    [Description]    VARCHAR (100) NULL,
    [IsAllowance]    BIT           NULL,
    [ElementName]    VARCHAR (100) NULL,
    CONSTRAINT [OrderInvoiceCharges_PK] PRIMARY KEY CLUSTERED ([Charge_ID] ASC)
);


GO
ALTER TABLE [dbo].[OrderInvoiceCharges] ENABLE CHANGE_TRACKING WITH (TRACK_COLUMNS_UPDATED = OFF);


GO
CREATE NONCLUSTERED INDEX [OrderInvoiceCharges_IX]
    ON [dbo].[OrderInvoiceCharges]([OrderHeader_Id] ASC);


GO
GRANT SELECT
    ON OBJECT::[dbo].[OrderInvoiceCharges] TO [IRMAAdminRole]
    AS [dbo];


GO
GRANT VIEW CHANGE TRACKING
    ON OBJECT::[dbo].[OrderInvoiceCharges] TO [IRMAAdminRole]
    AS [dbo];


GO
GRANT VIEW CHANGE TRACKING
    ON OBJECT::[dbo].[OrderInvoiceCharges] TO [IRMASupportRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[OrderInvoiceCharges] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT VIEW CHANGE TRACKING
    ON OBJECT::[dbo].[OrderInvoiceCharges] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[OrderInvoiceCharges] TO [IRMAReportsRole]
    AS [dbo];


GO
GRANT VIEW CHANGE TRACKING
    ON OBJECT::[dbo].[OrderInvoiceCharges] TO [IRMAReportsRole]
    AS [dbo];


GO
GRANT VIEW CHANGE TRACKING
    ON OBJECT::[dbo].[OrderInvoiceCharges] TO [IRMA_Teradata]
    AS [dbo];


GO
GRANT VIEW CHANGE TRACKING
    ON OBJECT::[dbo].[OrderInvoiceCharges] TO [BizTalk]
    AS [dbo];


GO
GRANT VIEW CHANGE TRACKING
    ON OBJECT::[dbo].[OrderInvoiceCharges] TO [IConInterface]
    AS [dbo];


GO
GRANT VIEW CHANGE TRACKING
    ON OBJECT::[dbo].[OrderInvoiceCharges] TO [iCONReportingRole]
    AS [dbo];


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = 'NULL = Unknown 1 = Allowance (negative value) 2 = Charge (positive value)', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'OrderInvoiceCharges', @level2type = N'COLUMN', @level2name = N'IsAllowance';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = 'Contains the XML Element that generated the Invoice Charge from an EInvoice. If not generated from an EInvoice this vlaue will be NULL', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'OrderInvoiceCharges', @level2type = N'COLUMN', @level2name = N'ElementName';

