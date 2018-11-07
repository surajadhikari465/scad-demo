CREATE PROCEDURE [dbo].[EIM_UploadSessionCost]
	@UploadSession_ID int,
	@UploadRow_ID int,
	@RetryCount int,
	@Item_Key int,
	@UploadToItemsStore bit,
	@LoggingLevel varchar(10)

AS

--**********************************************************************************************
-- Procedure: EIM_UploadSessionCost()
--
-- Description:
-- This procedure is called by EIM_UploadSession
--
-- Change History:
-- Date			Init.	TFS		Description
-- 2013/05/21	MZ		8531	When setting the primary vendor flag to true, also remove the values  
--								in DeleteDate and DeleteWorkStation on the ItemVendor record.
--***********************************************************************************************

	SET NOCOUNT ON

	DECLARE
		@TableName varchar(50),
		@ColumnName varchar(50),
		@ColumnValue varchar(200),
		@UploadValue_ID int, 
		@Cost varchar(200),
		@Store_No int,
		@StoreName varchar(50),
		@Vendor_ID varchar(200),
		@Promotional varchar(200),
		@CostEndDate varchar(200),
		@CostStartDate varchar(200),
		@CaseQty decimal(9,4),
		@Pack varchar(200),
		@CostUnit varchar(200),
		@FreightUnit varchar(200),
		@CurrentDate Datetime,
		@Freight varchar(200),
		@PrimaryVendor bit,
		@Item_ID varchar(20),
		@IsCostChange bit,
		@IsDealChange bit,
		@Allowance decimal(9,4),
		@AllowanceStartDate smalldatetime,
		@AllowanceEndDate smalldatetime,
		@Discount smallmoney,
		@DiscountStartDate smalldatetime,
		@DiscountEndDate smalldatetime,
		
		-- This is used to indicate whether the item id attribute
		-- has been uploaded by the user.
		-- If not, the corresponding value is loaded from
		-- the database for a given item and store.
		-- You can see below how the flag is set to true
		-- if the attribute is in the uploaded data.
		@HasAttribute_Item_ID bit,

		-- This holds the current db value of the item id
		-- for a given item and store
		-- and is used when the item id attribute is not in the uploaded data.
		@FromDB_Item_ID varchar(20),
		
		-- This holds the actual item id value being updated or inserted
		-- into the database and is either set to the value of the
		-- attribute if uploaded or that from the database if not.
		@ToUpload_Item_ID varchar(20),

		-- 20100402 - Dave Stacey - TFS 12316 - Handle UK date format
		@DisplayFormatString varchar(50),
		
		-- 2011-01-12 Victoria: delete item vendor association
		@IsDeleteVendor bit,
		@IsDeauthStore bit,
		@DeleteDate smalldatetime,
		@PrimVendCanSwap int,
		@AvailPrimVend int,
		@StoreNo int,
		@IsPrimaryVendor int,

		@IgnoreCasePack bit,
		@RetailCasePack varchar(200) -- should be decimal, but using varchar to match already used similar fields. eim is weird. 
		
		Declare @tmp_PrimVendCanSwap TABLE (PrimVendCanSwap int)
		Declare @tmp_AvailPrimVend TABLE (AvailPrimVend int, CompanyName varchar(50), ItmCnt int, Store_Name varchar(50), Identifier varchar (13) )
		
		Select @DeleteDate = convert(smalldatetime,convert(varchar, getdate(), 111))
		Select @PrimVendCanSwap = 0
		Select @AvailPrimVend = 0
		Select @StoreNo = 0
		Select @IsPrimaryVendor = 0

	-- initialize the flag to false
	SET @HasAttribute_Item_ID = 0

	EXEC dbo.EIM_Log @LoggingLevel, 'TRACE', @UploadSession_ID, @UploadRow_ID, @RetryCount, @Item_key, NULL, '4.0 Cost Change - [Begin]'

	-- set defaults
	select @Promotional = '0'
	
	EXEC dbo.EIM_Log @LoggingLevel, 'TRACE', @UploadSession_ID, @UploadRow_ID, @RetryCount, @Item_key, NULL, '4.0.2 Cost Change - [Preload Existing Data]'

	--20100406 - Dave Stacey - TFS 12316 - include Display Format String in cursor to allow reformatting date for US to allow date functions to operate

	DECLARE RowValues_cursor CURSOR FOR
	SELECT uv.UploadValue_ID, uv.Value As ColumnValue, LOWER(ua.TableName) As TableName, LOWER(ua.ColumnNameorKey) As ColumnName, UA.DisplayFormatString
	FROM UploadValue (NOLOCK) uv
		inner join UploadAttribute (NOLOCK) ua
		on uv.UploadAttribute_ID = ua.UploadAttribute_ID
		inner join UploadTypeAttribute (NOLOCK) uta
		on ua.UploadAttribute_ID = uta.UploadAttribute_ID
	WHERE uv.UploadRow_ID = @UploadRow_ID

	OPEN RowValues_cursor
	FETCH NEXT FROM RowValues_cursor INTO @UploadValue_ID, @ColumnValue, @TableName, @ColumnName, @DisplayFormatString
	--20100406 - Dave Stacey - TFS 12316 - Compare Display Format String w/in cursor to allow reformatting date for US to allow date functions to operate

		select @ColumnValue = CASE WHEN (@DisplayFormatString = 'dd/MM/yyyy' AND LEN(@ColumnValue) > 0)
			THEN  substring(@ColumnValue, 4, 3) + substring(@ColumnValue, 1, 2)+ Right(@ColumnValue, Len(@ColumnValue) - 5)
			ELSE @ColumnValue 
			END

	-- load UploadRow's values
	WHILE @@FETCH_STATUS = 0
	BEGIN

		IF @TableName = 'vendorcosthistory'
		BEGIN
			IF @ColumnName = 'unitcost'
			select @Cost = @ColumnValue
			ELSE
			IF @ColumnName = 'unitfreight'
			select @Freight = @ColumnValue
			ELSE
			IF @ColumnName = 'enddate'
			select @CostEndDate = dbo.fn_GetDateOnly(CAST(@ColumnValue AS DATETIME))
			ELSE
			IF @ColumnName = 'startdate'
			select @CostStartDate = dbo.fn_GetDateOnly(CAST(@ColumnValue AS DATETIME))
			ELSE
			IF @ColumnName = 'package_desc1'
			select @Pack = @ColumnValue
			ELSE
			IF @ColumnName = 'costunit_id'
			select @CostUnit = @ColumnValue
			ELSE
			IF @ColumnName = 'freightunit_id'
			select @FreightUnit = @ColumnValue
		END
		ELSE IF @TableName = 'storeitemvendor'
		BEGIN
			IF @ColumnName = 'primaryvendor'
				select @PrimaryVendor = @ColumnValue
		END
		ELSE IF @TableName = 'item'
		BEGIN
			IF @ColumnName = 'package_desc2'
				select @CaseQty = @ColumnValue
		END
		ELSE IF @TableName = 'itemvendor'
		BEGIN
			IF @ColumnName = 'vendor_id'
				select @Vendor_ID = @ColumnValue
			ELSE
			IF @ColumnName = 'item_id'
			BEGIN
				select @Item_ID = @ColumnValue
				SET @HasAttribute_Item_ID = 1
			END
			ELSE
			IF @ColumnName = 'ignorecasepack'
			BEGIN
				set @IgnoreCasePack = @ColumnValue
			END
			if @ColumnName = 'retailcasepack'
			BEGIN
				set @RetailCasePack = @ColumnValue
			END
		END
		ELSE IF @TableName = 'calculated'
		BEGIN
			IF @ColumnName = 'iscostchange'
				select @IsCostChange = @ColumnValue
			ELSE
			IF @ColumnName = 'isdealchange'
				select @IsDealChange = @ColumnValue
			ELSE
			IF @ColumnName = 'delete_vendor'
				select @IsDeleteVendor = @ColumnValue
			ELSE
			IF @ColumnName = 'deauth_store'
				select @IsDeauthStore = @ColumnValue				
		END
		ELSE IF @TableName = 'slim_vendordealview'
		BEGIN
			IF @ColumnName = 'allowance'
				select @Allowance = @ColumnValue
			ELSE
			IF @ColumnName = 'allowancestartdate'
				select @AllowanceStartDate = @ColumnValue
			ELSE
			IF @ColumnName = 'allowanceenddate'
				select @AllowanceEndDate = @ColumnValue
			ELSE
			IF @ColumnName = 'discount'
				select @Discount = @ColumnValue
			ELSE
			IF @ColumnName = 'discountstartdate'
				select @DiscountStartDate = @ColumnValue
			ELSE
			IF @ColumnName = 'discountenddate'
				select @DiscountEndDate = @ColumnValue
		END
		ELSE IF @TableName = 'store'
		BEGIN
			IF @ColumnName = 'store_no'
				select @Store_No = @ColumnValue
		END
				
		FETCH NEXT FROM RowValues_cursor INTO @UploadValue_ID, @ColumnValue, @TableName, @ColumnName, @DisplayFormatString
	END

	CLOSE RowValues_cursor
	DEALLOCATE RowValues_cursor
	
	EXEC dbo.EIM_Log @LoggingLevel, 'TRACE', @UploadSession_ID, @UploadRow_ID, @RetryCount, @Item_key, NULL, '4.0.3 Cost Change - [Load Uploaded Data]'

	IF @Vendor_ID IS NOT NULL
	BEGIN
	
		IF @IsDeleteVendor = 1 
		BEGIN
		
		-- For every existing store swap primary and secondary vendors before deleting a primary vendor
				
		Declare StoreNo_cursor CURSOR FOR
		Select store_no from store
		OPEN StoreNo_cursor 
		FETCH NEXT FROM StoreNo_cursor INTO @StoreNo
		WHILE @@FETCH_STATUS = 0
		BEGIN

				insert into @tmp_PrimVendCanSwap
				EXEC dbo.CheckIfPrimVendCanSwap @vendor_id, @item_key, @StoreNo
			
				Select @PrimVendCanSwap = PrimVendCanSwap from @tmp_PrimVendCanSwap
				Delete from @tmp_PrimVendCanSwap 
				
				IF @PrimVendCanSwap = 1
				BEGIN
				
					insert into @tmp_AvailPrimVend
					EXEC dbo.GetAvailPrimVend @vendor_id, @item_key, @StoreNo				        
				
					Select @AvailPrimVend = AvailPrimVend from @tmp_AvailPrimVend
					Delete from @tmp_AvailPrimVend
																
					EXEC dbo.SwitchPrimaryVendor @vendor_id, @AvailPrimVend, @item_key, @StoreNo
									
				END
			

				Select @PrimVendCanSwap = 0
				Select @AvailPrimVend = 0

		FETCH NEXT FROM StoreNo_cursor INTO @StoreNo
		END
		CLOSE StoreNo_cursor
		DEALLOCATE StoreNo_cursor
		
				
		EXEC dbo.DeleteItemVendor @Vendor_ID, @Item_Key, @DeleteDate
		EXEC dbo.EIM_Log @LoggingLevel, 'TRACE', @UploadSession_ID, @UploadRow_ID, @RetryCount, @Item_key, NULL, '4.8 Cost Change - [Delete ItemVendor]'
		
		END
		
		ELSE IF @IsDeleteVendor <>1 
		
		BEGIN
			
		-- Load the stores from the saved selection or use the
		-- store specified in the upload row being processed
		-- according to the the @UploadToItemsStore flag value.
		DECLARE CostUploadStore_cursor CURSOR FOR
			SELECT Store_No
			FROM UploadSessionUploadTypeStore us (NOLOCK)
				inner join UploadSessionUploadType ut (NOLOCK)
				on us.UploadSessionUploadType_ID = ut.UploadSessionUploadType_ID
			WHERE ut.UploadSession_ID = @UploadSession_ID and ut.UploadType_Code = 'COST_UPLOAD'
				AND @UploadToItemsStore = 0
			UNION
			SELECT @Store_No as Store_No
			WHERE @UploadToItemsStore = 1

		OPEN CostUploadStore_cursor

		FETCH NEXT FROM CostUploadStore_cursor INTO @Store_No

		-- Loop through all of the stores being uploaded to.
		WHILE @@FETCH_STATUS = 0
		BEGIN
		
			IF @IsDeauthStore = 1 
			BEGIN
			
				insert into @tmp_PrimVendCanSwap
				EXEC dbo.CheckIfPrimVendCanSwap @vendor_id, @item_key, @store_no
			
				Select @PrimVendCanSwap = PrimVendCanSwap from @tmp_PrimVendCanSwap
				Delete from @tmp_PrimVendCanSwap 
				
				IF @PrimVendCanSwap = 1
				BEGIN
				
					insert into @tmp_AvailPrimVend
					EXEC dbo.GetAvailPrimVend @vendor_id, @item_key, @store_no				        
				
					Select @AvailPrimVend = AvailPrimVend from @tmp_AvailPrimVend
					Delete from @tmp_AvailPrimVend
				
												
					EXEC dbo.SwitchPrimaryVendor @vendor_id, @AvailPrimVend, @item_key, @store_no
									
				END
			
				EXEC dbo.DeleteStoreItemVendor @Vendor_ID, @Store_No, @Item_Key, @DeleteDate
				EXEC dbo.EIM_Log @LoggingLevel, 'TRACE', @UploadSession_ID, @UploadRow_ID, @RetryCount, @Item_key, NULL, '4.0.4 Cost Change - [Delete StoreItemVendor]'
			
				Select @PrimVendCanSwap = 0
				Select @AvailPrimVend = 0
								
			END
		
			ELSE IF @IsDeauthStore <>1 
		
			BEGIN

			-- get the store name
			SELECT @StoreName = Store_Name FROM Store (NOLOCK) WHERE Store_No = @Store_No
			DECLARE @StoreNameMessage varchar(200)
			SELECT @StoreNameMessage = '4.0.4 Cost Grid Upload for Store - [' + @StoreName + ']'
			
			EXEC dbo.EIM_Log @LoggingLevel, 'TRACE', @UploadSession_ID, @UploadRow_ID, @RetryCount, @Item_key, NULL, @StoreNameMessage

			-- Load the item id value from the database
			-- to use when the item id attributes is
			-- not part of the uploaded data.
			SELECT  @FromDB_Item_ID = Item_ID
			  FROM ItemVendor (NOLOCK)
			  WHERE
				Item_Key = @Item_Key
				AND Vendor_ID = @Vendor_ID

			-- use the values from the database if there is no corresponding uploaded attribute
			SET @ToUpload_Item_ID = CASE WHEN @HasAttribute_Item_ID = 1 THEN @Item_ID ELSE @FromDB_Item_ID END

			-- create a ItemVendor row if there isn't one already
			IF NOT EXISTS(SELECT  1
			  FROM ItemVendor (NOLOCK)
			  WHERE
				Item_Key = @Item_Key
				AND Vendor_ID = @Vendor_ID)
			BEGIN

				BEGIN TRY
				
				  INSERT INTO ItemVendor
				  (
					  Item_Key,
					  Vendor_ID,
					  Item_ID
				  )
				  VALUES
				  (
					  @Item_Key,	
					  @Vendor_ID,
					  @ToUpload_Item_ID
				  )
				
					EXEC dbo.EIM_Log @LoggingLevel, 'TRACE', @UploadSession_ID, @UploadRow_ID, @RetryCount, @Item_key, NULL, '4.1 Cost Change - [Insert ItemVendor]'
				
				END TRY
				BEGIN CATCH

						EXEC dbo.EIM_LogAndRethrowException @UploadSession_ID, @UploadRow_ID, @RetryCount, @Item_key, NULL, '4.1 Cost Change - [Insert ItemVendor]'
				END CATCH

			END
			ELSE
			BEGIN
			
				BEGIN TRY
				
				  -- there is one so update it
					UPDATE	ItemVendor
					SET		Item_ID = @ToUpload_Item_ID,
							DeleteDate = NULL,
							DeleteWorkStation = NULL
					WHERE	Item_Key = @Item_Key AND 
							Vendor_ID = @Vendor_ID
			
					EXEC dbo.EIM_Log @LoggingLevel, 'TRACE', @UploadSession_ID, @UploadRow_ID, @RetryCount, @Item_key, NULL, '4.2 Cost Change - [Update ItemVendor]'
				
				END TRY
				BEGIN CATCH

						EXEC dbo.EIM_LogAndRethrowException @UploadSession_ID, @UploadRow_ID, @RetryCount, @Item_key, NULL, '4.2 Cost Change - [Update ItemVendor]'
				END CATCH
			END
			
			--------------------------------------
			-- begin handle primary vendor flag
			--------------------------------------
			
			IF @PrimaryVendor = 1 OR (@IsCostChange IS NULL OR @IsCostChange = 1)
			BEGIN
				
				IF NOT EXISTS(SELECT  1
				  FROM StoreItemVendor (NOLOCK)
				  WHERE
					Item_Key = @Item_Key
					AND Store_No = @Store_No
					AND Vendor_ID = @Vendor_ID)
				BEGIN
					
					BEGIN TRY
					
						-- create a StoreItemVendor row if there isn't one already
						INSERT INTO StoreItemVendor
						(
						  Item_Key,
						  Store_No,
						  Vendor_ID,
						  PrimaryVendor
						)
						VALUES
						(
						  @Item_Key,
						  @Store_No,
						  @Vendor_ID,
						  0 -- we have to set the primary flag with an update so the 
							-- update trigger business logic is run
						)
													
						EXEC dbo.EIM_Log @LoggingLevel, 'TRACE', @UploadSession_ID, @UploadRow_ID, @RetryCount, @Item_key, NULL, '4.3 Cost Change - [Insert StoreItemVendor]'
					
					END TRY
					BEGIN CATCH

							EXEC dbo.EIM_LogAndRethrowException @UploadSession_ID, @UploadRow_ID, @RetryCount, @Item_key, NULL, '4.3 Cost Change - [Insert StoreItemVendor]'
					END CATCH
				END
				
				-- whether there was one or not, update it
				-- but only if the uploaded primary vendor flag is true
				-- EIM doesn't allow the primary vendor
				-- to be unset, only set
				IF @PrimaryVendor = 1
				BEGIN				
				
					BEGIN TRY
					
					  -- update with the uploaded values
					  UPDATE StoreItemVendor
					  SET PrimaryVendor = 1,
						  DeleteDate = null,
						  DeleteWorkStation = Null
					  WHERE Item_Key = @Item_Key
						  AND Store_No = @Store_No
						  AND Vendor_ID = @Vendor_ID

						EXEC dbo.EIM_Log @LoggingLevel, 'TRACE', @UploadSession_ID, @UploadRow_ID, @RetryCount, @Item_key, NULL, '4.4 Cost Change - [Update Primary Vendor Flag]'
					
					END TRY
					BEGIN CATCH

							EXEC dbo.EIM_LogAndRethrowException @UploadSession_ID, @UploadRow_ID, @RetryCount, @Item_key, NULL, '4.4 Cost Change - [Update Primary Vendor Flag]'
					END CATCH
				END -- @PrimaryVendor = 1
			END -- @PrimaryVendor = 1 OR (@IsCostChange IS NULL OR @IsCostChange = 1)
			
			--------------------------------------
			-- end handle primary vendor flag
			--------------------------------------

			-- create the cost		
			Select @CurrentDate = getdate()
			
			If @IsCostChange IS NULL OR @IsCostChange = 1
			BEGIN
				-- don't insert if there's already a row there with that Item/Store/StartDate/PricType combo;
				-- this is needed because EIM attempts to insert data that has NOT changed in EIM; must prevent duplicates
				-- ignore 3 Sale_Earned_Discount fields because EIM passes in NULL every time
				IF NOT EXISTS(SELECT  1
				  FROM VendorCostHistory (NOLOCK)
				  JOIN StoreItemVendor (NOLOCK)
					ON StoreItemVendor.StoreItemVendorID = VendorCostHistory.StoreItemVendorID
				  WHERE
					StoreItemVendor.Item_Key = @Item_Key
					AND StoreItemVendor.Store_No = @Store_No
					AND StoreItemVendor.Vendor_ID = @Vendor_ID
					AND Promotional = 0
					AND UnitCost = @Cost
					AND UnitFreight = @Freight
					AND CostUnit_ID = @CostUnit
					AND FreightUnit_ID = @FreightUnit
					AND StartDate = @CostStartDate
					AND EndDate = @CostEndDate)
				BEGIN

					BEGIN TRY
					-- explicitly using parameter names to handle optional parameters.
					  EXEC dbo.InsertVendorCostHistory
						  @StoreList						= @Store_No,
						  @StoreListSeparator				= '|',
						  @Item_Key							= @Item_Key,
						  @Vendor_ID						= @Vendor_ID,
						  @UnitCost							= @Cost,
						  @UnitFreight						= @Freight,
						  @Package_Desc1					= @Pack,
						  @StartDate						= @CostStartDate,
						  @EndDate							= @CostEndDate,
						  @Promotional						= @Promotional,
						  @MSRP								= NULL,
						  @FromVendor						= 0,
						  @CostUnit_ID						= @CostUnit,
						  @FreightUnit_ID					= @FreightUnit,
						  @Currency							= null

						
						UPDATE	ItemVendor 
						SET		RetailCasePack = CASE	WHEN @IgnoreCasePack =1 
														THEN @RetailCasePack 
														ELSE @Cost 
												END,
								IgnoreCasePack = @IgnoreCasePack 
						WHERE	ItemVendor.Vendor_ID	= @Vendor_ID AND 
								ItemVendor.Item_Key		= @Item_Key 
						

				
						EXEC dbo.EIM_Log @LoggingLevel, 'TRACE', @UploadSession_ID, @UploadRow_ID, @RetryCount, @Item_key, NULL, '4.5 Cost Change - [InsertVendorCostHistory]'
					
					END TRY
					BEGIN CATCH

							EXEC dbo.EIM_LogAndRethrowException @UploadSession_ID, @UploadRow_ID, @RetryCount, @Item_key, NULL, '4.5 Cost Change - [InsertVendorCostHistory]'
					END CATCH
				END -- if cost doesn't already exist	
			END -- @IsCostChange IS NULL OR @IsCostChange = 1
			
			-- create the deals if the Is Deal flag is
			-- non null and true and the deal amounts are non zero
			If @IsDealChange IS NOT NULL AND @IsDealChange = 1
			BEGIN

				BEGIN TRY
				
				  -- insert the allowance if there is one	
				  If @Allowance IS NOT NULL AND @Allowance > 0
				  BEGIN
					  EXEC dbo.InsertVendorDealHistory
						  @Item_Key,
						  @Vendor_ID,
						  @Store_No,
						  '|',
						  @CaseQty,
						  @Pack,
						  @Allowance,
						  @AllowanceStartDate,
						  @AllowanceEndDate,
						  'A',
						  0,
						  3, -- promo
						  0    
				  END
				  
				  -- insert the discount if there is one	
				  If @Discount IS NOT NULL AND @Discount > 0
				  BEGIN
					  EXEC dbo.InsertVendorDealHistory
						  @Item_Key,
						  @Vendor_ID,
						  @Store_No,
						  '|',
						  @CaseQty,
						  @Pack,
						  @Discount,
						  @AllowanceStartDate,
						  @AllowanceEndDate,
						  'D',
						  0,
						  3, -- promo
						  0    
				  END		
				  
				  EXEC dbo.EIM_Log @LoggingLevel, 'TRACE', @UploadSession_ID, @UploadRow_ID, @RetryCount, @Item_key, NULL, '4.6 Cost Change - [InsertVendorDealHistory]'

				END TRY
				BEGIN CATCH

				EXEC dbo.EIM_LogAndRethrowException @UploadSession_ID, @UploadRow_ID, @RetryCount, @Item_key, NULL, '4.6 Cost Change - [InsertVendorDealHistory]'
				END CATCH
		
			  END -- @IsDealChange IS NOT NULL AND @IsDealChange = 1
			
			 END  -- @IsDeauthStore <> 1
			 
			FETCH NEXT FROM CostUploadStore_cursor INTO @Store_No
		END

		CLOSE CostUploadStore_cursor
		DEALLOCATE CostUploadStore_cursor

	  END -- @IsDeleteVendor <> 1

	END -- @Vendor_ID IS NOT NULL

	EXEC dbo.EIM_Log @LoggingLevel, 'TRACE', @UploadSession_ID, @UploadRow_ID, @RetryCount, @Item_key, NULL, '4.7 Cost Change - [End]'