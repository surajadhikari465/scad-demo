CREATE PROCEDURE [dbo].[UpdateOrdersApplyNewVendorCost] 
AS
/*********************************************************************************************
CHANGE LOG
DEV		DATE	TASK	Description
----------------------------------------------------------------------------------------------
BSR		070510	12793	Removed AND U.PayByAgreedCost = 1 so all orders will now be affected.
DN		080911	1638	Only OrderType_ID = 1, PO that is not closed, and non Vendor Payment
						vendor's PO will be re-costed.
MZ      121211  3748    Removed the update to the VendorCostDiscrepanciesAdmin_Reviewed column 
                        on the VendorCostHistory table. This column is removed in 4.4.
BS		101012	8133	Added 'ApplyNewVendorCost' to call to UpdateOrderRefreshCost in order
						to track from where UORC is called
***********************************************************************************************/
BEGIN
    SET NOCOUNT ON 
	
    BEGIN TRY                                     
		 		
		CREATE TABLE #SIV (StoreItemVendorID int,  LastCostAddedDate datetime PRIMARY KEY (StoreItemVendorID))
		
        INSERT INTO #SIV
			SELECT 
				StoreItemVendorID, LastCostAddedDate
			FROM 
				StoreItemVendor (nolock)
			WHERE 
				LastCostAddedDate > LastCostRefreshedDate AND DeleteDate IS NULL
			
		CREATE TABLE #SV (Vendor_ID int, Store_No int, Item_Key int, PRIMARY KEY(Vendor_ID, Store_No, Item_Key))
		
		INSERT INTO #SV
			SELECT 
				SIV.Vendor_ID, SIV.Store_No, SIV.Item_Key
			FROM 
				#SIV S
				INNER JOIN StoreItemVendor   SIV (nolock) ON SIV.StoreItemVendorID = S.StoreItemVendorID
				INNER JOIN VendorCostHistory VCH (nolock) ON VCH.StoreItemVendorID = S.StoreItemVendorID
			WHERE 
				VCH.InsertDate >= SIV.LastCostRefreshedDate
			GROUP BY 
				Vendor_ID, Store_No, Item_Key						
		
		-- Insert records where there is no vendor cost
		INSERT INTO #SV
			SELECT 
				SIV.Vendor_ID, SIV.Store_No, SIV.Item_Key
			FROM 
				#SIV S
				INNER JOIN StoreItemVendor	SIV	(nolock) ON SIV.StoreItemVendorID	= S.StoreItemVendorID
				INNER JOIN Vendor			V	(nolock) ON V.Vendor_ID		= SIV.Vendor_ID
			WHERE 
				NOT EXISTS (SELECT * FROM #SV SV WHERE SV.Vendor_ID = SIV.Vendor_ID AND SV.Store_No = SIV.Store_No AND SV.Item_Key = SIV.Item_Key)				
			
        DECLARE @UpdateList TABLE (OrderHeader_ID int PRIMARY KEY, PayByAgreedCost bit)
		
        INSERT INTO @UpdateList
			SELECT 
				OH.OrderHeader_ID, OH.PayByAgreedCost
			FROM 
				#SV S
				INNER JOIN OrderHeader OH		 (nolock) ON OH.Vendor_ID		= S.Vendor_ID
				INNER JOIN Vendor RV			 (nolock) ON RV.Vendor_ID		= OH.ReceiveLocation_ID AND RV.Store_No = S.Store_No
				INNER JOIN Vendor V				 (nolock) ON V.Vendor_ID		= OH.Vendor_ID
				INNER JOIN OrderItem OI			 (nolock) ON OI.OrderHeader_ID	= OH.OrderHeader_ID AND OI.Item_Key = S.Item_Key
				LEFT JOIN OrderExternalSource ES (nolock) ON ES.ID				= OH.OrderExternalSourceID
			WHERE
				OH.OrderType_ID = 1
				AND (V.Customer = 0 AND V.InternalCustomer = 0) -- external vendor
				AND ISNULL(ES.Description, '') <> 'OrderLink'	-- exclude OrderLink POs
				AND OH.UploadedDate IS NULL
				AND OH.Return_Order = 0
				AND OH.RefuseReceivingReasonID IS NULL
			GROUP BY 
				OH.OrderHeader_ID, OH.PayByAgreedCost
		
	   	INSERT INTO OrderHeaderApplyNewVendorCostQueue (OrderHeader_ID)
			SELECT 
				U.OrderHeader_ID
			FROM  
				@UpdateList U
				LEFT JOIN OrderHeaderApplyNewVendorCostQueue Q ON Q.OrderHeader_ID = U.OrderHeader_ID
			WHERE 
				Q.OrderHeader_ID IS NULL 
		
        UPDATE SIV 
		SET 
			LastCostRefreshedDate = S.LastCostAddedDate
		FROM 
			StoreItemVendor SIV
			INNER JOIN #SIV S ON S.StoreItemVendorID = SIV.StoreItemVendorID
		WHERE 
			S.LastCostAddedDate = SIV.LastCostAddedDate
		
        DROP TABLE #SIV
        DROP TABLE #SV
		
        CREATE TABLE #Orders (OrderHeader_ID int PRIMARY KEY)
		
		DECLARE @OHID int
		DECLARE @CloseDate datetime
		
		INSERT INTO #Orders
			SELECT 
				OrderHeader_ID 
			FROM 
				OrderHeaderApplyNewVendorCostQueue 
			ORDER BY OrderHeader_ID
		
		SET @OHID = (SELECT TOP 1 OrderHeader_ID FROM #Orders)

		WHILE @OHID IS NOT NULL
			BEGIN
				-- Lock if not already
				UPDATE OrderHeader
				SET User_ID = 0
				WHERE OrderHeader_ID = @OHID AND NULLIF(User_ID, 0) IS NULL
				
				-- If we locked the order, refresh the costs, unlock it and delete from the queue
				IF @@ROWCOUNT = 1
					BEGIN
						SELECT @CloseDate = CloseDate FROM OrderHeader WHERE OrderHeader_ID = @OHID

						IF @CloseDate IS NULL
							EXEC UpdateOrderRefreshCosts @OHID, 'ApplyNewVendorCost'
						ELSE
							EXEC UpdateOrderClosed @OHID, 0
				
						UPDATE OrderHeader
						SET User_ID = NULL
						WHERE OrderHeader_ID = @OHID   
				
						DELETE OrderHeaderApplyNewVendorCostQueue WHERE OrderHeader_ID = @OHID
					END
			
				DELETE #Orders WHERE OrderHeader_ID = @OHID
			
				SET @OHID = (SELECT TOP 1 OrderHeader_ID FROM #Orders)
			END
		
		DROP TABLE #Orders
		
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0
            ROLLBACK TRAN
			
        DECLARE @err_no int, @err_sev int
        SELECT @err_no = ERROR_NUMBER(), @err_sev = ERROR_SEVERITY()
        RAISERROR ('UpdateOrdersApplyNewVendorCost failed with @@ERROR: %d', @err_sev, 1, @err_no)
    END CATCH
	
    SET NOCOUNT OFF    
END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[UpdateOrdersApplyNewVendorCost] TO [IRMAAdminRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[UpdateOrdersApplyNewVendorCost] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[UpdateOrdersApplyNewVendorCost] TO [IRMASchedJobsRole]
    AS [dbo];

