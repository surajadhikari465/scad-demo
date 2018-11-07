CREATE TABLE [dbo].[OrderItemCOOL4020Detail] (
    [OrderItemCOOL4020DetailID] INT        IDENTITY (1, 1) NOT NULL,
    [OrderItem_ID]              INT        NOT NULL,
    [ActionCode]                CHAR (1)   NULL,
    [ReceivingDC]               NCHAR (2)  NULL,
    [ReceivingWarehouse]        NCHAR (2)  NULL,
    [IPSID]                     NCHAR (10) NULL,
    [Product]                   NCHAR (18) NULL,
    [ProductDetail]             NCHAR (1)  NULL,
    [ProductDescription]        NCHAR (30) NULL,
    [ProductSize]               NCHAR (10) NULL,
    [SupplierName]              NCHAR (30) NULL,
    [SupplierAddress1]          NCHAR (30) NULL,
    [SupplierAddress2]          NCHAR (30) NULL,
    [SupplierCity]              NCHAR (30) NULL,
    [SupplierState]             NCHAR (15) NULL,
    [SupplierZip]               NCHAR (10) NULL,
    [SupplierPhone]             NCHAR (20) NULL,
    [SupplierCountry]           NCHAR (15) NULL,
    [SupplierContact]           NCHAR (30) NULL,
    [EXEReceiptID]              NCHAR (10) NULL,
    [HostPOID]                  NCHAR (10) NULL,
    [ReceiptDate]               NCHAR (10) NULL,
    [ReceiptQuantity]           NCHAR (9)  NULL,
    [LotNumber]                 NCHAR (20) NULL,
    [InboundCarrierName]        NCHAR (99) NULL,
    [InboundCarrierAddress1]    NCHAR (50) NULL,
    [InboundCarrierAddress2]    NCHAR (30) NULL,
    [InboundCarrierCity]        NCHAR (29) NULL,
    [InboundCarrierState]       NCHAR (15) NULL,
    [InboundCarrierZip]         NCHAR (10) NULL,
    [InboundCarrierPhone]       NCHAR (20) NULL,
    [InboundCarrierCountry]     NCHAR (20) NULL,
    [IIPSRefNo]                 NCHAR (14) NULL,
    [IIPSRefSeq]                NCHAR (4)  NULL,
    CONSTRAINT [PK_OrderItemCOOL4020Detail] PRIMARY KEY CLUSTERED ([OrderItemCOOL4020DetailID] ASC)
);


GO
CREATE NONCLUSTERED INDEX [IX_Cool4020_OrderItemId]
    ON [dbo].[OrderItemCOOL4020Detail]([OrderItem_ID] ASC) WITH (FILLFACTOR = 90);

