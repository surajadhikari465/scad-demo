IF EXISTS (SELECT * FROM sysobjects WHERE type = 'P' AND name = 'JDASync_TransferLoadedCost')
	BEGIN
		DROP  Procedure  dbo.JDASync_TransferLoadedCost
	END

GO

set ANSI_NULLS ON
set QUOTED_IDENTIFIER ON
go

CREATE Procedure dbo.[JDASync_TransferLoadedCost]
AS
	-- ======================================================
	-- Purpose: This procedure copies the data from the IRMA
	-- side sync tables into the corresponding JDA side sync
	-- tables.
	--
	-- Author: David Marine; dmarine@athensgroup.com
	-- Created: 05.07.07
	-- ======================================================
		
    DECLARE @SyncJDA bit
	DECLARE @Debug bit
	DECLARE @Count int

	SELECT @Debug = 0

    SELECT @SyncJDA = dbo.fn_InstanceDataValue('SyncJDA', NULL)

	-- only if the instance data flag is set
	If @SyncJDA = 1
	BEGIN

		BEGIN TRY
		
			-- this is required
			-- not doing so will result in nested transaction issues
			SET XACT_ABORT ON

			BEGIN TRANSACTION
		
			-- ======================================================
			-- Step 1. Mark the rows in the sync loaded cost table to sync during this
			-- run.
			-- ======================================================
			IF @Debug = 1  SELECT 'Step 1'
    			
			UPDATE JDA_SYNC.FRESH.IRMASYNC.IRMA_LCST
			SET PFLAG = 1
			WHERE PFLAG = 0

			-- ======================================================
			-- Step 2. Transfer only the sync data marked for sync above
			-- from the IRMA to the JDA sync tables.
			-- ======================================================
			IF @Debug = 1 SELECT 'Step 2'
			
			DECLARE @Item_Key int,
				@Store_No int,
				@Vendor_ID int,
				@NetCost smallmoney,
				@UnitFreight smallmoney,
				@Package_Desc1 decimal(9,4),
				@StartDate smalldatetime,
				@EndDate smalldatetime,
				@Promotional bit,
				@CostUnit_ID int,
				@FreightUnit_ID int,
				@StoreList varchar(8000)
				
			IF @Debug = 1 SELECT 'Step Cursor'
			
			DECLARE LoadedCost_cursor CURSOR FOR
				SELECT
					lcst.LODITM as Item_Key,
					lcst.LODSTR as Store_No,
					40808 as Vendor_ID,
					lcst.LODAMT as NetCost,
					0 as UnitFreight,
					lcst.LODSPK as Package_Desc1,
					lcst.LODCDTZ as StartDate,
					lcst.LODADTZ as EndDate,
					0 as Promotional,
					jium.Unit_ID as CostUnit_ID,
					jium.Unit_ID as FreightUnit_ID
				FROM JDA_SYNC.FRESH.IRMASYNC.IRMA_LCST lcst (NOLOCK)
					JOIN JDA_ItemUnitMapping jium (NOLOCK)
						ON jium.JDA_ID = lcst.LODUM
					INNER JOIN Store s
					ON lcst.LODSTR  = s.Store_No	-- only process store records that exist in IRMA
				WHERE lcst.PFLAG = 1
				AND WFM_Store = 1

			OPEN LoadedCost_cursor
			
			IF @Debug = 1  SELECT 'Fetch 1'
			FETCH NEXT FROM LoadedCost_cursor
				INTO
					@Item_Key,
					@Store_No,
					@Vendor_ID,
					@NetCost,
					@UnitFreight,
					@Package_Desc1,
					@StartDate,
					@EndDate,
					@Promotional,
					@CostUnit_ID,
					@FreightUnit_ID

			
			WHILE @@FETCH_STATUS = 0
			BEGIN
				-- ======================================================
				-- Step 2A. Check to see if ItemVendor record exists for this
				--			item with the Warehouse as the vendor - create if not
				-- ======================================================		
				Select @Count = count(1)
				FROM ItemVendor
				WHERE Item_Key = @Item_Key AND Vendor_ID = 40808 -- Warehouse
				
				IF @Count = 0 
					-- Use Item Key (SKU Number) as Vendor Part Number
					INSERT INTO ItemVendor (Item_Key, Vendor_ID, Item_ID)
						VALUES (@Item_Key,@Vendor_ID, @Item_Key)
	
				-- ======================================================
				-- Step 2B. Check to see if StoreItemVendor record exists for this
				--			item and Store with the Warehouse as the vendor - create if not
				-- ======================================================		
				Select @Count = count(1)
				FROM StoreItemVendor
				WHERE Item_Key = @Item_Key AND Vendor_ID = 40808 -- Warehouse
				AND Store_No = @Store_No

				IF @Count = 0
				BEGIN
					-- Insert the new SIV record as the Primary Vendor to prevent de-authorization
					SELECt 'SIV INSERT',@store_no,@Item_Key,@vendor_Id,1	
                    INSERT INTO StoreItemVendor (Store_No, Item_Key, Vendor_ID, PrimaryVendor)
						VALUES( @Store_No, @Item_Key, @Vendor_ID, 1)						

				END
				ELSE
				BEGIN 
					-- Update the existing SIV record, removing the Delete date and setting to Primary
					SELECt 'SIV INSERT',@store_no,@Item_Key,@vendor_Id,1	

					UPDATE StoreItemVendor
					SET DeleteDate = null,
						PrimaryVendor = 1
					WHERE Vendor_Id = @Vendor_ID 
						  and Item_Key = @Item_Key 
						  and Store_No = @Store_No

				END

				-- Make sure no other vendors are set as primary for this store/item combo
				UPDATE StoreItemVendor
					SET PrimaryVendor = 0
				FROM StoreItemVendor
				WHERE Vendor_ID <> @Vendor_Id 
					  AND Item_Key = @Item_Key 
					  AND Store_No = @Store_No
					  AND PrimaryVendor = 1

				-- ======================================================
				-- Step 2C. Insert new cost in to VendroCostHistory table
				-- ======================================================		
				IF @Debug = 1  SELECT 'EXEC InsertVendorCostHistory'
				EXEC InsertVendorCostHistory @Store_No, '|', @Item_Key, @Vendor_ID,
					@NetCost, @UnitFreight, @Package_Desc1, @StartDate, @EndDate, @Promotional, NULL, 0, @CostUnit_ID, @FreightUnit_ID
					, 1 -- indicate the cost is from JDA so it is not synced back
					;
				
				--select 'Store list',@StoreList
				
				select @Item_Key, @Vendor_ID,
					 @NetCost, @UnitFreight, @Package_Desc1, @StartDate, @EndDate, @Promotional, NULL, 0, @CostUnit_ID, @FreightUnit_ID;

				FETCH NEXT FROM LoadedCost_cursor
					INTO
						@Item_Key,
						@Store_No,
						@Vendor_ID,
						@NetCost,
						@UnitFreight,
						@Package_Desc1,
						@StartDate,
						@EndDate,
						@Promotional,
						@CostUnit_ID,
						@FreightUnit_ID

			END

			-- ======================================================
			-- Step 3. Mark the transfered sync data as synced.
			-- ======================================================
			
			CLOSE LoadedCost_cursor
			DEALLOCATE LoadedCost_cursor

			IF @Debug = 1  SELECT 'Step 3'
			UPDATE JDA_SYNC.FRESH.IRMASYNC.IRMA_LCST
			SET PFLAG = 2
			WHERE PFLAG = 1

			-- log the run
			INSERT INTO dbo.JDA_SyncJobLog 
			(
				JobName,
				IsFailed,
				RunDate,
				ErrorMessage
			)
			VALUES
			(
				'SYNC_LOADEDCOST_JOB',
				0,
				GetDate(),
				null
			)
			
			-- send an email to the JDA admin
					
			EXEC dbo.JDASync_Notify
				@EventKey = 'SYNC_LOADEDCOST_JOB_SUCEEDED',
				@AdditionalBodyText = null

			COMMIT TRANSACTION

			-- reset
			SET XACT_ABORT OFF
		
		END TRY
		BEGIN CATCH
		
			ROLLBACK TRANSACTION

			-- reset
			SET XACT_ABORT OFF

			-- capture the error data
			DECLARE @ErrorNumber INT,
				@ErrorMessage VARCHAR(8000),
				@Severity INT

			SELECT @ErrorNumber = ERROR_NUMBER()
			SELECT @Severity = ERROR_SEVERITY()

			SELECT @ErrorMessage =
				'JDA Loaded Cost Sync failed with error: ' +
				CAST(ERROR_NUMBER() AS VARCHAR(100)) + ' | ' +
				CAST(ERROR_SEVERITY() AS VARCHAR(100)) + ' | ' +
				CAST(ERROR_STATE() AS VARCHAR(100)) + ' | ' +
				CAST(ERROR_LINE() AS VARCHAR(100)) + ' | ' +
				ERROR_MESSAGE()
			
			SELECT @ErrorMessage = CAST(ERROR_NUMBER() AS VARCHAR(100)) + ': ' + ERROR_MESSAGE()
		
			INSERT INTO dbo.JDA_SyncJobLog 
			(
				JobName,
				IsFailed,
				RunDate,
				ErrorMessage
			)
			VALUES
			(
				'SYNC_LOADEDCOST_JOB',
				1,
				GetDate(),
				@ErrorMessage
			)
			
		DECLARE @BodyText varchar(255)

		SELECT @BodyText = 'The following error occured when the JDA to IRMA loaded cost sync job executed. ' +
			'The error is also logged in the JDA_SyncJobLog table.' + @ErrorMessage

			
		EXEC dbo.JDASync_Notify
			@EventKey = 'SYNC_LOADEDCOST_JOB_FAILED',
			@AdditionalBodyText = @BodyText

		END CATCH
	
	END -- @SyncJDA = 1


GO


