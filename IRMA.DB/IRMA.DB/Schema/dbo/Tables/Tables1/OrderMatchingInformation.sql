CREATE TABLE [dbo].[OrderMatchingInformation] (
    [OrderHeader_Id]      INT            NOT NULL,
    [InvoiceCost]         MONEY          CONSTRAINT [DF_OrderMatchingInformation_InvoiceCost] DEFAULT ((0)) NOT NULL,
    [AllocatedCharges]    MONEY          CONSTRAINT [DF_OrderMatchingInformation_AllocatedCharges] DEFAULT ((0)) NOT NULL,
    [ReceivedOrderedCost] MONEY          CONSTRAINT [DF_OrderMatchingInformation_ReceivedOrderedCost] DEFAULT ((0)) NULL,
    [ReceivedInvoiceCost] MONEY          CONSTRAINT [DF_OrderMatchingInformation_ReceivedInvoiceCost] DEFAULT ((0)) NULL,
    [ReceivedCost]        MONEY          CONSTRAINT [DF_OrderMatchingInformation_ReceivedCost] DEFAULT ((0)) NULL,
    [Tolerance]           DECIMAL (5, 2) NULL,
    [ToleranceAmt]        MONEY          NULL,
    [Variance]            MONEY          NULL,
    [Info]                VARCHAR (512)  NULL,
    CONSTRAINT [PK_OrderMatchingInformation_OHID] PRIMARY KEY CLUSTERED ([OrderHeader_Id] ASC) WITH (FILLFACTOR = 80)
);


GO
GRANT SELECT
    ON OBJECT::[dbo].[OrderMatchingInformation] TO [IRMAReports]
    AS [dbo];

