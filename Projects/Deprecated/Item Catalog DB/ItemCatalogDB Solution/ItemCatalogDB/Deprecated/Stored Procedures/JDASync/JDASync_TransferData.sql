IF EXISTS (SELECT * FROM sysobjects WHERE type = 'P' AND name = 'JDASync_TransferData')
	BEGIN
		DROP  Procedure  dbo.JDASync_TransferData
	END

GO

set ANSI_NULLS ON
set QUOTED_IDENTIFIER ON
go

CREATE Procedure dbo.JDASync_TransferData
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
			-- Step 1. Mark the rows in the sync tables to sync during this
			-- run.
			-- ======================================================
			
			-- IMPORTANT: mark the tables in the reverse order of their dependencies
			-- i.e. mark the most dependent tables first
			-- this guarantees that the dependent data will be synced with the data it is
			-- dependent on

			UPDATE [dbo].[JDA_PriceSync]
			SET SyncState = 1
			WHERE Item_Key IN (SELECT DISTINCt top 300  Item_Key 
							  FROM [JDA_PriceSync]
							  WHERE SyncState = 0)
			AND SyncState = 0 

			UPDATE [dbo].[JDA_CostSync]
			SET SyncState = 1
			WHERE Item_Key IN (SELECT DISTINCt top 300  Item_Key 
							  FROM [JDA_CostSync]
							  WHERE SyncState = 0)
			AND SyncState = 0 

			UPDATE [dbo].[JDA_VendorSync]
			SET SyncState = 1
			WHERE SyncState = 0

			UPDATE [dbo].[JDA_StoreItemVendorSync]
			SET SyncState = 1
			WHERE SyncState = 0

			UPDATE [dbo].[JDA_ItemVendorSync]
			SET SyncState = 1
			WHERE SyncState = 0

			UPDATE [dbo].[JDA_ItemIdentifierSync]
			SET SyncState = 1
			WHERE SyncState = 0
			
			UPDATE [dbo].[JDA_ItemSync]
			SET SyncState = 1
			WHERE SyncState = 0


			-- ======================================================
			-- Step 2. Transfer only the sync data marked for sync above
			-- from the IRMA to the JDA sync tables.
			-- ======================================================

			-- only transfer synced data if there are no null mapped
			-- values in the data we just marked

			DECLARE @HasUnmappedItemLookUpValues bit
			DECLARE @HasUnmappedPriceLookUpValues bit
			
			SET @HasUnmappedItemLookUpValues = 0
			SET @HasUnmappedPriceLookUpValues = 0

			-- first check the item sync table
			-- only check the sync data rows we just marked


			-- Suspend sync if following values are NULL EXCEPT for Delete records
			SELECT *
			FROM JDA_ItemSync (NOLOCK)
			WHERE
				(JDA_Dept IS NULL
				OR JDA_SubDept IS NULL
				OR JDA_Class IS NULL
				OR JDA_SubClass IS NULL 
				OR JDA_Brand_ID IS NULL
				OR Package_Unit_Id IS NULL
				OR Retail_Unit_ID IS NULL
				OR Vendor_Unit_ID IS NULL)
				AND SyncState = 1
				AND (ActionCode <> 'D')

			SELECT @HasUnmappedItemLookUpValues = CASE WHEN @@ROWCOUNT = 0 THEN 0 ELSE 1 END

			-- then check the price sync table		
			-- only check the sync data rows we just marked

			SELECT *
			FROM JDA_PriceSync (NOLOCK)
			WHERE
				JDA_PricePriority IS NULL
				AND SyncState = 1

			SELECT @HasUnmappedPriceLookUpValues = CASE WHEN @@ROWCOUNT = 0 THEN 0 ELSE 1 END

			IF @HasUnmappedItemLookUpValues = 0 AND @HasUnmappedPriceLookUpValues = 0
			BEGIN -- transfer
				
				-- ---------------------------------
				-- Transfer 1. Item data
				-- ---------------------------------
			
				INSERT INTO JDA_SYNC.FRESH.IRMASYNC.IRMA_ITEM
				(
					ACTCODE
					,[DATETIME]
					,INUMBR
					,IDESCR
					,IDEPT
					,ISDEPT
					,ICLAS
					,ISCLAS
					,ISTDPK
					,ISELUW
					,ISELUD
					,IFINLN
					,ISLUM
					,IDSCCD
					,IATRB3
					,IBYUM
					,IATRB1
				)
				SELECT 
					[ActionCode]
					,[ApplyDate]
					,[Item_Key]
					,[Item_Description]
					,[JDA_Dept]
					,[JDA_SubDept]
					,[JDA_Class]
					,[JDA_SubClass]
					,[Package_Desc1]
					,[Package_Desc2]
					,[Package_Unit_Id]
					,[JDA_Brand_ID]
					,[Retail_Unit_ID]
					, CASE WHEN Deleted_Item = 1 THEN 'P' WHEN Discontinue_Item = 1 THEN 'D' ELSE 'A' END
					,[WFM_Item]
					,[Vendor_Unit_ID]
					,[Manager_ID]
				FROM [JDA_ItemSync] (NOLOCK)
				WHERE SyncState = 1
				
				-- ---------------------------------
				-- Transfer 2. ItemIdentifier data
				-- ---------------------------------

				INSERT INTO JDA_SYNC.FRESH.IRMASYNC.IRMA_UPC
				(
					ACTCODE
					,[DATETIME]
					,INUMBR
					,IUPC
					,NAUPC
					,IPPCCD
					)
				SELECT
					[ActionCode]
					,[ApplyDate]
					,[Item_Key]
					,[Identifier]
					,[National_Identifier]
					,[ItemType_ID]
				FROM [JDA_ItemIdentifierSync] (NOLOCK)
				WHERE SyncState = 1

				-- ---------------------------------
				-- Transfer 3. Vendor data
				-- ---------------------------------
				
				INSERT INTO JDA_SYNC.FRESH.IRMASYNC.IRMA_VEND
				(
					ACTCODE
					,[DATETIME]
					,ASNUM
					,ASNAME
					,AAADD1
					,AAADD2
					,AAADD3 
					,AASTAT
					,ASPSCD
					,AAHOME
					,AAPHON
					,[AAFAX#]
					,VXPSVN
					,ASTYPE
					,PONOTE
					,RCNOTE
					,ASOTHN
					)
					SELECT
						[ActionCode]
						,[ApplyDate]
						,[Vendor_ID]
						,LEFT([CompanyName],35)
						,[Address_Line_1]
						,[Address_Line_2]
						,[City]
						,[State]
						,[Zip_Code]
						,[Country]
						,[Phone]
						,[Fax]
						,[PS_Vendor_ID]
						,[Non_Product_Vendor]
						,[Po_Note]
						,[Receiving_Authorization_Note]
						,[Other_Name]
					FROM [JDA_VendorSync] (NOLOCK)
					WHERE SyncState = 1

				-- ---------------------------------
				-- Transfer 4. ItemVendor data
				-- ---------------------------------
				
				INSERT INTO JDA_SYNC.FRESH.IRMASYNC.IRMA_IMVR
				(
					ACTCODE
					,[DATETIME]
					,INUMBR
					,VXPSVN
					,[IVVND#]
					)
					SELECT
						[ActionCode]
						,[ApplyDate]
						,[Item_Key]
						,[Vendor_ID]
						,[Item_Id]
					FROM [JDA_ItemVendorSync] (NOLOCK)
					WHERE SyncState = 1

				-- ---------------------------------
				-- Transfer 5. StoreItemVendor data
				-- ---------------------------------
				
				INSERT INTO JDA_SYNC.FRESH.IRMASYNC.IRMA_STIVN
				(
					ACTCODE
					,[DATETIME]
					,IVSTOR
					,INUMBR
					,IVVNUM
					,IVPRIM
					)
					SELECT 
						[ActionCode]
						,[ApplyDate]
						,[Store_No]
						,[Item_Key]
						,[Vendor_ID]
						,[PrimaryVendor]
					FROM [JDA_StoreItemVendorSync] (NOLOCK)
					WHERE SyncState = 1

				-- ---------------------------------
				-- Transfer 6. Cost data
				-- ---------------------------------
				
				INSERT INTO JDA_SYNC.FRESH.IRMASYNC.IRMA_SCST
				(
					[DATETIME]
					,PPLSTR
					,CSTITM
					,VXPSVN
					,CSTTYPB
					,CSTAMT
					,CSTSPK
					,CSTCDTZ
					,CSTADTZ
					)
					SELECT
						[ApplyDate]
						,[Store_No]
						,[Item_Key]
						,[Vendor_Id]
						,[Promotional]
						,[NetCost]
						,[Package_Desc1]
						,dbo.JDA_LimitMaxDate([StartDate])
						,dbo.JDA_LimitMaxDate([EndDate])
					FROM [JDA_CostSync] (NOLOCK)
					WHERE SyncState = 1

				-- ---------------------------------
				-- Transfer 7. Price data
				-- ---------------------------------
				
				INSERT INTO JDA_SYNC.FRESH.IRMASYNC.IRMA_PRICE
				(
					[DATETIME]
					,PLNITM
					,PPLSTR
					,PLNTYP
					,PLNMLT
					,PLNAMT
					,PLNCDTZ
					,PLNADTZ
					)
					SELECT
						[ApplyDate]
						,[Item_Key]
						,[Store_No]
						,[JDA_PricePriority]
						,CASE [JDA_PricePriority] WHEN 10 THEN [Multiple] ELSE [Sale_Multiple] END
						,CASE [JDA_PricePriority] WHEN 10 THEN [Price] ELSE [Sale_Price] END
						,dbo.JDA_LimitMaxDate([Sale_Start_Date])
						,dbo.JDA_LimitMaxDate([Sale_End_Date])
					FROM [JDA_PriceSync] (NOLOCK)
					WHERE SyncState = 1

				-- ======================================================
				-- Step 3. Mark the transfered sync data as synced.
				-- ======================================================
				
				-- the order of marking does not matter here

				UPDATE [dbo].[JDA_ItemSync]
				SET SyncState = 2
				WHERE SyncState = 1

				UPDATE [dbo].[JDA_ItemIdentifierSync]
				SET SyncState = 2
				WHERE SyncState = 1

				UPDATE [dbo].[JDA_VendorSync]
				SET SyncState = 2
				WHERE SyncState = 1

				UPDATE [dbo].[JDA_ItemVendorSync]
				SET SyncState = 2
				WHERE SyncState = 1

				UPDATE [dbo].[JDA_StoreItemVendorSync]
				SET SyncState = 2
				WHERE SyncState = 1

				UPDATE [dbo].[JDA_PriceSync]
				SET SyncState = 2
				WHERE SyncState = 1

				UPDATE [dbo].[JDA_CostSync]
				SET SyncState = 2
				WHERE SyncState = 1
							
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
					'SYNC_JOB',
					0,
					GetDate(),
					null
				)

			END -- end transfer
			
			IF @HasUnmappedItemLookUpValues = 1
			BEGIN
				-- send an email to the JDA admin
						
				EXEC dbo.JDASync_Notify
					@EventKey = 'SYNC_JOB_SUSPENDED',
					@AdditionalBodyText = 'The IRMA to JDA sync has been suspended due to unmapped lookup (null) values in the JDA_ItemSync table.'

			END

			IF @HasUnmappedPriceLookUpValues = 1
			BEGIN
				-- send an email to the JDA admin
						
				EXEC dbo.JDASync_Notify
					@EventKey = 'SYNC_JOB_SUSPENDED',
					@AdditionalBodyText = 'The IRMA to JDA sync has been suspended due to unmapped lookup (null) values in the JDA_PriceSync table.'

			END
			
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
				'JDA Sync failed with error: ' +
				CAST(ERROR_NUMBER() AS VARCHAR(100)) + ' | ' +
				CAST(ERROR_SEVERITY() AS VARCHAR(100)) + ' | ' +
				CAST(ERROR_STATE() AS VARCHAR(100)) + ' | ' +
				CAST(ERROR_LINE() AS VARCHAR(100)) + ' | ' +
				ERROR_MESSAGE()
			
			SELECT @ErrorMessage = CAST(ERROR_NUMBER() AS varchar(200)) + ': ' + ERROR_MESSAGE()
		
			INSERT INTO dbo.JDA_SyncJobLog 
			(
				JobName,
				IsFailed,
				RunDate,
				ErrorMessage
			)
			VALUES
			(
				'SYNC_JOB',
				1,
				GetDate(),
				@ErrorMessage
			)
			
			DECLARE @BodyText varchar(8000)

			SELECT @BodyText = 'The following error occured when the JDA Sync Job executed. ' +
				'The error is also logged in the JDA_SyncJobLog table.' + @ErrorMessage

				
			EXEC dbo.JDASync_Notify
				@EventKey = 'SYNC_JOB_FAILED',
				@AdditionalBodyText = @BodyText
					
		END CATCH
	
	END -- @SyncJDA = 1

GO


