CREATE TABLE [dbo].[OrderItemRefused] (
    [OrderItemRefusedID] INT             IDENTITY (1, 1) NOT NULL,
    [OrderHeader_ID]     INT             NOT NULL,
    [OrderItem_ID]       INT             NULL,
    [Identifier]         VARCHAR (13)    NULL,
    [VendorItemNumber]   VARCHAR (255)   NULL,
    [Description]        VARCHAR (60)    NULL,
    [Unit]               VARCHAR (25)    NULL,
    [InvoiceQuantity]    DECIMAL (18, 4) NULL,
    [InvoiceCost]        MONEY           NULL,
    [RefusedQuantity]    DECIMAL (18, 4) NULL,
    [DiscrepancyCodeID]  INT             NULL,
    [UserAddedEntry]     BIT             NULL,
    [eInvoice_Id]        INT             NULL,
    CONSTRAINT [PK_OrderItemRefused] PRIMARY KEY CLUSTERED ([OrderItemRefusedID] ASC) WITH (FILLFACTOR = 80)
);


GO
GRANT SELECT
    ON OBJECT::[dbo].[OrderItemRefused] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[OrderItemRefused] TO [IRMAReportsRole]
    AS [dbo];

