CREATE PROCEDURE [dbo].[WFP_OrderImport]
	@InvoiceNumber							VARCHAR(20),
	@CustomerCode							VARCHAR(20),
	@ItemNumber								VARCHAR(20),
	@Price									VARCHAR(25),
	@Quantity								VARCHAR(25),
	@ArrivalDate							VARCHAR(25),
	@OrderDate								VARCHAR(25),
	@PackSize								VARCHAR(20),
	@SequenceNumber 						VARCHAR(20),
	@SubTeamNumber							VARCHAR(20),
	@Weight			 						VARCHAR(20)
AS
/*********************************************************************************************
CHANGE LOG
DEV		DATE	TASK	Description
----------------------------------------------------------------------------------------------
BSR		091709	11130	

***********************************************************************************************/
BEGIN
	DECLARE @wfp_vendorId					AS VARCHAR(20)
	DECLARE @customer_vendorId				AS VARCHAR(20)
	DECLARE @NewOrderId						AS INT
	DECLARE @Description					AS VARCHAR(255)
	DECLARE @Item_Key						INT
	DECLARE @SubTeam_No						INT
	DECLARE @CreatedById					INT
	DECLARE @ByWeight						BIT
	DECLARE @OrderItemId					INT
	DECLARE @CostUnit						INT
	DECLARE @PackageUnit					INT
	DECLARE @Customer_Store					INT
	
	DECLARE @log							AS TABLE ( msg  VARCHAR(MAX) );
	
	
	
	-- clear out quotes
	SET @InvoiceNumber =					REPLACE(@InvoiceNumber, '"','')
	SET @CustomerCode =						REPLACE(@CustomerCode, '"','')							
	SET @ItemNumber =						REPLACE(@ItemNumber, '"','')								
	SET @Price =							REPLACE(@Price, '"','')									
	SET @Quantity =							REPLACE(@Quantity, '"','')								
	SET @ArrivalDate =						REPLACE(@ArrivalDate, '"','')							
	SET @OrderDate =						REPLACE(@OrderDate, '"','')								
	SET @PackSize =							REPLACE(@PackSize, '"','')								
	SET @SequenceNumber =					REPLACE(@SequenceNumber, '"','') 						
	SET @SubTeamNumber =					REPLACE(@SubTeamNumber, '"','')							
	SET @Weight  =							REPLACE(@Weight, '"','')		
	
	-- make sure @price has no commas.
	SET @Price =							REPLACE(@price, ',','')
					

	
	SET	@Description =						@InvoiceNumber + ' from WFP for subteam ' + @SubTeamNumber + ' '
	SET	@CreatedById =						(
	   		              					    SELECT u.[User_ID]
	   		              					    FROM   Users u
	   		              					    WHERE  username = 'wfp_import'
	   		              					)
	   		                				
	
	SET @wfp_vendorId =						(
												SELECT v.Vendor_ID
												FROM   vendor v
												WHERE  v.PS_Export_Vendor_ID = '0000045567'
											)
	

	
	
	SET @customer_vendorId =				(
												SELECT v.Vendor_ID
												FROM   store s
													   INNER JOIN vendor v
															ON  s.Store_No = v.Store_no
												WHERE  s.BusinessUnit_ID = @CustomerCode
											)
	
	SET @Customer_Store =					(
												SELECT v.Store_No
												FROM Vendor v
												WHERE v.Vendor_ID = @customer_vendorId
											)
		
	
	SET	@SubTeam_No =						(
	   		                				    SELECT st.SubTeam_No
	   		                				    FROM   SubTeam st
	   		                				    WHERE  st.SubTeam_No IN (
	   		                				                                SELECT 
	   		                				                                       TOP 
	   		                				                                       1 
	   		                				                                       sst.SubTeam_No
	   		                				                                FROM   
	   		                				                                       StoreSubTeam 
	   		                				                                       sst
	   		                				                                       INNER JOIN 
	   		                				                                            Vendor v
	   		                				                                            ON  sst.Store_No = v.Store_no
	   		                				                                WHERE  v.Vendor_ID = @customer_vendorId
	   		                				                                       AND sst.PS_SubTeam_No = RIGHT(@SubTeamNumber, 4)
	   		                											)
	   		                				  )	
	
    
	
	BEGIN TRY
		
		-- check for valid dates
		IF not ISDATE(@ArrivalDate) = 1 
			RAISERROR( 'Arrival Date [ %s ] is not a valid date.', 16,1, @ArrivalDate)
		
		IF NOT ISDATE(@OrderDate) = 1 
			RAISERROR( 'Order Date [ %s ] is not a valid date.', 16,1, @OrderDate)
		
		-- make sure Price is not null
		IF @Price = ''
			RAISERROR( 'Price [ %s ] for item [ %s ] cannot be null.', 16,1, @Price, @ItemNumber)
		
		--make sure Price is positive
		IF cast(@Price AS MONEY) <= 0.0 
			RAISERROR( 'Price [ %s ] for item [ %s ] cannot be less than or equal to zero.', 16,1, @Price, @ItemNumber)
		
		IF cast(@Quantity AS int) < 0 
			RAISERROR( 'Quantity [ %s ] for item [ %s ] cannot be less than zero.', 16,1, @Quantity, @ItemNumber)

		
		IF @wfp_vendorId IS NULL
			RAISERROR ( 'Could not find Vendor record for PS Vendor ID [ 0000045567 ]. This should be the WFP vendor record.', 16,1)		
		
		IF @customer_vendorId IS NULL
			RAISERROR ( 'Could not find Vendor record for PS Vendor ID [%s].',16,1, @CustomerCode) 
			

		IF @SubTeam_No IS NULL
			RAISERROR ( 'Could not find the Subteam associated with this order [%s].',16,1, @SubTeamNumber)
			
			

			
			
		IF @SequenceNumber =	'1'
		BEGIN
			
			/*
			* When SequenceNumber = 1 create the OrderHeader record
			*/
			insert into @log values('[info] Creating OrderHeader record for WFP Order: [' + @InvoiceNumber + ']. ');
		    
		    set @NewOrderId = (SELECT  TOP 1 orderheader_id FROM OrderHeader oh WHERE oh.OrderHeaderDesc LIKE '%'+@Description+'%' ORDER BY oh.OrderDate desc)
		    
		    IF @NewOrderId IS NULL
		    BEGIN
		    
				-- Create OrderHeader
				EXEC InsertOrder2
					 @Vendor_ID =					@wfp_vendorId,
					 @OrderType_ID =				1,
					 @ProductType_ID =				1,
					 @PurchaseLocation_ID =			@customer_vendorId,
					 @ReceiveLocation_ID =			@customer_vendorId,
					 @Transfer_SubTeam =			null,
					 @Transfer_To_SubTeam =			@SubTeam_No,
					 @Fax_Order =					0,
					 @Expected_Date =				@ArrivalDate,
					 @CreatedBy =					@CreatedById,
					 @Return_Order =				0,
					 @NewOrderHeader_ID =			@NewOrderId OUTPUT
					 
					 insert into @log values('[info] Create new order: [' + cast(isnull(@NewOrderId,-99) AS VARCHAR(100))+ ']. ');
			         
				 -- Update OrderHeader Description
					 UPDATE OrderHeader
					 SET	orderheader.OrderHeaderDesc				= @Description,
							orderheader.OrderExternalSourceID		= 4,
							orderheader.OrderExternalSourceOrderID	= @InvoiceNumber				
					 WHERE OrderHeader_ID = @NewOrderId
				 
		    END
		    ELSE
		    BEGIN
		    		UPDATE orderheader 
		    			set Vendor_ID =					@wfp_vendorId,
						OrderType_ID =					1,
						ProductType_ID =				1,
						PurchaseLocation_ID =			@customer_vendorId,
						ReceiveLocation_ID =			@customer_vendorId,
						Transfer_SubTeam =				NULL,
						Transfer_To_SubTeam =			@SubTeam_No,
						Fax_Order =						0,
						Expected_Date =					@ArrivalDate,
						CreatedBy =						@CreatedById,
						Return_Order =					0,
						OrderHeaderDesc =				@Description,
						orderdate =						@OrderDate,
						OrderExternalSourceID =			4,
						OrderExternalSourceOrderID =	@InvoiceNumber				
		    		WHERE OrderHeader_ID = @NewOrderId
		    		insert into @log values('[info] Update existing order: [' + cast(isnull(@NewOrderId,-99) AS VARCHAR(100))+ ']. ');

					

		    END	
		    

		END
		
		IF @PackSize = ''
			RAISERROR ( 'Packsize for this item (%s) cannot be NULL ', 16,1, @ItemNumber)
			
		IF @Weight = ''
			RAISERROR ( 'Weight for this item (%s) cannot be NULL ', 16,1, @ItemNumber)				
		
		-- get OrderHeaderId
		IF @NewOrderId IS NULL
		SET @NewOrderId =						(
													SELECT top 1 oh.OrderHeader_ID
													FROM   OrderHeader oh
													WHERE  oh.OrderHeaderDesc LIKE @Description + '%' ORDER BY OrderDate DESC 
												)
		
		-- get Item_Key
		set @item_key  =						(
													 SELECT item_key
													 FROM   
													        ItemIdentifier 
													        ii
													 WHERE  ii.Identifier = @ItemNumber
													        AND ii.Deleted_Identifier = 0
												)
		
		
		DECLARE @ItemComments VARCHAR(255)
		SET @ItemComments = '[info] (Item, Packsize) from WFP: (' + @ItemNumber + ','  + @PackSize + ')' 


		IF @NewOrderId IS NULL
			RAISERROR ( 'Could not find the new OrderHeader Id for this order: [%s]. ', 16,1, @Description)
		
		IF @Item_Key IS NULL
			RAISERROR ( 'Could not find the Item Identifier and associated Item_Key for this item: [%s]. ', 16,1, @ItemNumber)
			
		

		
			
		IF NOT EXISTS (
		       SELECT vch.Package_Desc1
		       FROM   VendorCostHistory vch
		       WHERE  vch.StoreItemVendorID IN (SELECT siv.StoreItemVendorID
		                                        FROM   StoreItemVendor siv
		                                        WHERE  siv.Vendor_ID = @wfp_vendorId
		                                               AND siv.Item_Key = 
		                                                   @Item_Key)
		              AND vch.Package_Desc1 = @PackSize
		)
		RAISERROR ( 'Packsize (%s) for this item (%s) does not exist in IRMA. ', 16,1, @PackSize, @ItemNumber)
		
				
		
		-- check for cost and package unit. they are required.
		SET @CostUnit= (SELECT i.Cost_Unit_ID
		                 FROM item  i WHERE Item_Key = @Item_Key)
		          
		SET @PackageUnit= (SELECT i.Package_Unit_ID
		                 FROM item  i WHERE Item_Key = @Item_Key)
		                 
		IF @CostUnit IS NULL	
			RAISERROR ( 'Item.Cost_Unit_Id cannot be null for item %d', 16,1, @Item_Key)
			
		IF @PackageUnit IS NULL 
			RAISERROR ( 'Item.Package_Unit_Id cannot be null for item %d', 16,1, @Item_Key)


		
			
				
		SELECT @ByWeight =  iu.Weight_Unit 
		FROM item i
		INNER JOIN itemunit iu ON iu.Unit_ID = i.Retail_Unit_ID 
		WHERE item_key = @Item_Key
		
		SET @OrderItemId = (
		        SELECT TOP 1 oi.OrderItem_ID
		        FROM   orderitem oi
		        WHERE  oi.OrderHeader_ID = @NewOrderId
		               AND oi.Item_Key = @Item_Key
		               AND oi.QuantityOrdered = @Quantity
		               AND oi.Cost = @Price
		               AND oi.Package_Desc1 = CASE 
		                                           WHEN @ByWeight = 1 THEN @Weight
		                                           ELSE @PackSize
		                                      END 
		)
		
		

			
				
		IF @OrderItemId IS NULL
		BEGIN
			insert into @log values('[info] Inserting new orderitem for : [' + cast(isnull(@Item_Key,-99) AS VARCHAR(100))+ ']. ');
			INSERT INTO OrderItem 
				(OrderHeader_ID,
				Item_Key,
				Comments,
				Units_Per_Pallet,
				QuantityUnit,
				QuantityOrdered,
				Cost,
				CostUnit,
				Handling,
				HandlingUnit,
				Freight,
				FreightUnit,
				AdjustedCost,
				QuantityDiscount,
				DiscountType,
				LandedCost,
				LineItemCost,
				LineItemFreight,
				LineItemHandling,
				UnitCost,
				UnitExtCost,
				Package_Desc1,
				Package_Desc2,
				Package_Unit_ID,
				MarkupPercent,
				MarkupCost,
				Retail_Unit_ID,
				Origin_ID,
				CountryProc_ID,
				NetVendorItemDiscount)
			SELECT top 1
				oh.OrderHeader_Id					as OrderHeader_ID,
				@Item_Key							as Item_Key,
			   substring(@ItemComments,1,255)		as Comments,
			   0									as Units_Per_Pallet,
			   i.Cost_Unit_ID						as QuantityUnit,		-- *
			   @Quantity							as QuantityOrdered,
			   @Price								as Cost,
			   i.Cost_Unit_ID						as CostUnit,			-- *
			   0									as Handling,
			   i.Cost_Unit_ID						as HandlingUnit,		-- *
			   0									as Freight,			
			   i.Freight_Unit_ID					as FreightUnit,			-- *
			   @Price								as AdjustedCost,
			   0									as QuantityDiscount,
			   0									as DiscountType,
			   0									as LandedCost,
			   (CAST(ISNULL(@Price,0) AS money) * 
				CAST(ISNULL(@Quantity,0) AS INT))	as LineItemCost,
			   0									as LineItemFreight,
			   0									as LineItemHandling,
			   @Price                          		as UnitCost,
			   @Price                          		as UnitExtCost,
			   CASE WHEN @ByWeight = 1 THEN 
	   			@Weight ELSE @PackSize END			as Package_Desc1,		-- *
			   i.Package_Desc2                 		as Package_Desc2,
			   i.Package_Unit_ID               		as Package_Unit_ID,
			   0                               		as MarkupPercent,
			   0                               		as MarkupCost,
			   i.Retail_Unit_ID                		as Retail_Unit_ID,
			   i.Origin_ID                     		as Origin_ID,
			   i.CountryProc_ID                		as CountryProc_ID,
			   0							   		as NetVendorItemDiscount
		   FROM
			  OrderHeader oh (nolock)

			  JOIN Item i (nolock)
				 on ( i.Item_Key = @Item_Key )
		   WHERE
			  oh.OrderHeader_Id = @NewOrderId;

		END
		ELSE
		BEGIN
			insert into @log values('[info] Updating existing orderitem for: [' + cast(isnull(@Item_Key,-99) AS VARCHAR(100))+ ']. ');
			UPDATE orderitem
				set Item_Key = p1.item_key,
				Comments = p1.comments,
				Units_Per_Pallet = p1.Units_Per_Pallet,
				QuantityUnit  = p1.QuantityUnit,
				QuantityOrdered = p1.QuantityOrdered,
				Cost= p1.Cost,
				CostUnit=p1.CostUnit,
				Handling=p1.handling,
				HandlingUnit  = p1.HandlingUnit,
				Freight  = p1.Freight,
				FreightUnit  = p1.FreightUnit,
				AdjustedCost  = p1.AdjustedCost,
				QuantityDiscount  = p1.QuantityDiscount,
				DiscountType  = p1.DiscountType,
				LandedCost  = p1.LandedCost,
				LineItemCost  = p1.LineItemCost,
				LineItemFreight  = p1.LineItemFreight,
				LineItemHandling  = p1.LineItemHandling,
				UnitCost  = p1.UnitCost,
				UnitExtCost  = p1.UnitExtCost,
				Package_Desc1  = p1.Package_Desc1,
				Package_Desc2  = p1.Package_Desc2,
				Package_Unit_ID  = p1.Package_Unit_ID,
				MarkupPercent  = p1.MarkupPercent,
				MarkupCost  = p1.MarkupCost,
				Retail_Unit_ID  = p1.Retail_Unit_ID,
				Origin_ID  = p1.Origin_ID,
				CountryProc_ID  = p1.CountryProc_ID,
				NetVendorItemDiscount  = p1.NetVendorItemDiscount
			FROM (
					SELECT top 1
						oh.OrderHeader_Id					as OrderHeader_ID,
						@Item_Key							as Item_Key,
						substring(@ItemComments,1,255)		as Comments,
						0									as Units_Per_Pallet,
						i.Cost_Unit_ID						as QuantityUnit,		-- *
						@Quantity							as QuantityOrdered,
						@Price								as Cost,
						i.Cost_Unit_ID						as CostUnit,			-- *
						0									as Handling,
						i.Cost_Unit_ID						as HandlingUnit,		-- *
						0									as Freight,			
						i.Freight_Unit_ID					as FreightUnit,			-- *
						@Price								as AdjustedCost,
						0									as QuantityDiscount,
						0									as DiscountType,
						0									as LandedCost,
						(CAST(ISNULL(@Price,0) AS money) * 
						CAST(ISNULL(@Quantity,0) AS INT))	as LineItemCost,
						0									as LineItemFreight,
						0									as LineItemHandling,
						@Price                          	as UnitCost,
						@Price                          	as UnitExtCost,
						CASE WHEN @ByWeight = 1 THEN 
						@Weight ELSE @PackSize END			as Package_Desc1,		-- *
						i.Package_Desc2                 	as Package_Desc2,
						i.Package_Unit_ID               	as Package_Unit_ID,
						0                               	as MarkupPercent,
						0                               	as MarkupCost,
						i.Retail_Unit_ID                	as Retail_Unit_ID,
						i.Origin_ID                     	as Origin_ID,
						i.CountryProc_ID                	as CountryProc_ID,
						0							   		as NetVendorItemDiscount
					FROM
						OrderHeader oh (nolock)
							JOIN Item i (nolock) on ( i.Item_Key = @Item_Key )
					WHERE
					  oh.OrderHeader_Id = @NewOrderId
			)  p1
			WHERE OrderItem_ID = @OrderItemId AND orderitem.OrderHeader_ID = @NewOrderId
		END
			
	END TRY
	BEGIN CATCH
		insert into @log values ('[error] ' +  error_message());
	END CATCH 
	

	 select msg from @log;
	 		
END
GRANT EXECUTE ON [dbo].[WFP_OrderImport] TO [IRMAAdminRole]
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[WFP_OrderImport] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[WFP_OrderImport] TO [IRMASchedJobsRole]
    AS [dbo];

