
CREATE PROCEDURE [dbo].[InsertCloneStore] 
		@NewStoreNo int,				-- The new store no
        @StoreAbbr varchar(5),			-- Store Abbreviation
        @NewStoreName varchar(50),		-- Name of new store
        @StoreJurisdiction int,         -- StoreJurisdictionID
        @ZoneID int,					-- Zone Id From Form
        @TaxJurisdiction int,			-- TaxJurisdictionID
        @BusinessUnit_Id int,			-- Business Unit ID
        @PSI_Store_No int,				-- Planogram Store No
        @Plum_Store_No int,				-- Planogram Store No
		@OldStoreNo int,				-- The source store no
        @ISSPriceChgTypeID int,			-- Planogram Store No
        @StoreSubTeamSubstitutions varchar(max),		--double-delimited list of alternate store/subteam combos for cloning
        @VendorName varchar(50),		-- name for store as vendor
        @VendorAddress varchar(75),		-- New Store Address 
        @VendorCity varchar(50),		-- New Store City
        @VendorState varchar(5),		-- New Store State
        @VendorZipCode varchar(10),		-- New Store Zip
		@PeopleSoftVendorID varchar(10),-- PeopleSoft Vendor Number
		@IncSlim As Bit,				-- Include Slim Entries
		@IncFutureSale As Bit, 			-- Include Future Sale Items
		@IncPromoPlanner As Bit,		-- Include Promo Planner
		@GeoCode varchar(15)
AS
BEGIN

SET NOCOUNT ON

/*

########################################################################
Update History
########################################################################

====================================	
20080111 - DaveStacey
	Altered and interpreted from Tim Pope and Russell M. and Alex Z... 

====================================	
1/22/2010
Tom Lux
TFS 11641: New Store Creation tool does not copy over pending off sale records
IRMA v3.5.9
	(PriceBatchDetail section)
	*** NOTE: This change is replicated in the next query as well (bottom/2nd-piece of the UNION).
	Fixed issue with the next line of code to conditionally include ISS price changes.
	Regarding the bug/TFS title, this PBD query DOES pull over the most recent REG/Sale-Off for all items,
	but the "bug" with the @IncSlim caused NO (zero) PBD rows to be pulled over to the new store when @IncSlim = 1.
	The new logic handles this option correctly.
	The -1 is used if we want to include them (@IncSlim=1) because 'PB.PriceChgTypeID <> -1' should then always be TRUE.
====================================	
1/22/2010
Tom Lux
TFS 11642
IRMA 3.5.9
	Added PS_Team_No and PS_Subteam_No values to StoreSubteam section.

====================================	
2/11/2010
Tom Lux
TFS 11919
	Added 2nd query (joined to first by UNION) to pull Store-Item data for alternate stores and subteams.
	Added join to item table for subteam filtering.
	Added subteam exclude in WHERE clause of first query based on store-subteam list specified by user.

====================================	
2/18/10
Tom Lux
TFS 11641
3.5.9
	[Problem]
	The alt store-subteam code was not handling null PBD.subteam_no values
	when alt store-subteam pairs were passed in (select by user).
	Here's example for top query: "AND PB.subteam_no not in (%alt store-subteam list%)"
	The issue did not manifest itself if no alt store-subteam list was passed because
	it ended up being a "and null = null" statement.
	[Fixes (PriceBatchDetail Section)]
	1) Added join to item table so when PBD.subteam_no is null, we can take item.subteam_no.
	2) Changed condition in alt store-subteam lines to be "isnull(PB.subteam_no, i.subteam_no)"
	so that items with null PBD.subteam_no fields are not unconditionally excluded.
	3) Changed (in both top and bottom queries) all selected fields to include explicit table reference,
	since item has many of the same columns.

Brian R			12/29/10			???				[4.1] Added GeoCode Param and insert into store
=======================================

Tom Lux			2011-03-01			TFS 1510		[IRMA V4.2] Removed JOIN to Price table in the first/top query of the 'INSERT INTO [dbo].[StoreItem]...' section.
													There is no filtering/restriction on stores in this query, so it was processing potentially millions of extra rows
													because it included all stores (approx. # of extract rows = [store_count] - 1 * [item_count]).  The query worked, but was very inefficient.
													In order to help ensure we're still basing the item list off the price table, the JOIN to Price table was replaced by restricting the
													item list in the query to the unique list of item-keys from the Price table.

Tom Lux			2011-03-18			TFS 1510		[IRMA V4.2] Added logging to AppLog table before and after each step.
													Re-wrote VCH data-copy section as follows:
													1) We now build a temp table of VCH IDs that only includes the current and any future cost entries.
													We make two passes to build the temp table: one for main source store-subteams, and a second for alternate store-subteams.
													2) Removed WHERE NOT EXISTS clause because we only create stores once, so there's no need to make sure the data isn't already there.


########################################################################
########################################################################
*/


----------------------------------------------
-- Use TRY...CATCH for error handling
----------------------------------------------
BEGIN TRY
        ----------------------------------------------
        -- Wrap the updates in a transaction
        ----------------------------------------------
        BEGIN TRANSACTION

DECLARE
        @CurrDate smalldatetime,
        @CodeLocation varchar(128),
        @RowsAffected int,
        @VendorID int,
        @InsertPromoplanner int,
		@RegionCode varchar(2),
		@VendorCountry varchar(2),
		@CopyVendorKey varchar(2),
        @NewVendorKey varchar(10),		-- key for store as vendor (Region + New Store Abbrev e.g. 'RMNCP')
	    @StoreSubTeamSubstitutionsSeparator1 char(1),
		@StoreSubTeamSubstitutionsSeparator2 char(1),
        -- For logging to AppLog table.
        @DBEnv varchar(8)
        ,@LogSystemName varchar(64)
        ,@LogAppName varchar(64)
        ,@LogAppID uniqueidentifier
        ,@LogLevel varchar(8)
        ,@LogThread varchar(8)
        ,@LogMsg varchar(256)
        ,@LogExceptionMsg varchar(2000)
		,@now datetime

		select
			@LogSystemName = 'IRMA CLIENT'
			,@LogAppName = 'InsertCloneStore'
			,@LogLevel = 'INFO'
			,@LogThread = '0'
			,@LogExceptionMsg = ''

		-- Determine DB environment (from version table) so we can get appropriate app ID from app-config.  (Env short names from AppConfigEnv: 'TST', 'QA', 'PRD'.)
		select @DBEnv = case
			when Environment like '%q%' then 'QA'
			when Environment like '%pr%' then 'PRD'
			else 'TST'
			end
		from version
		-- Get IRMA Client app GUID for logging calls (AppLogInsertEntry).
		select @LogAppID = a.ApplicationID
		from AppConfigApp a
		join AppConfigEnv e on a.EnvironmentID = e.EnvironmentID
		where e.ShortName = @DBEnv and a.Name = @LogSystemName

		--DEBUG: select DBEnv = @DBEnv, LogAppID = @LogAppID

----------------------------------------------
-- Get the current date
----------------------------------------------
	SELECT @CurrDate = CONVERT(smalldatetime, CONVERT(varchar, GETDATE(), 101))

        ----------------------------------------------------------------------
        -- add a new store to the specified zone
        ----------------------------------------------------------------------
        SELECT @CodeLocation = 'INSERT INTO [Store]...'
		select @now = getdate(); exec dbo.AppLogInsertEntry @now, @LogAppID, @LogThread, @LogLevel, @LogAppName, @CodeLocation, @LogExceptionMsg;

        IF EXISTS (SELECT * FROM [Store] (NOLOCK) WHERE [Store_No] = @NewStoreNo)
        BEGIN
			select @RowsAffected = @@ROWCOUNT, @now = getdate(), @LogMsg = @CodeLocation + ' Rows Affected: 0 (data already exists)';
			exec dbo.AppLogInsertEntry @now, @LogAppID, @LogThread, @LogLevel, @LogAppName, @LogMsg, @LogExceptionMsg;
            PRINT '[' + convert(nvarchar, getdate(), 121) + '] ' + @LogMsg
		END
        ELSE
          BEGIN
			IF @OldStoreNo > 0
				BEGIN
					INSERT INTO dbo.Store (Store_No,Store_Name,Mega_Store,Distribution_Center,Manufacturer,WFM_Store,
						Internal,TelnetUser,TelnetPassword,BatchID,BatchRecords,BusinessUnit_ID,Zone_ID,UNFI_Store,
						LastRecvLogDate,LastRecvLog_No,RecvLogUser_ID,EXEWarehouse,Regional,LastSalesUpdateDate,
						StoreAbbr,PLUMStoreNo,TaxJurisdictionID,POSSystemId,PSI_Store_No, StoreJurisdictionID, UseAvgCostHistory, GeoCode)
					SELECT @NewStoreNo,@NewStoreName,Mega_Store,Distribution_Center,Manufacturer,WFM_Store,
						Internal,TelnetUser,TelnetPassword,BatchID,BatchRecords,@BusinessUnit_ID,@ZoneID,UNFI_Store,
						LastRecvLogDate,1,RecvLogUser_ID,EXEWarehouse,Regional,LastSalesUpdateDate,
						@StoreAbbr,@Plum_Store_No,@TaxJurisdiction,POSSystemId,@PSI_Store_No, @StoreJurisdiction, UseAvgCostHistory, @GeoCode
					FROM dbo.Store 
					where Store_No = @OldStoreNo

				select @RowsAffected = @@ROWCOUNT, @now = getdate(), @LogMsg = @CodeLocation + ' Rows Affected: ' + cast(@@ROWCOUNT as varchar);
				exec dbo.AppLogInsertEntry @now, @LogAppID, @LogThread, @LogLevel, @LogAppName, @LogMsg, @LogExceptionMsg;
                PRINT '[' + convert(nvarchar, getdate(), 121) + '] ' + @LogMsg
				END
			ELSE
			BEGIN 
					INSERT INTO dbo.Store (Store_No,Store_Name,BusinessUnit_ID,Zone_ID,LastRecvLog_No,
						StoreAbbr,PLUMStoreNo,PSI_Store_No, TaxJurisdictionID,StoreJurisdictionID, GeoCode)
					SELECT @NewStoreNo,@NewStoreName, @BusinessUnit_ID,@ZoneID,1,@StoreAbbr,@Plum_Store_No,@PSI_Store_No,
							@TaxJurisdiction, @StoreJurisdiction, @GeoCode

					select @RowsAffected = @@ROWCOUNT, @now = getdate(), @LogMsg = @CodeLocation + ' Rows Affected: ' + cast(@@ROWCOUNT as varchar);
					exec dbo.AppLogInsertEntry @now, @LogAppID, @LogThread, @LogLevel, @LogAppName, @LogMsg, @LogExceptionMsg;
					PRINT '[' + convert(nvarchar, getdate(), 121) + '] ' + @LogMsg
			END
          END

        ----------------------------------------------------------------------
        -- add a store region mapping
        ----------------------------------------------------------------------
        SELECT @CodeLocation = 'INSERT INTO [StoreRegionMapping]...'
		select @now = getdate(); exec dbo.AppLogInsertEntry @now, @LogAppID, @LogThread, @LogLevel, @LogAppName, @CodeLocation, @LogExceptionMsg;

        IF EXISTS (SELECT * FROM [StoreRegionMapping] (NOLOCK) WHERE [Store_No] = @NewStoreNo)
        BEGIN
			select @RowsAffected = @@ROWCOUNT, @now = getdate(), @LogMsg = @CodeLocation + ' Rows Affected: 0 (data already exists)';
			exec dbo.AppLogInsertEntry @now, @LogAppID, @LogThread, @LogLevel, @LogAppName, @LogMsg, @LogExceptionMsg;
            PRINT '[' + convert(nvarchar, getdate(), 121) + '] ' + @LogMsg
		END
        ELSE
          BEGIN
				
			IF @OldStoreNo > 0
				BEGIN
					SELECT TOP 1 @RegionCode = Region_Code from [StoreRegionMapping] WHERE Store_No = @OldStoreNo
					IF LEN(@RegionCode) > 1
						INSERT INTO [dbo].[StoreRegionMapping] ([Store_No], [Region_Code])
						VALUES (@NewStoreNo, @RegionCode)

					select @RowsAffected = @@ROWCOUNT, @now = getdate(), @LogMsg = @CodeLocation + ' Rows Affected: ' + cast(@@ROWCOUNT as varchar);
					exec dbo.AppLogInsertEntry @now, @LogAppID, @LogThread, @LogLevel, @LogAppName, @LogMsg, @LogExceptionMsg;
					PRINT '[' + convert(nvarchar, getdate(), 121) + '] ' + @LogMsg
				END
			ELSE
			BEGIN 
				SELECT TOP 1 @RegionCode = Region_Code from [StoreRegionMapping] 
				IF LEN(@RegionCode) > 1
					INSERT INTO [dbo].[StoreRegionMapping] ([Store_No], [Region_Code])
					VALUES (@NewStoreNo, @RegionCode)

				select @RowsAffected = @@ROWCOUNT, @now = getdate(), @LogMsg = @CodeLocation + ' Rows Affected: ' + cast(@@ROWCOUNT as varchar);
				exec dbo.AppLogInsertEntry @now, @LogAppID, @LogThread, @LogLevel, @LogAppName, @LogMsg, @LogExceptionMsg;
				PRINT '[' + convert(nvarchar, getdate(), 121) + '] ' + @LogMsg
			END
          END

--         ---------------------------------------------------------------------------------------
--         -- Enter Store as Vendor ...... StoreList Grids in the client look for this entry ...
--         ---------------------------------------------------------------------------------------

		SELECT @NewVendorKey = @RegionCode + @StoreAbbr
				
	   -- check for existence of new store as a vendor
		SELECT @VendorID = [Vendor_ID]
		FROM [Vendor] (NOLOCK)
		WHERE [Vendor_Key] = @NewVendorKey

        SELECT @CodeLocation = 'INSERT INTO [Vendor]...'
		select @now = getdate(); exec dbo.AppLogInsertEntry @now, @LogAppID, @LogThread, @LogLevel, @LogAppName, @CodeLocation, @LogExceptionMsg;

        IF @CopyVendorKey IS NOT NULL
          BEGIN
                INSERT INTO [dbo].[Vendor]
                        ([Vendor_Key], [CompanyName], [PayTo_CompanyName], [Store_no], [PS_Vendor_ID],
                        [Address_Line_1],[City],[State],[Zip_Code],[Country],
                        [Customer], [InternalCustomer], [ActiveVendor], [Order_By_Distribution], [Electronic_Transfer], [User_ID], [PS_Location_Code], [PS_Address_Sequence],
                        [WFM], [Non_Product_Vendor], [Default_GLNumber], [EFT], [InStoreManufacturedProducts], [EXEWarehouseVendSent], [EXEWarehouseCustSent], [AddVendor])
                SELECT
                        @NewVendorKey, @NewStoreName, [PayTo_CompanyName], @NewStoreNo, @PeopleSoftVendorID,
                        @VendorAddress, @VendorCity, @VendorState, @VendorZipCode, @VendorCountry,
                        [Customer], [InternalCustomer], [ActiveVendor], [Order_By_Distribution], [Electronic_Transfer], [User_ID], [PS_Location_Code], [PS_Address_Sequence],
                        [WFM], [Non_Product_Vendor], [Default_GLNumber], [EFT], [InStoreManufacturedProducts], [EXEWarehouseVendSent], [EXEWarehouseCustSent], [AddVendor]
                FROM [Vendor] (NOLOCK)
                WHERE [Vendor_Key] = @CopyVendorKey

                -- get the db ID for the new vendor
				select @VendorID = SCOPE_IDENTITY(), @RowsAffected = @@ROWCOUNT, @now = getdate(), @LogMsg = @CodeLocation + ' Rows Affected: ' + cast(@@ROWCOUNT as varchar);
				exec dbo.AppLogInsertEntry @now, @LogAppID, @LogThread, @LogLevel, @LogAppName, @LogMsg, @LogExceptionMsg;
				PRINT '[' + convert(nvarchar, getdate(), 121) + '] ' + @LogMsg
          END
        ELSE
          BEGIN
                INSERT INTO [dbo].[Vendor]
                        ([Vendor_Key], [CompanyName], [Store_no], [PS_Vendor_ID],
                        [Address_Line_1],[City],[State],[Zip_Code],[Country])
                SELECT
                        @NewVendorKey, @NewStoreName, @NewStoreNo, @PeopleSoftVendorID,
                        @VendorAddress, @VendorCity, @VendorState, @VendorZipCode, @VendorCountry

                -- get the db ID for the new vendor
				select @VendorID = SCOPE_IDENTITY(), @RowsAffected = @@ROWCOUNT, @now = getdate(), @LogMsg = @CodeLocation + ' Rows Affected: ' + cast(@@ROWCOUNT as varchar);
				exec dbo.AppLogInsertEntry @now, @LogAppID, @LogThread, @LogLevel, @LogAppName, @LogMsg, @LogExceptionMsg;
				PRINT '[' + convert(nvarchar, getdate(), 121) + '] ' + @LogMsg
          END

         ----------------------------------------------------------------------
         -- copy subteam relationships for the new store
         ----------------------------------------------------------------------
        SELECT @CodeLocation = 'INSERT INTO [StoreSubTeam]...'
		select @now = getdate(); exec dbo.AppLogInsertEntry @now, @LogAppID, @LogThread, @LogLevel, @LogAppName, @CodeLocation, @LogExceptionMsg;

			IF @OldStoreNo > 0
				BEGIN
				-- TFS 11642, 2010.01.22, IRMA 3.5.9, Tom Lux
				-- Added PS_Team_No and PS_Subteam_No values.
					INSERT INTO [dbo].[StoreSubTeam] (
							[Store_No]
							,[Team_No]
							,[SubTeam_No]
							,[CasePriceDiscount]
							,[CostFactor]
							,[ICVID]
							,[PS_Team_No]
							,[PS_SubTeam_No]
					)
						SELECT
							@NewStoreNo
							,[Team_No]
							,[SubTeam_No]
							,[CasePriceDiscount]
							,[CostFactor]
							,[ICVID]
							,[PS_Team_No]
							,[PS_SubTeam_No]
						FROM [StoreSubTeam] SST (NOLOCK)
						WHERE SST.[Store_No] = @OldStoreNo
								AND NOT EXISTS (SELECT *
												FROM [StoreSubTeam] (NOLOCK)
												WHERE [Store_No] = @NewStoreNo
														AND [Team_No] = SST.[Team_No]
														AND [SubTeam_No] = SST.[SubTeam_No])
				END
				
		select @RowsAffected = @@ROWCOUNT, @now = getdate(), @LogMsg = @CodeLocation + ' Rows Affected: ' + cast(@@ROWCOUNT as varchar);
		exec dbo.AppLogInsertEntry @now, @LogAppID, @LogThread, @LogLevel, @LogAppName, @LogMsg, @LogExceptionMsg;
		PRINT '[' + convert(nvarchar, getdate(), 121) + '] ' + @LogMsg

         ----------------------------------------------------------------------
         -- copy Price information for the new store from main source store
         ----------------------------------------------------------------------
        SELECT @CodeLocation = 'INSERT INTO [Price]...'
		select @now = getdate(); exec dbo.AppLogInsertEntry @now, @LogAppID, @LogThread, @LogLevel, @LogAppName, @CodeLocation, @LogExceptionMsg;

			IF @OldStoreNo > 0
				BEGIN
					INSERT INTO [dbo].[Price]
							(
					Item_Key,Store_No,Multiple,Price,MSRPPrice,MSRPMultiple,PricingMethod_ID,
					Sale_Multiple,Sale_Price,Sale_Start_Date,Sale_End_Date,Sale_Max_Quantity,
					Sale_Earned_Disc1,Sale_Earned_Disc2,Sale_Earned_Disc3,Restricted_Hours,
					AvgCostUpdated,IBM_Discount,POSPrice,POSSale_Price,NotAuthorizedForSale,
					CompFlag,PosTare,LinkedItem,GrillPrint,AgeCode,VisualVerify,SrCitizenDiscount,
					PriceChgTypeId,ExceptionSubteam_No,POSLinkCode,KitchenRoute_ID,Routing_Priority,
					Consolidate_Price_To_Prev_Item,Print_Condiment_On_Receipt,Age_Restrict,
					CompetitivePriceTypeID,BandwidthPercentageHigh,BandwidthPercentageLow,MixMatch,
					LocalItem,ItemSurcharge)
					SELECT P.Item_Key, @NewStoreNo, Multiple,Price,MSRPPrice,MSRPMultiple,PricingMethod_ID,
					Sale_Multiple,Sale_Price,Sale_Start_Date,Sale_End_Date,Sale_Max_Quantity,
					Sale_Earned_Disc1,Sale_Earned_Disc2,Sale_Earned_Disc3,Restricted_Hours,
					AvgCostUpdated,IBM_Discount,POSPrice,POSSale_Price,NotAuthorizedForSale,
					CompFlag,PosTare,LinkedItem,GrillPrint,AgeCode,VisualVerify,SrCitizenDiscount,
					PriceChgTypeId,ExceptionSubteam_No,POSLinkCode,KitchenRoute_ID,Routing_Priority,
					Consolidate_Price_To_Prev_Item,Print_Condiment_On_Receipt,Age_Restrict,
					CompetitivePriceTypeID,BandwidthPercentageHigh,BandwidthPercentageLow,MixMatch,
					P.LocalItem,P.ItemSurcharge
					FROM [dbo].[Price] P (NOLOCK)
							INNER JOIN Item I (NOLOCK) ON I.Item_Key = P.Item_Key
					WHERE I.Deleted_Item = 0
							AND P.Store_No = @OldStoreNo
							AND i.subteam_no not in (select Key_Value2 FROM fn_Parse_List_Two(@StoreSubTeamSubstitutions, @StoreSubTeamSubstitutionsSeparator1, @StoreSubTeamSubstitutionsSeparator2) IL GROUP BY Key_Value2)
							AND NOT EXISTS (SELECT Item_Key
											FROM [Price] (NOLOCK)
											WHERE Item_Key = P.Item_Key
													AND Store_No = @NewStoreNo)

		select @RowsAffected = @@ROWCOUNT, @now = getdate(), @LogMsg = @CodeLocation + ' Rows Affected: ' + cast(@@ROWCOUNT as varchar);
		exec dbo.AppLogInsertEntry @now, @LogAppID, @LogThread, @LogLevel, @LogAppName, @LogMsg, @LogExceptionMsg;
		PRINT '[' + convert(nvarchar, getdate(), 121) + '] ' + @LogMsg

				SELECT @CodeLocation = 'INSERT INTO [Price] alt store-subteams...'
				select @now = getdate(); exec dbo.AppLogInsertEntry @now, @LogAppID, @LogThread, @LogLevel, @LogAppName, @CodeLocation, @LogExceptionMsg;
				 ---------------------------------------------------------------------------------
				 -- copy Price information for the new store from substitute subteam store(s)
				 ---------------------------------------------------------------------------------
				INSERT INTO [dbo].[Price]
						(
				Item_Key,Store_No,Multiple,Price,MSRPPrice,MSRPMultiple,PricingMethod_ID,
				Sale_Multiple,Sale_Price,Sale_Start_Date,Sale_End_Date,Sale_Max_Quantity,
				Sale_Earned_Disc1,Sale_Earned_Disc2,Sale_Earned_Disc3,Restricted_Hours,
				AvgCostUpdated,IBM_Discount,POSPrice,POSSale_Price,NotAuthorizedForSale,
				CompFlag,PosTare,LinkedItem,GrillPrint,AgeCode,VisualVerify,SrCitizenDiscount,
				PriceChgTypeId,ExceptionSubteam_No,POSLinkCode,KitchenRoute_ID,Routing_Priority,
				Consolidate_Price_To_Prev_Item,Print_Condiment_On_Receipt,Age_Restrict,
				CompetitivePriceTypeID,BandwidthPercentageHigh,BandwidthPercentageLow,MixMatch,
				LocalItem,ItemSurcharge)
				SELECT P.Item_Key, @NewStoreNo, Multiple,Price,MSRPPrice,MSRPMultiple,PricingMethod_ID,
				Sale_Multiple,Sale_Price,Sale_Start_Date,Sale_End_Date,Sale_Max_Quantity,
				Sale_Earned_Disc1,Sale_Earned_Disc2,Sale_Earned_Disc3,Restricted_Hours,
				AvgCostUpdated,IBM_Discount,POSPrice,POSSale_Price,NotAuthorizedForSale,
				CompFlag,PosTare,LinkedItem,GrillPrint,AgeCode,VisualVerify,SrCitizenDiscount,
				PriceChgTypeId,ExceptionSubteam_No,POSLinkCode,KitchenRoute_ID,Routing_Priority,
				Consolidate_Price_To_Prev_Item,Print_Condiment_On_Receipt,Age_Restrict,
				CompetitivePriceTypeID,BandwidthPercentageHigh,BandwidthPercentageLow,MixMatch,
				P.LocalItem,P.ItemSurcharge
				FROM [dbo].[Price] P (NOLOCK)
						INNER JOIN Item I (NOLOCK) ON I.Item_Key = P.Item_Key
						JOIN (select Key_Value1, Key_Value2 FROM fn_Parse_List_Two(@StoreSubTeamSubstitutions, @StoreSubTeamSubstitutionsSeparator1, @StoreSubTeamSubstitutionsSeparator2) IL GROUP BY Key_Value1, Key_Value2) 
								as substores ON substores.Key_Value1 = P.Store_No and substores.Key_Value2 = I.Subteam_No
				WHERE I.Deleted_Item = 0
						AND NOT EXISTS (SELECT Item_Key
										FROM [Price] (NOLOCK)
										WHERE Item_Key = P.Item_Key
												AND Store_No = @NewStoreNo)
		END

		select @RowsAffected = @@ROWCOUNT, @now = getdate(), @LogMsg = @CodeLocation + ' Rows Affected: ' + cast(@@ROWCOUNT as varchar);
		exec dbo.AppLogInsertEntry @now, @LogAppID, @LogThread, @LogLevel, @LogAppName, @LogMsg, @LogExceptionMsg;
		PRINT '[' + convert(nvarchar, getdate(), 121) + '] ' + @LogMsg

        ----------------------------------------------------------------------
         -- copy SignQueue info
        ----------------------------------------------------------------------
        SELECT @CodeLocation = 'INSERT INTO [SignQueue]...'
		select @now = getdate(); exec dbo.AppLogInsertEntry @now, @LogAppID, @LogThread, @LogLevel, @LogAppName, @CodeLocation, @LogExceptionMsg;

			IF @OldStoreNo > 0
				BEGIN
					INSERT INTO [dbo].[SignQueue]
						  ([Item_Key], [Store_No], [Sign_Description], [Ingredients], [Identifier], [Sold_By_Weight], [Multiple], [Price],
						  [MSRPMultiple], [MSRPPrice], [Case_Price], [Sale_Multiple], [Sale_Price], [Sale_Start_Date], [Sale_End_Date],
						  [Sale_Earned_Disc1], [Sale_Earned_Disc2], [Sale_Earned_Disc3], [PricingMethod_ID], [SubTeam_No], [Origin_Name],
						  [Brand_Name], [Retail_Unit_Abbr], [Retail_Unit_Full], [Package_Unit], [Package_Desc1], [Package_Desc2],
						  [Sign_Printed], [Organic], [Vendor_Id], [User_ID], [User_ID_Date], [ItemType_ID], [ScaleDesc1],
						  [ScaleDesc2], [POS_Description], [Restricted_Hours], [Quantity_Required], [Price_Required], [Retail_Sale],
						  [Discountable], [Food_Stamps], [IBM_Discount], [New_Item], [Price_Change], [Item_Change],
						  [LastQueuedType], [POSPrice], [POSSale_Price], [PriceChgTypeId], [TagTypeID], [TagTypeID2], 
						  [LocalItem],[ItemSurcharge])
					SELECT
						  SQ.Item_Key, @NewStoreNo, SQ.Sign_Description, SQ.Ingredients, SQ.Identifier, SQ.Sold_By_Weight, SQ.Multiple, SQ.Price,
						  SQ.MSRPMultiple, SQ.MSRPPrice, SQ.Case_Price, SQ.Sale_Multiple, SQ.Sale_Price, SQ.Sale_Start_Date, SQ.Sale_End_Date,
						  SQ.Sale_Earned_Disc1, SQ.Sale_Earned_Disc2, SQ.Sale_Earned_Disc3, SQ.PricingMethod_ID, SQ.SubTeam_No, SQ.Origin_Name,
						  SQ.Brand_Name, SQ.Retail_Unit_Abbr, SQ.Retail_Unit_Full, SQ.Package_Unit, SQ.Package_Desc1, SQ.Package_Desc2,
						  SQ.Sign_Printed, SQ.Organic, SQ.Vendor_Id, SQ.[User_ID], @CurrDate AS User_ID_Date, SQ.ItemType_ID, SQ.ScaleDesc1,
						  SQ.ScaleDesc2, SQ.POS_Description, SQ.Restricted_Hours, SQ.Quantity_Required, SQ.Price_Required, SQ.Retail_Sale,
						  SQ.Discountable, SQ.Food_Stamps, SQ.IBM_Discount, SQ.New_Item, SQ.Price_Change, SQ.Item_Change,
						  SQ.LastQueuedType, SQ.POSPrice, SQ.POSSale_Price, SQ.PriceChgTypeId, SQ.TagTypeID, SQ.TagTypeID2, 
						  SQ.LocalItem, SQ.ItemSurcharge
					FROM [SignQueue] SQ (NOLOCK)
						JOIN dbo.Item I (NOLOCK) on i.Item_Key = SQ.Item_Key
					WHERE SQ.[Store_No] = @OldStoreNo
							AND i.subteam_no not in (select Key_Value2 FROM fn_Parse_List_Two(@StoreSubTeamSubstitutions, @StoreSubTeamSubstitutionsSeparator1, @StoreSubTeamSubstitutionsSeparator2) IL GROUP BY Key_Value2)
						  AND NOT EXISTS (SELECT *
										  FROM [SignQueue] (NOLOCK)
										  WHERE [Store_No] = @NewStoreNo
												  AND [Item_Key] = SQ.[Item_Key])
					UNION
					SELECT
						  SQ.Item_Key, @NewStoreNo, SQ.Sign_Description, SQ.Ingredients, SQ.Identifier, SQ.Sold_By_Weight, SQ.Multiple, SQ.Price,
						  SQ.MSRPMultiple, SQ.MSRPPrice, SQ.Case_Price, SQ.Sale_Multiple, SQ.Sale_Price, SQ.Sale_Start_Date, SQ.Sale_End_Date,
						  SQ.Sale_Earned_Disc1, SQ.Sale_Earned_Disc2, SQ.Sale_Earned_Disc3, SQ.PricingMethod_ID, SQ.SubTeam_No, SQ.Origin_Name,
						  SQ.Brand_Name, SQ.Retail_Unit_Abbr, SQ.Retail_Unit_Full, SQ.Package_Unit, SQ.Package_Desc1, SQ.Package_Desc2,
						  SQ.Sign_Printed, SQ.Organic, SQ.Vendor_Id, SQ.[User_ID], @CurrDate AS User_ID_Date, SQ.ItemType_ID, SQ.ScaleDesc1,
						  SQ.ScaleDesc2, SQ.POS_Description, SQ.Restricted_Hours, SQ.Quantity_Required, SQ.Price_Required, SQ.Retail_Sale,
						  SQ.Discountable, SQ.Food_Stamps, SQ.IBM_Discount, SQ.New_Item, SQ.Price_Change, SQ.Item_Change,
						  SQ.LastQueuedType, SQ.POSPrice, SQ.POSSale_Price, SQ.PriceChgTypeId, SQ.TagTypeID, SQ.TagTypeID2, 
						  SQ.LocalItem, SQ.ItemSurcharge
					FROM [SignQueue] SQ (NOLOCK)
						JOIN dbo.Item I (NOLOCK) on i.Item_Key = SQ.Item_Key
						JOIN (select Key_Value1, Key_Value2 FROM fn_Parse_List_Two(@StoreSubTeamSubstitutions, @StoreSubTeamSubstitutionsSeparator1, @StoreSubTeamSubstitutionsSeparator2) IL GROUP BY Key_Value1, Key_Value2) 
								as substores ON substores.Key_Value1 = SQ.Store_No and substores.Key_Value2 = I.Subteam_No
					WHERE NOT EXISTS (SELECT *
										  FROM [SignQueue] (NOLOCK)
										  WHERE [Store_No] = @NewStoreNo
												  AND [Item_Key] = SQ.[Item_Key])

		END
		select @RowsAffected = @@ROWCOUNT, @now = getdate(), @LogMsg = @CodeLocation + ' Rows Affected: ' + cast(@@ROWCOUNT as varchar);
		exec dbo.AppLogInsertEntry @now, @LogAppID, @LogThread, @LogLevel, @LogAppName, @LogMsg, @LogExceptionMsg;
		PRINT '[' + convert(nvarchar, getdate(), 121) + '] ' + @LogMsg

        ----------------------------------------------------------------------
        -- copy StoreItemVendor for the new store
        ----------------------------------------------------------------------
        SELECT @CodeLocation = 'INSERT INTO [StoreItemVendor]...'
		select @now = getdate(); exec dbo.AppLogInsertEntry @now, @LogAppID, @LogThread, @LogLevel, @LogAppName, @CodeLocation, @LogExceptionMsg;

			IF @OldStoreNo > 0
				BEGIN
					INSERT INTO [dbo].[StoreItemVendor]
							([Store_No], [Item_Key], [Vendor_ID], [AverageDelivery], [PrimaryVendor], [DeleteDate], [DeleteWorkStation])
					SELECT
							@NewStoreNo, SIV.[Item_Key], [Vendor_ID], [AverageDelivery], [PrimaryVendor], [DeleteDate], [DeleteWorkStation]
					FROM [StoreItemVendor] SIV (NOLOCK)
							INNER JOIN [Price] P (NOLOCK) ON P.Item_Key = SIV.Item_Key AND P.Store_No = @NewStoreNo
							JOIN dbo.Item I (NOLOCK) on i.Item_Key = P.Item_Key
					WHERE SIV.DeleteDate IS NULL
							AND SIV.Store_No = @OldStoreNo
							AND i.subteam_no not in (select Key_Value2 FROM fn_Parse_List_Two(@StoreSubTeamSubstitutions, @StoreSubTeamSubstitutionsSeparator1, @StoreSubTeamSubstitutionsSeparator2) IL GROUP BY Key_Value2)
							AND NOT EXISTS (SELECT *
											FROM [StoreItemVendor] (NOLOCK)
											WHERE Item_Key = P.Item_Key
													AND Store_No = @NewStoreNo)
					ORDER BY SIV.StoreItemVendorID

		select @RowsAffected = @@ROWCOUNT, @now = getdate(), @LogMsg = @CodeLocation + ' Rows Affected: ' + cast(@@ROWCOUNT as varchar);
		exec dbo.AppLogInsertEntry @now, @LogAppID, @LogThread, @LogLevel, @LogAppName, @LogMsg, @LogExceptionMsg;
		PRINT '[' + convert(nvarchar, getdate(), 121) + '] ' + @LogMsg
                
        SELECT @CodeLocation = 'INSERT INTO [StoreItemVendor]... alt store-subteams'
                
					INSERT INTO [dbo].[StoreItemVendor]
					([Store_No], [Item_Key], [Vendor_ID], [AverageDelivery], [PrimaryVendor], [DeleteDate], [DeleteWorkStation])
					SELECT
							@NewStoreNo, SIV.[Item_Key], [Vendor_ID], [AverageDelivery], [PrimaryVendor], [DeleteDate], [DeleteWorkStation]
					FROM [StoreItemVendor] SIV (NOLOCK)
							INNER JOIN [Price] P (NOLOCK) ON P.Item_Key = SIV.Item_Key AND P.Store_No = @NewStoreNo
							JOIN dbo.Item I (NOLOCK) on i.Item_Key = P.Item_Key
							JOIN (select Key_Value1, Key_Value2 FROM fn_Parse_List_Two(@StoreSubTeamSubstitutions, @StoreSubTeamSubstitutionsSeparator1, @StoreSubTeamSubstitutionsSeparator2) IL GROUP BY Key_Value1, Key_Value2) 
									as substores ON substores.Key_Value1 = SIV.Store_No and substores.Key_Value2 = I.Subteam_No
					WHERE SIV.DeleteDate IS NULL
							AND NOT EXISTS (SELECT *
											FROM [StoreItemVendor] (NOLOCK)
											WHERE Item_Key = P.Item_Key
													AND Store_No = @NewStoreNo)
					ORDER BY SIV.StoreItemVendorID
			END
			
		select @RowsAffected = @@ROWCOUNT, @now = getdate(), @LogMsg = @CodeLocation + ' Rows Affected: ' + cast(@@ROWCOUNT as varchar);
		exec dbo.AppLogInsertEntry @now, @LogAppID, @LogThread, @LogLevel, @LogAppName, @LogMsg, @LogExceptionMsg;
		PRINT '[' + convert(nvarchar, getdate(), 121) + '] ' + @LogMsg

        ----------------------------------------------------------------------
        -- copy Store Item for the new store
        ----------------------------------------------------------------------
        /*
			Tom Lux
			2010-02-11
			TFS 11919
			Added 2nd query (joined to first by UNION) to pull Store-Item data for alternate stores and subteams.
			Added join to item table for subteam filtering.
			Added subteam exclude in WHERE clause of first query based on store-subteam list specified by user.
        */
        SELECT @CodeLocation = 'INSERT INTO [StoreItem]...'
		select @now = getdate(); exec dbo.AppLogInsertEntry @now, @LogAppID, @LogThread, @LogLevel, @LogAppName, @CodeLocation, @LogExceptionMsg;

		INSERT INTO [dbo].[StoreItem](
			[Store_No],
			[Item_Key]
		)
        SELECT
			@NewStoreNo,
			i.Item_Key
        FROM
			dbo.Item I (NOLOCK)
        WHERE
			/*
			Added this WHERE-filtering so we do not process/check millions of unnecessary entries due to all stores being included in result set.
			*/
			i.item_key in ( select item_key from price group by item_key )
			AND
			NOT EXISTS (SELECT *
				FROM [StoreItem] (NOLOCK)
				WHERE Store_No = @NewStoreNo
				and Item_Key = i.Item_Key)
			-- Restrict to subteams not being pulled from another store by excluding the store-subteam list.
			AND i.subteam_no not in (
				select Key_Value2
				FROM fn_Parse_List_Two(@StoreSubTeamSubstitutions, @StoreSubTeamSubstitutionsSeparator1, @StoreSubTeamSubstitutionsSeparator2) IL
				GROUP BY Key_Value2
			)

		union
		
		SELECT
			@NewStoreNo,
			P.Item_Key
		FROM
			dbo.Price P (NOLOCK)
		JOIN
			dbo.Item I (NOLOCK) on i.Item_Key = P.Item_Key
		-- Pull alternate store-subteam data using the store-subteam list.	
		JOIN (
			select
				Key_Value1, 
				Key_Value2
			FROM fn_Parse_List_Two(@StoreSubTeamSubstitutions, @StoreSubTeamSubstitutionsSeparator1, @StoreSubTeamSubstitutionsSeparator2) IL
			GROUP BY Key_Value1, Key_Value2
		) as substores
			ON substores.Key_Value1 = P.Store_No and substores.Key_Value2 = I.Subteam_No
		WHERE
			NOT EXISTS (
				SELECT *
				FROM [StoreItem] (NOLOCK)
				WHERE Store_No = @NewStoreNo
				and Item_Key = P.Item_Key
			)

		ORDER BY Item_Key

		select @RowsAffected = @@ROWCOUNT, @now = getdate(), @LogMsg = @CodeLocation + ' Rows Affected: ' + cast(@@ROWCOUNT as varchar);
		exec dbo.AppLogInsertEntry @now, @LogAppID, @LogThread, @LogLevel, @LogAppName, @LogMsg, @LogExceptionMsg;
		PRINT '[' + convert(nvarchar, getdate(), 121) + '] ' + @LogMsg
        

        ----------------------------------------------------------------------
        -- copy VendorCostHistory for the new store
        ----------------------------------------------------------------------
        SELECT @CodeLocation = 'INSERT INTO [VendorCostHistory]...'
		select @now = getdate(); exec dbo.AppLogInsertEntry @now, @LogAppID, @LogThread, @LogLevel, @LogAppName, @CodeLocation, @LogExceptionMsg;

			IF @OldStoreNo > 0
				BEGIN
			        SELECT @CodeLocation = 'INSERT INTO [VendorCostHistory]... (build VCH List)'
					declare @vchList table ( vchid int primary key );
					declare @vchDate datetime = getdate();
					declare @vchTomorrowDate datetime = dateadd(day, 1, @vchDate);
					-- Build list of VCH entries to be copied to new store.
					INSERT INTO @vchList
						SELECT VendorCostHistoryID = MAX(VendorCostHistoryID)
						FROM
							VendorCostHistory vch (nolock)
						JOIN
							StoreItemVendor siv (nolock)
								ON vch.StoreItemVendorID = siv.StoreItemVendorID
						JOIN
							Item i (nolock)
								ON siv.item_key = i.item_key
						WHERE
							SIV.Store_No = @OldStoreNo
							AND i.SubTeam_No NOT IN (select Key_Value2 FROM fn_Parse_List_Two(@StoreSubTeamSubstitutions, @StoreSubTeamSubstitutionsSeparator1, @StoreSubTeamSubstitutionsSeparator2) IL GROUP BY Key_Value2)
							AND (
								(@vchDate >= StartDate AND @vchDate <= EndDate) -- Current VCH entries.
								OR
								(StartDate > @vchDate) -- Future VCH entries.
							)
							AND @vchDate < ISNULL(DeleteDate, @vchTomorrowDate)
						GROUP BY vch.StoreItemVendorID
					
		select @RowsAffected = @@ROWCOUNT, @now = getdate(), @LogMsg = @CodeLocation + ' Rows Affected: ' + cast(@@ROWCOUNT as varchar);
		exec dbo.AppLogInsertEntry @now, @LogAppID, @LogThread, @LogLevel, @LogAppName, @LogMsg, @LogExceptionMsg;
		PRINT '[' + convert(nvarchar, getdate(), 121) + '] ' + @LogMsg

			        SELECT @CodeLocation = 'INSERT INTO [VendorCostHistory]... (build VCH List alt store-subteams)'
					INSERT INTO @vchList
						SELECT VendorCostHistoryID = MAX(VendorCostHistoryID)
						FROM
							VendorCostHistory vch (nolock)
						JOIN
							StoreItemVendor siv (nolock)
								ON vch.StoreItemVendorID = siv.StoreItemVendorID
						JOIN
							Item i (nolock)
								ON siv.item_key = i.item_key
						JOIN (
							select Key_Value1, Key_Value2 FROM fn_Parse_List_Two(@StoreSubTeamSubstitutions, @StoreSubTeamSubstitutionsSeparator1, @StoreSubTeamSubstitutionsSeparator2) IL GROUP BY Key_Value1, Key_Value2
							) AS substores
								ON substores.Key_Value1 = SIV.Store_No and substores.Key_Value2 = I.Subteam_No
						WHERE
							(
								(@vchDate >= StartDate AND @vchDate <= EndDate) -- Current VCH entries.
								OR
								(StartDate > @vchDate) -- Future VCH entries.
							)
							AND @vchDate < ISNULL(DeleteDate, @vchTomorrowDate)
						GROUP BY vch.StoreItemVendorID

		select @RowsAffected = @@ROWCOUNT, @now = getdate(), @LogMsg = @CodeLocation + ' Rows Affected: ' + cast(@@ROWCOUNT as varchar);
		exec dbo.AppLogInsertEntry @now, @LogAppID, @LogThread, @LogLevel, @LogAppName, @LogMsg, @LogExceptionMsg;
		PRINT '[' + convert(nvarchar, getdate(), 121) + '] ' + @LogMsg

					SELECT @CodeLocation = 'INSERT INTO [VendorCostHistory]...'
					select @now = getdate(); exec dbo.AppLogInsertEntry @now, @LogAppID, @LogThread, @LogLevel, @LogAppName, @CodeLocation, @LogExceptionMsg;
					
					INSERT INTO [dbo].[VendorCostHistory]
							([StoreItemVendorID], [Promotional], [UnitCost], [UnitFreight], [Package_Desc1], [StartDate], [EndDate], [FromVendor],
							 [MSRP], [InsertWorkStation], [CostUnit_ID], [FreightUnit_ID])
					SELECT
							NewSIV.[StoreItemVendorID], VCH.[Promotional], VCH.[UnitCost], VCH.[UnitFreight], VCH.[Package_Desc1], VCH.[StartDate],
							VCH.[EndDate], VCH.[FromVendor], VCH.[MSRP], 'NEW STORE SCRIPT',
							VCH.[CostUnit_ID], VCH.[FreightUnit_ID]
					FROM [VendorCostHistory] VCH (NOLOCK)
							JOIN @vchList vchl on VCH.VendorCostHistoryID = vchl.vchid
							JOIN [StoreItemVendor] SIV (NOLOCK) ON SIV.StoreItemVendorID = VCH.StoreItemVendorID
							JOIN [StoreItemVendor] NewSIV (NOLOCK) ON NewSIV.Item_Key = SIV.Item_Key AND NewSIV.Vendor_ID = SIV.Vendor_ID
							JOIN dbo.Item I (NOLOCK) on i.Item_Key = SIV.Item_Key
					WHERE SIV.Store_No = @OldStoreNo
							AND NewSIV.Store_No = @NewStoreNo
							AND i.subteam_no not in (select Key_Value2 FROM fn_Parse_List_Two(@StoreSubTeamSubstitutions, @StoreSubTeamSubstitutionsSeparator1, @StoreSubTeamSubstitutionsSeparator2) IL GROUP BY Key_Value2)

		select @RowsAffected = @@ROWCOUNT, @now = getdate(), @LogMsg = @CodeLocation + ' Rows Affected: ' + cast(@@ROWCOUNT as varchar);
		exec dbo.AppLogInsertEntry @now, @LogAppID, @LogThread, @LogLevel, @LogAppName, @LogMsg, @LogExceptionMsg;
		PRINT '[' + convert(nvarchar, getdate(), 121) + '] ' + @LogMsg

					SELECT @CodeLocation = 'INSERT INTO [VendorCostHistory]... alt store-subteams'
					
					INSERT INTO [dbo].[VendorCostHistory]
							([StoreItemVendorID], [Promotional], [UnitCost], [UnitFreight], [Package_Desc1], [StartDate], [EndDate], [FromVendor],
							 [MSRP], [InsertWorkStation], [CostUnit_ID], [FreightUnit_ID])
					SELECT
							NewSIV.[StoreItemVendorID], VCH.[Promotional], VCH.[UnitCost], VCH.[UnitFreight], VCH.[Package_Desc1], VCH.[StartDate],
							VCH.[EndDate], VCH.[FromVendor], VCH.[MSRP], 'NEW STORE SCRIPT',
							VCH.[CostUnit_ID], VCH.[FreightUnit_ID]
					FROM [VendorCostHistory] VCH (NOLOCK)
							JOIN @vchList vchl on VCH.VendorCostHistoryID = vchl.vchid
							JOIN [StoreItemVendor] SIV (NOLOCK) ON SIV.StoreItemVendorID = VCH.StoreItemVendorID
							JOIN [StoreItemVendor] NewSIV (NOLOCK) ON NewSIV.Item_Key = SIV.Item_Key AND NewSIV.Vendor_ID = SIV.Vendor_ID
							JOIN dbo.Item I (NOLOCK) on i.Item_Key = SIV.Item_Key
							JOIN (select Key_Value1, Key_Value2 FROM fn_Parse_List_Two(@StoreSubTeamSubstitutions, @StoreSubTeamSubstitutionsSeparator1, @StoreSubTeamSubstitutionsSeparator2) IL GROUP BY Key_Value1, Key_Value2) 
									as substores ON substores.Key_Value1 = SIV.Store_No and substores.Key_Value2 = I.Subteam_No
					WHERE NewSIV.Store_No = @NewStoreNo

			END
		select @RowsAffected = @@ROWCOUNT, @now = getdate(), @LogMsg = @CodeLocation + ' Rows Affected: ' + cast(@@ROWCOUNT as varchar);
		exec dbo.AppLogInsertEntry @now, @LogAppID, @LogThread, @LogLevel, @LogAppName, @LogMsg, @LogExceptionMsg;
		PRINT '[' + convert(nvarchar, getdate(), 121) + '] ' + @LogMsg


		----------------------------------------------------------------------
		-- copy VendorDealHistory for the new store
		----------------------------------------------------------------------
		SELECT @CodeLocation = 'INSERT INTO [VendorDealHistory]...'
		select @now = getdate(); exec dbo.AppLogInsertEntry @now, @LogAppID, @LogThread, @LogLevel, @LogAppName, @CodeLocation, @LogExceptionMsg;

			IF @OldStoreNo > 0
				BEGIN
				INSERT INTO [dbo].[VendorDealHistory]
					([StoreItemVendorID], [CaseQty], [Package_Desc1], [CaseAmt], [StartDate], [EndDate], [VendorDealTypeID], [FromVendor],
					 [InsertWorkStation], [CostPromoCodeTypeID], [NotStackable], [InsertDate])
				SELECT
					NewSIV.[StoreItemVendorID], VDH.[CaseQty], VDH.[Package_Desc1], VDH.[CaseAmt], VDH.[StartDate], VDH.[EndDate], VDH.[VendorDealTypeID],
					VDH.[FromVendor], 'NEW STORE SCRIPT',
					VDH.[CostPromoCodeTypeID], VDH.[NotStackable], [InsertDate]
				FROM [VendorDealHistory] VDH (NOLOCK)
					INNER JOIN [StoreItemVendor] SIV (NOLOCK) ON SIV.StoreItemVendorID = VDH.StoreItemVendorID
					INNER JOIN [StoreItemVendor] NewSIV (NOLOCK) ON NewSIV.Item_Key = SIV.Item_Key AND NewSIV.Vendor_ID = SIV.Vendor_ID
					JOIN dbo.Item I (NOLOCK) on i.Item_Key = SIV.Item_Key
				WHERE SIV.Store_No = @OldStoreNo
					AND NewSIV.Store_No = @NewStoreNo
					AND i.subteam_no not in (select Key_Value2 FROM fn_Parse_List_Two(@StoreSubTeamSubstitutions, @StoreSubTeamSubstitutionsSeparator1, @StoreSubTeamSubstitutionsSeparator2) IL GROUP BY Key_Value2)
					AND NOT EXISTS (SELECT *
									FROM [VendorDealHistory] (NOLOCK)
									WHERE StoreItemVendorID = NewSIV.StoreItemVendorID
											AND EndDate = VDH.EndDate
											AND StartDate = VDH.StartDate
											AND Package_Desc1 = VDH.Package_Desc1
											AND CostPromoCodeTypeID = VDH.CostPromoCodeTypeID
											AND CaseAmt = VDH.CaseAmt
											AND CaseQty = VDH.CaseQty)
				GROUP BY
					NewSIV.[StoreItemVendorID], VDH.[CaseQty], VDH.[Package_Desc1], VDH.[CaseAmt], VDH.[StartDate], VDH.[EndDate], 
					VDH.[VendorDealTypeID],	VDH.[FromVendor], VDH.[CostPromoCodeTypeID], VDH.[NotStackable], VDH.[InsertDate]
				ORDER BY 
					VDH.StartDate

		select @RowsAffected = @@ROWCOUNT, @now = getdate(), @LogMsg = @CodeLocation + ' Rows Affected: ' + cast(@@ROWCOUNT as varchar);
		exec dbo.AppLogInsertEntry @now, @LogAppID, @LogThread, @LogLevel, @LogAppName, @LogMsg, @LogExceptionMsg;
		PRINT '[' + convert(nvarchar, getdate(), 121) + '] ' + @LogMsg

		SELECT @CodeLocation = 'INSERT INTO [VendorDealHistory]... alt store-subteams'
		
				INSERT INTO [dbo].[VendorDealHistory]
					([StoreItemVendorID], [CaseQty], [Package_Desc1], [CaseAmt], [StartDate], [EndDate], [VendorDealTypeID], [FromVendor],
					 [InsertWorkStation], [CostPromoCodeTypeID], [NotStackable], [InsertDate])
				SELECT
					NewSIV.[StoreItemVendorID], VDH.[CaseQty], VDH.[Package_Desc1], VDH.[CaseAmt], VDH.[StartDate], VDH.[EndDate], VDH.[VendorDealTypeID],
					VDH.[FromVendor], 'NEW STORE SCRIPT',
					VDH.[CostPromoCodeTypeID], VDH.[NotStackable], [InsertDate]
				FROM [VendorDealHistory] VDH (NOLOCK)
					INNER JOIN [StoreItemVendor] SIV (NOLOCK) ON SIV.StoreItemVendorID = VDH.StoreItemVendorID
					INNER JOIN [StoreItemVendor] NewSIV (NOLOCK) ON NewSIV.Item_Key = SIV.Item_Key AND NewSIV.Vendor_ID = SIV.Vendor_ID
						JOIN dbo.Item I (NOLOCK) on i.Item_Key = SIV.Item_Key
						JOIN (select Key_Value1, Key_Value2 FROM fn_Parse_List_Two(@StoreSubTeamSubstitutions, @StoreSubTeamSubstitutionsSeparator1, @StoreSubTeamSubstitutionsSeparator2) IL GROUP BY Key_Value1, Key_Value2) 
								as substores ON substores.Key_Value1 = SIV.Store_No and substores.Key_Value2 = I.Subteam_No
				WHERE NewSIV.Store_No = @NewStoreNo
					AND NOT EXISTS (SELECT *
									FROM [VendorDealHistory] (NOLOCK)
									WHERE StoreItemVendorID = NewSIV.StoreItemVendorID
											AND EndDate = VDH.EndDate
											AND StartDate = VDH.StartDate
											AND Package_Desc1 = VDH.Package_Desc1
											AND CostPromoCodeTypeID = VDH.CostPromoCodeTypeID
											AND CaseAmt = VDH.CaseAmt
											AND CaseQty = VDH.CaseQty)
				GROUP BY
					NewSIV.[StoreItemVendorID], VDH.[CaseQty], VDH.[Package_Desc1], VDH.[CaseAmt], VDH.[StartDate], VDH.[EndDate], 
					VDH.[VendorDealTypeID],	VDH.[FromVendor], VDH.[CostPromoCodeTypeID], VDH.[NotStackable], VDH.[InsertDate]
				ORDER BY 
					VDH.StartDate
		END

		select @RowsAffected = @@ROWCOUNT, @now = getdate(), @LogMsg = @CodeLocation + ' Rows Affected: ' + cast(@@ROWCOUNT as varchar);
		exec dbo.AppLogInsertEntry @now, @LogAppID, @LogThread, @LogLevel, @LogAppName, @LogMsg, @LogExceptionMsg;
		PRINT '[' + convert(nvarchar, getdate(), 121) + '] ' + @LogMsg


		----------------------------------------------------------------------
		-- Update Store Auths
		----------------------------------------------------------------------
		SELECT @CodeLocation = 'UPDATE [StoreItem]...'
		select @now = getdate(); exec dbo.AppLogInsertEntry @now, @LogAppID, @LogThread, @LogLevel, @LogAppName, @CodeLocation, @LogExceptionMsg;

			IF @OldStoreNo > 0
				BEGIN
					UPDATE dbo.StoreItem
					SET Authorized = SI2.Authorized
					FROM dbo.StoreItem (NOLOCK)
						JOIN dbo.StoreItem SI2 (NOLOCK) ON StoreItem.Item_Key = SI2.Item_Key
						JOIN dbo.Item I (NOLOCK) on i.Item_Key = SI2.Item_Key
					WHERE StoreItem.Store_No = @NewStoreNo
						AND SI2.Store_No = @OldStoreNo
						AND i.subteam_no not in (select Key_Value2 FROM fn_Parse_List_Two(@StoreSubTeamSubstitutions, @StoreSubTeamSubstitutionsSeparator1, @StoreSubTeamSubstitutionsSeparator2) IL GROUP BY Key_Value2)
						AND StoreItem.Authorized <> SI2.Authorized

		select @RowsAffected = @@ROWCOUNT, @now = getdate(), @LogMsg = @CodeLocation + ' Rows Affected: ' + cast(@@ROWCOUNT as varchar);
		exec dbo.AppLogInsertEntry @now, @LogAppID, @LogThread, @LogLevel, @LogAppName, @LogMsg, @LogExceptionMsg;
		PRINT '[' + convert(nvarchar, getdate(), 121) + '] ' + @LogMsg

		SELECT @CodeLocation = 'UPDATE [StoreItem]... alt store-subteams'
		
					UPDATE dbo.StoreItem
					SET Authorized = SI2.Authorized
					FROM dbo.StoreItem (NOLOCK)
						JOIN dbo.StoreItem SI2 (NOLOCK) ON StoreItem.Item_Key = SI2.Item_Key
						JOIN dbo.Item I (NOLOCK) on i.Item_Key = SI2.Item_Key
						JOIN (select Key_Value1, Key_Value2 FROM fn_Parse_List_Two(@StoreSubTeamSubstitutions, @StoreSubTeamSubstitutionsSeparator1, @StoreSubTeamSubstitutionsSeparator2) IL GROUP BY Key_Value1, Key_Value2) 
								as substores ON substores.Key_Value1 = SI2.Store_No and substores.Key_Value2 = I.Subteam_No
					WHERE StoreItem.Store_No = @NewStoreNo
						/*
							TFS 11919, Tom Lux, 2/11/2010: Removed "SI2.Store_No = @OldStoreNo" restriction here because this update
							intends to take attributes from the alternate store and subteam list, so this inapproriate criteria was
							causing no rows to be returned (no rows authed for alt stores and subteams).
						 */
						AND StoreItem.Authorized <> SI2.Authorized
			END

		select @RowsAffected = @@ROWCOUNT, @now = getdate(), @LogMsg = @CodeLocation + ' Rows Affected: ' + cast(@@ROWCOUNT as varchar);
		exec dbo.AppLogInsertEntry @now, @LogAppID, @LogThread, @LogLevel, @LogAppName, @LogMsg, @LogExceptionMsg;
		PRINT '[' + convert(nvarchar, getdate(), 121) + '] ' + @LogMsg


		----------------------------------------------------------------------
		-- Delete all the triggered dbo.pricebatchdetail records
		----------------------------------------------------------------------
		SELECT @CodeLocation = 'DELETE [PriceBatchDetail]...'
		select @now = getdate(); exec dbo.AppLogInsertEntry @now, @LogAppID, @LogThread, @LogLevel, @LogAppName, @CodeLocation, @LogExceptionMsg;

			DELETE [dbo].[PriceBatchDetail]
			WHERE store_no = @NewStoreNo

		select @RowsAffected = @@ROWCOUNT, @now = getdate(), @LogMsg = @CodeLocation + ' Rows Affected: ' + cast(@@ROWCOUNT as varchar);
		exec dbo.AppLogInsertEntry @now, @LogAppID, @LogThread, @LogLevel, @LogAppName, @LogMsg, @LogExceptionMsg;
		PRINT '[' + convert(nvarchar, getdate(), 121) + '] ' + @LogMsg


		----------------------------------------------
		-- Copy PriceBatchDetailRecords for ON/OFF Sales
		----------------------------------------------

		SELECT @CodeLocation = 'INSERT INTO [PriceBatchdetail]...'
		select @now = getdate(); exec dbo.AppLogInsertEntry @now, @LogAppID, @LogThread, @LogLevel, @LogAppName, @CodeLocation, @LogExceptionMsg;

		IF @OldStoreNo > 0
			BEGIN
				DECLARE @REG_PCT INT
				SELECT @REG_PCT= PriceChgTypeID from dbo.PriceChgType (NOLOCK) where On_Sale = 0

				/*
					Notes:
					The UNION below is to handle the case where we need to pull data from a different store-subteam.
					The differences are:
						WHERE PB.[Store_No] = @OldStoreNo
						vs.
						WHERE PB.[Store_No] <> @OldStoreNo
					in the top and bottom pieces of the UNION respectively,
					and
						AND PB.subteam_no not in (select Key_Value2 FROM fn_Parse_List_Two
						vs.
						AND PB.subteam_no in (select Key_Value2 FROM fn_Parse_List_Two
					in the top and bottom pieces of the UNION respectively.
				*/
				/*
					TFS 11641
					3.5.9
					Tom Lux
					2/18/10
					[Problem]
					The alt store-subteam code was not handling null PBD.subteam_no values
					when alt store-subteam pairs were passed in (select by user).
					Here's example for top query: "AND PB.subteam_no not in (%alt store-subteam list%)"
					The issue did not manifest itself if no alt store-subteam list was passed because
					it ended up being a "and null = null" statement.
					[Fixes]
					1) Added join to item table so when PBD.subteam_no is null, we can take item.subteam_no.
					2) Changed condition in alt store-subteam lines to be "isnull(PB.subteam_no, i.subteam_no)"
					so that items with null PBD.subteam_no fields are not unconditionally excluded.
					3) Changed (in both top and bottom queries) all selected fields to include explicit table reference,
					since item has many of the same columns.
					

				*/
				INSERT INTO [dbo].[PriceBatchDetail]
					([Item_Key], [Store_No], [ItemChgTypeID],[PriceChgTypeID],[StartDate],[Multiple],[Price],[MSRPPrice],
					 [MSRPMultiple],[PricingMethod_ID],[Sale_Multiple],[Sale_Price],[Sale_End_Date],[Sale_Max_Quantity],[Sale_Mix_Match],[Sale_Earned_Disc1],
					 [Sale_Earned_Disc2],[Sale_Earned_Disc3],[Case_Price],[Sign_Description],[Ingredients],[Identifier],[Sold_By_Weight],[SubTeam_No],[Origin_Name],
					 [Brand_Name],[Retail_Unit_Abbr],[Retail_Unit_Full],[Package_Unit],[Package_Desc1],[Package_Desc2],[Organic],[Vendor_Id],[ItemType_ID],
					 [ScaleDesc1],[ScaleDesc2],[POS_Description],[Restricted_Hours],[Quantity_Required],[Price_Required],[Retail_Sale],[Discountable],
					 [Food_Stamps],[IBM_Discount],[Hobart_Item],[PrintSign],[LineDrive],[POSPrice],[POSSale_Price],[Offer_ID],[AvgCostUpdated],
					  [NotAuthorizedForSale],[Deleted_Item],[User_ID], [User_ID_Date],[LabelType_ID],[OfferChgTypeID],[QtyProhibit],[GroupList],[PosTare],
					 [LinkedItem],[GrillPrint],[AgeCode],[VisualVerify],[SrCitizenDiscount],[AsOfDate],[AutoGenerated],[Expired],[POSLinkCode],
					 [InsertApplication],[RetailUnit_WeightUnit],[TagTypeID],[TagTypeID2], [LocalItem], [ItemSurcharge])
				SELECT 
					 pb.[Item_Key], @NewStoreNo, pb.[ItemChgTypeID],pb.[PriceChgTypeID],pb.[StartDate],pb.[Multiple],pb.[Price],pb.[MSRPPrice],
					 pb.[MSRPMultiple],pb.[PricingMethod_ID],pb.[Sale_Multiple],pb.[Sale_Price],pb.[Sale_End_Date],pb.[Sale_Max_Quantity],pb.[Sale_Mix_Match],pb.[Sale_Earned_Disc1],
					 pb.[Sale_Earned_Disc2],pb.[Sale_Earned_Disc3],pb.[Case_Price],pb.[Sign_Description],pb.[Ingredients],pb.[Identifier],pb.[Sold_By_Weight],pb.[SubTeam_No],pb.[Origin_Name],
					 pb.[Brand_Name],pb.[Retail_Unit_Abbr],pb.[Retail_Unit_Full],pb.[Package_Unit],pb.[Package_Desc1],pb.[Package_Desc2],pb.[Organic],pb.[Vendor_Id],pb.[ItemType_ID],
					 pb.[ScaleDesc1],pb.[ScaleDesc2],pb.[POS_Description],pb.[Restricted_Hours],pb.[Quantity_Required],pb.[Price_Required],pb.[Retail_Sale],pb.[Discountable],
					 pb.[Food_Stamps],pb.[IBM_Discount],pb.[Hobart_Item],pb.[PrintSign],pb.[LineDrive],pb.[POSPrice],pb.[POSSale_Price],pb.[Offer_ID],pb.[AvgCostUpdated],
					  pb.[NotAuthorizedForSale],pb.[Deleted_Item],pb.[User_ID],pb.[User_ID_Date],pb.[LabelType_ID],pb.[OfferChgTypeID],pb.[QtyProhibit],pb.[GroupList],pb.[PosTare],
					 pb.[LinkedItem],pb.[GrillPrint],pb.[AgeCode],pb.[VisualVerify],pb.[SrCitizenDiscount],pb.[AsOfDate],pb.[AutoGenerated],pb.[Expired],pb.[POSLinkCode],
					 'NEW STORE SCRIPT',pb.[RetailUnit_WeightUnit],pb.[TagTypeID],pb.[TagTypeID2], pb.[LocalItem], pb.[ItemSurcharge]
				FROM [PriceBatchDetail] PB (NOLOCK)
				-- TFS 11641, Tom Lux, 2/18/10, Added join to item so PBD rows with null subteam are not excluded when alt store-subteams are passed.  See other comments herein for full details.
				join [item] i (nolock)
					on pb.item_key = i.item_key
				-- The following line differs from the one in the next query.
				WHERE PB.[Store_No] = @OldStoreNo 
					AND ((@IncFutureSale = 1 AND StartDate > dateadd(day,1, getdate())) OR	--bring in upcoming sale end records
							(@IncFutureSale = 0 AND StartDate < dateadd(day,1, getdate()) AND Sale_End_Date > dateadd(day,1, getdate())) OR	
						(pb.pricebatchdetailid in (																	-- bring in last known regs
							SELECT TOP 1 pb2.pricebatchdetailid	
							FROM PriceBatchDetail pb2 (NOLOCK)
							WHERE pb.pricechgtypeid = @REG_PCT 
								AND pb2.item_key = pb.item_key 
								AND pb2.store_no = pb.store_no
							ORDER BY pb2.pricebatchdetailid DESC)))
					AND NOT EXISTS (SELECT *
									  FROM dbo.PriceBatchDetail (NOLOCK)
									  WHERE [Store_No] = @NewStoreNo
											  AND [Item_Key] = PB.[Item_Key]
											  AND ISNULL([PriceChgTypeID],0) = ISNULL(PB.[PriceChgTypeID],0)
											  AND ISNULL([ItemChgTypeID],0) = ISNULL(PB.[ItemChgTypeID],0)
											  AND startdate > dateadd(day, 1 , getdate()))
					-- TFS 11641, Tom Lux, 2/18/10, Added isnull() check on next line so null subteam are not excluded when alt store-subteams are passed.  See other comments herein for full details.
					-- The following line differs from the one in the next query.
					AND isnull(PB.subteam_no, i.subteam_no) not in (select Key_Value2 FROM fn_Parse_List_Two(@StoreSubTeamSubstitutions, @StoreSubTeamSubstitutionsSeparator1, @StoreSubTeamSubstitutionsSeparator2) IL GROUP BY Key_Value2)
					/*
						*** NOTE: This change is replicated in the next query as well (bottom/2nd-piece of the UNION).
						TFS 11641: New Store Creation tool does not copy over pending off sale records
						IRMA v3.5.9
						2010.01.22
						Tom Lux
						Fixed issue with the next line of code to conditionally include ISS price changes.
						Regarding the bug/TFS title, this PBD query DOES pull over the most recent REG/Sale-Off for all items,
						but the "bug" with the @IncSlim caused NO (zero) PBD rows to be pulled over to the new store when @IncSlim = 1.
						The new logic handles this option correctly.
						The -1 is used if we want to include them (@IncSlim=1) because 'PB.PriceChgTypeID <> -1' should then always be TRUE.
					*/
					AND PB.PriceChgTypeID <> case when @IncSlim = 0 then @ISSPriceChgTypeID else -1 end
					AND PB.InsertApplication <> CASE WHEN @IncPromoPlanner = 1 THEN 'PROMO PLANNER' ELSE 'NOT PROMO PLANNER' END
			UNION
				-- TFS 11641, Tom Lux, 2/18/10, Added explicit table reference for all fields due to new item-table join.  See other comments herein for full details.
				SELECT 
					 pb.[Item_Key], @NewStoreNo, pb.[ItemChgTypeID],pb.[PriceChgTypeID],pb.[StartDate],pb.[Multiple],pb.[Price],pb.[MSRPPrice],
					 pb.[MSRPMultiple],pb.[PricingMethod_ID],pb.[Sale_Multiple],pb.[Sale_Price],pb.[Sale_End_Date],pb.[Sale_Max_Quantity],pb.[Sale_Mix_Match],pb.[Sale_Earned_Disc1],
					 pb.[Sale_Earned_Disc2],pb.[Sale_Earned_Disc3],pb.[Case_Price],pb.[Sign_Description],pb.[Ingredients],pb.[Identifier],pb.[Sold_By_Weight],pb.[SubTeam_No],pb.[Origin_Name],
					 pb.[Brand_Name],pb.[Retail_Unit_Abbr],pb.[Retail_Unit_Full],pb.[Package_Unit],pb.[Package_Desc1],pb.[Package_Desc2],pb.[Organic],pb.[Vendor_Id],pb.[ItemType_ID],
					 pb.[ScaleDesc1],pb.[ScaleDesc2],pb.[POS_Description],pb.[Restricted_Hours],pb.[Quantity_Required],pb.[Price_Required],pb.[Retail_Sale],pb.[Discountable],
					 pb.[Food_Stamps],pb.[IBM_Discount],pb.[Hobart_Item],pb.[PrintSign],pb.[LineDrive],pb.[POSPrice],pb.[POSSale_Price],pb.[Offer_ID],pb.[AvgCostUpdated],
					  pb.[NotAuthorizedForSale],pb.[Deleted_Item],pb.[User_ID],pb.[User_ID_Date],pb.[LabelType_ID],pb.[OfferChgTypeID],pb.[QtyProhibit],pb.[GroupList],pb.[PosTare],
					 pb.[LinkedItem],pb.[GrillPrint],pb.[AgeCode],pb.[VisualVerify],pb.[SrCitizenDiscount],pb.[AsOfDate],pb.[AutoGenerated],pb.[Expired],pb.[POSLinkCode],
					 'NEW STORE SCRIPT',pb.[RetailUnit_WeightUnit],pb.[TagTypeID],pb.[TagTypeID2],pb.[LocalItem],pb.[ItemSurcharge]
				FROM [PriceBatchDetail] PB (NOLOCK)
				-- TFS 11641, Tom Lux, 2/18/10, Added join to item so PBD rows with null subteam are not excluded when alt store-subteams are passed.  See other comments herein for full details.
				join [item] i (nolock)
					on pb.item_key = i.item_key
				inner join 
					(select Key_Value1 as Store_No, Key_Value2 as Subteam_No 
					FROM fn_Parse_List_Two(@StoreSubTeamSubstitutions, @StoreSubTeamSubstitutionsSeparator1, @StoreSubTeamSubstitutionsSeparator2)
					) SUB on SUB.Subteam_No = isnull(PB.subteam_no, i.subteam_no) and SUB.Store_No = PB.Store_No

				WHERE PB.[Store_No] <> @OldStoreNo 
				
					AND ((@IncFutureSale = 1 AND StartDate > dateadd(day,1, getdate())) OR	--bring in upcoming sale end records
							(@IncFutureSale = 0 AND StartDate < dateadd(day,1, getdate()) AND Sale_End_Date > dateadd(day,1, getdate())) OR	
						(pb.pricebatchdetailid in (																	-- bring in last known regs
							SELECT TOP 1 pb2.pricebatchdetailid	
							FROM PriceBatchDetail pb2 (NOLOCK)
							WHERE pb.pricechgtypeid = @REG_PCT 
								AND pb2.item_key = pb.item_key 
								AND pb2.store_no = pb.store_no
							ORDER BY pb2.pricebatchdetailid DESC)))
					AND NOT EXISTS (SELECT *
									  FROM dbo.PriceBatchDetail (NOLOCK)
									  WHERE [Store_No] = @NewStoreNo
											  AND [Item_Key] = PB.[Item_Key]
											  AND ISNULL([PriceChgTypeID],0) = ISNULL(PB.[PriceChgTypeID],0)
											  AND ISNULL([ItemChgTypeID],0) = ISNULL(PB.[ItemChgTypeID],0)
											  AND startdate > dateadd(day, 1 , getdate()))
					-- @IncSlim fix.  See comment in top query of the above UNION.
					AND PB.PriceChgTypeID <> case when @IncSlim = 0 then @ISSPriceChgTypeID else -1 end
					AND PB.InsertApplication <> CASE WHEN @IncPromoPlanner = 1 THEN 'PROMO PLANNER' ELSE 'NOT PROMO PLANNER' END
				ORDER BY PB.Item_Key, PB.SubTeam_No
		END

		select @RowsAffected = @@ROWCOUNT, @now = getdate(), @LogMsg = @CodeLocation + ' Rows Affected: ' + cast(@@ROWCOUNT as varchar);
		exec dbo.AppLogInsertEntry @now, @LogAppID, @LogThread, @LogLevel, @LogAppName, @LogMsg, @LogExceptionMsg;
		PRINT '[' + convert(nvarchar, getdate(), 121) + '] ' + @LogMsg


        ----------------------------------------------------------------------
        -- add a default SLIM e-mail entries
        ----------------------------------------------------------------------
        SELECT @CodeLocation = 'INSERT INTO [SlimEmail]...'
		select @now = getdate(); exec dbo.AppLogInsertEntry @now, @LogAppID, @LogThread, @LogLevel, @LogAppName, @CodeLocation, @LogExceptionMsg;

		IF @OldStoreNo > 0
			BEGIN
				INSERT INTO [dbo].[SlimEmail] 
					([Store_No], [Team_No], [TeamLeader_email], [BA_email], [Other_email], [Insert_Date])
				SELECT @NewStoreNo, [Team_No], [TeamLeader_email], [BA_email], [Other_email], GetDate()
				FROM [SlimEmail] SE (NOLOCK)
				WHERE SE.Store_No = @OldStoreNo
						AND NOT EXISTS (SELECT *
										FROM [SlimEmail] (NOLOCK)
										WHERE Store_No = @NewStoreNo
											AND Team_No = SE.Team_No)
			END

		select @RowsAffected = @@ROWCOUNT, @now = getdate(), @LogMsg = @CodeLocation + ' Rows Affected: ' + cast(@@ROWCOUNT as varchar);
		exec dbo.AppLogInsertEntry @now, @LogAppID, @LogThread, @LogLevel, @LogAppName, @LogMsg, @LogExceptionMsg;
		PRINT '[' + convert(nvarchar, getdate(), 121) + '] ' + @LogMsg


		--------------------------------------------------------------------------
        -- Copy ItemUomOverride entries from the current store to the new store
        --------------------------------------------------------------------------
		SELECT @CodeLocation = 'INSERT [ItemUomOverride]...'
		select @now = getdate(); exec dbo.AppLogInsertEntry @now, @LogAppID, @LogThread, @LogLevel, @LogAppName, @CodeLocation, @LogExceptionMsg;

			IF @OldStoreNo > 0
				BEGIN
					insert into
						dbo.ItemUomOverride
					select
						iuo.Item_Key,
						@NewStoreNo,
						iuo.Scale_ScaleUomUnit_ID,
						iuo.Scale_FixedWeight,
						iuo.Scale_ByCount,
						iuo.Retail_Unit_ID
					from
						dbo.ItemUomOverride iuo
					where
						iuo.Store_No = @OldStoreNo
				END

		select @RowsAffected = @@ROWCOUNT, @now = getdate(), @LogMsg = @CodeLocation + ' Rows Affected: ' + cast(@@ROWCOUNT as varchar);
		exec dbo.AppLogInsertEntry @now, @LogAppID, @LogThread, @LogLevel, @LogAppName, @LogMsg, @LogExceptionMsg;
		PRINT '[' + convert(nvarchar, getdate(), 121) + '] ' + @LogMsg


        ----------------------------------------------
        -- Commit the transaction
        ----------------------------------------------
        IF @@TRANCOUNT > 0
                COMMIT TRANSACTION

        PRINT REPLACE(SPACE(120), SPACE(1), '-') + CHAR(13) + CHAR(10)
                + 'Successfully added data associated with the store ''' + @NewStoreName + '''' + CHAR(13) + CHAR(10)
                + REPLACE(SPACE(120), SPACE(1), '-')

		PRINT 'Use the ADMIN client to add store records for the following information:' + CHAR(13) + CHAR(10)
			+ CHAR(9) + CHAR(149) + ' POS System,' + CHAR(13) + CHAR(10)
			+ CHAR(9) + CHAR(149) + ' File Writer type,' + CHAR(13) + CHAR(10)
			+ CHAR(9) + CHAR(149) + ' Scale Writer type,' + CHAR(13) + CHAR(10)
			+ CHAR(9) + CHAR(149) + ' Tag Writer type,' + CHAR(13) + CHAR(10)
			+ CHAR(9) + CHAR(149) + ' Acknowledgement type,' + CHAR(13) + CHAR(10)
			+ CHAR(9) + CHAR(149) + ' FTP Info for writers,' + CHAR(13) + CHAR(10)
			+ CHAR(9) + CHAR(149) + ' Overrides to Instance Data Flags'

END TRY
--===============================================================================================
BEGIN CATCH
        ----------------------------------------------
        -- Rollback the transaction
        ----------------------------------------------
        IF @@TRANCOUNT > 0
                ROLLBACK TRANSACTION

        ----------------------------------------------
        -- Display a detailed error message
        ----------------------------------------------
        PRINT REPLACE(SPACE(120), SPACE(1), '-') + CHAR(13) + CHAR(10)
                + 'Error ' + CONVERT(varchar, ERROR_NUMBER()) + ': ' + ERROR_MESSAGE() + CHAR(13) + CHAR(10)
                + CHAR(9) + ' at statement  ''' + @CodeLocation + ''' (' + ISNULL(ERROR_PROCEDURE() + ', ', '') + 'line ' + CONVERT(varchar, ERROR_LINE()) + ')' + CHAR(13) + CHAR(10)
                + REPLACE(SPACE(120), SPACE(1), '-') + CHAR(13) + CHAR(10)
                + 'Database changes were rolled back.' + CHAR(13) + CHAR(10)
                + REPLACE(SPACE(120), SPACE(1), '-')

    SELECT
        ERROR_NUMBER() AS ErrorNumber,
        ERROR_SEVERITY() AS ErrorSeverity,
        ERROR_STATE() AS ErrorState,
        ERROR_PROCEDURE() AS ErrorProcedure,
        ERROR_LINE() AS ErrorLine,
        ERROR_MESSAGE() AS ErrorMessage
END CATCH

PRINT '[' + convert(nvarchar, getdate(), 121) + '] ' + 'Done.'
--===============================================================================================
--
END
SET NOCOUNT OFF

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[InsertCloneStore] TO [IRMAAdminRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[InsertCloneStore] TO [IRMAClientRole]
    AS [dbo];

