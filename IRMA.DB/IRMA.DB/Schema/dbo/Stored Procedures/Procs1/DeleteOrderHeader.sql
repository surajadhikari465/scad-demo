CREATE PROCEDURE [dbo].[DeleteOrderHeader] 
    @OrderHeader_ID int,
    @User_ID int,
    @DeletedReasonCode int = null  
AS 
   -- **************************************************************************
   -- Procedure: DeleteOrderHeader()
   --    Author: n/a
   --      Date: n/a
   --
   -- Description:
   -- This procedure is called from the IRMA client and removes an OrderHeader and
   -- all associated FKs and dependent records
   --
   -- Modification History:
   -- Date			Init	Comment
   -- 03/12/2010	BBB		Updated for readability
   -- 06/17/2009	MD		Added delete from child table OrderHeaderApplyNewVendorCostQueue
   -- 06/15/2009	DN		Added delete from child table VendorACKCostHistoryQueue 
   --						before the delete to OrderItem table
   -- 12/12/2011    MZ      Removed all the references to VendorACKCostHistoryQueue table 
   --                       because the table is deleted in 4.4 
   --11/07/2012     AB		Added  logic for deleted orders
   -- **************************************************************************
BEGIN
    DECLARE @Error_No int
    SELECT @Error_No = 0
    DECLARE @DeletedOrder_ID int

    BEGIN TRAN
   
    --**************************************************************************
	--Declare internal variables
	--**************************************************************************
    DECLARE @OrderStart				datetime
    DECLARE @OrderEnd				datetime
    DECLARE @OrderType_ID			int
    DECLARE @Vendor_ID				int
    DECLARE @Transfer_From_SubTeam	int 
    DECLARE @ReceiveLocation_ID		int
    DECLARE @Return_Order			bit
    DECLARE @Transfer_To_SubTeam	int

    --**************************************************************************
	--Populate internal variables
	--**************************************************************************
    SELECT 
		@Vendor_ID				= Vendor_ID, 
		@Transfer_From_SubTeam	= Transfer_SubTeam, 
		@OrderType_ID			= OrderType_ID,
        @ReceiveLocation_ID		= ReceiveLocation_ID, 
		@Return_Order			= Return_Order, 
		@Transfer_To_SubTeam	= Transfer_To_SubTeam 
    FROM 
		OrderHeader (nolock) 
	WHERE 
		OrderHeader_ID = @OrderHeader_ID

    --**************************************************************************
	--Populate internal variables
	--**************************************************************************
    SELECT 
		@OrderStart = OrderStart,             
        @OrderEnd	= CASE 
						WHEN @Transfer_From_SubTeam = @Transfer_To_SubTeam THEN 
							OrderEnd -- not transfer
						ELSE 
							OrderEndTransfers -- is transfer
					  END
    FROM 
		ZoneSubTeam 		(nolock) zst
        INNER JOIN Vendor	(nolock) v		ON	v.Vendor_ID		= @Vendor_ID 
											AND	v.Store_No		= zst.Supplier_Store_No 
											AND zst.SubTeam_No	= @Transfer_From_SubTeam
        INNER JOIN Vendor	(nolock) rl		ON	rl.Vendor_ID	= @ReceiveLocation_ID
        INNER JOIN Store	(nolock) s		ON	s.Store_No		= rl.Store_No 
											AND s.Zone_ID		= zst.Zone_ID
    WHERE 
		@Return_Order		= 0 
		AND @OrderType_ID	= 2
		AND NOT EXISTS (SELECT * FROM Users (nolock) WHERE User_ID = @User_ID AND Warehouse = 1)

    --**************************************************************************
	--Begin Delete
	--**************************************************************************
    IF (DATEDIFF(minute, CONVERT(varchar(255), ISNULL(@OrderStart, CONVERT(smalldatetime, GETDATE())), 108), CONVERT(varchar(255), CONVERT(smalldatetime, GETDATE()), 108)) >= 0) OR (DATEDIFF(minute, CONVERT(varchar(255), CONVERT(smalldatetime, GETDATE()), 108), CONVERT(varchar(255), ISNULL(@OrderEnd, CONVERT(smalldatetime, GETDATE())), 108)) >= 0)
		BEGIN
		
    
        SELECT @Error_No = @@ERROR   

		--**************************************************************************
		--ItemHistory delete
		--**************************************************************************		
        IF @Error_No = 0
			BEGIN
				DELETE 
					ItemHistory
				FROM 
					ItemHistory				(rowlock)	ih
					INNER JOIN OrderItem	(nolock)	oi ON ih.OrderItem_ID = oi.OrderItem_ID
				WHERE 
					oi.OrderHeader_ID = @OrderHeader_ID
				
					DELETE 
					ItemHistoryQueue
				FROM 
					ItemHistoryQueue				(rowlock)	ih
					INNER JOIN OrderItem	(nolock)	oi ON ih.OrderItem_ID = oi.OrderItem_ID
				WHERE 
					oi.OrderHeader_ID = @OrderHeader_ID
	        
				PRINT 'ItemHistory delete'
				
				SELECT @Error_No = @@ERROR
			END

		--**************************************************************************
		--OrderHeaderApplyNewVendorCostQueue delete
		--**************************************************************************	
		IF @Error_No = 0
        BEGIN
            DELETE
				OrderHeaderApplyNewVendorCostQueue
            FROM 
				OrderHeaderApplyNewVendorCostQueue (rowlock)
			WHERE 
				OrderHeader_ID = @OrderHeader_ID
				
				PRINT 'OrderHeaderApplyNewVendorCostQueue delete'
        
            SELECT @Error_No = @@ERROR
        END
		
		--**************************************************************************
		--Suspended Avg Cost
		--**************************************************************************	
		if @error_no = 0
			begin		
				DELETE	SuspendedAvgCost
				WHERE	OrderItem_ID IN (	SELECT	OrderItem_ID 
											FROM	OrderItem (nolock) 
											WHERE	OrderHeader_ID = @OrderHeader_ID
										)
			end
			

	
    
		--**************************************************************************
		--ReturnOrderList delete
		--**************************************************************************	
        IF @Error_No = 0
			BEGIN
				DELETE
					ReturnOrderList
				FROM 
					ReturnOrderList (rowlock)
				WHERE 
					ReturnOrderHeader_ID = @OrderHeader_ID
					
				PRINT 'ReturnOrderList delete'
	    
				SELECT @Error_No = @@ERROR
			END
    
		--**************************************************************************
		--OrderInvoice_Freight3Party delete
		--**************************************************************************	
        IF @Error_No = 0
			BEGIN
				DELETE
					OrderInvoice_Freight3Party
				FROM 
					OrderInvoice_Freight3Party (rowlock)
				WHERE 
					OrderHeader_ID = @OrderHeader_ID
					
				PRINT 'OrderInvoice_Freight3Party delete'
	    
				SELECT @Error_No = @@ERROR
			END
    
		--**************************************************************************
		--OrderInvoice delete
		--**************************************************************************	
        IF @Error_No = 0
			BEGIN
				DELETE
					OrderInvoice
				FROM 
					OrderInvoice (rowlock)
				WHERE 
					OrderHeader_ID = @OrderHeader_ID
	    
				PRINT 'OrderInvoice delete'
	    
				SELECT @Error_No = @@ERROR
			END
        
		--**************************************************************************
		--OrderTransmissionOverride delete
		--**************************************************************************	
        IF @Error_No = 0
			BEGIN
				DELETE 
					OrderTransmissionOverride
				FROM 
					OrderTransmissionOverride (rowlock)
				WHERE 
					OrderHeader_ID = @OrderHeader_ID
					
				PRINT 'OrderTransmissionOverride delete'
	    
				SELECT @Error_No = @@ERROR
			END
    
	
		--**************************************************************************
		--OrderHeader select
		--**************************************************************************	
        IF @Error_No = 0
			BEGIN
				--INSERT INTO DeletedOrder (OrderHeader_ID, User_ID, Vendor_ID, ReceiveLocation_ID, CreatedBy, OrderDate, SentDate, Transfer_SubTeam, Transfer_To_SubTeam, OrderType_ID, ProductType_ID)
				--	SELECT @OrderHeader_ID, @User_ID, Vendor_ID, ReceiveLocation_ID, CreatedBy, OrderDate, SentDate, Transfer_SubTeam, Transfer_To_SubTeam, OrderType_ID, ProductType_ID 
				--	FROM OrderHeader (nolock)
				INSERT into DeletedOrder (OrderHeader_ID,User_ID,Vendor_ID,ReceiveLocation_ID,CreatedBy,OrderDate,SentDate,Transfer_SubTeam,Transfer_To_SubTeam,OrderType_ID,ProductType_ID
		,InvoiceNumber,OrderHeaderDesc,PurchaseLocation_ID,CloseDate,OriginalCloseDate,SystemGenerated,Sent,Fax_Order,Expected_Date,QuantityDiscount,DiscountType
        ,Return_Order,OrderHeader_User_ID,Temperature,Accounting_In_DateStamp,Accounting_In_UserID,InvoiceDate,ApprovedDate,ApprovedBy,UploadedDate,RecvLogDate
	    ,RecvLog_No,RecvLogUser_ID,VendorDoc_ID,VendorDocDate,WarehouseSent,WarehouseSentDate,SentToFaxDate,FromQueue,ClosedBy,IsDropShipment
	    ,OrderExternalSourceID,OrderExternalSourceOrderID,MatchingValidationCode,MatchingUser_ID,MatchingDate,Freight3Party_OrderCost,DVOOrderID,eInvoice_Id
	    ,Electronic_Order,PayByAgreedCost,Email_Order,SentToEmailDate,OverrideTransmissionMethod,SupplyTransferToSubTeam,AccountingUploadDate,SentToElectronicDate
	    ,InvoiceDiscrepancy,InvoiceDiscrepancySentDate,InvoiceProcessingDiscrepancy,WarehouseCancelled,PurchaseAccountsTotal,CurrencyID,APUploadedCost,QtyShippedProvided
	    ,POCostDate,AdminNotes,ResolutionCodeID,InReview,InReviewUser,ReasonCodeDetailID,RefuseReceivingReasonID,OrderedCost,OriginalReceivedCost,TotalPaidCost,
	    AdjustedReceivedCost,DeletedReason,InvoiceTotalCost,ReturnOrderHeader_ID,DSDOrder,PartialShipment,InvoiceCost,InvoiceFreight)
				SELECT @OrderHeader_ID,@User_ID,Vendor_ID,ReceiveLocation_ID,CreatedBy,OrderDate,SentDate,Transfer_SubTeam,Transfer_To_SubTeam,OrderType_ID,ProductType_ID
		,InvoiceNumber,OrderHeaderDesc,PurchaseLocation_ID,CloseDate,OriginalCloseDate,SystemGenerated,Sent,Fax_Order,Expected_Date,QuantityDiscount,DiscountType
        ,Return_Order,User_ID,Temperature,Accounting_In_DateStamp,Accounting_In_UserID,InvoiceDate,ApprovedDate,ApprovedBy,UploadedDate,RecvLogDate
	    ,RecvLog_No,RecvLogUser_ID,VendorDoc_ID,VendorDocDate,WarehouseSent,WarehouseSentDate,SentToFaxDate,FromQueue,ClosedBy,IsDropShipment
	    ,OrderExternalSourceID,OrderExternalSourceOrderID,MatchingValidationCode,MatchingUser_ID,MatchingDate,Freight3Party_OrderCost,DVOOrderID,eInvoice_Id
	    ,Electronic_Order,PayByAgreedCost,Email_Order,SentToEmailDate,OverrideTransmissionMethod,SupplyTransferToSubTeam,AccountingUploadDate,SentToElectronicDate
	    ,InvoiceDiscrepancy,InvoiceDiscrepancySentDate,InvoiceProcessingDiscrepancy,WarehouseCancelled,PurchaseAccountsTotal,CurrencyID,APUploadedCost,QtyShippedProvided
	    ,POCostDate,AdminNotes,ResolutionCodeID,InReview,InReviewUser,ReasonCodeDetailID,RefuseReceivingReasonID,OrderedCost,OriginalReceivedCost
	    ,TotalPaidCost,AdjustedReceivedCost,@DeletedReasonCode,InvoiceTotalCost,ReturnOrderHeader_ID,DSDOrder,PartialShipment,InvoiceCost,InvoiceFreight
				FROM OrderHeader oh (nolock)
				LEFT JOIN	ReturnOrderList (nolock) rf		ON	oh.OrderHeader_ID			= rf.OrderHeader_ID
			    LEFT JOIN	OrderInvoice	(nolock) ov		ON	oh.OrderHeader_ID			= ov.OrderHeader_ID 
					WHERE oh.OrderHeader_ID = @OrderHeader_ID
					
					SET @DeletedOrder_ID = @@IDENTITY
	    
				SELECT @Error_No = @@ERROR
			END
				--**************************************************************************
		--OrderItem select
		--**************************************************************************	
			IF @Error_No = 0
			BEGIN
			 INSERT INTO DeletedOrderItem (OrderItem_ID,OrderHeader_ID,Item_Key,ExpirationDate,QuantityOrdered,QuantityUnit,QuantityReceived,Total_Weight,Units_per_Pallet,Cost,UnitCost,UnitExtCost,CostUnit
				,QuantityDiscount,DiscountType,AdjustedCost,Handling,HandlingUnit,Freight,FreightUnit,DateReceived,OriginalDateReceived,Comments,LineItemCost,LineItemHandling
				,LineItemFreight,ReceivedItemCost,ReceivedItemHandling,ReceivedItemFreight,LandedCost,MarkupPercent,MarkupCost,Package_Desc1,Package_Desc2,Package_Unit_ID
				,Retail_Unit_ID,Origin_ID,ReceivedFreight,UnitsReceived,CreditReason_ID,QuantityAllocated,CountryProc_ID,Lot_No,NetVendorItemDiscount,CostAdjustmentReason_ID
				,Freight3Party,LineItemFreight3Party,HandlingCharge,eInvoiceQuantity,SACCost,OrderItemCOOL,OrderItemBIO,Carrier,InvoiceQuantityUnit,InvoiceCost,InvoiceExtendedCost
				,InvoiceExtendedFreight,InvoiceTotalWeight,VendorCostHistoryID,OrigReceivedItemCost,OrigReceivedItemUnit,CatchWeightCostPerWeight,QuantityShipped,WeightShipped
				,OHOrderDate,SustainabilityRankingID,eInvoiceWeight,ReasonCodeDetailID,ReceivingDiscrepancyReasonCodeID,PaidCost,ApprovedDate,ApprovedByUserId,AdminNotes
				,ResolutionCodeID,PaymentTypeID,LineItemSuspended,DeletedOrder_ID)
		  SELECT OrderItem_ID,OrderHeader_ID,Item_Key,ExpirationDate,QuantityOrdered,QuantityUnit,QuantityReceived,Total_Weight,Units_per_Pallet,Cost,UnitCost,UnitExtCost,CostUnit
				,QuantityDiscount,DiscountType,AdjustedCost,Handling,HandlingUnit,Freight,FreightUnit,DateReceived,OriginalDateReceived,Comments,LineItemCost,LineItemHandling
				,LineItemFreight,ReceivedItemCost,ReceivedItemHandling,ReceivedItemFreight,LandedCost,MarkupPercent,MarkupCost,Package_Desc1,Package_Desc2,Package_Unit_ID
				,Retail_Unit_ID,Origin_ID,ReceivedFreight,UnitsReceived,CreditReason_ID,QuantityAllocated,CountryProc_ID,Lot_No,NetVendorItemDiscount,CostAdjustmentReason_ID
				,Freight3Party,LineItemFreight3Party,HandlingCharge,eInvoiceQuantity,SACCost,OrderItemCOOL,OrderItemBIO,Carrier,InvoiceQuantityUnit,InvoiceCost,InvoiceExtendedCost
				,InvoiceExtendedFreight,InvoiceTotalWeight,VendorCostHistoryID,OrigReceivedItemCost,OrigReceivedItemUnit,CatchWeightCostPerWeight,QuantityShipped,WeightShipped
				,OHOrderDate,SustainabilityRankingID,eInvoiceWeight,ReasonCodeDetailID,ReceivingDiscrepancyReasonCodeID,PaidCost,ApprovedDate,ApprovedByUserId,AdminNotes
				,ResolutionCodeID,PaymentTypeID,LineItemSuspended,@DeletedOrder_ID
		  FROM OrderItem (nolock)
					WHERE OrderHeader_ID = @OrderHeader_ID
	    
				SELECT @Error_No = @@ERROR
			END
    	
		--**************************************************************************
		--OrderItem delete
		--**************************************************************************	
        IF @Error_No = 0
			BEGIN 
				DELETE
					OrderItem
				FROM 
					OrderItem (rowlock)
				WHERE 
					OrderHeader_ID = @OrderHeader_ID
	    
				PRINT 'OrderItem delete'
	    
				SELECT @Error_No = @@ERROR
			END
    
		--**************************************************************************
		--OrderHeader delete
		--**************************************************************************	  
        IF @Error_No = 0
			BEGIN
				DELETE
					OrderHeader
				FROM 
					OrderHeader (rowlock)
				WHERE 
					OrderHeader_ID = @OrderHeader_ID
	    
				PRINT 'OrderHeader delete'
	    
				SELECT @Error_No = @@ERROR
			END
    
		--**************************************************************************
		--Commit
		--**************************************************************************	
        IF @Error_No = 0
            COMMIT TRAN
        ELSE
			BEGIN
				ROLLBACK TRAN
				DECLARE @Severity smallint
				SELECT @Severity = ISNULL((SELECT severity FROM master.dbo.sysmessages WHERE error = @Error_No), 16)
				RAISERROR ('DeleteOrderHeader failed with @@ERROR: %d', @Severity, 1, @Error_No)
			END
			
		END
    ELSE
		BEGIN
			DECLARE @WindowStart varchar(255), @WindowEnd varchar(255)
			SELECT @WindowStart = CONVERT(varchar(255), @OrderStart, 108), @WindowEnd = CONVERT(varchar(255), @OrderEnd, 108)
			RAISERROR(50002, 16, 1, @WindowStart, @WindowEnd)
		END
END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[DeleteOrderHeader] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[DeleteOrderHeader] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[DeleteOrderHeader] TO [IRMAReportsRole]
    AS [dbo];

