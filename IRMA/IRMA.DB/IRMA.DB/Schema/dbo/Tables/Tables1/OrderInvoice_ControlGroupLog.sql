CREATE TABLE [dbo].[OrderInvoice_ControlGroupLog] (
    [InsertDate]                         DATETIME     NOT NULL,
    [UpdateUser_ID]                      INT          NULL,
    [OrderInvoice_ControlGroup_ID]       INT          NOT NULL,
    [ExpectedGrossAmt]                   MONEY        NULL,
    [ExpectedInvoiceCount]               INT          NULL,
    [OrderInvoice_ControlGroupStatus_ID] INT          NULL,
    [CurrentGrossAmt]                    MONEY        NOT NULL,
    [CurrentInvoiceCount]                INT          NOT NULL,
    [InvoiceType]                        INT          NULL,
    [Return_Order]                       BIT          NULL,
    [InvoiceCost]                        SMALLMONEY   NULL,
    [InvoiceFreight]                     SMALLMONEY   NULL,
    [InvoiceNumber]                      VARCHAR (16) NULL,
    [Vendor_ID]                          INT          NULL,
    CONSTRAINT [FK_OrderInvoice_ControlGroupLog_InvoiceType] FOREIGN KEY ([InvoiceType]) REFERENCES [dbo].[OrderInvoice_InvoiceType] ([OrderInvoice_InvoiceType_ID]),
    CONSTRAINT [FK_OrderInvoice_ControlGroupLog_Vendor_ID] FOREIGN KEY ([Vendor_ID]) REFERENCES [dbo].[Vendor] ([Vendor_ID])
);

