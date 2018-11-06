CREATE TABLE [dbo].[OrderHeader] (
    [OrderHeader_ID]               INT            IDENTITY (1, 1) NOT NULL,
    [InvoiceNumber]                VARCHAR (20)   NULL,
    [OrderHeaderDesc]              VARCHAR (4000) NULL,
    [Vendor_ID]                    INT            NOT NULL,
    [PurchaseLocation_ID]          INT            NOT NULL,
    [ReceiveLocation_ID]           INT            NOT NULL,
    [CreatedBy]                    INT            NOT NULL,
    [OrderDate]                    SMALLDATETIME  CONSTRAINT [DF_OrderHeader_OrderDate] DEFAULT (CONVERT([varchar](12),getdate(),(101))) NOT NULL,
    [CloseDate]                    DATETIME       NULL,
    [OriginalCloseDate]            DATETIME       NULL,
    [SystemGenerated]              BIT            CONSTRAINT [DF__OrderHead__Syste__64ECEE3F] DEFAULT ((0)) NOT NULL,
    [Sent]                         BIT            CONSTRAINT [DF__OrderHeade__Sent__67C95AEA] DEFAULT ((0)) NOT NULL,
    [Fax_Order]                    BIT            CONSTRAINT [DF__OrderHead__Fax_O__68BD7F23] DEFAULT ((1)) NOT NULL,
    [Expected_Date]                SMALLDATETIME  NULL,
    [SentDate]                     SMALLDATETIME  NULL,
    [QuantityDiscount]             DECIMAL (9, 2) CONSTRAINT [DF__OrderHead__Quant__6AA5C795] DEFAULT ((0)) NOT NULL,
    [DiscountType]                 INT            CONSTRAINT [DF__OrderHead__Disco__6B99EBCE] DEFAULT ((0)) NOT NULL,
    [Transfer_SubTeam]             INT            NULL,
    [Transfer_To_SubTeam]          INT            NOT NULL,
    [Return_Order]                 BIT            CONSTRAINT [DF_OrderHeader_Return_Order] DEFAULT ((0)) NOT NULL,
    [User_ID]                      INT            NULL,
    [Temperature]                  TINYINT        NULL,
    [Accounting_In_DateStamp]      SMALLDATETIME  NULL,
    [Accounting_In_UserID]         INT            NULL,
    [InvoiceDate]                  SMALLDATETIME  NULL,
    [ApprovedDate]                 SMALLDATETIME  NULL,
    [ApprovedBy]                   INT            NULL,
    [UploadedDate]                 SMALLDATETIME  NULL,
    [RecvLogDate]                  DATETIME       NULL,
    [RecvLog_No]                   INT            NULL,
    [RecvLogUser_ID]               INT            NULL,
    [VendorDoc_ID]                 VARCHAR (16)   NULL,
    [VendorDocDate]                SMALLDATETIME  NULL,
    [WarehouseSent]                BIT            CONSTRAINT [DF_OrderHeader_WarehouseSend] DEFAULT ((0)) NOT NULL,
    [WarehouseSentDate]            SMALLDATETIME  NULL,
    [SentToFaxDate]                SMALLDATETIME  NULL,
    [OrderType_ID]                 INT            NOT NULL,
    [ProductType_ID]               INT            NOT NULL,
    [FromQueue]                    BIT            CONSTRAINT [DF_OrderHeader_FromOrderQueue] DEFAULT ((0)) NOT NULL,
    [ClosedBy]                     INT            NULL,
    [MatchingValidationCode]       INT            NULL,
    [MatchingUser_ID]              INT            NULL,
    [MatchingDate]                 DATETIME       NULL,
    [Freight3Party_OrderCost]      SMALLMONEY     NULL,
    [DVOOrderID]                   VARCHAR (10)   NULL,
    [eInvoice_Id]                  INT            NULL,
    [Email_Order]                  BIT            DEFAULT ((0)) NOT NULL,
    [SentToEmailDate]              SMALLDATETIME  NULL,
    [OverrideTransmissionMethod]   BIT            CONSTRAINT [DF_OrderHeader_OverrideTransmissionMethod] DEFAULT ((0)) NOT NULL,
    [SupplyTransferToSubTeam]      INT            NULL,
    [AccountingUploadDate]         DATETIME       NULL,
    [Electronic_Order]             BIT            DEFAULT ((0)) NOT NULL,
    [SentToElectronicDate]         SMALLDATETIME  NULL,
    [IsDropShipment]               BIT            DEFAULT ((0)) NOT NULL,
    [InvoiceDiscrepancy]           BIT            NULL,
    [InvoiceDiscrepancySentDate]   SMALLDATETIME  NULL,
    [InvoiceProcessingDiscrepancy] BIT            NULL,
    [WarehouseCancelled]           DATETIME       NULL,
    [PayByAgreedCost]              BIT            CONSTRAINT [DF_OrderHeader_PayByAgreedCost] DEFAULT ((0)) NOT NULL,
    [PurchaseAccountsTotal]        MONEY          NULL,
    [CurrencyID]                   INT            NULL,
    [APUploadedCost]               MONEY          NULL,
    [OrderExternalSourceID]        INT            NULL,
    [OrderExternalSourceOrderID]   INT            NULL,
    [QtyShippedProvided]           BIT            NULL,
    [POCostDate]                   DATETIME       NULL,
    [AdminNotes]                   VARCHAR (5000) NULL,
    [ResolutionCodeID]             INT            NULL,
    [InReview]                     BIT            DEFAULT ((0)) NOT NULL,
    [InReviewUser]                 INT            NULL,
    [ReasonCodeDetailID]           INT            NULL,
    [RefuseReceivingReasonID]      INT            NULL,
    [OrderedCost]                  MONEY          NULL,
    [OriginalReceivedCost]         MONEY          NULL,
    [TotalPaidCost]                MONEY          NULL,
    [AdjustedReceivedCost]         MONEY          NULL,
    [OrderRefreshCostSource_ID]    INT            NULL,
    [PartialShipment]              BIT            DEFAULT ((0)) NOT NULL,
    [DSDOrder]                     BIT            DEFAULT ((0)) NOT NULL,
    [TotalRefused]                 MONEY          NULL,
    CONSTRAINT [PK_OrderHeader_OrderHeader_ID] PRIMARY KEY CLUSTERED ([OrderHeader_ID] ASC) WITH (FILLFACTOR = 80),
    FOREIGN KEY ([OrderExternalSourceID]) REFERENCES [dbo].[OrderExternalSource] ([ID]),
    CONSTRAINT [FK__OrderHead__Creat__63F8CA06] FOREIGN KEY ([CreatedBy]) REFERENCES [dbo].[Users] ([User_ID]),
    CONSTRAINT [FK__OrderHead__Purch__025D5595] FOREIGN KEY ([PurchaseLocation_ID]) REFERENCES [dbo].[Vendor] ([Vendor_ID]),
    CONSTRAINT [FK__OrderHead__Recei__04459E07] FOREIGN KEY ([ReceiveLocation_ID]) REFERENCES [dbo].[Vendor] ([Vendor_ID]),
    CONSTRAINT [FK__OrderHead__RecvL__6A02E22E] FOREIGN KEY ([RecvLogUser_ID]) REFERENCES [dbo].[Users] ([User_ID]),
    CONSTRAINT [FK__OrderHead__User___75235608] FOREIGN KEY ([User_ID]) REFERENCES [dbo].[Users] ([User_ID]),
    CONSTRAINT [FK__OrderHead__Vendo__07220AB2] FOREIGN KEY ([Vendor_ID]) REFERENCES [dbo].[Vendor] ([Vendor_ID]),
    CONSTRAINT [FK_Currency_OrderHeader] FOREIGN KEY ([CurrencyID]) REFERENCES [dbo].[Currency] ([CurrencyID]),
    CONSTRAINT [FK_OrderHeader_ClosedBy] FOREIGN KEY ([ClosedBy]) REFERENCES [dbo].[Users] ([User_ID]),
    CONSTRAINT [FK_OrderHeader_MatchingUser_ID] FOREIGN KEY ([MatchingUser_ID]) REFERENCES [dbo].[Users] ([User_ID]),
    CONSTRAINT [FK_OrderHeader_MatchingValidationCode] FOREIGN KEY ([MatchingValidationCode]) REFERENCES [dbo].[ValidationCode] ([ValidationCode]),
    CONSTRAINT [FK_OrderHeader_OrderRefreshCostSource] FOREIGN KEY ([OrderRefreshCostSource_ID]) REFERENCES [dbo].[OrderRefreshCostSource] ([OrderRefreshCostSource_ID]),
    CONSTRAINT [FK_OrderHeader_ReasonCodeDetail] FOREIGN KEY ([ReasonCodeDetailID]) REFERENCES [dbo].[ReasonCodeDetail] ([ReasonCodeDetailID]),
    CONSTRAINT [FK_OrderHeader_SubTeam] FOREIGN KEY ([Transfer_SubTeam]) REFERENCES [dbo].[SubTeam] ([SubTeam_No]),
    CONSTRAINT [FK_OrderHeader_SubTeam1] FOREIGN KEY ([Transfer_To_SubTeam]) REFERENCES [dbo].[SubTeam] ([SubTeam_No]),
    CONSTRAINT [FK_OrderHeader_SupplyTransferToSubTeam] FOREIGN KEY ([SupplyTransferToSubTeam]) REFERENCES [dbo].[SubTeam] ([SubTeam_No]),
    CONSTRAINT [FK_OrderHeader_Users] FOREIGN KEY ([Accounting_In_UserID]) REFERENCES [dbo].[Users] ([User_ID]),
    CONSTRAINT [FK_OrderHeader_Users1] FOREIGN KEY ([ApprovedBy]) REFERENCES [dbo].[Users] ([User_ID]),
    CONSTRAINT [FK_OrderItem_RefuseReceivingReasonID] FOREIGN KEY ([RefuseReceivingReasonID]) REFERENCES [dbo].[ReasonCodeDetail] ([ReasonCodeDetailID])
);


GO
ALTER TABLE [dbo].[OrderHeader] ENABLE CHANGE_TRACKING WITH (TRACK_COLUMNS_UPDATED = OFF);


GO
CREATE NONCLUSTERED INDEX [_dta_IX_OrderHeader_Vendor_ID_OrderHeader_ID]
    ON [dbo].[OrderHeader]([Vendor_ID] ASC, [OrderHeader_ID] ASC)
    INCLUDE([InvoiceNumber]);


GO
CREATE NONCLUSTERED INDEX [idxVendorOrder_ID]
    ON [dbo].[OrderHeader]([InvoiceNumber] ASC) WITH (FILLFACTOR = 80);


GO
CREATE NONCLUSTERED INDEX [IX_OH_OHID_ExternalOrderId]
    ON [dbo].[OrderHeader]([OrderHeader_ID] ASC, [OrderExternalSourceOrderID] ASC)
    INCLUDE([SentDate], [ReceiveLocation_ID]) WITH (FILLFACTOR = 80);


GO
CREATE NONCLUSTERED INDEX [OH_ExtSrcID_ExtSrcOrdID]
    ON [dbo].[OrderHeader]([OrderExternalSourceID] ASC, [OrderExternalSourceOrderID] ASC) WITH (FILLFACTOR = 90);


GO
CREATE NONCLUSTERED INDEX [idxApprovedCloseDate]
    ON [dbo].[OrderHeader]([CloseDate] ASC, [ApprovedDate] ASC)
    INCLUDE([OrderHeader_ID], [InvoiceNumber], [Vendor_ID], [PurchaseLocation_ID], [OrderDate], [Transfer_To_SubTeam], [InvoiceDate], [eInvoice_Id], [ResolutionCodeID]) WITH (FILLFACTOR = 90);


GO
CREATE STATISTICS [_dta_stat_OrderHeader_OrderHeader_ID_ReceiveLocation_ID]
    ON [dbo].[OrderHeader]([OrderHeader_ID], [ReceiveLocation_ID]);


GO
CREATE STATISTICS [_dta_stat_OrderHeader_Return_Order_OrderHeader_ID]
    ON [dbo].[OrderHeader]([Return_Order], [OrderHeader_ID]);


GO
CREATE STATISTICS [_dta_stat_OrderHeader_ReceiveLocation_ID_OrderHeader_ID]
    ON [dbo].[OrderHeader]([ReceiveLocation_ID], [OrderHeader_ID]);


GO
CREATE STATISTICS [_dta_stat_OrderHeader_OrderHeader_ID_CloseDate]
    ON [dbo].[OrderHeader]([OrderHeader_ID], [CloseDate]);


GO
CREATE STATISTICS [_dta_stat_OrderHeader_MatchingValidationCode_MatchingUser_ID]
    ON [dbo].[OrderHeader]([MatchingValidationCode], [MatchingUser_ID]);


GO
CREATE STATISTICS [_dta_stat_OrderHeader_ApprovedBy_MatchingValidationCode_MatchingUser_ID]
    ON [dbo].[OrderHeader]([ApprovedBy], [MatchingValidationCode], [MatchingUser_ID]);


GO
CREATE STATISTICS [_dta_stat_OrderHeader_OrderHeader_ID_MatchingValidationCode_MatchingUser_ID]
    ON [dbo].[OrderHeader]([OrderHeader_ID], [MatchingValidationCode], [MatchingUser_ID]);


GO
CREATE STATISTICS [_dta_stat_OrderHeader_OrderHeader_ID_ApprovedBy_MatchingValidationCode_MatchingUser_ID]
    ON [dbo].[OrderHeader]([OrderHeader_ID], [ApprovedBy], [MatchingValidationCode], [MatchingUser_ID]);


GO
CREATE Trigger [OrderHeaderDel] 
ON dbo.OrderHeader
FOR DELETE
AS
BEGIN
    DECLARE @Error_No int
    SELECT @Error_No = 0

	UPDATE OrderExportDeletedQueue
	SET QueueInsertedDate = GetDate(), DeliveredToStoreOpsDate = NULL
	WHERE OrderHeader_ID in (SELECT OrderHeader_ID FROM Deleted)

	IF @@ROWCOUNT=0
	BEGIN
	    INSERT INTO OrderExportDeletedQueue
		SELECT OrderHeader_ID, GetDate(), NULL FROM Deleted
	END

    SELECT @Error_No = @@ERROR

	-- Amazon event 
	IF @Error_No = 0
	BEGIN
		IF (SELECT ISNULL(dbo.fn_InstanceDataValue('EnableAmazonEventGeneration', null), 0)) = 1
		BEGIN
			DECLARE @unprocessedStatusCode NVARCHAR(1) = 'U';
			DECLARE @TSF_DeleteEventTypeID INT = (
					SELECT TOP 1 EventTypeID 
					FROM amz.EventType 
					WHERE EventTypeDescription = 'Transfer Order Deletion')
			DECLARE @PO_DeleteEventTypeID INT = (
					SELECT TOP 1 EventTypeID 
					FROM amz.EventType 
					WHERE EventTypeDescription = 'Purchase Order Deletion')
			DECLARE @poLineDeletionEventTypeId INT = (
					SELECT TOP 1 EventTypeID
					FROM amz.EventType
					WHERE EventTypeDescription = 'Purchase Order Line Item Deletion'
					);
			DECLARE @transferLineDeletionEventTypeId INT = (
					SELECT TOP 1 EventTypeID
					FROM amz.EventType
					WHERE EventTypeDescription = 'Transfer Line Item Deletion'
					);
	
			-- If the entire order is deleted, remove the individul line item deletion event generated by the OrderItemDelete event.
			DELETE q
			FROM   amz.OrderQueue q
			JOIN   deleted d ON q.KeyID = d.OrderHeader_ID
			WHERE  q.EventTypeId in (@poLineDeletionEventTypeId, @transferLineDeletionEventTypeId)
			  AND  q.Status = @unprocessedStatusCode

			INSERT INTO amz.OrderQueue (EventTypeID, KeyID, InsertDate, Status, MessageTimestampUtc)
			SELECT CASE 
					WHEN deleted.OrderType_ID <> 3 THEN @PO_DeleteEventTypeID
					ELSE @TSF_DeleteEventTypeID
				   END,
				deleted.OrderHeader_ID,
				SYSDATETIME(),
				@unprocessedStatusCode, -- for 'Unprocessed'
				SYSUTCDATETIME()
			FROM deleted
			WHERE deleted.Sent = 1
		END
	END

    IF @Error_No <> 0
    BEGIN
        ROLLBACK TRAN
        DECLARE @Severity smallint
        SELECT @Severity = ISNULL((SELECT severity FROM master.dbo.sysmessages WHERE error = @Error_No), 16)
        RAISERROR ('OrderHeaderDel trigger failed with @@ERROR: %d', @Severity, 1, @Error_No)
    END
END
GO

CREATE Trigger [OrderHeaderUpdate] 
ON dbo.OrderHeader
FOR UPDATE
AS

-- **************************************************************************
-- Trigger: OrderHeaderUpdate
--    Author: n/a
--      Date: n/a
--
-- Description:
-- Trigger for whenever OrderHeader is updated
--
-- Modification History:
-- Date			Init	TFS		Comment
-- 11/26/2012	BAS		8133	Added OrderRefreshCostSource_ID for tracking
--								where UpdateOrderRefreshCost is called on
--								an order
-- 08/17/2017   MZ      20620   Register an order for a WFM ordering banner store to
--                              the [infor].[OrderExpectedDateChangeQueue] table when
--                              its expected date changes before the order is closed.
-- 03/20/2017   MZ		26106   Register an order for a Amazon Extract store to
--                              the [infor].[OrderExpectedDateChangeQueue] table when
--                              its expected date changes before the order is closed.
-- 08/16/2018	BJ		28014	Added queuing events for Amazon when an order is updated
-- **************************************************************************

BEGIN
	DECLARE @Error_No int
	SELECT @Error_No = 0

	-- Log changes
	INSERT INTO	OrderHeaderHistory (OrderHeader_ID, InvoiceNumber, OrderHeaderDesc, Vendor_ID, PurchaseLocation_ID, ReceiveLocation_ID, CreatedBy, OrderDate, CloseDate, OriginalCloseDate, SystemGenerated, Sent, Fax_Order, Expected_Date, SentDate, QuantityDiscount, DiscountType, Transfer_SubTeam, Transfer_To_SubTeam, Return_Order, User_ID, Temperature, Accounting_In_DateStamp, Accounting_In_UserID, InvoiceDate, ApprovedDate, ApprovedBy, UploadedDate, RecvLogDate, RecvLog_No, RecvLogUser_ID, VendorDoc_ID, VendorDocDate, WarehouseSent, WarehouseSentDate, OrderType_ID, ProductType_ID, FromQueue, SentToFaxDate, [Host_Name], ClosedBy, MatchingValidationCode, MatchingUser_ID, MatchingDate, Freight3Party_OrderCost, PayByAgreedCost, OrderRefreshCostSource_ID)
	SELECT		Inserted.OrderHeader_ID, Inserted.InvoiceNumber, Inserted.OrderHeaderDesc, Inserted.Vendor_ID, Inserted.PurchaseLocation_ID, Inserted.ReceiveLocation_ID, Inserted.CreatedBy, Inserted.OrderDate, Inserted.CloseDate, Inserted.OriginalCloseDate, Inserted.SystemGenerated, Inserted.Sent, Inserted.Fax_Order, Inserted.Expected_Date, Inserted.SentDate, Inserted.QuantityDiscount, Inserted.DiscountType, Inserted.Transfer_SubTeam, Inserted.Transfer_To_SubTeam, Inserted.Return_Order, Inserted.User_ID, Inserted.Temperature, Inserted.Accounting_In_DateStamp, Inserted.Accounting_In_UserID, Inserted.InvoiceDate, Inserted.ApprovedDate, Inserted.ApprovedBy, Inserted.UploadedDate, Inserted.RecvLogDate, Inserted.RecvLog_No, Inserted.RecvLogUser_ID, Inserted.VendorDoc_ID, Inserted.VendorDocDate, Inserted.WarehouseSent, Inserted.WarehouseSentDate, Inserted.OrderType_ID, Inserted.ProductType_ID, Inserted.FromQueue, Inserted.SentToFaxDate, HOST_NAME(), Inserted.ClosedBy, Inserted.MatchingValidationCode, Inserted.MatchingUser_ID, Inserted.MatchingDate, Inserted.Freight3Party_OrderCost, Inserted.PayByAgreedCost, Inserted.OrderRefreshCostSource_ID
	FROM			INSERTED		INNER JOIN	DELETED	ON	Deleted.OrderHeader_ID = Inserted.OrderHeader_ID
	WHERE		(Inserted.Sent <> Deleted.SENT 
				OR Inserted.Fax_Order <> Deleted.Fax_Order
				OR ISNULL(Inserted.SentDate, 0) <> ISNULL(Deleted.SentDate, 0)
				OR ISNULL(Inserted.ClosedBy, -1) <> ISNULL(Deleted.ClosedBy, -1)
				OR ISNULL(Inserted.ApprovedDate, 0) <> ISNULL(Deleted.ApprovedDate, 0)
				OR ISNULL(Inserted.ApprovedBy, -1) <> ISNULL(Deleted.ApprovedBy, -1)
				OR ISNULL(Inserted.UploadedDate, 0) <> ISNULL(Deleted.UploadedDate, 0)
				OR ISNULL(Inserted.MatchingValidationCode, -1) <> ISNULL(Deleted.MatchingValidationCode, -1)
				OR ISNULL(Inserted.MatchingDate, 0) <> ISNULL(Deleted.MatchingDate, 0)
				OR ISNULL(Inserted.MatchingUser_ID, -1) <> ISNULL(Deleted.MatchingUser_ID, -1)
				OR ISNULL(Inserted.Expected_Date, 0) <> ISNULL(Deleted.Expected_Date, 0)
				OR Inserted.PayByAgreedCost <> Deleted.PayByAgreedCost
				OR Inserted.OrderRefreshCostSource_ID <> Deleted.OrderRefreshCostSource_ID
				)
	UNION	
	SELECT		Deleted.OrderHeader_ID, Deleted.InvoiceNumber, Deleted.OrderHeaderDesc, Deleted.Vendor_ID, Deleted.PurchaseLocation_ID, Deleted.ReceiveLocation_ID, Deleted.CreatedBy, Deleted.OrderDate, Deleted.CloseDate, Deleted.OriginalCloseDate, Deleted.SystemGenerated, Deleted.Sent, Deleted.Fax_Order, Deleted.Expected_Date, Deleted.SentDate, Deleted.QuantityDiscount, Deleted.DiscountType, Deleted.Transfer_SubTeam, Deleted.Transfer_To_SubTeam, Deleted.Return_Order, Deleted.User_ID, Deleted.Temperature, Deleted.Accounting_In_DateStamp, Deleted.Accounting_In_UserID, Deleted.InvoiceDate, Deleted.ApprovedDate, Deleted.ApprovedBy, Deleted.UploadedDate, Deleted.RecvLogDate, Deleted.RecvLog_No, Deleted.RecvLogUser_ID, Deleted.VendorDoc_ID, Deleted.VendorDocDate, Deleted.WarehouseSent, Deleted.WarehouseSentDate, Deleted.OrderType_ID, Deleted.ProductType_ID, Deleted.FromQueue, Deleted.SentToFaxDate, HOST_NAME(), Deleted.ClosedBy, Deleted.MatchingValidationCode, Deleted.MatchingUser_ID, Deleted.MatchingDate, Deleted.Freight3Party_OrderCost, Deleted.PayByAgreedCost, Deleted.OrderRefreshCostSource_ID
	FROM			DELETED	INNER JOIN	INSERTED					ON Inserted.OrderHeader_ID = Deleted.OrderHeader_ID
						LEFT JOIN	OrderHeaderHistory OHH		ON Deleted.OrderHeader_ID = OHH.OrderHeader_ID
	WHERE		OHH.OrderHeader_ID IS NULL
				AND (Inserted.Sent <> Deleted.Sent
				OR Inserted.Fax_Order <> Deleted.Fax_Order
				OR ISNULL(Inserted.SentDate, 0) <> ISNULL(Deleted.SentDate, 0)
				OR ISNULL(Inserted.CloseDate, 0) <> ISNULL(Deleted.CloseDate, 0)
				OR ISNULL(Inserted.ClosedBy, -1) <> ISNULL(Deleted.ClosedBy, -1) 
				OR ISNULL(Inserted.ApprovedDate, 0) <> ISNULL(Deleted.ApprovedDate, 0)
				OR ISNULL(Inserted.ApprovedBy, -1) <> ISNULL(Deleted.ApprovedBy, -1)
				OR ISNULL(Inserted.UploadedDate, 0) <> ISNULL(Deleted.UploadedDate, 0)
				OR ISNULL(Inserted.MatchingValidationCode, -1) <> ISNULL(Deleted.MatchingValidationCode, -1)
				OR ISNULL(Inserted.MatchingDate, 0) <> ISNULL(Deleted.MatchingDate, 0)
				OR ISNULL(Inserted.MatchingUser_ID, -1) <> ISNULL(Deleted.MatchingUser_ID, -1)
				OR ISNULL(Inserted.Expected_Date, 0) <> ISNULL(Deleted.Expected_Date, 0)
				OR Inserted.PayByAgreedCost <> Deleted.PayByAgreedCost
				OR Inserted.OrderRefreshCostSource_ID <> Deleted.OrderRefreshCostSource_ID
				)

	SELECT @Error_No = @@ERROR

	--
	-- StoreOps Export 
	--
	IF @Error_No = 0
	BEGIN
  
			DECLARE @OrdersToExport TABLE (OrderHeader_Id int NOT NULL PRIMARY KEY) 
			DECLARE @InsertDate datetime
			

			INSERT INTO @OrdersToExport
			SELECT DISTINCT Inserted.OrderHeader_ID
			FROM		Inserted	INNER JOIN	Deleted		ON Inserted.OrderHeader_ID = Deleted.OrderHeader_ID
			WHERE 
					(Inserted.SentDate IS NOT NULL)
					AND
					((Deleted.SentDate IS NULL) 
					OR (ISNULL(Inserted.CloseDate, 0) <> ISNULL(Deleted.CloseDate, 0))
					OR (Inserted.Vendor_ID <> Deleted.Vendor_ID)
					OR (Inserted.ReceiveLocation_ID <> Deleted.ReceiveLocation_ID)
					OR (ISNULL(Inserted.Transfer_To_SubTeam, 0) <> ISNULL(Deleted.Transfer_To_SubTeam, 0)) 
					OR (ISNULL(Inserted.Transfer_SubTeam, 0) <> ISNULL(Deleted.Transfer_SubTeam, 0))
					OR (ISNULL(Inserted.Expected_Date, 0) <> ISNULL(Deleted.Expected_Date, 0))
					OR (ISNULL(Inserted.RecvLogDate, 0) <> ISNULL(Deleted.RecvLogDate, 0))
					OR (ISNULL(Inserted.UploadedDate, 0) <> ISNULL(Deleted.UploadedDate, 0))
					OR (ISNULL(Inserted.ApprovedDate, 0) <> ISNULL(Deleted.ApprovedDate, 0))
					OR (ISNULL(Inserted.OrderHeaderDesc, '') <> ISNULL(Deleted.OrderHeaderDesc, ''))
					OR (Inserted.Return_Order <> Deleted.Return_Order)
					OR (ISNULL(Inserted.InvoiceNumber, '') <> ISNULL(Deleted.InvoiceNumber, ''))
					OR (ISNULL(Inserted.InvoiceDate, 0) <> ISNULL(Deleted.InvoiceDate, 0))
					)
				

			DELETE oeq
			FROM		OrderExportQueue oeq	INNER JOIN	@OrdersToExport ote		ON oeq.orderHeader_id = ote.OrderHeader_Id
			
			SET @InsertDate = GETDATE()

			INSERT INTO	OrderExportQueue
			SELECT		OrderHeader_ID, @InsertDate, NULL 
			FROM			@OrdersToExport
	
			SELECT @Error_No = @@ERROR
	END

	--
	-- Infor Export 
	--
	IF @Error_No = 0
	BEGIN
		INSERT INTO	infor.OrderExpectedDateChangeQueue (OrderHeader_ID, InsertDate)
		SELECT		Inserted.OrderHeader_ID, GETDATE()
		FROM		INSERTED		
		INNER JOIN	DELETED	ON	Deleted.OrderHeader_ID = Inserted.OrderHeader_ID
		INNER JOIN  Vendor psl on Inserted.PurchaseLocation_ID = psl.Vendor_ID
		INNER JOIN  Store s on s.Store_No = psl.Store_no 
		WHERE       ISNULL(Inserted.Expected_Date, 0) <> ISNULL(Deleted.Expected_Date, 0)
		AND         Inserted.Sent = 1
		AND         Inserted.OrderType_ID <> 3
		AND         Inserted.OriginalCloseDate is null
		AND         (s.mega_store = 1 
		 OR			 s.BusinessUnit_ID in (SELECT Key_Value FROM [dbo].[fn_Parse_List]([dbo].[fn_GetAppConfigValue]('WFMBannerStoresForOrdering', 'IRMA CLIENT'), '|'))
		 OR          s.BusinessUnit_ID in (SELECT Key_Value FROM [dbo].[fn_Parse_List]([dbo].[fn_GetAppConfigValue]('StoreBUsForAMZExtract', 'IRMA CLIENT'), '|')))	

		SELECT @Error_No = @@ERROR
	END

	--
	-- Amazon event 
	--
	IF @Error_No = 0
BEGIN
	IF (SELECT ISNULL(dbo.fn_InstanceDataValue('EnableAmazonEventGeneration', null), 0)) = 1
	BEGIN
		DECLARE @po_creEventTypeID INT = (SELECT TOP 1 EventTypeID FROM amz.EventType WHERE EventTypeCode = 'PO_CRE')
		DECLARE @po_modEventTypeID INT = (SELECT TOP 1 EventTypeID FROM amz.EventType WHERE EventTypeCode = 'PO_MOD')
		DECLARE @tsf_creEventTypeID INT = (SELECT TOP 1 EventTypeID FROM amz.EventType WHERE EventTypeCode = 'TSF_CRE')
		DECLARE @tsf_modEventTypeID INT = (SELECT TOP 1 EventTypeID FROM amz.EventType WHERE EventTypeCode = 'TSF_MOD')
		DECLARE @unprocessedStatus NCHAR(1) = 'U'

		IF(object_ID ('tempdb..#tempOrders') IS NOT NULL) DROP TABLE #tempOrders;
		SELECT		CASE
						WHEN (d.Sent = 0 AND i.OrderType_ID <> 3) THEN @po_creEventTypeID
						WHEN (d.Sent = 0 AND i.OrderType_ID = 3) THEN @tsf_creEventTypeID
						ELSE 
						CASE 
								WHEN i.OrderType_ID = 3 THEN @tsf_modEventTypeID
								ELSE @po_modEventTypeID
							 END	
					END EventTypeID,
					i.OrderHeader_ID KeyID
		INTO	#tempOrders
		FROM		INSERTED	i	
		INNER JOIN	DELETED		d	ON	i.OrderHeader_ID = d.OrderHeader_ID
		WHERE	i.Sent = 1 
		AND		(d.Sent = 0 --Order is Sent
				OR	CAST(ISNULL(i.Expected_Date, GETDATE()) AS DATE) <> CAST(ISNULL(d.Expected_Date, GETDATE()) AS DATE) --Expected date changed
				OR	(i.CloseDate IS NOT NULL AND d.CloseDate IS NULL) --Order is closed
				)

		INSERT INTO amz.OrderQueue (EventTypeID, KeyID, Status)
		SELECT 
			EventTypeID,
			KeyID,
			@unprocessedStatus
		FROM #tempOrders o
		WHERE NOT EXISTS
		(
			SELECT 1 
			FROM amz.OrderQueue q
			WHERE o.EventTypeID = q.EventTypeID
				AND o.KeyID = q.KeyID
				AND q.Status = @unprocessedStatus
		)
		ORDER BY EventTypeID 

		SELECT @Error_No = @@ERROR
	END
	END

	IF @Error_No <> 0
	BEGIN
		DECLARE @Severity smallint
		SELECT @Severity = ISNULL((SELECT severity FROM master.dbo.sysmessages WHERE error = @Error_No), 16)
		RAISERROR ('OrderHeaderUpdate trigger failed with @@ERROR: %d', @Severity, 1, @Error_No)
	END

	IF(object_ID ('tempdb..#tempOrders') is Not null) drop table #tempOrders;
END

GO
CREATE TRIGGER OrderHeaderAdd
ON [dbo].[OrderHeader]
AFTER INSERT
AS

/*
	This trigger will populate Orderheader.PayByAgreedCost with the correct value for every order that is created.
	Added for bug 10586
*/

BEGIN
	UPDATE 
		OrderHeader	
	SET 
		PayByAgreedCost = dbo.fn_IsPayByAgreedCostStoreVendor(vs.Store_No, oh.Vendor_ID, GETDATE())
	FROM
		OrderHeader		oh
		JOIN Vendor		vs	ON	oh.PurchaseLocation_ID	= vs.Vendor_ID
		JOIN Inserted	i	ON	oh.OrderHeader_ID		= i.OrderHeader_ID

	IF (SELECT ISNULL(dbo.fn_InstanceDataValue('EnableAmazonEventGeneration', null), 0)) = 1
	BEGIN
		DECLARE @PO_CRE_EvenTypeID INT = (SELECT EventTypeID FROM amz.EventType WHERE EventTypeCode = 'PO_CRE')
		DECLARE @TSF_CRE_EvenTypeID INT = (SELECT EventTypeID FROM amz.EventType WHERE EventTypeCode = 'TSF_CRE')

		INSERT INTO amz.OrderQueue (EventTypeID, KeyID, InsertDate, Status, MessageTimestampUtc)
		SELECT CASE
			WHEN inserted.OrderType_ID <> 3 THEN @PO_CRE_EvenTypeID
			ELSE @TSF_CRE_EvenTypeID
			END,
			inserted.OrderHeader_ID,
			SYSDATETIME(),
			'U', -- for 'Unprocessed'
			SYSUTCDATETIME()
		FROM inserted
		WHERE inserted.Sent = 1
	END
END
GO
GRANT VIEW CHANGE TRACKING
    ON OBJECT::[dbo].[OrderHeader] TO [IRMAAdminRole]
    AS [dbo];


GO
GRANT VIEW CHANGE TRACKING
    ON OBJECT::[dbo].[OrderHeader] TO [IRMASupportRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[OrderHeader] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[OrderHeader] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT VIEW CHANGE TRACKING
    ON OBJECT::[dbo].[OrderHeader] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT INSERT
    ON OBJECT::[dbo].[OrderHeader] TO [IRMAReportsRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[OrderHeader] TO [IRMAReportsRole]
    AS [dbo];


GO
GRANT UPDATE
    ON OBJECT::[dbo].[OrderHeader] TO [IRMAReportsRole]
    AS [dbo];


GO
GRANT VIEW CHANGE TRACKING
    ON OBJECT::[dbo].[OrderHeader] TO [IRMAReportsRole]
    AS [dbo];


GO
GRANT INSERT
    ON OBJECT::[dbo].[OrderHeader] TO [IRMASchedJobs]
    AS [dbo];


GO
GRANT UPDATE
    ON OBJECT::[dbo].[OrderHeader] TO [IRMASchedJobs]
    AS [dbo];


GO
GRANT VIEW CHANGE TRACKING
    ON OBJECT::[dbo].[OrderHeader] TO [IRMA_Teradata]
    AS [dbo];


GO
GRANT VIEW CHANGE TRACKING
    ON OBJECT::[dbo].[OrderHeader] TO [BizTalk]
    AS [dbo];


GO
GRANT VIEW CHANGE TRACKING
    ON OBJECT::[dbo].[OrderHeader] TO [IConInterface]
    AS [dbo];


GO
GRANT VIEW CHANGE TRACKING
    ON OBJECT::[dbo].[OrderHeader] TO [iCONReportingRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[OrderHeader] TO [IRMAPDXExtractRole]
    AS [dbo];
