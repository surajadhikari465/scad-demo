CREATE TABLE [dbo].[DeletedOrderItem] (
    [DeletedOrderItem_ID]              INT             IDENTITY (1, 1) NOT NULL,
    [DeletedOrder_ID]                  INT             NOT NULL,
    [OrderItem_ID]                     INT             NOT NULL,
    [OrderHeader_ID]                   INT             NOT NULL,
    [Item_Key]                         INT             NOT NULL,
    [ExpirationDate]                   SMALLDATETIME   NULL,
    [QuantityOrdered]                  DECIMAL (18, 4) NOT NULL,
    [QuantityUnit]                     INT             NOT NULL,
    [QuantityReceived]                 DECIMAL (18, 4) NULL,
    [Total_Weight]                     DECIMAL (18, 4) NOT NULL,
    [Units_per_Pallet]                 SMALLINT        NOT NULL,
    [Cost]                             MONEY           NOT NULL,
    [UnitCost]                         MONEY           NOT NULL,
    [UnitExtCost]                      MONEY           NOT NULL,
    [CostUnit]                         INT             NULL,
    [QuantityDiscount]                 DECIMAL (18, 4) NOT NULL,
    [DiscountType]                     INT             NOT NULL,
    [AdjustedCost]                     MONEY           NOT NULL,
    [Handling]                         MONEY           NOT NULL,
    [HandlingUnit]                     INT             NULL,
    [Freight]                          MONEY           NOT NULL,
    [FreightUnit]                      INT             NULL,
    [DateReceived]                     DATETIME        NULL,
    [OriginalDateReceived]             DATETIME        NULL,
    [Comments]                         VARCHAR (255)   NULL,
    [LineItemCost]                     MONEY           NOT NULL,
    [LineItemHandling]                 MONEY           NOT NULL,
    [LineItemFreight]                  MONEY           NOT NULL,
    [ReceivedItemCost]                 MONEY           NOT NULL,
    [ReceivedItemHandling]             MONEY           NOT NULL,
    [ReceivedItemFreight]              MONEY           NOT NULL,
    [LandedCost]                       MONEY           NOT NULL,
    [MarkupPercent]                    DECIMAL (18, 4) NOT NULL,
    [MarkupCost]                       MONEY           NOT NULL,
    [Package_Desc1]                    DECIMAL (9, 4)  NOT NULL,
    [Package_Desc2]                    DECIMAL (9, 4)  NOT NULL,
    [Package_Unit_ID]                  INT             NULL,
    [Retail_Unit_ID]                   INT             NULL,
    [Origin_ID]                        INT             NULL,
    [ReceivedFreight]                  MONEY           NOT NULL,
    [UnitsReceived]                    DECIMAL (18, 4) NOT NULL,
    [CreditReason_ID]                  INT             NULL,
    [QuantityAllocated]                DECIMAL (18, 4) NULL,
    [CountryProc_ID]                   INT             NULL,
    [Lot_No]                           VARCHAR (12)    NULL,
    [VendorCostHistoryID]              INT             NULL,
    [OHOrderDate]                      SMALLDATETIME   NULL,
    [SustainabilityRankingID]          INT             NULL,
    [NetVendorItemDiscount]            MONEY           NULL,
    [CostAdjustmentReason_ID]          INT             NULL,
    [Freight3Party]                    SMALLMONEY      NULL,
    [LineItemFreight3Party]            SMALLMONEY      NULL,
    [HandlingCharge]                   SMALLMONEY      NULL,
    [eInvoiceQuantity]                 DECIMAL (18, 4) NULL,
    [SACCost]                          SMALLMONEY      NULL,
    [OrderItemCOOL]                    BIT             NOT NULL,
    [OrderItemBIO]                     BIT             NOT NULL,
    [Carrier]                          VARCHAR (99)    NULL,
    [InvoiceQuantityUnit]              INT             NULL,
    [InvoiceCost]                      MONEY           NULL,
    [InvoiceExtendedCost]              MONEY           NULL,
    [InvoiceExtendedFreight]           MONEY           NULL,
    [InvoiceTotalWeight]               DECIMAL (18, 4) NULL,
    [OrigReceivedItemCost]             MONEY           NULL,
    [OrigReceivedItemUnit]             INT             NULL,
    [CatchWeightCostPerWeight]         MONEY           NULL,
    [QuantityShipped]                  DECIMAL (18, 4) NULL,
    [WeightShipped]                    DECIMAL (18, 4) NULL,
    [eInvoiceWeight]                   DECIMAL (18, 4) NULL,
    [ReasonCodeDetailID]               INT             NULL,
    [ReceivingDiscrepancyReasonCodeID] INT             NULL,
    [PaidCost]                         MONEY           NULL,
    [ApprovedDate]                     DATETIME        NULL,
    [ApprovedByUserId]                 INT             NULL,
    [AdminNotes]                       VARCHAR (5000)  NULL,
    [ResolutionCodeID]                 INT             NULL,
    [PaymentTypeID]                    INT             NULL,
    [LineItemSuspended]                BIT             NULL,
    CONSTRAINT [FK__DeletedOrderItem__CostU] FOREIGN KEY ([CostUnit]) REFERENCES [dbo].[ItemUnit] ([Unit_ID]),
    CONSTRAINT [FK__DeletedOrderItem__Freig] FOREIGN KEY ([FreightUnit]) REFERENCES [dbo].[ItemUnit] ([Unit_ID]),
    CONSTRAINT [FK__DeletedOrderItem__Handl] FOREIGN KEY ([HandlingUnit]) REFERENCES [dbo].[ItemUnit] ([Unit_ID]),
    CONSTRAINT [FK__DeletedOrderItem__OrderHeader_ID] FOREIGN KEY ([OrderHeader_ID]) REFERENCES [dbo].[OrderHeader] ([OrderHeader_ID]),
    CONSTRAINT [FK__DeletedOrderItem__Origi] FOREIGN KEY ([Origin_ID]) REFERENCES [dbo].[ItemOrigin] ([Origin_ID]),
    CONSTRAINT [FK__DeletedOrderItem__Quant] FOREIGN KEY ([QuantityUnit]) REFERENCES [dbo].[ItemUnit] ([Unit_ID]),
    CONSTRAINT [FK_DeletedOrderItem_CostAdjustmentReason] FOREIGN KEY ([CostAdjustmentReason_ID]) REFERENCES [dbo].[CostAdjustmentReason] ([CostAdjustmentReason_ID]),
    CONSTRAINT [FK_DeletedOrderItem_Item] FOREIGN KEY ([Item_Key]) REFERENCES [dbo].[Item] ([Item_Key]),
    CONSTRAINT [FK_DeletedOrderItem_ItemOrigin_CountryProc_ID] FOREIGN KEY ([CountryProc_ID]) REFERENCES [dbo].[ItemOrigin] ([Origin_ID]),
    CONSTRAINT [FK_DeletedOrderItem_ItemUnit] FOREIGN KEY ([Package_Unit_ID]) REFERENCES [dbo].[ItemUnit] ([Unit_ID]),
    CONSTRAINT [FK_DeletedOrderItem_ItemUnit1] FOREIGN KEY ([Retail_Unit_ID]) REFERENCES [dbo].[ItemUnit] ([Unit_ID]),
    CONSTRAINT [FK_DeletedOrderItem_ItemUnit2] FOREIGN KEY ([InvoiceQuantityUnit]) REFERENCES [dbo].[ItemUnit] ([Unit_ID]),
    CONSTRAINT [FK_DeletedOrderItem_ReasonCodeDetail] FOREIGN KEY ([ReasonCodeDetailID]) REFERENCES [dbo].[ReasonCodeDetail] ([ReasonCodeDetailID]),
    CONSTRAINT [FK_DeletedOrderItem_ReceivingDiscrepancyReasonCodeDetail] FOREIGN KEY ([ReceivingDiscrepancyReasonCodeID]) REFERENCES [dbo].[ReasonCodeDetail] ([ReasonCodeDetailID]),
    CONSTRAINT [FK_DeletedOrderItem_SustainabilityRanking] FOREIGN KEY ([SustainabilityRankingID]) REFERENCES [dbo].[SustainabilityRanking] ([ID])
);


GO
ALTER TABLE [dbo].[DeletedOrderItem] NOCHECK CONSTRAINT [FK__DeletedOrderItem__CostU];


GO
ALTER TABLE [dbo].[DeletedOrderItem] NOCHECK CONSTRAINT [FK__DeletedOrderItem__Freig];


GO
ALTER TABLE [dbo].[DeletedOrderItem] NOCHECK CONSTRAINT [FK__DeletedOrderItem__Handl];


GO
ALTER TABLE [dbo].[DeletedOrderItem] NOCHECK CONSTRAINT [FK__DeletedOrderItem__OrderHeader_ID];


GO
ALTER TABLE [dbo].[DeletedOrderItem] NOCHECK CONSTRAINT [FK__DeletedOrderItem__Origi];


GO
ALTER TABLE [dbo].[DeletedOrderItem] NOCHECK CONSTRAINT [FK__DeletedOrderItem__Quant];


GO
ALTER TABLE [dbo].[DeletedOrderItem] NOCHECK CONSTRAINT [FK_DeletedOrderItem_Item];


GO
ALTER TABLE [dbo].[DeletedOrderItem] NOCHECK CONSTRAINT [FK_DeletedOrderItem_ItemOrigin_CountryProc_ID];


GO
ALTER TABLE [dbo].[DeletedOrderItem] NOCHECK CONSTRAINT [FK_DeletedOrderItem_ItemUnit];


GO
ALTER TABLE [dbo].[DeletedOrderItem] NOCHECK CONSTRAINT [FK_DeletedOrderItem_ItemUnit1];


GO
CREATE NONCLUSTERED INDEX [_dta_IX_OrderItem_OrderHeader_ID_DiscountType_NetVendorItemDiscount_OrderItem_ID]
    ON [dbo].[DeletedOrderItem]([OrderHeader_ID] ASC, [DiscountType] ASC, [NetVendorItemDiscount] ASC, [OrderItem_ID] ASC)
    INCLUDE([LineItemCost], [ReceivedItemCost], [ReceivedItemFreight]) WITH (FILLFACTOR = 80);


GO
CREATE NONCLUSTERED INDEX [_dta_IX_OrderItem_OrderHeader_ID_Item_Key_OrderItem_ID]
    ON [dbo].[DeletedOrderItem]([OrderHeader_ID] ASC, [Item_Key] ASC, [OrderItem_ID] ASC)
    INCLUDE([UnitsReceived]) WITH (FILLFACTOR = 80);


GO
CREATE NONCLUSTERED INDEX [_dta_IX_OrderItem_OrderHeader_ID_OrderItem_ID_Item_Key]
    ON [dbo].[DeletedOrderItem]([OrderHeader_ID] ASC, [OrderItem_ID] ASC, [Item_Key] ASC)
    INCLUDE([DateReceived], [LineItemHandling], [LineItemFreight]) WITH (FILLFACTOR = 80);


GO
CREATE NONCLUSTERED INDEX [_dta_IX_OrderItem_OrderHeader_ID_OrderItem_ID_QuantityUnit_Item_Key]
    ON [dbo].[DeletedOrderItem]([OrderHeader_ID] ASC, [OrderItem_ID] ASC, [QuantityUnit] ASC, [Item_Key] ASC)
    INCLUDE([QuantityOrdered], [QuantityReceived], [Total_Weight], [QuantityDiscount], [DiscountType], [Package_Desc1], [Package_Desc2]) WITH (FILLFACTOR = 80);


GO
CREATE NONCLUSTERED INDEX [_dta_IX_OrderItem_OrderHeader_ID_Package_Unit_ID_OrderItem_ID_Item_Key_QuantityUnit]
    ON [dbo].[DeletedOrderItem]([OrderHeader_ID] ASC, [Package_Unit_ID] ASC, [OrderItem_ID] ASC, [Item_Key] ASC, [QuantityUnit] ASC)
    INCLUDE([QuantityOrdered], [QuantityReceived], [Total_Weight], [Cost], [UnitCost], [UnitExtCost], [QuantityDiscount], [DiscountType], [AdjustedCost], [LineItemCost], [LineItemHandling], [LineItemFreight], [Package_Desc1], [Package_Desc2], [Origin_ID], [CountryProc_ID], [Lot_No]) WITH (FILLFACTOR = 80);


GO
CREATE NONCLUSTERED INDEX [_dta_IX_OrderItem_OrderHeader_ID_QuantityReceived_Origin_ID_OrderItem_ID_Item_Key]
    ON [dbo].[DeletedOrderItem]([OrderHeader_ID] ASC, [QuantityReceived] ASC, [Origin_ID] ASC, [OrderItem_ID] ASC, [Item_Key] ASC) WITH (FILLFACTOR = 80);


GO
CREATE NONCLUSTERED INDEX [idxDiscountType]
    ON [dbo].[DeletedOrderItem]([OrderHeader_ID] ASC)
    INCLUDE([Item_Key], [DiscountType]) WITH (FILLFACTOR = 80);


GO
CREATE UNIQUE NONCLUSTERED INDEX [idxOrderItemIDHeaderID]
    ON [dbo].[DeletedOrderItem]([OrderHeader_ID] ASC, [OrderItem_ID] ASC) WITH (FILLFACTOR = 80);


GO
CREATE NONCLUSTERED INDEX [idxVendorCostHistoryID]
    ON [dbo].[DeletedOrderItem]([VendorCostHistoryID] ASC) WITH (FILLFACTOR = 80);

