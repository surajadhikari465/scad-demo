alter PROCEDURE [dbo].[EIM_UploadSession_ExistingItems]
	@UploadSession_ID			integer,
	@UploadRow_ID				int,
	@RetryCount					int,
	@Item_key					int,
	@FourLevelHierarchyFlag		bit,
	@UseStoreJurisdictions		bit,
	@LoggingLevel				varchar(10)

AS

-- ******************************************************
-- Called by EIM to update existing items.
--
-- David Marine
--
-- ******************************************************
	
/*********************************************************************************************
CHANGE LOG
DEV		DATE		TASK	Description
----------------------------------------------------------------------------------------------
DBS		20110125	1241	Merge Up FSA Changes
BAS		20130101	8755	Removed Discontinue_Item as it has moved to EIM_UploadSessionPrice
KM		2013-01-08	9251	Populate internal variables with ItemOverride values where available;
KM		2013-01-22	9394	Populate internal variables with ItemScaleOverride values where available;
KM		2013-01-25	9382	More ItemScaleOverride values;
KM		2013-01-25	9393	Add Nutrifact_ID to the list of scale override values;
KM		2014-04-04	14939	Only update canonical fields if the item's default identifier is not validated in Icon;
KM		2014-06-20	15195	Fix bug where OUTPUT parameter was not correctly applied to @BlockCanonicalFieldUpdate;
KM		2015-01-14	15737	Before doing the Item table update, check to see if subteam and category changes
							should be allowed or not;
KM		2015-02-09	15784 (7188)	Allow subteam and category updates for non-aligned subteams;
KM		2015-04-20	16067 (8354)	Allow non-retail items to move across aligned subteams;
KM		2015-05-21	16168 (7296)	Override the canonical field blocking for alternate jurisdiction updates;
MZ      2015-06-16  16200 (9290)    Block canonical fields update for validated items on their alternate jurisdiction records;
MZ      2015-06-23  16214 (9634)    Do not wipe out existing Nutrifact_Id and Scale_FixedWeight values 
                                    on updated item's alternate jurisdiction record.
MZ      2015-08-19  16352 (10976)   Do NOT allow retail sail flag to be set to true if the item to be updated is linked
                                    to an ingredient identifier (defined by ranges)
KM		2015-09-15	11338			Include the 3 sign attributes (Locality, Long & Short Romance) in the update;
KM		2015-09-23	11338			More sign attributes;
MU/MZ	2016-03-18	TFS18686	
						PBI13711	Adding Sold By WFM and Sold By 365 to accommodate setting via EIM
MU/MZ   2016-03-22  TFS18729
						PBI14651    Disallow Organic field to be updated for validated items
MZ      2017-07-21  PBI22360        Added two alternate jurisdiction fields Sign Romance Short and Sign Romance Long 
                                    to EIM
MZ      2017-12-18  PBI22360        Added logic to only allow Retail Size and Retail UOM update for non-validated items, 
                                    or validated produce items if the region doesn't have any stores on GPM. 
									If a region has a store on GPM, the Retail Size and Retail UOM update will be blocked
									after the item is validated.
EM      2018-08-16  PBI28634        Added logic to disallow Sign Caption updates if the validated Customer Friendly Description is in use
***********************************************************************************************/
	
	SET NOCOUNT ON

	DECLARE
		@Tablename varchar(200),
		@ColumnName varchar(200),
		@ColumnValue varchar(4200),
		@ColumnDbDataType varchar(200),
		@UploadValue_ID int,
		@Existing_ItemScale_ID int,
		@CheckForItemAttributeRow bit,
		@TableAndColumnName varchar(200),
		@FlexibleAttributeUpdateSQL varchar(8000),
		@IsScaleItem bit	
	
	DECLARE
		-- item data
		@POS_Description varchar(26), 
		@Item_Description varchar(60), 
		@Sign_Description varchar(60),    
		@Min_Temperature smallint, 
		@Max_Temperature smallint,
		@Average_Unit_Weight decimal(9,4),     
		@Package_Desc1 decimal(9,4), 
		@Package_Desc2 decimal(9,4), 
		@Package_Unit_ID int,     
		@Retail_Unit_ID int, 
		@SubTeam_No int, 
		@SubTeam_Name varchar(100),
		@Brand_ID int, 
		@Category_ID int, 
		@Origin_ID int, 
		@Retail_Sale bit, 
		@Keep_Frozen bit, 
		@Full_Pallet_Only bit, 
		@Shipper_Item bit, 
		@WFM_Item bit, 
		@Units_Per_Pallet int, 
		@Vendor_Unit_ID int,
		@Distribution_Unit_ID int,    
		@Tie tinyint,
		@High tinyint,
		@Yield decimal(9,4),
		@NoDistMarkup  bit,
		@Organic bit,
		@Refrigerated bit,
		@Not_Available bit,
		@Pre_Order bit,
		@ItemType_ID int,
		@Sales_Account varchar(6),
		@HFM_Item tinyint,   
		@Not_AvailableNote varchar(255),
		@CountryProc_ID int,
		@Manufacturing_Unit_ID int,
		@EXEDistributed bit,
		@NatClassID int, 
		@DistSubTeam_No int,
		@CostedByWeight bit,
		@TaxClassID int,
		@LabelType_ID int,
		@User_ID_Date varchar(255),
		@User_ID int,
		@Manager_ID tinyint,
		@Recall_Flag bit,
		@ProdHierarchyLevel4_ID int,
		@LockAuth_Flag bit,
		@PurchaseThresholdCouponSubTeam bit,
		@PurchaseThresholdCouponAmount smallmoney,
		@IsDefaultJurisdiction bit,
		@StoreJurisdictionID int,
		@HandlingChargeOverride smallmoney,
		@CatchWeightRequired bit,
		@COOL bit,
		@BIO bit,
		@Ingredient bit,
		@SustainabilityRankingRequired bit,
		@SustainabilityRankingID int,
		@UseLastReceivedCost bit,

		-- scale data
		@ScaleDesc1 varchar(64),
		@ScaleDesc2 varchar(64),
		@ScaleDesc3 varchar(64),
		@ScaleDesc4 varchar(64),
		@Ingredients varchar(3500),
		@Item_ShelfLife_Length smallint,
		@ShelfLife_ID int,	
		@Tare int,
		@UseBy int,
		@ForcedTare char(1),
		@Product_Code varchar(15),
		@Unit_Price_Category int,

		-- *new* scale data
		@Nutrifact_ID int,
		@Scale_ExtraText_ID int,
		@New_Scale_ExtraText_ID int,
		@Scale_StorageData_ID int,
		@New_Scale_StorageData_ID int,
		@Scale_Allergen_ID int,
		@New_Scale_Allergen_ID int,
		@Scale_Ingredient_ID int,
		@New_Scale_Ingredient_ID int,
		@Scale_Tare_ID int,
		@Scale_Alternate_Tare_ID int,
		@Scale_LabelStyle_ID int,
		@Scale_EatBy_ID smallint,
		@Scale_Grade_ID int,	
		@Scale_RandomWeightType_ID int,
		@Scale_ScaleUOMUnit_ID int,
		@Scale_FixedWeight varchar(25),
		@Scale_ByCount int,
		@ForceTare bit,
		@PrintBlankShelfLife bit,	
		@PrintBlankEatBy bit,
		@PrintBlankPackDate bit,
		@PrintBlankWeight bit,
		@PrintBlankUnitPrice bit,
		@PrintBlankTotalPrice bit,
		@ShelfLife_Length smallint,
		
		-- extra text data
		@Scale_LabelType_ID int,
		@ExtraTextDescription varchar(50),
		@ExtraText varchar(4200),

		-- Storage data
		@StorageDescription varchar(50),
		@StorageData varchar(1024),

		-- Scale Allergens
		@ScaleAllergen_LabelType_ID int,
		@ScaleAllergenDescription varchar(50),
		@ScaleAllergen varchar(4200),

		-- Scale Ingredients
		@ScaleIngredient_LabelType_ID int,
		@ScaleIngredientDescription varchar(50),
		@ScaleIngredient varchar(4200),

		-- pos data
		@Food_Stamps bit,
		@Price_Required bit,
		@Quantity_Required bit, 
		@QtyProhibit bit,
		@GroupList int,
		@Case_Discount bit,
		@Coupon_Multiplier bit,
		@FSA_Eligible bit,
		@Misc_Transaction_Sale smallint,
		@Misc_Transaction_Refund smallint,
		@Ice_Tare int,
		
		-- chain ids
		@ItemChainIDs varchar(1000),
		@ChainId int,

		-- sign attributes
		@Locality varchar(50),
		@ShortSignRomance varchar(140),
		@LongSignRomance varchar(300),
		@ShortSignRomanceAlt varchar(140),
		@LongSignRomanceAlt varchar(300),
		@ChicagoBaby varchar(50),
		@TagUom int,
		@Exclusive date,
		@ColorAdded bit,

		-- check if the item is non-retail ingredient item that needs to be in Icon
		@IsNonRetailItemInIcon bit 
		
		SELECT @User_ID = CreatedByUserID FROM UploadSession WHERE UploadSession_ID = @UploadSession_ID
		SELECT @User_ID_Date = GETDATE()
										
		EXEC dbo.EIM_Log @LoggingLevel, 'TRACE', @UploadSession_ID, @UploadRow_ID, @RetryCount, @Item_key, NULL, '3.0 Update Existing Item - [Begin]'
		
		-- initialize this flag for each row
		SET @CheckForItemAttributeRow = 1		
			
		-- pre-populate data variables
		DECLARE @IsDefaultJurisdictionValue varchar(10),
			@StoreJurisdictionIDValue varchar(10)

		-- get the jurtisdiction values from the upload
		SELECT @IsDefaultJurisdictionValue = uv.Value
			FROM UploadValue (NOLOCK) uv
				inner join UploadAttribute ua (NOLOCK) 
				on uv.UploadAttribute_ID = ua.UploadAttribute_ID
				inner join UploadTypeAttribute uta (NOLOCK) 
				on ua.UploadAttribute_ID = uta.UploadAttribute_ID
			WHERE uv.UploadRow_ID = @UploadRow_ID and uta.uploadtype_code = 'ITEM_MAINTENANCE'
				AND LOWER(ua.TableName) = 'item'
				AND LOWER(ua.ColumnNameorKey) = LOWER('IsDefaultJurisdiction')
		
		SELECT @StoreJurisdictionIDValue = uv.Value
			FROM UploadValue (NOLOCK) uv
				inner join UploadAttribute ua (NOLOCK) 
				on uv.UploadAttribute_ID = ua.UploadAttribute_ID
				inner join UploadTypeAttribute uta (NOLOCK) 
				on ua.UploadAttribute_ID = uta.UploadAttribute_ID
			WHERE uv.UploadRow_ID = @UploadRow_ID and uta.uploadtype_code = 'ITEM_MAINTENANCE'
				AND LOWER(ua.TableName) = 'item'
				AND LOWER(ua.ColumnNameorKey) = LOWER('StoreJurisdictionID')

		EXEC dbo.EIM_Log @LoggingLevel, 'TRACE', @UploadSession_ID, @UploadRow_ID, @RetryCount, @Item_key, NULL, '3.0.1 Update Existing Item - [Get Initial Jurisdictional Data]'

		SET @IsDefaultJurisdiction = CAST(@IsDefaultJurisdictionValue as bit)
		SET @StoreJurisdictionID = CAST(@StoreJurisdictionIDValue as int)

		IF @UseStoreJurisdictions = 0
		BEGIN
			SELECT @StoreJurisdictionID = (SELECT TOP 1 StoreJurisdictionID
				FROM StoreJurisdiction)
		END
		
		If @UseStoreJurisdictions = 1 AND @IsDefaultJurisdiction = 0
		BEGIN
			SELECT
				@IsScaleItem =
					CASE WHEN (SELECT COUNT(*) FROM ItemIdentifier (NOLOCK) WHERE Item_Key = @Item_Key AND (dbo.fn_IsScaleItem(Identifier) = 1)) > 0 THEN 1 ELSE 0 END

			SELECT
				@Item_Description = Item_Description,
				@Sign_Description = Sign_Description,
				@Package_Desc1 = Package_Desc1,
				@Package_Desc2 = Package_Desc2,
				@Package_Unit_ID = Package_Unit_ID,
				@Retail_Unit_ID = Retail_Unit_ID,
				@Vendor_Unit_ID = Vendor_Unit_ID,
				@Distribution_Unit_ID = Distribution_Unit_ID,
				@POS_Description = POS_Description,
				@Food_Stamps = Food_Stamps,
				@Price_Required = Price_Required,
				@Quantity_Required = Quantity_Required,
				@Manufacturing_Unit_ID = Manufacturing_Unit_ID,
				@QtyProhibit = QtyProhibit,
				@GroupList = GroupList,
				@Case_Discount = Case_Discount,
				@Coupon_Multiplier = Coupon_Multiplier,
				@Misc_Transaction_Sale = Misc_Transaction_Sale,
				@Misc_Transaction_Refund = Misc_Transaction_Refund,
				@Ice_Tare = Ice_Tare,
				@Brand_Id = Brand_ID,
				@Origin_Id = Origin_ID,
				@CountryProc_Id = CountryProc_ID,
				@SustainabilityRankingRequired = SustainabilityRankingRequired,
				@SustainabilityRankingID = SustainabilityRankingID,
				@LabelType_ID = LabelType_ID,
				@CostedByWeight = CostedByWeight,
				@Average_Unit_Weight = Average_Unit_Weight,                           
				@Ingredient = Ingredient,                                  
				@Recall_Flag = Recall_Flag,                                 
				@LockAuth_Flag = LockAuth,                                   
				@Not_Available = Not_Available,                                
				@Not_AvailableNote = Not_AvailableNote,                            
				@FSA_Eligible = FSA_Eligible,                                  
				@Product_Code = Product_Code,                                  
				@Unit_Price_Category = Unit_Price_Category,
				@ShortSignRomanceAlt = SignRomanceTextShort,
			    @LongSignRomanceAlt = SignRomanceTextLong

			FROM 
				ItemOverride (NOLOCK)
			
			WHERE 
				Item_Key = @Item_Key AND StoreJurisdictionID = @StoreJurisdictionID

			-- prepopulate scale data			
			SELECT
					@ScaleDesc1 = Scale_Description1,
					@ScaleDesc2 = Scale_Description2,
					@ScaleDesc3 = Scale_Description3,
					@ScaleDesc4 = Scale_Description4,
					@Scale_ExtraText_ID = Scale_ExtraText_ID,
					@Scale_Tare_ID = @Scale_Tare_ID,
					@Scale_LabelStyle_ID = Scale_LabelStyle_ID,
					@Scale_ScaleUOMUnit_ID = Scale_ScaleUOMUnit_ID,
					@Scale_FixedWeight = Scale_FixedWeight,
					@ForceTare = ForceTare,
					@Scale_Alternate_Tare_ID = Scale_Alternate_Tare_ID,
					@Scale_EatBy_ID = Scale_EatBy_ID,
					@Scale_Grade_ID = Scale_Grade_ID,
					@PrintBlankEatBy = PrintBlankEatBy,
					@PrintBlankPackDate = PrintBlankPackDate,
					@PrintBlankShelfLife = PrintBlankShelfLife,
					@PrintBlankTotalPrice = PrintBlankTotalPrice,
					@PrintBlankUnitPrice = PrintBlankUnitPrice,
					@PrintBlankWeight = PrintBlankWeight,
					@Nutrifact_ID = Nutrifact_ID
					
			FROM 
				ItemScaleOverride (NOLOCK)
			WHERE 
				Item_Key = @Item_Key AND StoreJurisdictionID = @StoreJurisdictionID
				
			IF @Scale_ExtraText_ID IS NOT NULL
			BEGIN
				
				SELECT @ExtraTextDescription = Description
				FROM dbo.Scale_ExtraText (NOLOCK)
				WHERE Scale_ExtraText_ID = @Scale_ExtraText_ID	
			END

			IF @Item_Description IS NULL
			BEGIN
				SELECT 
					@POS_Description = POS_Description,
					@Item_Description = Item_Description,
					@Package_Desc1 = Package_Desc1,
					@Brand_ID = Brand_ID,
					@TaxClassID = TaxClassID,
					@Food_Stamps = Food_Stamps
				FROM 
					Item (NOLOCK)
				WHERE 
					Item_Key = @Item_Key
			END
		END
		ELSE
		BEGIN
			SELECT
				-- item data
				@IsScaleItem =
					CASE WHEN (SELECT COUNT(*) FROM ItemIdentifier (NOLOCK) WHERE Item_Key = @Item_Key AND (dbo.fn_IsScaleItem(Identifier) = 1)) > 0 THEN 1 ELSE 0 END,
				@POS_Description = POS_Description,
				@Item_Description = Item_Description,
				@Sign_Description = Sign_Description,
				@Min_Temperature = Min_Temperature,
				@Max_Temperature = Max_Temperature,
				@Average_Unit_Weight = Average_Unit_Weight,
				@Package_Desc1 = Package_Desc1,
				@Package_Desc2 = Package_Desc2,
				@Package_Unit_ID = Package_Unit_ID,
				@Retail_Unit_ID = Retail_Unit_ID,
				@SubTeam_No = SubTeam_No,
				@Brand_ID = Brand_ID,
				@Category_ID = Category_ID,
				@Origin_ID = Origin_ID,
				@Retail_Sale = Retail_Sale,
				@Keep_Frozen = Keep_Frozen,
				@Full_Pallet_Only = Full_Pallet_Only,
				@Shipper_Item = Shipper_Item,
				@WFM_Item = WFM_Item,
				@Units_Per_Pallet = Units_Per_Pallet,
				@Vendor_Unit_ID = Vendor_Unit_ID,
				@Distribution_Unit_ID = Distribution_Unit_ID,
				@Tie = Tie,
				@High = High,
				@Yield = Yield,
				@NoDistMarkup = NoDistMarkup,
				@Organic = Organic,
				@Refrigerated = Refrigerated,
				@Not_Available = Not_Available,
				@Pre_Order = Pre_Order,
				@ItemType_ID = ItemType_ID,
				@Sales_Account = Sales_Account,
				@HFM_Item = HFM_Item,
				@Not_AvailableNote = Not_AvailableNote,
				@CountryProc_ID = CountryProc_ID,
				@Manufacturing_Unit_ID = Manufacturing_Unit_ID,
				@EXEDistributed = EXEDistributed,
				@NatClassID = ClassID,
				@DistSubTeam_No = DistSubTeam_No,
				@CostedByWeight = CostedByWeight,
				@TaxClassID = TaxClassID,
				@LabelType_ID = LabelType_ID,
				@Manager_ID = Manager_ID,
				@Recall_Flag = Recall_Flag,
				@ProdHierarchyLevel4_ID = ProdHierarchyLevel4_ID,
				@LockAuth_Flag = LockAuth,
				@PurchaseThresholdCouponSubTeam = PurchaseThresholdCouponSubTeam,
				@PurchaseThresholdCouponAmount = PurchaseThresholdCouponAmount,
				@CatchWeightRequired = CatchWeightRequired,
				@COOL = COOL,
				@BIO = BIO,
				@Ingredient = Ingredient,
				@SustainabilityRankingRequired = SustainabilityRankingRequired,
				@SustainabilityRankingID = SustainabilityRankingID,
				@UseLastReceivedCost = UseLastReceivedCost,

				-- scale data
				@Item_Key = Item_Key,
				@ScaleDesc1 = ScaleDesc1,
				@ScaleDesc2 = ScaleDesc2,
				@ScaleDesc3 = ScaleDesc3,
				@ScaleDesc4 = ScaleDesc4,
				@Ingredients = Ingredients,
				@Item_ShelfLife_Length = ShelfLife_Length,
				@ShelfLife_ID = ShelfLife_ID,
				@Tare = ScaleTare,
				@UseBy = ScaleUseBy,
				@ForcedTare = ScaleForcedTare,

				-- pos data
				@Item_Key = Item_Key,
				@Food_Stamps = Food_Stamps,
				@Price_Required = Price_Required,
				@Quantity_Required = Quantity_Required,
				@QtyProhibit = QtyProhibit,
				@GroupList = GroupList,
				@Case_Discount = Case_Discount,
				@Coupon_Multiplier = Coupon_Multiplier,
				@FSA_Eligible = FSA_Eligible,
				@Misc_Transaction_Sale = Misc_Transaction_Sale,
				@Misc_Transaction_Refund = Misc_Transaction_Refund,
				@Ice_Tare = Ice_Tare,
				@Product_Code = Product_Code,
				@Unit_Price_Category = Unit_Price_Category

			FROM 
				Item (NOLOCK)
			WHERE 
				Item_Key = @Item_Key
			
			SELECT 
			   @SubTeam_Name = st.SubTeam_Name
			FROM 
				Item i (NOLOCK)
			INNER JOIN 
				SubTeam st (NOLOCK) ON i.SubTeam_No = st.SubTeam_No
			WHERE 
				Item_Key = @Item_Key

			-- prepopulate scale data			
			SELECT
				@Existing_ItemScale_ID = ItemScale_ID,
				@Nutrifact_ID = Nutrifact_ID,
				@Scale_ExtraText_ID = Scale_ExtraText_ID,
				@Scale_StorageData_ID = Scale_StorageData_ID,
				@Scale_Allergen_ID = Scale_Allergen_ID,
				@Scale_Ingredient_ID = Scale_Ingredient_ID,
				@Scale_Tare_ID = Scale_Tare_ID,
				@Scale_Alternate_Tare_ID = Scale_Alternate_Tare_ID,
				@Scale_LabelStyle_ID = Scale_LabelStyle_ID,
				@Scale_EatBy_ID = Scale_EatBy_ID,
				@Scale_Grade_ID = Scale_Grade_ID,
				@Scale_RandomWeightType_ID = Scale_RandomWeightType_ID,
				@Scale_ScaleUOMUnit_ID = Scale_ScaleUOMUnit_ID,
				@Scale_FixedWeight = Scale_FixedWeight,
				@Scale_ByCount = Scale_ByCount,
				@ForceTare = ForceTare,
				@PrintBlankShelfLife = PrintBlankShelfLife,
				@PrintBlankEatBy = PrintBlankEatBy,
				@PrintBlankPackDate = PrintBlankPackDate,
				@PrintBlankWeight = PrintBlankWeight,
				@PrintBlankUnitPrice = PrintBlankUnitPrice,
				@PrintBlankTotalPrice = PrintBlankTotalPrice,
				@ScaleDesc1 = Scale_Description1,
				@ScaleDesc2 = Scale_Description2,
				@ScaleDesc3 = Scale_Description3,
				@ScaleDesc4 = Scale_Description4,
				@ShelfLife_Length = ShelfLife_Length
			FROM 
				ItemScale (NOLOCK)
			WHERE
				Item_Key = @Item_Key
					
			IF @Scale_ExtraText_ID IS NOT NULL
			BEGIN
				SELECT @ExtraTextDescription = Description
				FROM dbo.Scale_ExtraText (NOLOCK)
				WHERE Scale_ExtraText_ID = @Scale_ExtraText_ID	
			END

			IF @Scale_StorageData_ID IS NOT NULL
			BEGIN
				SELECT @StorageDescription = Description
				FROM dbo.Scale_StorageData (NOLOCK)
				WHERE Scale_StorageData_ID = @Scale_StorageData_ID	
			END

			IF @Scale_Allergen_ID IS NOT NULL
			BEGIN
				SELECT @ScaleAllergenDescription = Description
				FROM dbo.Scale_Allergen (NOLOCK)
				WHERE Scale_Allergen_ID = @Scale_Allergen_ID	
			END

			IF @Scale_Ingredient_ID IS NOT NULL
			BEGIN
				SELECT @ScaleIngredientDescription = Description
				FROM dbo.Scale_Ingredient (NOLOCK)
				WHERE Scale_Ingredient_ID = @Scale_Ingredient_ID	
			END
		END
		
		SELECT @HandlingChargeOverride = dbo.fn_GetFacilityHandlingChargeOverride(@Item_Key,null)

		-- populate chain ids
		SELECT
			@ItemChainIDs = dbo.fn_EIM_GetListOfItemChains(@Item_Key)

		-- populate sign attributes
		SELECT
			@Locality = isa.Locality,
			@ShortSignRomance = isa.SignRomanceTextShort,
			@LongSignRomance = isa.SignRomanceTextLong,
			@ChicagoBaby = isa.UomRegulationChicagoBaby,
			@TagUom = isa.UomRegulationTagUom,
			@Exclusive = isa.Exclusive,
			@ColorAdded = isa.ColorAdded
		FROM
			ItemSignAttribute (nolock) isa
		WHERE
			isa.Item_Key = @Item_key
			
		EXEC dbo.EIM_Log @LoggingLevel, 'TRACE', @UploadSession_ID, @UploadRow_ID, @RetryCount, @Item_key, NULL, '3.0.2 Update Existing Item - [Preload Existing Data]'

		-- Determine if the item is validated in Icon.  This is the only check necessary to determine whether or not to enforce canonical field lockdown.
		DECLARE @BlockCanonicalFieldUpdate bit
		EXEC dbo.IsValidatedItemInIcon @Item_key, @BlockCanonicalFieldUpdate OUTPUT

		DECLARE @RegionalFlagValue bit, @NumStoresWithOverrides int, @NumStoresTotal int, @pricesAreGloballyManaged bit
		 SELECT 
			@RegionalFlagValue = RegionalFlagValue,
			@NumStoresWithOverrides = NumStoresWithOverrides,
			@NumStoresTotal = NumStoresTotal
		 FROM dbo.IDF_OverrideStoreCountView 
		WHERE FlagKey = 'GlobalPriceManagement'

		IF @RegionalFlagValue = 1 AND @NumStoresWithOverrides < @NumStoresTotal
			SET @pricesAreGloballyManaged = 1
		ELSE IF @RegionalFlagValue = 0 AND @NumStoresWithOverrides > 0
			SET @pricesAreGloballyManaged = 1
		ELSE 
			SET @pricesAreGloballyManaged = 0

       
		-- Figuring out whether to do subteam/category locking requires taking several different values into account.

		-- 1. Determine if the region is subteam-aligned.
		DECLARE @IsRegionSubteamAligned bit = dbo.fn_InstanceDataValue('UKIPS', null)

		-- 2. Need to find out if the subteam is changing in order to know whether to enforce category update blocking.
		DECLARE @SubteamColumnNameOrKey varchar(16) = 'subteam_no'
		DECLARE @SubteamNumberUploadValue int = 
		(
			select
				CAST(uv.Value as int)
			from
				UploadValue	uv
				join UploadAttribute ua on uv.UploadAttribute_ID = ua.UploadAttribute_ID
			where
				uv.UploadRow_ID = @UploadRow_ID and
				ua.ColumnNameOrKey = @SubteamColumnNameOrKey
		)

		DECLARE @IsSubteamChanging bit = CASE WHEN @SubTeam_No <> @SubteamNumberUploadValue THEN 1 ELSE 0 END

		-- 3. Find out if the current and uploaded subteam values are non-aligned.
		DECLARE @NonAlignedSubteamNumbers TABLE (SubteamNumber int)
		INSERT INTO
			@NonAlignedSubteamNumbers 
		SELECT
			Subteam_No
		FROM
			Subteam
		WHERE
			AlignedSubTeam = 0

		-- 4. In order to properly handle non-retail items, we need to know if the retail sale value is changing as part of the upload.
		DECLARE @RetailSaleColumnNameOrKey varchar(16) = 'retail_sale'
		DECLARE @RetailSaleUploadValue bit =
		(
			select
				CAST(uv.Value as bit)
			from
				UploadValue	uv
				join UploadAttribute ua on uv.UploadAttribute_ID = ua.UploadAttribute_ID
			where
				uv.UploadRow_ID = @UploadRow_ID and
				ua.ColumnNameOrKey = @RetailSaleColumnNameOrKey
		)

		-- Get the aligned status of the item's current subteam and the subteam in the user's upload.
		DECLARE @IsCurrentSubteamNonAligned bit = (CASE WHEN EXISTS (SELECT SubteamNumber FROM @NonAlignedSubteamNumbers WHERE SubteamNumber = @SubTeam_No) THEN 1 ELSE 0 END)
		DECLARE @IsUploadedSubteamNonAligned bit = (CASE WHEN EXISTS (SELECT SubteamNumber FROM @NonAlignedSubteamNumbers WHERE SubteamNumber = @SubteamNumberUploadValue) THEN 1 ELSE 0 END)

		-- These flags will be used in the update statement later to determine whether a specific update is blocked or not.
		DECLARE @BlockSubteamUpdate bit
		DECLARE @BlockCategoryUpdate bit

		-- Now to determine what gets blocked based on all of the values collected above.
		IF @IsRegionSubteamAligned = 0
			BEGIN
				-- If the region is not subteam-aligned, nothing is blocked.  Easy enough.  It only gets more complicated from here.
				SET @BlockSubteamUpdate = 0
				SET @BlockCategoryUpdate = 0
			END
		ElSE
			BEGIN
				-- If the region is subteam-aligned, we need to branch based upon whether the item belongs to an aligned or non-aligned subteam.
				IF @IsCurrentSubteamNonAligned = 1
					BEGIN
						-- If the current subteam is non-aligned, let's first determine whether the user actually tried to change the subteam.
						IF @IsSubteamChanging = 0
							BEGIN
								-- If the subteam value isn't actually changing, then the subteam block doesn't matter, but we do want to be sure to allow category updates just
								-- in case the user did make a category change.
								SET @BlockSubteamUpdate = 0
								SET @BlockCategoryUpdate = 0
							END
						ELSE
							BEGIN
								-- If the subteam value is changing, then we have to know whether the user changed it to another non-aligned subteam.
								IF @IsUploadedSubteamNonAligned = 1
									BEGIN
										-- The new subteam value is also non-aligned, so everything is fine and nothing needs to be blocked.
										SET @BlockSubteamUpdate = 0
										SET @BlockCategoryUpdate = 0
									END
								ELSE
									BEGIN
										-- The user tried to change a non-aligned subteam to an aligned subteam which isn't permitted.
										SET @BlockSubteamUpdate = 1
										SET @BlockCategoryUpdate = 1
									END
							END
					END
				ELSE
					BEGIN
						-- If the current subteam is aligned, then the subteam can only be changed if the item is not retail sale (or if the item is becoming non-retail as part
						-- of the upload).
						IF @RetailSaleUploadValue = 0
							BEGIN
								-- If it's not retail sale, first determine if the subteam actually did change.
								IF @IsSubteamChanging = 0
									BEGIN
										-- If the subteam value isn't actually changing, then the subteam block doesn't matter, but we do want to be sure to allow category updates just
										-- in case the user did make a category change.
										SET @BlockSubteamUpdate = 0
										SET @BlockCategoryUpdate = 0
									END
								ELSE
									BEGIN
										-- If the subteam value did change, we have to be sure that the user chose another aligned subteam.
										IF @IsUploadedSubteamNonAligned = 1
											BEGIN
												-- User chose non-aligned, so the update has to be blocked.
												SET @BlockSubteamUpdate = 1
												SET @BlockCategoryUpdate = 1
											END
										ELSE
											BEGIN
												-- User chose aligned, so everything's okay.
												SET @BlockSubteamUpdate = 0
												SET @BlockCategoryUpdate = 0
											END
									END
							END
						ELSE
							BEGIN
								-- If the item is retail sale (or will be as part of the upload), then we immediately know that the subteam cannot be changed.
								SET @BlockSubteamUpdate = 1
											
								-- Now we just need to find out whether or not category updates are allowed.
								IF @IsSubteamChanging = 0
									BEGIN
										-- If it's not changing, then allow category updates.
										SET @BlockCategoryUpdate = 0
									END
								ELSE
									BEGIN
										-- If it is changing, then do not allow category updates. 
										SET @BlockCategoryUpdate = 1
									END
							END
					END
			END
		
		SET @IsNonRetailItemInIcon = 0

		SELECT @IsNonRetailItemInIcon = 1
		  FROM ItemIdentifier
		 WHERE Item_Key = @Item_Key
		   AND ((CONVERT(FLOAT, Identifier) >= 46000000000 And CONVERT(FLOAT, Identifier)  <= 46999999999) Or (CONVERT(FLOAT, Identifier) >= 48000000000 And CONVERT(FLOAT, Identifier) <= 48999999999))
		   AND Deleted_Identifier = 0 
		   AND Remove_Identifier = 0

		--check whether the Sign Caption is controlled by Icon or not
		DECLARE	@enableIconSignCaptionUpdatedFlag bit = ISNULL((SELECT FlagValue FROM dbo.InstanceDataFlags WHERE FlagKey = 'EnableIconSignCaptionUpdates')	,0)
				
		-- Now loop through the UploadValues for the UploadRow
		-- and marshall the values into the appropriate variable
		DECLARE UploadValue_cursor CURSOR FOR
			SELECT uv.UploadValue_ID, uv.Value As ColumnValue, LOWER(ua.TableName) As TableName, LOWER(ua.ColumnNameorKey) As ColumnName,
				ua.DbDataType
			FROM UploadValue (NOLOCK) uv
				inner join UploadAttributeView ua (NOLOCK) 
				on uv.UploadAttribute_ID = ua.UploadAttribute_ID
				inner join UploadTypeAttributeView uta (NOLOCK) 
				on ua.UploadAttribute_ID = uta.UploadAttribute_ID
			WHERE uv.UploadRow_ID = @UploadRow_ID and uta.uploadtype_code = 'ITEM_MAINTENANCE'

		OPEN UploadValue_cursor
		
		FETCH NEXT FROM UploadValue_cursor INTO @UploadValue_ID, @ColumnValue, @TableName, @ColumnName, @columnDbDataType

		-- Extract the uploaded values
		WHILE @@FETCH_STATUS = 0
		BEGIN
				
			-- extract the new item values by table and column name							
			IF @TableName = 'item'
			BEGIN
				
				-- item data
				IF @ColumnName = LOWER('POS_Description')
					SELECT  @POS_Description = CASE WHEN @BlockCanonicalFieldUpdate = 0 THEN CAST(@ColumnValue AS varchar(26)) ELSE @POS_Description END
				ELSE IF @ColumnName = LOWER('Item_Description')
					SELECT  @Item_Description = CASE WHEN @BlockCanonicalFieldUpdate = 0 THEN CAST(@ColumnValue AS varchar(60)) ELSE @Item_Description END
				ELSE IF @ColumnName = LOWER('Sign_Description') 
					SELECT @Sign_Description = CASE 
						WHEN @BlockCanonicalFieldUpdate = 0 
							THEN CAST(@ColumnValue AS varchar(60)) 
						WHEN @IsDefaultJurisdiction = 0 
							THEN CAST(@ColumnValue AS varchar(60)) 
						WHEN @IsDefaultJurisdiction = 1 AND @enableIconSignCaptionUpdatedFlag = 1
							THEN @Sign_Description
						ELSE @Sign_Description
					END
				ELSE IF @ColumnName = LOWER('Min_Temperature') 
					SELECT  @Min_Temperature = CAST(@ColumnValue AS smallint)
				ELSE IF @ColumnName = LOWER('Max_Temperature') 
					SELECT  @Max_Temperature = CAST(@ColumnValue AS smallint)
				ELSE IF @ColumnName = LOWER('Average_Unit_Weight') 
					SELECT  @Average_Unit_Weight = CAST(@ColumnValue AS decimal(9,4))
				ELSE IF @ColumnName = LOWER('Package_Desc1')
					SELECT  @Package_Desc1 = CASE WHEN @BlockCanonicalFieldUpdate = 0 THEN CAST(@ColumnValue AS decimal(9,4)) ELSE @Package_Desc1 END
				ELSE IF @ColumnName = LOWER('Package_Desc2') 
					SELECT  @Package_Desc2 = CASE 
												WHEN @UseStoreJurisdictions = 1 AND @IsDefaultJurisdiction = 0 -- Pack Size update is not blocked for alternate jurisdiction 
													THEN CAST(@ColumnValue AS decimal(9,4))
												WHEN @BlockCanonicalFieldUpdate = 0
													THEN CAST(@ColumnValue AS decimal(9,4)) 
												WHEN @BlockCanonicalFieldUpdate = 1 AND @pricesAreGloballyManaged = 0 AND LOWER(@SubTeam_Name) = 'produce' 
													THEN CAST(@ColumnValue AS decimal(9,4))
												ELSE @Package_Desc2 END
				ELSE IF @ColumnName = LOWER('Package_Unit_ID') 
					SELECT  @Package_Unit_ID = CASE
												WHEN @UseStoreJurisdictions = 1 AND @IsDefaultJurisdiction = 0 -- Pack UOM update is not blocked for alternate jurisdiction 
													THEN CAST(@ColumnValue AS int)
												WHEN @BlockCanonicalFieldUpdate = 0 
													THEN CAST(@ColumnValue AS int) 
												WHEN @BlockCanonicalFieldUpdate = 1 AND @pricesAreGloballyManaged = 0 AND LOWER(@SubTeam_Name) = 'produce'
													THEN CAST(@ColumnValue AS int)
												ELSE @Package_Unit_ID END
				ELSE IF @ColumnName = LOWER('Retail_Unit_ID') 
					SELECT  @Retail_Unit_ID = CAST(@ColumnValue AS int)
				ELSE IF @ColumnName = LOWER('SubTeam_No')
					SELECT  @SubTeam_No = CASE WHEN @BlockSubteamUpdate = 0 THEN CAST(@ColumnValue AS int) ELSE @SubTeam_No END
				ELSE IF @ColumnName = LOWER('Brand_ID')
					SELECT  @Brand_ID = CASE WHEN @BlockCanonicalFieldUpdate = 0 THEN CAST(@ColumnValue AS int) ELSE @Brand_ID END
				ELSE IF @ColumnName = LOWER('Category_ID')
					SELECT  @Category_ID = CASE WHEN @BlockCategoryUpdate = 0 THEN CAST(@ColumnValue AS int) ELSE @Category_ID END
				ELSE IF @ColumnName = LOWER('Origin_ID') 
					SELECT  @Origin_ID = CAST(@ColumnValue AS int)
				ELSE IF @ColumnName = LOWER('Retail_Sale')
					IF (@IsNonRetailItemInIcon = 1) 
						SELECT @Retail_Sale = 0
					ELSE
						SELECT  @Retail_Sale = CAST(@ColumnValue AS bit)
				ELSE IF @ColumnName = LOWER('Keep_Frozen') 
					SELECT  @Keep_Frozen = CAST(@ColumnValue AS bit)
				ELSE IF @ColumnName = LOWER('Full_Pallet_Only') 
					SELECT  @Full_Pallet_Only = CAST(@ColumnValue AS bit)
				ELSE IF @ColumnName = LOWER('Shipper_Item') 
					SELECT  @Shipper_Item = CAST(@ColumnValue AS bit)
				ELSE IF @ColumnName = LOWER('WFM_Item') 
					SELECT  @WFM_Item = CAST(@ColumnValue AS bit)
				ELSE IF @ColumnName = LOWER('Units_Per_Pallet') 
					SELECT  @Units_Per_Pallet = CAST(@ColumnValue AS int)
				ELSE IF @ColumnName = LOWER('Vendor_Unit_ID') 
					SELECT  @Vendor_Unit_ID = CAST(@ColumnValue AS int)
				ELSE IF @ColumnName = LOWER('Distribution_Unit_ID') 
					SELECT  @Distribution_Unit_ID = CAST(@ColumnValue AS int)
				ELSE IF @ColumnName = LOWER('Tie') 
					SELECT  @Tie = CAST(@ColumnValue AS tinyint)
				ELSE IF @ColumnName = LOWER('High') 
					SELECT  @High = CAST(@ColumnValue AS tinyint)
				ELSE IF @ColumnName = LOWER('Yield') 
					SELECT  @Yield = CAST(@ColumnValue AS decimal(9,4))
				ELSE IF @ColumnName = LOWER('NoDistMarkup') 
					SELECT  @NoDistMarkup = CAST(@ColumnValue AS bit)
				ELSE IF @ColumnName = LOWER('Organic') 
					SELECT  @Organic = CASE WHEN @BlockCanonicalFieldUpdate = 0 THEN CAST(@ColumnValue AS bit) ELSE @Organic END
				ELSE IF @ColumnName = LOWER('Refrigerated') 
					SELECT  @Refrigerated = CAST(@ColumnValue AS bit)
				ELSE IF @ColumnName = LOWER('Not_Available') 
					SELECT  @Not_Available = CAST(@ColumnValue AS bit)
				ELSE IF @ColumnName = LOWER('Pre_Order') 
					SELECT  @Pre_Order = CAST(@ColumnValue AS bit)
				ELSE IF @ColumnName = LOWER('ItemType_ID') 
					SELECT  @ItemType_ID = CAST(@ColumnValue AS int)
				ELSE IF @ColumnName = LOWER('Sales_Account') 
					SELECT  @Sales_Account = CAST(@ColumnValue AS varchar(6))
				ELSE IF @ColumnName = LOWER('HFM_Item') 
					SELECT  @HFM_Item = CAST(CAST(@ColumnValue AS bit) AS tinyint)
				ELSE IF @ColumnName = LOWER('Not_AvailableNote') 
					SELECT  @Not_AvailableNote = CAST(@ColumnValue AS varchar(255))
				ELSE IF @ColumnName = LOWER('CountryProc_ID') 
					SELECT  @CountryProc_ID = CAST(@ColumnValue AS int)
				ELSE IF @ColumnName = LOWER('Manufacturing_Unit_ID') 
					SELECT  @Manufacturing_Unit_ID = CAST(@ColumnValue AS int)
				ELSE IF @ColumnName = LOWER('EXEDistributed') 
					SELECT  @EXEDistributed = CAST(@ColumnValue AS bit)
				ELSE IF @ColumnName = LOWER('ClassID') 
					SELECT  @NatClassID = CAST(@ColumnValue AS int)
				ELSE IF @ColumnName = LOWER('DistSubTeam_No') 
					SELECT  @DistSubTeam_No = CAST(@ColumnValue AS int)
				ELSE IF @ColumnName = LOWER('CostedByWeight') 
					SELECT  @CostedByWeight = CAST(@ColumnValue AS bit)
				ELSE IF @ColumnName = LOWER('TaxClassID')
					SELECT  @TaxClassID = CASE WHEN @BlockCanonicalFieldUpdate = 0 THEN CAST(@ColumnValue AS int) ELSE @TaxClassID END
				ELSE IF @ColumnName = LOWER('LabelType_ID') 
					SELECT  @LabelType_ID = CAST(@ColumnValue AS int)
				ELSE IF @ColumnName = LOWER('Manager_ID') 
					SELECT  @Manager_ID = CAST(@ColumnValue AS tinyint)
				ELSE IF @ColumnName = LOWER('Recall_Flag') 
					SELECT  @Recall_Flag = CAST(@ColumnValue AS bit)
				ELSE IF @ColumnName = LOWER('ProdHierarchyLevel4_ID') AND @FourLevelHierarchyFlag = 1
					SELECT  @ProdHierarchyLevel4_ID = CASE WHEN @BlockCategoryUpdate = 0 THEN CAST(@ColumnValue AS int) ELSE @ProdHierarchyLevel4_ID END
				ELSE IF @ColumnName = LOWER('PurchaseThresholdCouponSubTeam') 
					SELECT  @PurchaseThresholdCouponSubTeam = CAST(@ColumnValue AS bit)
				ELSE IF @ColumnName = LOWER('PurchaseThresholdCouponAmount') 
					SELECT  @PurchaseThresholdCouponAmount = CAST(@ColumnValue AS smallmoney)
				ELSE IF @ColumnName = LOWER('Product_Code') 
					SELECT  @Product_Code = CAST(@ColumnValue AS varchar(15))
				ELSE IF @ColumnName = LOWER('Unit_Price_Category') 
					SELECT  @Unit_Price_Category = CAST(@ColumnValue AS int)				
				ELSE IF @ColumnName = LOWER('CatchWeightRequired') 
					SELECT  @CatchWeightRequired = CAST(@ColumnValue AS bit)										
				ELSE IF @ColumnName = LOWER('COOL') 
					SELECT  @COOL = CAST(@ColumnValue AS bit)
				ELSE IF @ColumnName = LOWER('BIO') 
					SELECT  @BIO = CAST(@ColumnValue AS bit)					
				ELSE IF @ColumnName = LOWER('Ingredient') 
					SELECT  @Ingredient = CAST(@ColumnValue AS bit)
				ELSE IF @ColumnName = LOWER('SustainabilityRankingRequired') 
					SELECT  @SustainabilityRankingRequired = CAST(@ColumnValue AS bit)
				ELSE IF @ColumnName = LOWER('SustainabilityRankingID') 
					SELECT  @SustainabilityRankingID = CAST(@ColumnValue AS int)	
				ELSE IF @ColumnName = LOWER('UseLastReceivedCost') 
					SELECT  @UseLastReceivedCost = CAST(@ColumnValue AS bit)
				-- pos data
				ELSE IF @ColumnName = LOWER('Food_Stamps')
					SELECT  @Food_Stamps = CASE WHEN @BlockCanonicalFieldUpdate = 0 THEN CAST(@ColumnValue AS bit) ELSE @Food_Stamps END
				ELSE IF @ColumnName = LOWER('Price_Required') 
					SELECT  @Price_Required = CAST(@ColumnValue AS bit)
				ELSE IF @ColumnName = LOWER('Quantity_Required') 
					SELECT  @Quantity_Required = CAST(@ColumnValue AS bit)
				ELSE IF @ColumnName = LOWER('QtyProhibit') 
					SELECT  @QtyProhibit = CAST(@ColumnValue AS bit)
				ELSE IF @ColumnName = LOWER('GroupList') 
					SELECT  @GroupList = CAST(@ColumnValue AS int)
				ELSE IF @ColumnName = LOWER('Case_Discount') 
					SELECT  @Case_Discount = CAST(@ColumnValue AS bit)
				ELSE IF @ColumnName = LOWER('Coupon_Multiplier') 
					SELECT  @Coupon_Multiplier = CAST(@ColumnValue AS bit)	
				ELSE IF @ColumnName = LOWER('FSA_Eligible') 
					SELECT  @FSA_Eligible = CAST(@ColumnValue AS bit)
				ELSE IF @ColumnName = LOWER('Misc_Transaction_Sale') 
					SELECT  @Misc_Transaction_Sale = CAST(@ColumnValue AS smallint)
				ELSE IF @ColumnName = LOWER('Misc_Transaction_Refund') 
					SELECT  @Misc_Transaction_Refund = CAST(@ColumnValue AS smallint)
				ELSE IF @ColumnName = LOWER('Ice_Tare') 
					SELECT  @Ice_Tare = CAST(@ColumnValue AS int)		
				
				-- item scale data
				
				ELSE IF @ColumnName = LOWER('Ingredients') 
					SELECT  @Ingredients = CAST(@ColumnValue AS varchar(3500))
				ELSE IF @ColumnName = LOWER('ShelfLife_Length') 
					SELECT  @Item_ShelfLife_Length = CAST(@ColumnValue AS smallint)
				ELSE IF @ColumnName = LOWER('ShelfLife_ID') 
					SELECT  @ShelfLife_ID = CAST(@ColumnValue AS int)
				ELSE IF @ColumnName = LOWER('Tare') 
					SELECT  @Tare = CAST(@ColumnValue AS int)
				ELSE IF @ColumnName = LOWER('UseBy') 
					SELECT  @UseBy = CAST(@ColumnValue AS int)
				ELSE IF @ColumnName = LOWER('ForcedTare') 
					SELECT  @ForcedTare = CAST(@ColumnValue AS char(1))
				ELSE IF @ColumnName = LOWER('LockAuth')
					SELECT  @LockAuth_Flag = CAST(@ColumnValue AS bit)
					
				-- store jurisdiction values extracted before
				-- preload section above
			END
			ELSE
				IF @TableName = 'itemsignattribute'
					BEGIN
						IF @ColumnName = LOWER('Locality') 
							SELECT  @Locality = CAST(@ColumnValue AS varchar(50))
						ELSE IF @ColumnName = LOWER('SignRomanceTextShort')
						    BEGIN
								IF @UseStoreJurisdictions = 1 AND @IsDefaultJurisdiction = 0
									SELECT  @ShortSignRomanceAlt = CAST(@ColumnValue AS varchar(140))
								ELSE
									SELECT  @ShortSignRomance = CAST(@ColumnValue AS varchar(140))	
							END
						ELSE IF @ColumnName = LOWER('SignRomanceTextLong')
							BEGIN
								IF @UseStoreJurisdictions = 1 AND @IsDefaultJurisdiction = 0
									SELECT  @LongSignRomanceAlt = CAST(@ColumnValue AS varchar(300))
								ELSE
									SELECT  @LongSignRomance = CAST(@ColumnValue AS varchar(300))
							END
						ELSE IF @ColumnName = LOWER('UomRegulationChicagoBaby')
							SELECT  @ChicagoBaby = CAST(@ColumnValue AS varchar(50))	
						ELSE IF @ColumnName = LOWER('UomRegulationTagUom')
							SELECT  @TagUom = CAST(@ColumnValue AS int)	
						ELSE IF @ColumnName = LOWER('Exclusive')
							SELECT  @Exclusive = CAST(@ColumnValue AS date)	
						ELSE IF @ColumnName = LOWER('ColorAdded')
							SELECT  @ColorAdded =	CASE 
														WHEN (@ColorAdded is null and CAST(@ColumnValue AS bit) = 1) THEN CAST(@ColumnValue AS bit) 
														WHEN (@ColorAdded is not null) THEN CAST(@ColumnValue AS bit) 
														ELSE null
													END
						END
			ELSE
			IF @TableName = 'calculated'
			BEGIN
			
				-- item chain ids
				IF @ColumnName = LOWER('itemchains')
					SELECT  @ItemChainIDs = CAST(@ColumnValue AS varchar(1000))
			
			END
			ELSE
			IF @TableName = 'itemscale'
			BEGIN
			
				-- *new* item scale data
				IF @ColumnName = LOWER('Nutrifact_ID')
					SELECT  @Nutrifact_ID = CAST(@ColumnValue AS int)
				ELSE IF @ColumnName = LOWER('Scale_ExtraText_ID')
					SELECT  @Scale_ExtraText_ID = CAST(@ColumnValue AS int)
				ELSE IF @ColumnName = LOWER('Scale_StorageData_ID')
					SELECT  @Scale_StorageData_ID = CAST(@ColumnValue AS int)
				ELSE IF @ColumnName = LOWER('Scale_Allergen_ID')
					SELECT  @Scale_Allergen_ID = CAST(@ColumnValue AS int)
				ELSE IF @ColumnName = LOWER('Scale_Ingredient_ID')
					SELECT  @Scale_Ingredient_ID = CAST(@ColumnValue AS int)
				ELSE IF @ColumnName = LOWER('Scale_Tare_ID')
					SELECT  @Scale_Tare_ID = CAST(@ColumnValue AS int)
				ELSE IF @ColumnName = LOWER('Scale_Alternate_Tare_ID')
					SELECT  @Scale_Alternate_Tare_ID = CAST(@ColumnValue AS int)
				ELSE IF @ColumnName = LOWER('Scale_LabelStyle_ID')
					SELECT  @Scale_LabelStyle_ID = CAST(@ColumnValue AS int)
				ELSE IF @ColumnName = LOWER('Scale_EatBy_ID')
					SELECT  @Scale_EatBy_ID = CAST(@ColumnValue AS smallint)
				ELSE IF @ColumnName = LOWER('Scale_Grade_ID')
					SELECT  @Scale_Grade_ID = CAST(@ColumnValue AS int)
				ELSE IF @ColumnName = LOWER('Scale_RandomWeightType_ID')
					SELECT  @Scale_RandomWeightType_ID = CAST(@ColumnValue AS int)
				ELSE IF @ColumnName = LOWER('Scale_ScaleUOMUnit_ID')
					SELECT  @Scale_ScaleUOMUnit_ID = CAST(@ColumnValue AS int)
				ELSE IF @ColumnName = LOWER('Scale_FixedWeight')
					SELECT  @Scale_FixedWeight = CAST(@ColumnValue AS varchar(25))
				ELSE IF @ColumnName = LOWER('Scale_ByCount')
					SELECT  @Scale_ByCount = CAST(@ColumnValue AS int)
				ELSE IF @ColumnName = LOWER('ForceTare')
					SELECT  @ForceTare = CAST(@ColumnValue AS bit)
				ELSE IF @ColumnName = LOWER('PrintBlankShelfLife')
					SELECT  @PrintBlankShelfLife = CAST(@ColumnValue AS bit)
				ELSE IF @ColumnName = LOWER('PrintBlankEatBy')
					SELECT  @PrintBlankEatBy = CAST(@ColumnValue AS bit)
				ELSE IF @ColumnName = LOWER('PrintBlankPackDate')
					SELECT  @PrintBlankPackDate = CAST(@ColumnValue AS bit)
				ELSE IF @ColumnName = LOWER('PrintBlankWeight')
					SELECT  @PrintBlankWeight = CAST(@ColumnValue AS bit)
				ELSE IF @ColumnName = LOWER('PrintBlankUnitPrice')
					SELECT  @PrintBlankUnitPrice = CAST(@ColumnValue AS bit)
				ELSE IF @ColumnName = LOWER('PrintBlankTotalPrice')
					SELECT  @PrintBlankTotalPrice = CAST(@ColumnValue AS bit)
				ELSE IF @ColumnName = LOWER('Scale_Description1')
					SELECT  @ScaleDesc1 = CAST(@ColumnValue AS varchar(64))
				ELSE IF @ColumnName = LOWER('Scale_Description2')
					SELECT  @ScaleDesc2 = CAST(@ColumnValue AS varchar(64))
				ELSE IF @ColumnName = LOWER('Scale_Description3')
					SELECT  @ScaleDesc3 = CAST(@ColumnValue AS varchar(64))
				ELSE IF @ColumnName = LOWER('Scale_Description4')
					SELECT  @ScaleDesc4 = CAST(@ColumnValue AS varchar(64))
				ELSE IF @ColumnName = LOWER('ShelfLife_Length')
					SELECT  @ShelfLife_Length = CAST(@ColumnValue AS smallint)

			END
			ELSE
			IF @TableName = 'scale_extratext'
			BEGIN

				-- *new* item scale extra text data
				IF @ColumnName = LOWER('Scale_LabelType_ID')
					SELECT  @Scale_LabelType_ID = CAST(@ColumnValue AS int)
				ELSE IF @ColumnName = LOWER('ExtraText')
					SELECT  @ExtraText = CAST(@ColumnValue AS varchar(4200))

			END
			ELSE
			IF @TableName = 'scale_storagedata'
			BEGIN

				-- *new* item scale storage data
				IF @ColumnName = LOWER('StorageData')
					SELECT  @StorageData = CAST(@ColumnValue AS varchar(1024))

			END
			ELSE
			IF @TableName = 'scale_allergen'
			BEGIN

				-- *new* item scale allergen data
				IF @ColumnName = LOWER('Allergens')
					SELECT  @ScaleAllergen = CAST(@ColumnValue AS varchar(4200))

			END
			ELSE
			IF @TableName = 'scale_ingredient'
			BEGIN

				-- *new* item scale allergen data
				IF @ColumnName = LOWER('Ingredients')
					SELECT  @ScaleIngredient = CAST(@ColumnValue AS varchar(4200))

			END
			ELSE
			IF @TableName = 'itemvendor'
			BEGIN
				-- Facility Handling Charge override for the DC
				IF @ColumnName = LOWER('CaseDistHandlingChargeOverride')
					SELECT  @HandlingChargeOverride = CAST(@ColumnValue AS smallmoney)
			END
			ELSE
			IF @TableName = 'itemattribute'
			BEGIN
			
				-- we've got a value for an flexible attribute in the
				-- ItemAttribute table
				
				-- let's check to see if there is a row in this table for the item
				-- once and create one if not
				
				IF @CheckForItemAttributeRow = 1
				BEGIN
				
					-- set the flag so we only do this once per row/item
					SET @CheckForItemAttributeRow = 0
					
					SELECT ITEM_KEY FROM ItemAttribute WHERE Item_Key = @Item_Key
					
					IF @@ROWCOUNT = 0
					BEGIN
						-- no ItemAttribute row exists for the item yet
						-- so we need to creat one
						
						INSERT INTO dbo.ItemAttribute (Item_Key) VALUES (@Item_Key)
					END
				END
								
				-- construct the itemAttribute update SQL
				
				-- format value according to data type
				IF LOWER(@ColumnDbDataType) = 'bit'
				BEGIN
					IF @ColumnValue = '' OR @ColumnValue = 'FALSE'
						select @ColumnValue = 0
						
					IF @ColumnValue = 'TRUE'
						select @ColumnValue = 1
						
				END
				ELSE IF LOWER(@ColumnDbDataType) = 'varchar' or LOWER(@ColumnDbDataType) = 'datetime'
				BEGIN
				
					-- escape any single quotes
					SET @ColumnValue = REPLACE(@ColumnValue, '''', '''''')
					
					if @ColumnValue = '' or @ColumnValue is null				
						select @ColumnValue = 'NULL'
					else
						select @ColumnValue = '''' + @ColumnValue + ''''
					
				END
				ELSE
				BEGIN
					
					-- we have a numeric value
					if @ColumnValue = '' or @ColumnValue is null				
						select @ColumnValue = 'NULL'
					
				END
				
				-- build the SQL
				SELECT @TableAndColumnName = @TableName + '.' + @ColumnName
				
				IF @FlexibleAttributeUpdateSQL IS NULL
				BEGIN
					SELECT @FlexibleAttributeUpdateSQL = 'UPDATE ItemAttribute SET ' + @TableAndColumnName + ' = ' + @ColumnValue
				END
				ELSE
				BEGIN
				
					SELECT @FlexibleAttributeUpdateSQL = @FlexibleAttributeUpdateSQL + ', ' + @TableAndColumnName + ' = ' + @ColumnValue
				END
			END
			
			FETCH NEXT FROM UploadValue_cursor INTO @UploadValue_ID, @ColumnValue, @TableName, @ColumnName, @columnDbDataType
		END

		CLOSE UploadValue_cursor
		DEALLOCATE UploadValue_cursor

		EXEC dbo.EIM_Log @LoggingLevel, 'TRACE', @UploadSession_ID, @UploadRow_ID, @RetryCount, @Item_key, NULL, '3.0.3 Update Existing Item - [Load Uploaded Data]'

		-- translate sign caption null to empty string
		SELECT @Sign_Description = IsNull(@Sign_Description, '')
		
		-- update or insert scale extra text data only if scale item
		IF @IsScaleItem = 1
		BEGIN
				
			-- create new extra text data
			-- only if there isn't any
			IF @Scale_LabelType_ID IS NOT NULL
				AND @ExtraText IS NOT NULL
			BEGIN
			
				IF @ExtraTextDescription IS NULL
				BEGIN
					-- default new ingredient descriptions
					-- to the item's default identifier
					SELECT @ExtraTextDescription = Identifier
					FROM ItemIdentifier (NOLOCK)
					WHERE Item_Key = @Item_Key
						AND Default_Identifier = 1
						AND Deleted_Identifier = 0
				END
				
					SET @Scale_ExtraText_ID = IsNull(@Scale_ExtraText_ID, 0)

					BEGIN TRY

						EXEC dbo.Scale_InsertUpdateExtraText
							@Scale_ExtraText_ID,
							@ExtraTextDescription,
							@Scale_LabelType_ID,
							@ExtraText,
							@New_Scale_ExtraText_ID OUTPUT
						
						EXEC dbo.EIM_Log @LoggingLevel, 'TRACE', @UploadSession_ID, @UploadRow_ID, @RetryCount, @Item_key, NULL, '3.1.1 Update Existing Item - [Scale_InsertUpdateExtraText]'
						
					END TRY
					BEGIN CATCH

							EXEC dbo.EIM_LogAndRethrowException @UploadSession_ID, @UploadRow_ID, @RetryCount, @Item_key, NULL, '3.1.1 Update Existing Item - [Scale_InsertUpdateExtraText]'
					END CATCH

					-- move the newly inserted id into the orig id var
					-- to be used below when inserting/updating the scale data
					If @Scale_ExtraText_ID = 0
						SET @Scale_ExtraText_ID = @New_Scale_ExtraText_ID
			END

			-- create new storage data
			-- only if there isn't any
			IF @StorageData IS NOT NULL
			BEGIN
			
				IF @StorageDescription IS NULL
				BEGIN
					-- default new storagedata descriptions
					-- to the item's default identifier
					SELECT @StorageDescription = Identifier
					FROM ItemIdentifier (NOLOCK)
					WHERE Item_Key = @Item_Key
						AND Default_Identifier = 1
						AND Deleted_Identifier = 0
				END
				
					SET @Scale_StorageData_ID = IsNull(@Scale_StorageData_ID, 0)

					BEGIN TRY

						EXEC dbo.Scale_InsertUpdateStorageData
							@Scale_StorageData_ID,
							@StorageDescription,
							@StorageData,
							@New_Scale_StorageData_ID OUTPUT
						
						EXEC dbo.EIM_Log @LoggingLevel, 'TRACE', @UploadSession_ID, @UploadRow_ID, @RetryCount, @Item_key, NULL, '3.1.2 Update Existing Item - [Scale_InsertUpdateStorageData]'
						
					END TRY
					BEGIN CATCH

							EXEC dbo.EIM_LogAndRethrowException @UploadSession_ID, @UploadRow_ID, @RetryCount, @Item_key, NULL, '3.1.2 Update Existing Item - [Scale_InsertUpdateStorageData]'
					END CATCH

					-- move the newly inserted id into the orig id var
					-- to be used below when inserting/updating the scale data
					If @Scale_StorageData_ID = 0
						SET @Scale_StorageData_ID = @New_Scale_StorageData_ID
			END

			-- create new allergen data
			-- only if there isn't any
			IF @ScaleAllergen IS NOT NULL
			BEGIN
			
				SET @ScaleAllergen_LabelType_ID = ISNULL(@ScaleAllergen_LabelType_ID, 0)

				IF @ScaleAllergenDescription IS NULL
				BEGIN
					-- default new allergen descriptions
					-- to the item's default identifier
					SELECT @ScaleAllergenDescription = Identifier
					FROM ItemIdentifier (NOLOCK)
					WHERE Item_Key = @Item_Key
						AND Default_Identifier = 1
						AND Deleted_Identifier = 0
				END
				
					SET @Scale_Allergen_ID = IsNull(@Scale_Allergen_ID, 0)

					BEGIN TRY

						EXEC dbo.Scale_InsertUpdateAllergen
							@Scale_Allergen_ID,
							@ScaleAllergenDescription,
							@ScaleAllergen_LabelType_ID,
							@ScaleAllergen,
							@New_Scale_Allergen_ID OUTPUT
						
						EXEC dbo.EIM_Log @LoggingLevel, 'TRACE', @UploadSession_ID, @UploadRow_ID, @RetryCount, @Item_key, NULL, '3.1.1 Update Existing Item - [Scale_InsertUpdateAllergen]'
						
					END TRY
					BEGIN CATCH

							EXEC dbo.EIM_LogAndRethrowException @UploadSession_ID, @UploadRow_ID, @RetryCount, @Item_key, NULL, '3.1.1 Update Existing Item - [Scale_InsertUpdateAllergen]'
					END CATCH

					-- move the newly inserted id into the orig id var
					-- to be used below when inserting/updating the scale data
					If @Scale_Allergen_ID = 0
						SET @Scale_Allergen_ID = @New_Scale_Allergen_ID
			END

			-- create new ingredient data
			-- only if there isn't any
			IF @ScaleIngredient IS NOT NULL
			BEGIN
			
				SET @ScaleIngredient_LabelType_ID = ISNULL(@ScaleIngredient_LabelType_ID, 0)
			
				IF @ScaleIngredientDescription IS NULL
				BEGIN
					-- default new ingredient descriptions
					-- to the item's default identifier
					SELECT @ScaleIngredientDescription = Identifier
					FROM ItemIdentifier (NOLOCK)
					WHERE Item_Key = @Item_Key
						AND Default_Identifier = 1
						AND Deleted_Identifier = 0
				END
				
					SET @Scale_Ingredient_ID = IsNull(@Scale_Ingredient_ID, 0)

					BEGIN TRY

						EXEC dbo.Scale_InsertUpdateIngredient
							@Scale_Ingredient_ID,
							@ScaleIngredientDescription,
							@ScaleIngredient_LabelType_ID,
							@ScaleIngredient,
							@New_Scale_Ingredient_ID OUTPUT
						
						EXEC dbo.EIM_Log @LoggingLevel, 'TRACE', @UploadSession_ID, @UploadRow_ID, @RetryCount, @Item_key, NULL, '3.1.1 Update Existing Item - [Scale_InsertUpdateIngredient]'
						
					END TRY
					BEGIN CATCH

							EXEC dbo.EIM_LogAndRethrowException @UploadSession_ID, @UploadRow_ID, @RetryCount, @Item_key, NULL, '3.1.1 Update Existing Item - [Scale_InsertUpdateIngredient]'
					END CATCH

					-- move the newly inserted id into the orig id var
					-- to be used below when inserting/updating the scale data
					If @Scale_Ingredient_ID = 0
						SET @Scale_Ingredient_ID = @New_Scale_Ingredient_ID
			END
		END
				
		-- only insert/update the override item and scale tables if
		-- the region is using store jurisdictions
		-- and the upload row is not for the item's
		-- default jurisdiction
		If @UseStoreJurisdictions = 1 AND @IsDefaultJurisdiction = 0
		BEGIN
		
			BEGIN TRY
			
				EXEC dbo.InsertUpdateItemOverride
					@Item_Key,
					@StoreJurisdictionID,
					@Item_Description,
					@Sign_Description,
					@Package_Desc1,
					@Package_Desc2,
					@Package_Unit_ID,
					@Retail_Unit_ID,
					@Vendor_Unit_ID,
					@Distribution_Unit_ID,
					@POS_Description,
					@Food_Stamps,
					@Price_Required,
					@Quantity_Required,
					@Manufacturing_Unit_ID,
					@QtyProhibit,
					@GroupList,
					@Case_Discount,
					@Coupon_Multiplier,
					@Misc_Transaction_Sale,
					@Misc_Transaction_Refund,
					@Ice_Tare,
					@Brand_Id,
					@Origin_Id,
					@CountryProc_Id,
					@SustainabilityRankingRequired,
					@SustainabilityRankingID,
					@LabelType_ID,
					@CostedByWeight,
					@Average_Unit_Weight,                           
					@Ingredient,                                  
					@Recall_Flag,                                 
					@LockAuth_Flag,                                   
					@Not_Available,                                
					@Not_AvailableNote,                            
					@FSA_Eligible,                                  
					@Product_Code,                                  
					@Unit_Price_Category,
					@LongSignRomanceAlt,
					@ShortSignRomanceAlt                          
			
				EXEC dbo.EIM_Log @LoggingLevel, 'TRACE', @UploadSession_ID, @UploadRow_ID, @RetryCount, @Item_key, NULL, '3.2 Update Existing Item - [InsertUpdateItemOverride]'
				
			END TRY
			BEGIN CATCH
					EXEC dbo.EIM_LogAndRethrowException @UploadSession_ID, @UploadRow_ID, @RetryCount, @Item_key, NULL, '3.2 Update Existing Item - [InsertUpdateItemOverride]'
			END CATCH
		
			-- update or insert scale data only if scale item
			IF @IsScaleItem = 1
			BEGIN

				BEGIN TRY
			
					EXEC dbo.Scale_InsertUpdateItemScaleOverride
						@Item_Key,
						@StoreJurisdictionID,
						@ScaleDesc1,
						@ScaleDesc2,
						@ScaleDesc3,
						@ScaleDesc4,
						@Scale_ExtraText_ID,
						@Scale_Tare_ID,
						@Scale_LabelStyle_ID,
						@Scale_ScaleUOMUnit_ID,
						@Scale_RandomWeightType_ID,
						@Scale_FixedWeight,
						@Scale_ByCount,
						@ShelfLife_Length,
						@ForceTare,
						@Scale_Alternate_Tare_ID,
						@Scale_EatBy_ID,
						@Scale_Grade_ID,
						@PrintBlankEatBy,
						@PrintBlankPackDate,
						@PrintBlankShelfLife,
						@PrintBlankTotalPrice,
						@PrintBlankUnitPrice,
						@PrintBlankWeight,
						@Nutrifact_ID			
	
					EXEC dbo.EIM_Log @LoggingLevel, 'TRACE', @UploadSession_ID, @UploadRow_ID, @RetryCount, @Item_key, NULL, '3.3 Update Existing Item - [Scale_InsertUpdateItemScaleOverride]'

				END TRY
				BEGIN CATCH

						EXEC dbo.EIM_LogAndRethrowException @UploadSession_ID, @UploadRow_ID, @RetryCount, @Item_key, NULL, '3.3 Update Existing Item - [Scale_InsertUpdateItemScaleOverride]'
				END CATCH

			END
		
		END
		ELSE
		BEGIN
		
			-- complete the flexible attribute update SQL
			-- if needed
			IF @FlexibleAttributeUpdateSQL IS NOT NULL
			BEGIN
				SELECT @FlexibleAttributeUpdateSQL = @FlexibleAttributeUpdateSQL + ' WHERE Item_Key = ' + CAST(@Item_Key AS varchar(200))
			END
					
			BEGIN TRY
				
				-- update the item
				EXEC [dbo].[UpdateItemInfo]
					@Item_Key,
					@POS_Description, 
					@Item_Description, 
					@Sign_Description,    
					@Min_Temperature, 
					@Max_Temperature,
					@Average_Unit_Weight,     
					@Package_Desc1, 
					@Package_Desc2, 
					@Package_Unit_ID,     
					@Retail_Unit_ID, 
					@SubTeam_No, 
					@Brand_ID, 
					@Category_ID, 
					@Origin_ID, 
					@Retail_Sale, 
					@Keep_Frozen, 
					@Full_Pallet_Only, 
					@Shipper_Item, 
					@WFM_Item, 
					@Units_Per_Pallet, 
					@Vendor_Unit_ID,
					@Distribution_Unit_ID,    
					@Tie,
					@High,
					@Yield,
					@NoDistMarkup ,
					@Organic,
					@Refrigerated,
					@Not_Available,
					@Pre_Order,
					@ItemType_ID,
					@Sales_Account,
					@HFM_Item,   
					@Not_AvailableNote,
					@CountryProc_ID,
					@Manufacturing_Unit_ID,
					@EXEDistributed,
					@NatClassID, 
					@DistSubTeam_No,
					@CostedByWeight,
					@TaxClassID,
					@LabelType_ID,
					@User_ID,
					@User_ID_Date,
					@Manager_ID,
					@Recall_Flag,
					@ProdHierarchyLevel4_ID,
					@LockAuth_Flag,
					@PurchaseThresholdCouponSubTeam,
					@PurchaseThresholdCouponAmount,
					@HandlingChargeOverride,
					@CatchWeightRequired,
					@COOL,
					@BIO,
					@Ingredient,
					@SustainabilityRankingRequired,
					@SustainabilityRankingID,
					@UseLastReceivedCost
					
				UPDATE Item SET User_ID = NULL WHERE Item_Key = @Item_Key -- Set the User_ID to NULL to prevent the item from locking
		
				EXEC dbo.EIM_Log @LoggingLevel, 'TRACE', @UploadSession_ID, @UploadRow_ID, @RetryCount, @Item_key, NULL, '3.4 Update Existing Item - [UpdateItemInfo]'
			
			END TRY
			BEGIN CATCH

					EXEC dbo.EIM_LogAndRethrowException @UploadSession_ID, @UploadRow_ID, @RetryCount, @Item_key, NULL, '3.4 Update Existing Item - [UpdateItemInfo]'
			END CATCH

			BEGIN TRY

				-- update item table scale info
				EXEC [dbo].[UpdateItemScaleData]
					@Item_Key,
					@ScaleDesc1,
					@ScaleDesc2,
					@ScaleDesc3,
					@ScaleDesc4,
					@Ingredients,
					@Item_ShelfLife_Length,
					@ShelfLife_ID,	
					@Tare,
					@UseBy,
					@ForcedTare
				
				EXEC dbo.EIM_Log @LoggingLevel, 'TRACE', @UploadSession_ID, @UploadRow_ID, @RetryCount, @Item_key, NULL, '3.5 Update Existing Item - [UpdateItemScaleData]'

			END TRY
			BEGIN CATCH

					EXEC dbo.EIM_LogAndRethrowException @UploadSession_ID, @UploadRow_ID, @RetryCount, @Item_key, NULL, '3.5 Update Existing Item - [UpdateItemScaleData]'
			END CATCH

			BEGIN TRY

				-- update POS data
				EXEC [dbo].[UpdateItemPOSData]
					@Item_Key,
					@Food_Stamps,
					@Price_Required,
					@Quantity_Required, 
					@QtyProhibit,
					@GroupList,
					@Case_Discount,
					@Coupon_Multiplier,
					@FSA_Eligible,
					@Misc_Transaction_Sale,
					@Misc_Transaction_Refund,
					@Ice_Tare,
					@Product_Code,
					@Unit_Price_Category
				EXEC dbo.EIM_Log @LoggingLevel, 'TRACE', @UploadSession_ID, @UploadRow_ID, @RetryCount, @Item_key, NULL, '3.6 Update Existing Item - [UpdateItemPOSData]'

			END TRY
			BEGIN CATCH

					EXEC dbo.EIM_LogAndRethrowException @UploadSession_ID, @UploadRow_ID, @RetryCount, @Item_key, NULL, '3.6 Update Existing Item - [UpdateItemPOSData]'
			END CATCH

			-- update or insert scale data only if scale item
			IF @IsScaleItem = 1
			BEGIN
						
				-- update scale info
				-- in the new table
				
				-- set the @Existing_ItemScale_ID to zero if null
				-- as this indicates th the Scale_InsertUpdateItemScaleDetails proc
				-- to insert a new ItemScale row
				SET @Existing_ItemScale_ID = IsNull(@Existing_ItemScale_ID, 0)

				BEGIN TRY
					
					EXEC dbo.Scale_InsertUpdateItemScaleDetails
						@Existing_ItemScale_ID,
						@Item_Key,
						@Nutrifact_ID,
						@Scale_ExtraText_ID,
						@Scale_Tare_ID,
						@Scale_Alternate_Tare_ID,
						@Scale_LabelStyle_ID,
						@Scale_EatBy_ID,
						@Scale_Grade_ID,	
						@Scale_RandomWeightType_ID,
						@Scale_ScaleUOMUnit_ID,
						@Scale_FixedWeight,
						@Scale_ByCount,
						@ForceTare,
						@PrintBlankShelfLife,	
						@PrintBlankEatBy,
						@PrintBlankPackDate,
						@PrintBlankWeight,
						@PrintBlankUnitPrice,
						@PrintBlankTotalPrice,
						@ScaleDesc1,
						@ScaleDesc2,
						@ScaleDesc3,
						@ScaleDesc4,
						@ShelfLife_Length,
						@User_ID,
						@User_ID_Date,
						@Scale_StorageData_ID = @Scale_StorageData_ID,
						@Scale_Allergen_ID = @Scale_Allergen_ID,
						@Scale_Ingredient_ID = @Scale_Ingredient_ID
					

					EXEC dbo.EIM_Log @LoggingLevel, 'TRACE', @UploadSession_ID, @UploadRow_ID, @RetryCount, @Item_key, NULL, '3.7 Update Existing Item - [Scale_InsertUpdateItemScaleDetails]'

				END TRY
				BEGIN CATCH

						EXEC dbo.EIM_LogAndRethrowException @UploadSession_ID, @UploadRow_ID, @RetryCount, @Item_key, NULL, '3.7 Update Existing Item - [Scale_InsertUpdateItemScaleDetails]'
				END CATCH

			END

			-- Update sign attributes (the "exists" logic is there because EIM runs this stored procedure after running the new item upload stored procedure, and I didn't want
			-- a record to be created in ItemSignAttribute if there was no actual data in the upload).
			BEGIN TRY
				IF	EXISTS (select Item_Key from ItemSignAttribute where Item_Key = @Item_key) OR 
					(NOT EXISTS (select Item_Key from ItemSignAttribute where Item_Key = @Item_key) 
						AND (@Locality IS NOT NULL OR @ShortSignRomance IS NOT NULL OR @LongSignRomance IS NOT NULL OR @ChicagoBaby IS NOT NULL OR @TagUom IS NOT NULL OR @Exclusive IS NOT NULL OR @ColorAdded IS NOT NULL))
					BEGIN
						DECLARE
							@AnimalWelfareRating [nvarchar](10),
							@Biodynamic [bit],
							@CheeseMilkType [nvarchar](40),
							@CheeseRaw [bit],
							@EcoScaleRating [nvarchar](30),
							@GlutenFree [bit],
							@HealthyEatingRating [nvarchar](10),
							@Kosher [bit],
							@NonGmo [bit],
							@OrganicSignAttribute [bit],
							@PremiumBodyCare [bit],
							@ProductionClaims [nvarchar](30),
							@FreshOrFrozen [nvarchar](30),
							@SeafoodCatchType [nvarchar](15),
							@Vegan [bit],
							@Vegetarian [bit],
							@WholeTrade [bit]
							
						SELECT
							@AnimalWelfareRating = isa.AnimalWelfareRating,
							@Biodynamic = isa.Biodynamic,
							@CheeseMilkType = isa.CheeseMilkType,
							@CheeseRaw = isa.CheeseRaw,
							@EcoScaleRating = isa.EcoScaleRating,
							@GlutenFree = isa.GlutenFree,
							@HealthyEatingRating = isa.HealthyEatingRating,
							@Kosher = isa.Kosher,
							@NonGmo = isa.NonGmo,
							@OrganicSignAttribute = isa.Organic,
							@PremiumBodyCare = isa.PremiumBodyCare,
							@ProductionClaims = isa.ProductionClaims,
							@FreshOrFrozen = isa.FreshOrFrozen,
							@SeafoodCatchType = isa.SeafoodCatchType,
							@Vegan = isa.Vegan,
							@Vegetarian = isa.Vegetarian,
							@WholeTrade = isa.WholeTrade
						FROM
							ItemSignAttribute (nolock) isa
						WHERE
							isa.Item_Key = @Item_key

						EXEC dbo.InsertOrUpdateItemSignAttribute
							@Item_Key,
							@Locality,
							@LongSignRomance,
							@ShortSignRomance,
							@AnimalWelfareRating,
							@Biodynamic,
							@CheeseMilkType,
							@CheeseRaw,
							@EcoScaleRating,
							@GlutenFree,
							@HealthyEatingRating,
							@Kosher,
							@NonGmo,
							@OrganicSignAttribute,
							@PremiumBodyCare,
							@ProductionClaims,
							@FreshOrFrozen,
							@SeafoodCatchType,
							@Vegan,
							@Vegetarian,
							@WholeTrade,
							@ChicagoBaby,
							@TagUom,
							@Exclusive,
							@ColorAdded
					
						EXEC dbo.EIM_Log @LoggingLevel, 'TRACE', @UploadSession_ID, @UploadRow_ID, @RetryCount, @Item_key, NULL, '3.8 Update Existing Item - [InsertOrUpdateItemSignAttribute]'
					END
			END TRY
			BEGIN CATCH
				EXEC dbo.EIM_LogAndRethrowException @UploadSession_ID, @UploadRow_ID, @RetryCount, @Item_key, NULL, '3.8 Update Existing Item - [InsertOrUpdateItemSignAttribute]'
			END CATCH
			
			
			-- add the item to the chains, if any
			
			-- first remove from all chains
			DELETE FROM ItemChainItem
			WHERE Item_Key = @Item_Key
			
			-- now parse the comma delimeted list of chain ids
			-- and add the item to each chain	
			DECLARE ItemChains_cursor CURSOR FOR
			SELECT
				LTRIM(RTRIM(Key_Value))
			FROM
				dbo.fn_Parse_List(@ItemChainIDs, ',')

			OPEN ItemChains_cursor
			
			FETCH NEXT FROM ItemChains_cursor
				INTO @ChainId
				
			BEGIN TRY
			
				WHILE @@FETCH_STATUS = 0 AND @ChainId IS NOT NULL
				BEGIN

					EXEC dbo.InsertItemChainItem
						@ChainId,
						@Item_Key

					FETCH NEXT FROM ItemChains_cursor
						into @ChainId
				END
			
				EXEC dbo.EIM_Log @LoggingLevel, 'TRACE', @UploadSession_ID, @UploadRow_ID, @RetryCount, @Item_key, NULL, '3.9 Update Existing Item - [InsertItemChainItem]'

			END TRY
			BEGIN CATCH

					EXEC dbo.EIM_LogAndRethrowException @UploadSession_ID, @UploadRow_ID, @RetryCount, @Item_key, NULL, '3.9 Update Existing Item - [InsertItemChainItem]'
			END CATCH
			
			CLOSE ItemChains_cursor
			DEALLOCATE ItemChains_cursor
			
			-- update flexible attributes if needed
			IF @FlexibleAttributeUpdateSQL IS NOT NULL
			BEGIN
				-- insert an ItemAttribute row if one doesn't exist
				IF NOT EXISTS(SELECT * FROM ItemAttribute (NOLOCK) WHERE Item_Key = @Item_Key)
				BEGIN
					INSERT INTO ItemAttribute (Item_Key) Values (@Item_Key)
				END
					
				BEGIN TRY
				
					-- update flexible attributes
					EXECUTE(@FlexibleAttributeUpdateSQL)
				
					EXEC dbo.EIM_Log @LoggingLevel, 'TRACE', @UploadSession_ID, @UploadRow_ID, @RetryCount, @Item_key, NULL, '3.9.1 Update Existing Item - [Update Flex Attributes]'

				END TRY
				BEGIN CATCH
				
					DECLARE @ErrorMessage varchar(max)
					
					SET @ErrorMessage = '3.9.1 Update Existing Item - [Update Flex Attributes: ' + @FlexibleAttributeUpdateSQL + ']'

					EXEC dbo.EIM_LogAndRethrowException @UploadSession_ID, @UploadRow_ID, @RetryCount, @Item_key, NULL, @ErrorMessage
				END CATCH
				
				-- clear the flexible attribute sql
				SET @FlexibleAttributeUpdateSQL = NULL
			END

		END --  @UseStoreJurisdictions = 1 AND @IsDefaultJurisdiction = 0
		
		EXEC dbo.EIM_Log @LoggingLevel, 'TRACE', @UploadSession_ID, @UploadRow_ID, @RetryCount, @Item_key, NULL, '3.9.2 Update Existing Item - [End]'