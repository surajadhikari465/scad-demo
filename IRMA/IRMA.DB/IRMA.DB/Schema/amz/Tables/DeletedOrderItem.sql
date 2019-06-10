CREATE TABLE [amz].[DeletedOrderItem]
(
	[DeletedOrderItem_ID] [int] IDENTITY(1,1) NOT NULL,
	[OrderItem_ID] [int] NOT NULL,
	[OrderHeader_ID] [int] NOT NULL,
	[Item_Key] [int] NOT NULL,
	[ExpirationDate] [smalldatetime] NULL,
	[QuantityOrdered] [decimal](18, 4) NOT NULL,
	[QuantityUnit] [int] NOT NULL,
	[QuantityReceived] [decimal](18, 4) NULL,
	[Total_Weight] [decimal](18, 4) NOT NULL,
	[Units_per_Pallet] [smallint] NOT NULL,
	[Cost] [money] NOT NULL,
	[UnitCost] [money] NOT NULL,
	[UnitExtCost] [money] NOT NULL,
	[CostUnit] [int] NULL,
	[QuantityDiscount] [decimal](18, 4) NOT NULL,
	[DiscountType] [int] NOT NULL,
	[AdjustedCost] [money] NOT NULL,
	[Handling] [money] NOT NULL,
	[HandlingUnit] [int] NULL,
	[Freight] [money] NOT NULL,
	[FreightUnit] [int] NULL,
	[DateReceived] [datetime] NULL,
	[OriginalDateReceived] [datetime] NULL,
	[Comments] [varchar](255) NULL,
	[LineItemCost] [money] NOT NULL,
	[LineItemHandling] [money] NOT NULL,
	[LineItemFreight] [money] NOT NULL,
	[ReceivedItemCost] [money] NOT NULL,
	[ReceivedItemHandling] [money] NOT NULL,
	[ReceivedItemFreight] [money] NOT NULL,
	[LandedCost] [money] NOT NULL,
	[MarkupPercent] [decimal](18, 4) NOT NULL,
	[MarkupCost] [money] NOT NULL,
	[Package_Desc1] [decimal](9, 4) NOT NULL,
	[Package_Desc2] [decimal](9, 4) NOT NULL,
	[Package_Unit_ID] [int] NULL,
	[Retail_Unit_ID] [int] NULL,
	[Origin_ID] [int] NULL,
	[ReceivedFreight] [money] NOT NULL,
	[UnitsReceived] [decimal](18, 4) NOT NULL,
	[CreditReason_ID] [int] NULL,
	[QuantityAllocated] [decimal](18, 4) NULL,
	[CountryProc_ID] [int] NULL,
	[Lot_No] [varchar](12) NULL,
	[VendorCostHistoryID] [int] NULL,
	[OHOrderDate] [smalldatetime] NULL,
	[SustainabilityRankingID] [int] NULL,
	[NetVendorItemDiscount] [money] NULL,
	[CostAdjustmentReason_ID] [int] NULL,
	[Freight3Party] [smallmoney] NULL,
	[LineItemFreight3Party] [smallmoney] NULL,
	[HandlingCharge] [smallmoney] NULL,
	[eInvoiceQuantity] [decimal](18, 4) NULL,
	[SACCost] [smallmoney] NULL,
	[OrderItemCOOL] [bit] NOT NULL,
	[OrderItemBIO] [bit] NOT NULL,
	[Carrier] [varchar](99) NULL,
	[InvoiceQuantityUnit] [int] NULL,
	[InvoiceCost] [money] NULL,
	[InvoiceExtendedCost] [money] NULL,
	[InvoiceExtendedFreight] [money] NULL,
	[InvoiceTotalWeight] [decimal](18, 4) NULL,
	[OrigReceivedItemCost] [money] NULL,
	[OrigReceivedItemUnit] [int] NULL,
	[CatchWeightCostPerWeight] [money] NULL,
	[QuantityShipped] [decimal](18, 4) NULL,
	[WeightShipped] [decimal](18, 4) NULL,
	[eInvoiceWeight] [decimal](18, 4) NULL,
	[ReasonCodeDetailID] [int] NULL,
	[ReceivingDiscrepancyReasonCodeID] [int] NULL,
	[PaidCost] [money] NULL,
	[ApprovedDate] [datetime] NULL,
	[ApprovedByUserId] [int] NULL,
	[AdminNotes] [varchar](5000) NULL,
	[ResolutionCodeID] [int] NULL,
	[PaymentTypeID] [int] NULL,
	[LineItemSuspended] [bit] NULL,
	[OrderType_ID] [int] NOT NULL,
	[InsertDate] DateTime NULL CONSTRAINT DF_AMZ_DeletedOrderItem_InsertDate DEFAULT (GetDate())
) ON [PRIMARY]
GO

ALTER TABLE [amz].[DeletedOrderItem]  WITH NOCHECK ADD  CONSTRAINT [FK_AMZDeletedOrderItem__CostU] FOREIGN KEY([CostUnit])
REFERENCES [dbo].[ItemUnit] ([Unit_ID])
GO

ALTER TABLE [amz].[DeletedOrderItem] NOCHECK CONSTRAINT [FK_AMZDeletedOrderItem__CostU]
GO

ALTER TABLE [amz].[DeletedOrderItem]  WITH NOCHECK ADD  CONSTRAINT [FK_AMZDeletedOrderItem__Freig] FOREIGN KEY([FreightUnit])
REFERENCES [dbo].[ItemUnit] ([Unit_ID])
GO

ALTER TABLE [amz].[DeletedOrderItem] NOCHECK CONSTRAINT [FK_AMZDeletedOrderItem__Freig]
GO

ALTER TABLE [amz].[DeletedOrderItem]  WITH NOCHECK ADD  CONSTRAINT [FK_AMZDeletedOrderItem__Handl] FOREIGN KEY([HandlingUnit])
REFERENCES [dbo].[ItemUnit] ([Unit_ID])
GO

ALTER TABLE [amz].[DeletedOrderItem] NOCHECK CONSTRAINT [FK_AMZDeletedOrderItem__Handl]
GO

ALTER TABLE [amz].[DeletedOrderItem]  WITH NOCHECK ADD  CONSTRAINT [FK_AMZDeletedOrderItem__OrderHeader_ID] FOREIGN KEY([OrderHeader_ID])
REFERENCES [dbo].[OrderHeader] ([OrderHeader_ID])
GO

ALTER TABLE [amz].[DeletedOrderItem] NOCHECK CONSTRAINT [FK_AMZDeletedOrderItem__OrderHeader_ID]
GO

ALTER TABLE [amz].[DeletedOrderItem]  WITH NOCHECK ADD  CONSTRAINT [FK_AMZDeletedOrderItem__Origi] FOREIGN KEY([Origin_ID])
REFERENCES [dbo].[ItemOrigin] ([Origin_ID])
GO

ALTER TABLE [amz].[DeletedOrderItem] NOCHECK CONSTRAINT [FK_AMZDeletedOrderItem__Origi]
GO

ALTER TABLE [amz].[DeletedOrderItem]  WITH NOCHECK ADD  CONSTRAINT [FK_AMZDeletedOrderItem__Quant] FOREIGN KEY([QuantityUnit])
REFERENCES [dbo].[ItemUnit] ([Unit_ID])
GO

ALTER TABLE [amz].[DeletedOrderItem] NOCHECK CONSTRAINT [FK_AMZDeletedOrderItem__Quant]
GO

ALTER TABLE [amz].[DeletedOrderItem]  WITH CHECK ADD  CONSTRAINT [FK_AMZDeletedOrderItem_CostAdjustmentReason] FOREIGN KEY([CostAdjustmentReason_ID])
REFERENCES [dbo].[CostAdjustmentReason] ([CostAdjustmentReason_ID])
GO

ALTER TABLE [amz].[DeletedOrderItem] CHECK CONSTRAINT [FK_AMZDeletedOrderItem_CostAdjustmentReason]
GO

ALTER TABLE [amz].[DeletedOrderItem]  WITH NOCHECK ADD  CONSTRAINT [FK_AMZDeletedOrderItem_Item] FOREIGN KEY([Item_Key])
REFERENCES [dbo].[Item] ([Item_Key])
GO

ALTER TABLE [amz].[DeletedOrderItem] NOCHECK CONSTRAINT [FK_AMZDeletedOrderItem_Item]
GO

ALTER TABLE [amz].[DeletedOrderItem]  WITH NOCHECK ADD  CONSTRAINT [FK_AMZDeletedOrderItem_ItemOrigin_CountryProc_ID] FOREIGN KEY([CountryProc_ID])
REFERENCES [dbo].[ItemOrigin] ([Origin_ID])
GO

ALTER TABLE [amz].[DeletedOrderItem] NOCHECK CONSTRAINT [FK_AMZDeletedOrderItem_ItemOrigin_CountryProc_ID]
GO

ALTER TABLE [amz].[DeletedOrderItem]  WITH NOCHECK ADD  CONSTRAINT [FK_AMZDeletedOrderItem_ItemUnit] FOREIGN KEY([Package_Unit_ID])
REFERENCES [dbo].[ItemUnit] ([Unit_ID])
GO

ALTER TABLE [amz].[DeletedOrderItem] NOCHECK CONSTRAINT [FK_AMZDeletedOrderItem_ItemUnit]
GO

ALTER TABLE [amz].[DeletedOrderItem]  WITH NOCHECK ADD  CONSTRAINT [FK_AMZDeletedOrderItem_ItemUnit1] FOREIGN KEY([Retail_Unit_ID])
REFERENCES [dbo].[ItemUnit] ([Unit_ID])
GO

ALTER TABLE [amz].[DeletedOrderItem] NOCHECK CONSTRAINT [FK_AMZDeletedOrderItem_ItemUnit1]
GO

ALTER TABLE [amz].[DeletedOrderItem]  WITH CHECK ADD  CONSTRAINT [FK_AMZDeletedOrderItem_ItemUnit2] FOREIGN KEY([InvoiceQuantityUnit])
REFERENCES [dbo].[ItemUnit] ([Unit_ID])
GO

ALTER TABLE [amz].[DeletedOrderItem] CHECK CONSTRAINT [FK_AMZDeletedOrderItem_ItemUnit2]
GO

ALTER TABLE [amz].[DeletedOrderItem]  WITH CHECK ADD  CONSTRAINT [FK_AMZDeletedOrderItem_ReasonCodeDetail] FOREIGN KEY([ReasonCodeDetailID])
REFERENCES [dbo].[ReasonCodeDetail] ([ReasonCodeDetailID])
GO

ALTER TABLE [amz].[DeletedOrderItem] CHECK CONSTRAINT [FK_AMZDeletedOrderItem_ReasonCodeDetail]
GO

ALTER TABLE [amz].[DeletedOrderItem]  WITH CHECK ADD  CONSTRAINT [FK_AMZDeletedOrderItem_ReceivingDiscrepancyReasonCodeDetail] FOREIGN KEY([ReceivingDiscrepancyReasonCodeID])
REFERENCES [dbo].[ReasonCodeDetail] ([ReasonCodeDetailID])
GO

ALTER TABLE [amz].[DeletedOrderItem] CHECK CONSTRAINT [FK_AMZDeletedOrderItem_ReceivingDiscrepancyReasonCodeDetail]
GO

ALTER TABLE [amz].[DeletedOrderItem]  WITH CHECK ADD  CONSTRAINT [FK_AMZDeletedOrderItem_SustainabilityRanking] FOREIGN KEY([SustainabilityRankingID])
REFERENCES [dbo].[SustainabilityRanking] ([ID])
GO

ALTER TABLE [amz].[DeletedOrderItem] CHECK CONSTRAINT [FK_AMZDeletedOrderItem_SustainabilityRanking]
GO

GRANT SELECT ON [amz].[DeletedOrderItem] TO [IRMAReports];
GO