CREATE PROCEDURE [dbo].[EInvoicing_UpdAllocLineItemCharge]
	@OrderHeader_Id INT
AS
BEGIN
	-- 20080815 - DaveStacey - This query adds the allocated and line item eInvoice-related cost to the items in a closed PO  
	-- 20100701 - Alex Z - Removed all references to average cost update or insert into average cost queue - initially added for DC functionality - not used anymore
	----------------------------------------------
	-- Use TRY...CATCH for error handling
	----------------------------------------------  
	BEGIN TRY
		----------------------------------------------
		-- Wrap the updates in a transaction
		----------------------------------------------  
		BEGIN TRANSACTION  
		DECLARE @CodeLocation VARCHAR(50)  
		BEGIN
			DECLARE @AllocationValue  SMALLMONEY,
			        @LineItemValue    SMALLMONEY
			
			SELECT @AllocationValue = 0,
			       @LineItemValue = 0 
			-- Get ID for non-allocated SACTypes  
			DECLARE @AllocatedSACType_Id  AS INT,
			        @LineItemSACType_Id   AS INT  
			
			SELECT TOP 1 @AllocatedSACType_Id = SACType_Id
			FROM   dbo.einvoicing_sactypes (NOLOCK)
			WHERE  LTRIM(RTRIM(SACType)) = 'Allocated'     
			
			SELECT TOP 1 @LineItemSACType_Id = SACType_Id
			FROM   dbo.einvoicing_sactypes (NOLOCK)
			WHERE  LTRIM(RTRIM(SACType)) = 'Line Item'     
			
			DECLARE @SACChargeExists  BIT,
			        @SACInvoiceTotal  SMALLMONEY
			
			SELECT @SACInvoiceTotal = SUM(OIC.Value)
			FROM   dbo.OrderInvoice OI
			       JOIN dbo.OrderInvoiceCharges OIC
			            ON  OIC.OrderHeader_ID = OI.OrderHeader_ID
			       JOIN dbo.OrderItem OIT
			            ON  OIC.OrderHeader_ID = OIT.OrderHeader_ID
			WHERE  SACType_ID IN (@AllocatedSACType_Id, @LineItemSACType_Id)
			       AND OIC.OrderHeader_ID = @OrderHeader_ID
			       AND OIT.SACCost > 0
			
			IF @SACInvoiceTotal > 0
			    --Update Invoice Cost amount w/sum of allocations and line item charges  
			    UPDATE OrderInvoice 
			    SET    OrderInvoice.InvoiceCost = OrderInvoice.InvoiceCost - @SACInvoiceTotal
			    FROM   dbo.OrderInvoice 
			           JOIN dbo.OrderInvoiceCharges OIC
			                ON  OIC.OrderHeader_ID = OrderInvoice.OrderHeader_ID
			           JOIN dbo.OrderItem OIT
			                ON  OIC.OrderHeader_ID = OIT.OrderHeader_ID
			    WHERE  SACType_ID IN (@AllocatedSACType_Id, @LineItemSACType_Id)
			           AND OIC.OrderHeader_ID = @OrderHeader_ID
			           AND OIT.SACCost > 0  
			
			DECLARE @AdjCost INT  
			SELECT @AdjCost = 0  
			SELECT @AdjCost = 1
			FROM   dbo.OrderItem OI
			WHERE  OI.orderheader_id = @OrderHeader_Id
			       AND OI.AdjustedCost > 0
			
			IF @AdjCost = 0
			BEGIN
			    SELECT @CodeLocation = 'Remove Previous Cost allocation...'  
			    UPDATE dbo.OrderItem
			    SET    Cost = Cost - SACCost,
			           MarkupCost = MarkupCost - SACCost,
			           LineItemCost = LineItemCost - ROUND((SACCost / QuantityReceived), 2),
			           UnitCost = UnitCost - ROUND((SACCost / QuantityReceived), 2),
			           UnitExtCost = UnitExtCost - ROUND((SACCost / QuantityReceived), 2)
			    FROM   dbo.OrderItem OI(NOLOCK)
			    WHERE  OI.orderheader_id = @OrderHeader_Id
			           AND OI.saccost > 0
			END
			ELSE 
			IF @AdjCost = 1
			BEGIN
			    SELECT @CodeLocation = 'Remove Previous Adj Cost allocation...'  
			    UPDATE dbo.OrderItem
			    SET    AdjustedCost = AdjustedCost - SACCost,
			           MarkupCost = MarkupCost - SACCost,
			           LineItemCost = LineItemCost - ROUND((SACCost / QuantityReceived), 2),
			           UnitCost = UnitCost - ROUND((SACCost / QuantityReceived), 2),
			           UnitExtCost = UnitExtCost - ROUND((SACCost / QuantityReceived), 2)
			    FROM   dbo.OrderItem OI(NOLOCK)
			    WHERE  OI.orderheader_id = @OrderHeader_Id
			           AND OI.saccost > 0
			END  
			
			SELECT @CodeLocation = 'Reset Allocated Cost to 0...'  
			UPDATE dbo.OrderItem
			SET    SACCost = 0
			WHERE  OrderHeader_ID = @OrderHeader_ID
			
			SELECT @SACChargeExists = CASE 
			                               WHEN (
			                                        SELECT COUNT(1)
			                                        FROM   dbo.OrderInvoiceCharges 
			                                               OIC
			                                        WHERE  OIC.OrderHeader_ID = 
			                                               @OrderHeader_ID
			                                               AND OIC.SACType_ID = 
			                                                   @AllocatedSACType_Id
			                                    ) > 0 THEN 1
			                               ELSE 0
			                          END  
			
			
			IF @SACChargeExists = 1
			BEGIN
			    SELECT @CodeLocation = 'Update Allocated Costs...'  
			    UPDATE dbo.ORDERITEM
			    SET    SACCost = ROUND(
			               oic.value 
			               * (
			                   SELECT CASE 
			                               WHEN adjustedcost > 0 THEN 
			                                    adjustedcost
			                               ELSE cost
			                          END
			               ) / (
			                   SELECT CASE 
			                               WHEN SUM(adjustedcost) > 0 THEN SUM(adjustedcost)
			                               ELSE SUM(cost)
			                          END
			                   FROM   dbo.orderitem
			                   WHERE  orderheader_id = @OrderHeader_Id
			                   GROUP BY
			                          ORDERHEADER_ID
			               ),
			               2
			           )
			    FROM   dbo.OrderItem OI(NOLOCK)
			           JOIN dbo.OrderHeader OH(NOLOCK)
			                ON  OI.OrderHeader_ID = OH.OrderHeader_ID
			           JOIN dbo.OrderInvoiceCharges OIC
			                ON  OIC.OrderHeader_ID = OH.OrderHeader_ID
			    WHERE  OH.CloseDate IS NOT NULL
			           AND OI.orderheader_id = @OrderHeader_Id
			           AND OIC.SACType_ID = @AllocatedSACType_Id
			END  
			
			
			SELECT @SACChargeExists = CASE 
			                               WHEN (
			                                        SELECT COUNT(1)
			                                        FROM   dbo.OrderInvoiceCharges 
			                                               OIC
			                                        WHERE  OIC.OrderHeader_ID = 
			                                               @OrderHeader_ID
			                                               AND OIC.SACType_ID = 
			                                                   @LineItemSACType_Id
			                                    ) > 0 THEN 1
			                               ELSE 0
			                          END  
			
			IF @SACChargeExists = 1
			BEGIN
			    SELECT @CodeLocation = 'Update Line Item Costs...'  
			    
			    UPDATE dbo.OrderItem
			    SET    SACCost = ROUND(SACCost + (oic.value * oi.quantityreceived), 2)
			    FROM   dbo.OrderItem OI(NOLOCK)
			           JOIN dbo.OrderHeader OH(NOLOCK)
			                ON  OI.OrderHeader_ID = OH.OrderHeader_ID
			           JOIN dbo.OrderInvoiceCharges OIC
			                ON  OIC.OrderHeader_ID = OH.OrderHeader_ID
			                AND OI.OrderItem_ID = OIC.OrderItem_ID
			    WHERE  OH.CloseDate IS NOT NULL
			           AND OI.orderheader_id = @OrderHeader_Id
			           AND OIC.SACType_ID = @LineItemSACType_Id
			END  
			
			SELECT @CodeLocation = 'Update Line Item Costs...'  
			IF @AdjCost = 0
			BEGIN
			    UPDATE dbo.OrderItem
			    SET    Cost = Cost + SACCost,
			           MarkupCost = MarkupCost + SACCost,
			           LineItemCost = LineItemCost + ROUND((SACCost / QuantityReceived), 2),
			           UnitCost = UnitCost + ROUND((SACCost / QuantityReceived), 2),
			           UnitExtCost = UnitExtCost + ROUND((SACCost / QuantityReceived), 2)
			    FROM   dbo.OrderItem OI(NOLOCK)
			    WHERE  OI.orderheader_id = @OrderHeader_Id
			END
			ELSE 
			IF @AdjCost = 1
			BEGIN
			    UPDATE dbo.OrderItem
			    SET    AdjustedCost = AdjustedCost + SACCost,
			           MarkupCost = MarkupCost + SACCost,
			           LineItemCost = LineItemCost + ROUND((SACCost / QuantityReceived), 2),
			           UnitCost = UnitCost + ROUND((SACCost / QuantityReceived), 2),
			           UnitExtCost = UnitExtCost + ROUND((SACCost / QuantityReceived), 2)
			    FROM   dbo.OrderItem OI(NOLOCK)
			    WHERE  OI.orderheader_id = @OrderHeader_Id
			END 
			--Update Invoice Cost amount w/sum of allocations and line item charges  
			UPDATE OrderInvoice
			SET    InvoiceCost = InvoiceCost + ISNULL(@SACInvoiceTotal, 0)
			FROM   OrderInvoice OI
			       JOIN OrderInvoiceCharges OIC
			            ON  OIC.OrderHeader_ID = OI.OrderHeader_ID
			WHERE  SACType_ID IN (@AllocatedSACType_Id, @LineItemSACType_Id)
			       AND OIC.OrderHeader_ID = @OrderHeader_ID  
			
			DECLARE @StoreNo INT  
			
			SELECT TOP 1 @StoreNo = V.Store_No
			FROM   dbo.OrderHeader OH
			       JOIN dbo.OrderItem OI
			            ON  OI.OrderHeader_ID = OH.OrderHeader_ID
			       JOIN dbo.Vendor V
			            ON  V.Vendor_ID = OH.ReceiveLocation_ID
			WHERE  OH.OrderHeader_ID = @OrderHeader_ID
			
			
		END 
		----------------------------------------------
		-- Commit the transaction
		----------------------------------------------  
		IF @@TRANCOUNT > 0
		    COMMIT TRANSACTION
	END TRY 
	--===============================================================================================  
	BEGIN CATCH
		----------------------------------------------
		-- Rollback the transaction
		----------------------------------------------  
		IF @@TRANCOUNT > 0
		    ROLLBACK TRANSACTION 
		
		----------------------------------------------
		-- Display a detailed error message          ----------------------------------------------  
		PRINT REPLACE(SPACE(120), SPACE(1), '-') + CHAR(13) + CHAR(10) 
		+ 'Error ' + CONVERT(VARCHAR, ERROR_NUMBER()) + ': ' + ERROR_MESSAGE() + CHAR(13) 
		+ CHAR(10) 
		+ CHAR(9) + ' at statement  ''' + @CodeLocation + ''' (' + ISNULL(ERROR_PROCEDURE() + ', ', '') 
		+ 'line ' + CONVERT(VARCHAR, ERROR_LINE()) + ')' + CHAR(13) + CHAR(10) 
		+ REPLACE(SPACE(120), SPACE(1), '-') + CHAR(13) + CHAR(10) 
		+ 'Database changes were rolled back.' + CHAR(13) + CHAR(10) 
		+ REPLACE(SPACE(120), SPACE(1), '-')  
		
		SELECT ERROR_NUMBER() AS ErrorNumber,
		       ERROR_SEVERITY() AS ErrorSeverity,
		       ERROR_STATE() AS ErrorState,
		       ERROR_PROCEDURE() AS ErrorProcedure,
		       ERROR_LINE() AS ErrorLine,
		       ERROR_MESSAGE() AS ErrorMessage
	END CATCH 
	--===============================================================================================
	--
END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[EInvoicing_UpdAllocLineItemCharge] TO [IRMAAdminRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[EInvoicing_UpdAllocLineItemCharge] TO [IRMAClientRole]
    AS [dbo];

