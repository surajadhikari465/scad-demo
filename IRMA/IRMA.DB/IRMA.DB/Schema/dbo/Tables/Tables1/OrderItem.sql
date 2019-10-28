CREATE TABLE [dbo].[OrderItem] (
    [OrderItem_ID]                     INT             IDENTITY (1, 1) NOT NULL,
    [OrderHeader_ID]                   INT             NOT NULL,
    [Item_Key]                         INT             NOT NULL,
    [ExpirationDate]                   SMALLDATETIME   NULL,
    [QuantityOrdered]                  DECIMAL (18, 4) CONSTRAINT [DF__OrderItem__Quant__38AF44A5] DEFAULT ((0)) NOT NULL,
    [QuantityUnit]                     INT             NOT NULL,
    [QuantityReceived]                 DECIMAL (18, 4) NULL,
    [Total_Weight]                     DECIMAL (18, 4) CONSTRAINT [DF__OrderItem__Total__3C7FD589] DEFAULT ((0)) NOT NULL,
    [Units_per_Pallet]                 SMALLINT        CONSTRAINT [DF__OrderItem__Units__3D73F9C2] DEFAULT ((0)) NOT NULL,
    [Cost]                             MONEY           CONSTRAINT [DF__OrderItem__Cost__3E681DFB] DEFAULT ((0)) NOT NULL,
    [UnitCost]                         MONEY           CONSTRAINT [DF__OrderItem__UnitC__3F5C4234] DEFAULT ((0)) NOT NULL,
    [UnitExtCost]                      MONEY           CONSTRAINT [DF__OrderItem__UnitE__4050666D] DEFAULT ((0)) NOT NULL,
    [CostUnit]                         INT             NULL,
    [QuantityDiscount]                 DECIMAL (18, 4) CONSTRAINT [DF__OrderItem__Quant__4238AEDF] DEFAULT ((0)) NOT NULL,
    [DiscountType]                     INT             CONSTRAINT [DF__OrderItem__Disco__432CD318] DEFAULT ((0)) NOT NULL,
    [AdjustedCost]                     MONEY           CONSTRAINT [DF__OrderItem__Adjus__4420F751] DEFAULT ((0)) NOT NULL,
    [Handling]                         MONEY           CONSTRAINT [DF__OrderItem__Handl__45151B8A] DEFAULT ((0)) NOT NULL,
    [HandlingUnit]                     INT             NULL,
    [Freight]                          MONEY           CONSTRAINT [DF__OrderItem__Freig__46FD63FC] DEFAULT ((0)) NOT NULL,
    [FreightUnit]                      INT             NULL,
    [DateReceived]                     DATETIME        NULL,
    [OriginalDateReceived]             DATETIME        NULL,
    [Comments]                         VARCHAR (255)   NULL,
    [LineItemCost]                     MONEY           CONSTRAINT [DF__OrderItem__LineI__61B15A38] DEFAULT ((0)) NOT NULL,
    [LineItemHandling]                 MONEY           CONSTRAINT [DF__OrderItem__LineI__62A57E71] DEFAULT ((0)) NOT NULL,
    [LineItemFreight]                  MONEY           CONSTRAINT [DF__OrderItem__LineI__6399A2AA] DEFAULT ((0)) NOT NULL,
    [ReceivedItemCost]                 MONEY           CONSTRAINT [DF__OrderItem__Recei__648DC6E3] DEFAULT ((0)) NOT NULL,
    [ReceivedItemHandling]             MONEY           CONSTRAINT [DF__OrderItem__Recei__6581EB1C] DEFAULT ((0)) NOT NULL,
    [ReceivedItemFreight]              MONEY           CONSTRAINT [DF__OrderItem__Recei__66760F55] DEFAULT ((0)) NOT NULL,
    [LandedCost]                       MONEY           CONSTRAINT [DF__OrderItem__Lande__676A338E] DEFAULT ((0)) NOT NULL,
    [MarkupPercent]                    DECIMAL (18, 4) CONSTRAINT [DF_OrderItem_MarkupPercent_1] DEFAULT ((0)) NOT NULL,
    [MarkupCost]                       MONEY           CONSTRAINT [DF_OrderItem_MarkupCost_1] DEFAULT ((0)) NOT NULL,
    [Package_Desc1]                    DECIMAL (9, 4)  CONSTRAINT [DF_OrderItem_Package_Desc1_1] DEFAULT ((0)) NOT NULL,
    [Package_Desc2]                    DECIMAL (9, 4)  CONSTRAINT [DF_OrderItem_Package_Desc2_1] DEFAULT ((0)) NOT NULL,
    [Package_Unit_ID]                  INT             NULL,
    [Retail_Unit_ID]                   INT             NULL,
    [Origin_ID]                        INT             NULL,
    [ReceivedFreight]                  MONEY           CONSTRAINT [DF__OrderItem__Recei__7F4817D7] DEFAULT ((0)) NOT NULL,
    [UnitsReceived]                    DECIMAL (18, 4) CONSTRAINT [DF__OrderItem__Units__003C3C10] DEFAULT ((0)) NOT NULL,
    [CreditReason_ID]                  INT             NULL,
    [QuantityAllocated]                DECIMAL (18, 4) NULL,
    [CountryProc_ID]                   INT             NULL,
    [Lot_No]                           VARCHAR (12)    NULL,
    [NetVendorItemDiscount]            MONEY           NULL,
    [CostAdjustmentReason_ID]          INT             NULL,
    [Freight3Party]                    SMALLMONEY      NULL,
    [LineItemFreight3Party]            SMALLMONEY      NULL,
    [HandlingCharge]                   SMALLMONEY      NULL,
    [eInvoiceQuantity]                 DECIMAL (18, 4) NULL,
    [SACCost]                          SMALLMONEY      NULL,
    [OrderItemCOOL]                    BIT             DEFAULT ((0)) NOT NULL,
    [OrderItemBIO]                     BIT             DEFAULT ((0)) NOT NULL,
    [Carrier]                          VARCHAR (99)    NULL,
    [InvoiceQuantityUnit]              INT             NULL,
    [InvoiceCost]                      MONEY           NULL,
    [InvoiceExtendedCost]              MONEY           NULL,
    [InvoiceExtendedFreight]           MONEY           NULL,
    [InvoiceTotalWeight]               DECIMAL (18, 4) NULL,
    [VendorCostHistoryID]              INT             NULL,
    [OrigReceivedItemCost]             MONEY           CONSTRAINT [DF_OrderItem_OrigReceivedItemCost] DEFAULT ((0.00)) NULL,
    [OrigReceivedItemUnit]             INT             CONSTRAINT [DF_OrderItem_OrigReceivedItemUnit] DEFAULT ((0)) NULL,
    [CatchWeightCostPerWeight]         MONEY           DEFAULT ((0)) NULL,
    [QuantityShipped]                  DECIMAL (18, 4) NULL,
    [WeightShipped]                    DECIMAL (18, 4) NULL,
    [OHOrderDate]                      SMALLDATETIME   NULL,
    [SustainabilityRankingID]          INT             NULL,
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
    [ReceivedViaGun]                   BIT             DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK__OrderItem__6D0D32F4] PRIMARY KEY CLUSTERED ([OrderItem_ID] ASC) WITH (FILLFACTOR = 80),
    CONSTRAINT [FK__OrderItem__CostU__1A34DF26] FOREIGN KEY ([CostUnit]) REFERENCES [dbo].[ItemUnit] ([Unit_ID]),
    CONSTRAINT [FK__OrderItem__Freig__1B29035F] FOREIGN KEY ([FreightUnit]) REFERENCES [dbo].[ItemUnit] ([Unit_ID]),
    CONSTRAINT [FK__OrderItem__Handl__1C1D2798] FOREIGN KEY ([HandlingUnit]) REFERENCES [dbo].[ItemUnit] ([Unit_ID]),
    CONSTRAINT [FK__OrderItem__Order__1FEDB87C] FOREIGN KEY ([OrderHeader_ID]) REFERENCES [dbo].[OrderHeader] ([OrderHeader_ID]),
    CONSTRAINT [FK__OrderItem__Origi__20E1DCB5] FOREIGN KEY ([Origin_ID]) REFERENCES [dbo].[ItemOrigin] ([Origin_ID]),
    CONSTRAINT [FK__OrderItem__Quant__21D600EE] FOREIGN KEY ([QuantityUnit]) REFERENCES [dbo].[ItemUnit] ([Unit_ID]),
    CONSTRAINT [FK_OrderItem_CostAdjustmentReason] FOREIGN KEY ([CostAdjustmentReason_ID]) REFERENCES [dbo].[CostAdjustmentReason] ([CostAdjustmentReason_ID]),
    CONSTRAINT [FK_OrderItem_Item] FOREIGN KEY ([Item_Key]) REFERENCES [dbo].[Item] ([Item_Key]),
    CONSTRAINT [FK_OrderItem_ItemOrigin_CountryProc_ID] FOREIGN KEY ([CountryProc_ID]) REFERENCES [dbo].[ItemOrigin] ([Origin_ID]),
    CONSTRAINT [FK_OrderItem_ItemUnit] FOREIGN KEY ([Package_Unit_ID]) REFERENCES [dbo].[ItemUnit] ([Unit_ID]),
    CONSTRAINT [FK_OrderItem_ItemUnit1] FOREIGN KEY ([Retail_Unit_ID]) REFERENCES [dbo].[ItemUnit] ([Unit_ID]),
    CONSTRAINT [FK_OrderItem_ItemUnit2] FOREIGN KEY ([InvoiceQuantityUnit]) REFERENCES [dbo].[ItemUnit] ([Unit_ID]),
    CONSTRAINT [FK_OrderItem_ReasonCodeDetail] FOREIGN KEY ([ReasonCodeDetailID]) REFERENCES [dbo].[ReasonCodeDetail] ([ReasonCodeDetailID]),
    CONSTRAINT [FK_OrderItem_ReceivingDiscrepancyReasonCodeDetail] FOREIGN KEY ([ReceivingDiscrepancyReasonCodeID]) REFERENCES [dbo].[ReasonCodeDetail] ([ReasonCodeDetailID]),
    CONSTRAINT [FK_OrderItem_SustainabilityRanking] FOREIGN KEY ([SustainabilityRankingID]) REFERENCES [dbo].[SustainabilityRanking] ([ID])
);


GO
ALTER TABLE [dbo].[OrderItem] ENABLE CHANGE_TRACKING WITH (TRACK_COLUMNS_UPDATED = OFF);


GO
CREATE UNIQUE NONCLUSTERED INDEX [idxOrderItemIDHeaderID]
    ON [dbo].[OrderItem]([OrderHeader_ID] ASC, [OrderItem_ID] ASC) WITH (FILLFACTOR = 80);


GO
CREATE NONCLUSTERED INDEX [_dta_IX_OrderItem_OrderHeader_ID_Package_Unit_ID_OrderItem_ID_Item_Key_QuantityUnit]
    ON [dbo].[OrderItem]([OrderHeader_ID] ASC, [Package_Unit_ID] ASC, [OrderItem_ID] ASC, [Item_Key] ASC, [QuantityUnit] ASC)
    INCLUDE([QuantityOrdered], [QuantityReceived], [Total_Weight], [Cost], [UnitCost], [UnitExtCost], [QuantityDiscount], [DiscountType], [AdjustedCost], [LineItemCost], [LineItemHandling], [LineItemFreight], [Package_Desc1], [Package_Desc2], [Origin_ID], [CountryProc_ID], [Lot_No]);


GO
CREATE NONCLUSTERED INDEX [_dta_IX_OrderItem_OrderHeader_ID_OrderItem_ID_QuantityUnit_Item_Key]
    ON [dbo].[OrderItem]([OrderHeader_ID] ASC, [OrderItem_ID] ASC, [QuantityUnit] ASC, [Item_Key] ASC)
    INCLUDE([QuantityOrdered], [QuantityReceived], [Total_Weight], [QuantityDiscount], [DiscountType], [Package_Desc1], [Package_Desc2]);


GO
CREATE NONCLUSTERED INDEX [_dta_IX_OrderItem_OrderHeader_ID_DiscountType_NetVendorItemDiscount_OrderItem_ID]
    ON [dbo].[OrderItem]([OrderHeader_ID] ASC, [DiscountType] ASC, [NetVendorItemDiscount] ASC, [OrderItem_ID] ASC)
    INCLUDE([LineItemCost], [ReceivedItemCost], [ReceivedItemFreight]);


GO
CREATE NONCLUSTERED INDEX [_dta_IX_OrderItem_OrderHeader_ID_OrderItem_ID_Item_Key]
    ON [dbo].[OrderItem]([OrderHeader_ID] ASC, [OrderItem_ID] ASC, [Item_Key] ASC)
    INCLUDE([DateReceived], [LineItemHandling], [LineItemFreight]);


GO
CREATE NONCLUSTERED INDEX [_dta_IX_OrderItem_OrderHeader_ID_QuantityReceived_Origin_ID_OrderItem_ID_Item_Key]
    ON [dbo].[OrderItem]([OrderHeader_ID] ASC, [QuantityReceived] ASC, [Origin_ID] ASC, [OrderItem_ID] ASC, [Item_Key] ASC);


GO
CREATE NONCLUSTERED INDEX [_dta_IX_OrderItem_OrderHeader_ID_Item_Key_OrderItem_ID]
    ON [dbo].[OrderItem]([OrderHeader_ID] ASC, [Item_Key] ASC, [OrderItem_ID] ASC)
    INCLUDE([UnitsReceived]);


GO
CREATE NONCLUSTERED INDEX [idxDiscountType]
    ON [dbo].[OrderItem]([OrderHeader_ID] ASC)
    INCLUDE([Item_Key], [DiscountType]) WITH (FILLFACTOR = 90);


GO
CREATE NONCLUSTERED INDEX [IX_OrderItem_DateReceived]
    ON [dbo].[OrderItem]([DateReceived] ASC)
    INCLUDE([Item_Key]) WITH (FILLFACTOR = 80);


GO
CREATE NONCLUSTERED INDEX [IX_OrderItem_Item_Key]
    ON [dbo].[OrderItem]([Item_Key] ASC)
    INCLUDE([OrderHeader_ID], [DateReceived]) WITH (FILLFACTOR = 80);


GO
CREATE STATISTICS [_dta_stat_OrderItem_OrderItem_ID_Package_Unit_ID]
    ON [dbo].[OrderItem]([OrderItem_ID], [Package_Unit_ID]);


GO
CREATE STATISTICS [_dta_stat_OrderItem_OrderItem_ID_DiscountType]
    ON [dbo].[OrderItem]([OrderItem_ID], [DiscountType]);


GO
CREATE STATISTICS [_dta_stat_OrderItem_OrderItem_ID_QuantityOrdered_QuantityReceived]
    ON [dbo].[OrderItem]([OrderItem_ID], [QuantityOrdered], [QuantityReceived]);


GO
CREATE STATISTICS [_dta_stat_OrderItem_Item_Key_OrderHeader_ID_QuantityReceived]
    ON [dbo].[OrderItem]([Item_Key], [OrderHeader_ID], [QuantityReceived]);


GO
CREATE STATISTICS [_dta_stat_OrderItem_Origin_ID_OrderItem_ID_Item_Key]
    ON [dbo].[OrderItem]([Origin_ID], [OrderItem_ID], [Item_Key]);


GO
CREATE STATISTICS [_dta_stat_OrderItem_NetVendorItemDiscount_DiscountType_OrderItem_ID]
    ON [dbo].[OrderItem]([NetVendorItemDiscount], [DiscountType], [OrderItem_ID]);


GO
CREATE STATISTICS [_dta_stat_OrderItem_OrderHeader_ID_DiscountType_NetVendorItemDiscount]
    ON [dbo].[OrderItem]([OrderHeader_ID], [OrderItem_ID], [DiscountType], [NetVendorItemDiscount]);


GO
CREATE STATISTICS [_dta_stat_OrderItem_OrderHeader_ID_QuantityReceived_OrderItem_ID_Item_Key]
    ON [dbo].[OrderItem]([OrderHeader_ID], [QuantityReceived], [OrderItem_ID], [Item_Key]);


GO
CREATE STATISTICS [_dta_stat_OrderItem_OrderItem_ID_Origin_ID_OrderHeader_ID_QuantityReceived]
    ON [dbo].[OrderItem]([OrderItem_ID], [Origin_ID], [OrderHeader_ID], [QuantityReceived]);


GO
CREATE STATISTICS [_dta_stat_OrderItem_Item_Key_OrderHeader_ID_OrderItem_ID_Origin_ID_QuantityReceived]
    ON [dbo].[OrderItem]([Item_Key], [OrderHeader_ID], [OrderItem_ID], [Origin_ID], [QuantityReceived]);


GO

CREATE TRIGGER [dbo].[OrderItemAdd]
ON [dbo].[OrderItem]
FOR INSERT
AS 
BEGIN
    BEGIN TRY
    -- StoreOps Export 
	UPDATE OrderExportQueue
	SET QueueInsertedDate = GetDate(), DeliveredToStoreOpsDate = null
	WHERE OrderHeader_ID in (
	    SELECT DISTINCT OH.OrderHeader_ID
		FROM 
			OrderHeader OH
		INNER JOIN
			(SELECT OrderHeader_ID FROM Inserted) OI
			ON OI.OrderHeader_ID = OH.OrderHeader_ID
		WHERE (OH.SentDate IS NOT NULL)
		-- excludes closed and reconciled (reconciled in StoreOPs)warehouse orders sending updates
		and Not(OH.OrderType_Id = 2 and OH.CloseDate is not null and OH.Return_Order=0)
		 	
	)
	IF @@ROWCOUNT=0
	BEGIN
		INSERT INTO OrderExportQueue
		SELECT DISTINCT OH.OrderHeader_ID, GetDate(), null
		FROM 
			OrderHeader OH
		INNER JOIN
			(SELECT OrderHeader_ID FROM Inserted) OI
			ON OI.OrderHeader_ID = OH.OrderHeader_ID
		WHERE (OH.SentDate IS NOT NULL)
		-- excludes closed and reconciled (reconciled in StoreOPs)warehouse orders sending updates
		and Not(OH.OrderType_Id = 2 and OH.CloseDate is not null and OH.Return_Order=0)
	END

    --Copy the Cost and CostUnit value in OrigReceivedItemCost and OrigReceivedItemUnit
    --when data is inserted into OrderItem table
    UPDATE OI
       SET OI.OrigReceivedItemCost = I.Cost
	     , OI.OrigReceivedItemUnit = I.CostUnit
      FROM OrderItem OI
INNER JOIN INSERTED I ON I.OrderItem_ID  = OI.OrderItem_ID     	 
    
    --Copy the OrderDate from OrderHeader table insert into OHOrderDate when data is inserted into OrderItem table
    UPDATE OI
    SET OI.OHOrderDate = OH.OrderDate 
    FROM OrderItem OI
    INNER JOIN INSERTED I ON
            I.OrderItem_ID  = OI.OrderItem_ID 
    INNER JOIN OrderHeader OH ON
            OI.OrderHeader_ID = OH.OrderHeader_ID
                        
	----
	-- Amazon Events
	----
	IF (SELECT ISNULL(dbo.fn_InstanceDataValue('EnableAmazonEventGeneration', null), 0)) = 1
	BEGIN
			DECLARE @orderReceiptCreationEventTypeCode NVARCHAR(25) = 'RCPT_CRE' --'Order Receipt Creation'
			DECLARE @poLineAddEventTypeCode NVARCHAR(25) = 'PO_LINE_ADD' -- 'Purchase Order Line Item Add'

			DECLARE @receiptOrderMessageType NVARCHAR(50) = 'ReceiptMessage'
			DECLARE @purchaseOrderLineItemMessageType NVARCHAR(50) = 'PurchaseOrder'
		

			INSERT INTO amz.OrderQueue (
				EventTypeCode
				, MessageType
				, KeyID
				, SecondaryKeyID
				, InsertDate
				, MessageTimestampUtc
				)
			SELECT
				@poLineAddEventTypeCode
				,@purchaseOrderLineItemMessageType
				,i.OrderHeader_ID
				,i.OrderItem_ID
				,SYSDATETIME()
				,SYSUTCDATETIME()
			FROM inserted i
			JOIN dbo.OrderHeader oh ON oh.OrderHeader_ID = i.OrderHeader_ID
			WHERE oh.Sent = 1

			INSERT INTO amz.ReceiptQueue (
				EventTypeCode
				,MessageType
				,KeyID
				,SecondaryKeyID
				,InsertDate
				,MessageTimestampUtc
				)
			SELECT @orderReceiptCreationEventTypeCode
				,@receiptOrderMessageType
				,i.OrderHeader_ID
				,i.OrderItem_ID
				,SYSDATETIME()
				,SYSUTCDATETIME()
			FROM inserted i
			WHERE i.QuantityReceived IS NOT NULL
	END

    END TRY
    BEGIN CATCH
        DECLARE @err_no int, @err_sev int, @err_msg nvarchar(4000)
        SELECT @err_no = ERROR_NUMBER(), @err_sev = ERROR_SEVERITY(), @err_msg = ERROR_MESSAGE()
        IF @@TranCount > 0 
          begin
            ROLLBACK TRAN
          end
        
        RAISERROR ('OrderItemAdd trigger failed with @@ERROR: %d - %s', @err_sev, 1, @err_no, @err_msg)
    END CATCH	
END

GO

CREATE TRIGGER [dbo].[OrderItemUpdate]
ON [dbo].[OrderItem]
FOR UPDATE 
AS 
BEGIN
    BEGIN TRY

    -- StoreOps Export 
	UPDATE OrderExportQueue
	SET QueueInsertedDate = GetDate(), DeliveredToStoreOpsDate = null
	WHERE OrderHeader_ID in (
	    SELECT DISTINCT OH.OrderHeader_ID
		FROM 
			OrderHeader OH
		INNER JOIN
			(SELECT OrderHeader_ID FROM Inserted
			 UNION
			 SELECT OrderHeader_ID FROM Deleted) OI
			ON OI.OrderHeader_ID = OH.OrderHeader_ID
		WHERE (OH.SentDate IS NOT NULL)
		-- excludes closed and reconciled (reconciled in StoreOPs)warehouse orders sending updates
		and Not(OH.OrderType_Id = 2 and OH.CloseDate is not null and OH.Return_Order=0)
		 	
	)
	IF @@ROWCOUNT=0
	BEGIN
		INSERT INTO OrderExportQueue
		SELECT DISTINCT OH.OrderHeader_ID, GetDate(), null
		FROM 
			OrderHeader OH
		INNER JOIN
			(SELECT OrderHeader_ID FROM Inserted
			 UNION
			 SELECT OrderHeader_ID FROM Deleted) OI
			ON OI.OrderHeader_ID = OH.OrderHeader_ID
		WHERE (OH.SentDate IS NOT NULL)
		-- excludes closed and reconciled (reconciled in StoreOPs)warehouse orders sending updates
		and Not(OH.OrderType_Id = 2 and OH.CloseDate is not null and OH.Return_Order=0)
	END	 
    
    -- Capture for the update avgcost/onhand process 
    INSERT INTO ItemHistoryInsertedQueue (Store_No, Item_Key, DateStamp, SubTeam_No, ItemHistoryID, Adjustment_ID)
    SELECT IH.Store_No, IH.Item_Key, IH.DateStamp, IH.SubTeam_No, IH.ItemHistoryID, IH.Adjustment_ID
    FROM Inserted
    INNER JOIN Deleted ON Inserted.OrderItem_ID = Deleted.OrderItem_ID
    INNER JOIN ItemHistory IH (nolock) ON IH.OrderItem_ID = Inserted.OrderItem_ID
    INNER JOIN Item (nolock) ON Item.Item_Key = Inserted.Item_Key
    INNER JOIN Store (nolock) ON Store.Store_No = IH.Store_No
    -- exclude ingredient items unless the affected store is a Distribution Center
    WHERE (((Ingredient = 0 AND ISNULL(UseLastReceivedCost, 0) = 0) AND (Item.Subteam_No = IH.Subteam_No)) OR Store.Distribution_Center = 1)
        AND IH.Adjustment_ID = 5
        AND (
            ((Inserted.ReceivedItemCost + Inserted.ReceivedItemFreight) <> (Deleted.ReceivedItemCost + Deleted.ReceivedItemFreight))
            OR Inserted.UnitsReceived <> Deleted.UnitsReceived
            )
                        
    -- For updates, keep receiving ItemHistory in synch
    -- Use a table variable and a while loop instead of a cursor
    DECLARE @ReceivedList TABLE (OrderItem_ID int PRIMARY KEY)
    DECLARE @OrderItem_ID int
        
    INSERT INTO @ReceivedList
    SELECT Inserted.OrderItem_ID
    FROM Inserted
    INNER JOIN Deleted ON Inserted.OrderItem_ID = Deleted.OrderItem_ID
    WHERE (Inserted.UnitsReceived <> Deleted.UnitsReceived)
        
    WHILE EXISTS (SELECT * FROM @ReceivedList)
    BEGIN
        SET @OrderItem_ID = (SELECT TOP 1 OrderItem_ID FROM @ReceivedList)
            
        EXEC InsertReceivingItemHistory @OrderItem_ID, 0  -- Does not matter who the user is as far as ItemHistory since there is a link to OrderItem.  The receiver should be recorded in the OrderHeader record.
            
        DELETE @ReceivedList WHERE OrderItem_ID = @OrderItem_ID
    END
    
    ----
	-- Amazon Events
	----
	IF (SELECT ISNULL(dbo.fn_InstanceDataValue('EnableAmazonEventGeneration', null), 0)) = 1
	BEGIN
		DECLARE @orderReceiptCreationEventTypeCode NVARCHAR(25) = 'RCPT_CRE' -- 'Order Receipt Creation'
		DECLARE @orderReceiptModificationEventTypeCode NVARCHAR(25) = 'RCPT_MOD' -- 'Order Receipt Modification'
		DECLARE @poLineModificationEventTypeCode NVARCHAR(25) = 'PO_LINE_MOD' -- 'Purchase Order Line Item Modification'

		DECLARE @receiptOrderMessageType NVARCHAR(50) = 'ReceiptMessage'
		DECLARE @poLineItemMessageType NVARCHAR(50) = 'PurchaseOrder'

		INSERT INTO amz.OrderQueue (
			EventTypeCode
			, MessageType
			, KeyID
			, SecondaryKeyID
			, InsertDate
			, MessageTimestampUtc
			)
		SELECT 
			@poLineModificationEventTypeCode
			,@poLineItemMessageType
			,oh.OrderHeader_ID
			,i.OrderItem_ID
			,SYSDATETIME()
			,SYSUTCDATETIME()
		FROM inserted i
		JOIN deleted d ON i.OrderItem_ID = d.OrderItem_ID
		JOIN dbo.OrderHeader oh ON oh.OrderHeader_ID = i.OrderHeader_ID
		WHERE ISNULL(i.QuantityOrdered, 0) <> ISNULL(d.QuantityOrdered, 0)
		  AND oh.Sent =  1
		  AND oh.OrderType_ID <> 3

		INSERT INTO amz.ReceiptQueue (
			EventTypeCode
			,MessageType
			,KeyID
			,SecondaryKeyID
			,InsertDate
			,MessageTimestampUtc
			)
		SELECT @orderReceiptCreationEventTypeCode
			,@receiptOrderMessageType
			,i.OrderHeader_ID
			,i.OrderItem_ID
			,SYSDATETIME()
			,SYSUTCDATETIME()
		FROM inserted i
		JOIN deleted d ON i.OrderItem_ID = d.OrderItem_ID
		WHERE i.QuantityReceived IS NOT NULL 
		AND d.QuantityReceived IS NULL

		INSERT INTO amz.ReceiptQueue (
			EventTypeCode
			,MessageType
			,KeyID
			,SecondaryKeyID
			,InsertDate
			,MessageTimestampUtc
			)
		SELECT
			@orderReceiptModificationEventTypeCode
			,@receiptOrderMessageType
			,i.OrderHeader_ID
			,i.OrderItem_ID
			,SYSDATETIME()
			,SYSUTCDATETIME()
		FROM inserted i
		JOIN deleted d ON i.OrderItem_ID = d.OrderItem_ID
		WHERE (i.QuantityReceived IS NULL AND d.QuantityReceived IS NOT NULL)	----Line item receipt information is modified after the receipt info was entered
		OR (i.QuantityReceived <> d.QuantityReceived)
	END
    END TRY
    BEGIN CATCH
        DECLARE @err_no int, @err_sev int, @err_msg nvarchar(4000)
        SELECT @err_no = ERROR_NUMBER(), @err_sev = ERROR_SEVERITY(), @err_msg = ERROR_MESSAGE()
        IF @@TranCount > 0 
          begin
            ROLLBACK TRAN
          end
        
        RAISERROR ('OrderItemUpdate trigger failed with @@ERROR: %d - %s', @err_sev, 1, @err_no, @err_msg)
    END CATCH	
END

GO

CREATE TRIGGER [dbo].[OrderItemDelete]
ON [dbo].[OrderItem]
FOR DELETE 
AS 
BEGIN
    BEGIN TRY
    -- StoreOps Export 
	UPDATE OrderExportQueue
	SET QueueInsertedDate = GetDate(), DeliveredToStoreOpsDate = null
	WHERE OrderHeader_ID in (
	    SELECT DISTINCT OH.OrderHeader_ID
		FROM 
			OrderHeader OH
		INNER JOIN
			(SELECT OrderHeader_ID FROM Deleted) OI
			ON OI.OrderHeader_ID = OH.OrderHeader_ID
		WHERE (OH.SentDate IS NOT NULL)
		-- excludes closed and reconciled (reconciled in StoreOPs)warehouse orders sending updates
		and Not(OH.OrderType_Id = 2 and OH.CloseDate is not null and OH.Return_Order=0)
		 	
	)
	IF @@ROWCOUNT=0
	BEGIN
		INSERT INTO OrderExportQueue
		SELECT DISTINCT OH.OrderHeader_ID, GetDate(), null
		FROM 
			OrderHeader OH
		INNER JOIN
			(SELECT OrderHeader_ID FROM Deleted) OI
			ON OI.OrderHeader_ID = OH.OrderHeader_ID
		WHERE (OH.SentDate IS NOT NULL)
		-- excludes closed and reconciled (reconciled in StoreOPs)warehouse orders sending updates
		and Not(OH.OrderType_Id = 2 and OH.CloseDate is not null and OH.Return_Order=0)
	END 	 

    END TRY
    BEGIN CATCH
        DECLARE @err_no int, @err_sev int, @err_msg nvarchar(4000)
        SELECT @err_no = ERROR_NUMBER(), @err_sev = ERROR_SEVERITY(), @err_msg = ERROR_MESSAGE()
        IF @@TranCount > 0 
          begin
            ROLLBACK TRAN
          end
        
        RAISERROR ('OrderItemDelete trigger failed with @@ERROR: %d - %s', @err_sev, 1, @err_no, @err_msg)
    END CATCH	
END

GO

GRANT VIEW CHANGE TRACKING
    ON OBJECT::[dbo].[OrderItem] TO [IRMAAdminRole]
    AS [dbo];


GO
GRANT VIEW CHANGE TRACKING
    ON OBJECT::[dbo].[OrderItem] TO [IRMASupportRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[OrderItem] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[OrderItem] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT VIEW CHANGE TRACKING
    ON OBJECT::[dbo].[OrderItem] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[OrderItem] TO [IRMAReportsRole]
    AS [dbo];


GO
GRANT VIEW CHANGE TRACKING
    ON OBJECT::[dbo].[OrderItem] TO [IRMAReportsRole]
    AS [dbo];


GO
GRANT INSERT
    ON OBJECT::[dbo].[OrderItem] TO [IRMASchedJobs]
    AS [dbo];


GO
GRANT UPDATE
    ON OBJECT::[dbo].[OrderItem] TO [IRMASchedJobs]
    AS [dbo];


GO
GRANT VIEW CHANGE TRACKING
    ON OBJECT::[dbo].[OrderItem] TO [IRMA_Teradata]
    AS [dbo];





GO
GRANT VIEW CHANGE TRACKING
    ON OBJECT::[dbo].[OrderItem] TO [IConInterface]
    AS [dbo];


GO
GRANT VIEW CHANGE TRACKING
    ON OBJECT::[dbo].[OrderItem] TO [iCONReportingRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[OrderItem] TO [IRMAPDXExtractRole]
    AS [dbo];

