﻿CREATE PROCEDURE dbo.DeleteOrderItem 
    @OrderItem_ID int,
    @User_ID int 
AS 
-- **************************************************************************
   -- Procedure: DeleteOrderItem()
   --    Author: n/a
   --      Date: n/a
   --
   -- Description:
   -- This procedure is called from the IRMA client and removes an OrderItem and
   -- all associated FKs and dependent records
   --
   -- Modification History:
   -- Date			Init	Comment
   -- 12/12/2011    MZ      Removed all the references to VendorACKCostHistoryQueue table 
   --                       because the table is deleted in 4.4 
   -- 10/12/2018    MZ      29631  Insert deleted line items from an active order in SENT
   --                              status into the amz.DeletedOrderItem table and generate 
   --                              a line item delelete event so that the deleted line
   --                              can be sent to AMZ.
   -- **************************************************************************
BEGIN
    DECLARE @Error_No int
    SELECT @Error_No = 0

    BEGIN TRAN

    DECLARE @OrderStart datetime, @OrderEnd datetime,
            @OrderType_ID int, @Vendor_ID int, @Transfer_From_SubTeam int, 
            @ReceiveLocation_ID int, @Return_Order bit, @Transfer_To_SubTeam int

    SELECT @Vendor_ID = Vendor_ID, @Transfer_From_SubTeam = Transfer_SubTeam, @OrderType_ID = OrderType_ID,
           @ReceiveLocation_ID = ReceiveLocation_ID, @Return_Order = Return_Order, @Transfer_To_SubTeam = Transfer_To_SubTeam
    FROM OrderHeader (NOLOCK) WHERE OrderHeader_ID = (SELECT TOP 1 OrderHeader_ID FROM OrderItem (NOLOCK) WHERE OrderItem_ID = @OrderItem_ID)

    SELECT 
		@OrderStart = OrderStart, 
		@OrderEnd = CASE WHEN @Transfer_From_SubTeam = @Transfer_To_SubTeam
                           THEN OrderEnd -- not transfer
                           ELSE OrderEndTransfers -- is transfer
                         END
    FROM ZoneSubTeam ZST (NOLOCK)
        INNER JOIN 
            Vendor (NOLOCK) 
            ON Vendor.Vendor_ID = @Vendor_ID AND Vendor.Store_No = ZST.Supplier_Store_No 
               AND ZST.SubTeam_No = @Transfer_From_SubTeam
        INNER JOIN 
            Vendor RL (NOLOCK) 
            ON RL.Vendor_ID = @ReceiveLocation_ID
        INNER JOIN 
            Store (NOLOCK) 
            ON Store.Store_No = RL.Store_No AND Store.Zone_ID = ZST.Zone_ID
    WHERE @Return_Order = 0 and @OrderType_ID = 2
          AND NOT EXISTS (SELECT * FROM Users (NOLOCK) WHERE User_ID = @User_ID AND Warehouse = 1)

    IF (DATEDIFF(minute, CONVERT(varchar(255), ISNULL(@OrderStart, CONVERT(smalldatetime, GETDATE())), 108), CONVERT(varchar(255), CONVERT(smalldatetime, GETDATE()), 108)) >= 0) OR
        (DATEDIFF(minute, CONVERT(varchar(255), CONVERT(smalldatetime, GETDATE()), 108), CONVERT(varchar(255), ISNULL(@OrderEnd, CONVERT(smalldatetime, GETDATE())), 108)) >= 0)
    BEGIN     

        SELECT @Error_No = @@ERROR

        IF @Error_No = 0
        BEGIN
            DELETE ItemHistory
            FROM ItemHistory 
            WHERE OrderItem_ID = @OrderItem_ID
						   
            DELETE ItemHistoryQueue
            FROM ItemHistoryQueue 
            WHERE OrderItem_ID = @OrderItem_ID
        
            SELECT @Error_No = @@ERROR
        END
        
	     -- suspended avgcost
		if @Error_No = 0        
		begin
			DELETE	SuspendedAvgCost
			WHERE	OrderItem_ID = @OrderItem_ID
		END
        
		if @Error_No = 0
		begin
			 INSERT INTO amz.DeletedOrderItem (OrderItem_ID,OrderHeader_ID,Item_Key,ExpirationDate,QuantityOrdered,QuantityUnit,QuantityReceived,Total_Weight,Units_per_Pallet,Cost,UnitCost,UnitExtCost,CostUnit
				,QuantityDiscount,DiscountType,AdjustedCost,Handling,HandlingUnit,Freight,FreightUnit,DateReceived,OriginalDateReceived,Comments,LineItemCost,LineItemHandling
				,LineItemFreight,ReceivedItemCost,ReceivedItemHandling,ReceivedItemFreight,LandedCost,MarkupPercent,MarkupCost,Package_Desc1,Package_Desc2,Package_Unit_ID
				,Retail_Unit_ID,Origin_ID,ReceivedFreight,UnitsReceived,CreditReason_ID,QuantityAllocated,CountryProc_ID,Lot_No,NetVendorItemDiscount,CostAdjustmentReason_ID
				,Freight3Party,LineItemFreight3Party,HandlingCharge,eInvoiceQuantity,SACCost,OrderItemCOOL,OrderItemBIO,Carrier,InvoiceQuantityUnit,InvoiceCost,InvoiceExtendedCost
				,InvoiceExtendedFreight,InvoiceTotalWeight,VendorCostHistoryID,OrigReceivedItemCost,OrigReceivedItemUnit,CatchWeightCostPerWeight,QuantityShipped,WeightShipped
				,OHOrderDate,SustainabilityRankingID,eInvoiceWeight,ReasonCodeDetailID,ReceivingDiscrepancyReasonCodeID,PaidCost,ApprovedDate,ApprovedByUserId,AdminNotes
				,ResolutionCodeID,PaymentTypeID,LineItemSuspended)
		  SELECT OrderItem_ID,OrderHeader_ID,Item_Key,ExpirationDate,QuantityOrdered,QuantityUnit,QuantityReceived,Total_Weight,Units_per_Pallet,Cost,UnitCost,UnitExtCost,CostUnit
				,QuantityDiscount,DiscountType,AdjustedCost,Handling,HandlingUnit,Freight,FreightUnit,DateReceived,OriginalDateReceived,Comments,LineItemCost,LineItemHandling
				,LineItemFreight,ReceivedItemCost,ReceivedItemHandling,ReceivedItemFreight,LandedCost,MarkupPercent,MarkupCost,Package_Desc1,Package_Desc2,Package_Unit_ID
				,Retail_Unit_ID,Origin_ID,ReceivedFreight,UnitsReceived,CreditReason_ID,QuantityAllocated,CountryProc_ID,Lot_No,NetVendorItemDiscount,CostAdjustmentReason_ID
				,Freight3Party,LineItemFreight3Party,HandlingCharge,eInvoiceQuantity,SACCost,OrderItemCOOL,OrderItemBIO,Carrier,InvoiceQuantityUnit,InvoiceCost,InvoiceExtendedCost
				,InvoiceExtendedFreight,InvoiceTotalWeight,VendorCostHistoryID,OrigReceivedItemCost,OrigReceivedItemUnit,CatchWeightCostPerWeight,QuantityShipped,WeightShipped
				,OHOrderDate,SustainabilityRankingID,eInvoiceWeight,ReasonCodeDetailID,ReceivingDiscrepancyReasonCodeID,PaidCost,ApprovedDate,ApprovedByUserId,AdminNotes
				,ResolutionCodeID,PaymentTypeID,LineItemSuspended
		  FROM OrderItem (nolock)
		 WHERE OrderItem_ID = @OrderItem_ID
	    
				SELECT @Error_No = @@ERROR
		end
        
		----
	    -- Amazon Events
	    ----
		IF (SELECT ISNULL(dbo.fn_InstanceDataValue('EnableAmazonEventGeneration', null), 0)) = 1
		BEGIN
			DECLARE @unprocessedStatusCode NVARCHAR(1) = 'U';
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

			INSERT INTO amz.OrderQueue (EventTypeID, KeyID,SecondaryKeyID, Status)
			SELECT 
				CASE
					WHEN oh.OrderType_ID = 3 THEN @transferLineDeletionEventTypeId
					ELSE @poLineDeletionEventTypeId 
				END
				,oh.OrderHeader_ID
				,oi.OrderItem_ID
				,@unprocessedStatusCode
			FROM OrderItem oi (nolock)
            JOIN OrderHeader oh on oi.OrderHeader_ID = oh.OrderHeader_ID
		   WHERE oi.OrderItem_ID = @OrderItem_ID
             AND oh.Sent = 1
	         AND NOT EXISTS
			(
				SELECT 1 
				  FROM amz.OrderQueue q
				 WHERE q.KeyID = oh.OrderHeader_ID
				   AND q.SecondaryKeyID = oi.OrderItem_ID
		           AND (q.EventTypeID = @poLineDeletionEventTypeId
		            OR  q.EventTypeID = @transferLineDeletionEventTypeId)
			       AND q.Status = @unprocessedStatusCode
			) 
			
			SELECT @Error_No = @@ERROR
		end

        IF @Error_No = 0
        BEGIN
            DELETE 
            FROM OrderItem 
            WHERE OrderItem_ID = @OrderItem_ID 
    
            SELECT @Error_No = @@ERROR
        END
    
        IF @Error_No = 0
            COMMIT TRAN
        ELSE
        BEGIN
            ROLLBACK TRAN
            DECLARE @Severity smallint
            SELECT @Severity = ISNULL((SELECT severity FROM master.dbo.sysmessages WHERE error = @Error_No), 16)
            RAISERROR ('DeleteOrderItem failed with @@ERROR: %d', @Severity, 1, @Error_No)      
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
    ON OBJECT::[dbo].[DeleteOrderItem] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[DeleteOrderItem] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[DeleteOrderItem] TO [IRMAReportsRole]
    AS [dbo];

