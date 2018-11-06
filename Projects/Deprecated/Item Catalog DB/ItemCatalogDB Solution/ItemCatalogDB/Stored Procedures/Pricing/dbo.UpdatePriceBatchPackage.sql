SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

if exists (select * from dbo.sysobjects where id = object_id(N'dbo.UpdatePriceBatchPackage') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure dbo.UpdatePriceBatchPackage
GO

CREATE PROCEDURE [dbo].[UpdatePriceBatchPackage]
    @PriceBatchHeaderID int

AS

-- NOTE:  If changes are made to the logic in this stored procedure, the Replenishment_POSPush_UpdateOffPromoCostRecords
-- procedure also needs to be kept in sync.

/*********************************************************************************************
CHANGE LOG
DEV		DATE		TASK	Description
----------------------------------------------------------------------------------------------
DBS		20110125	1241	Merge Up FSA Changes
MU		20110126	 744	add ItemSurcharge
KM		2013/01/01	9251	Check ItemOverride for new 4.8 override values (Origin, Brand, LabelType,
							FSA_Eligible, Recall_Flag, Product_Code, Unit_Price_Category);
***********************************************************************************************/

BEGIN

    SET NOCOUNT ON

    DECLARE @error_no int
    SELECT @error_no = 0

    BEGIN TRAN

		-- Check the Regional Scale Flag
		DECLARE @UseRegionalScaleFile bit
		SELECT @UseRegionalScaleFile = FlagValue FROM InstanceDataFlags (NOLOCK) WHERE FlagKey='UseRegionalScaleFile'

		-- Check the Store Jurisdiction Flag
		DECLARE @UseStoreJurisdictions int
		SELECT @UseStoreJurisdictions = FlagValue FROM InstanceDataFlags (NOLOCK) WHERE FlagKey = 'UseStoreJurisdictions'

		DECLARE @ItemChgTypeID tinyint, @PriceChgTypeID tinyint, @StartDate smalldatetime

		-- Determine if this batch contains Item Changes, Price Changes, or Both and grab the StartDate on the header for a null check in PBD
		SELECT 
			@ItemChgTypeID = ISNULL(ItemChgTypeID, 0), 
			@PriceChgTypeID = ISNULL(PriceChgTypeID, 0),
			@StartDate = StartDate 
		FROM 
			PriceBatchHeader (NOLOCK)
		WHERE 
			PriceBatchHeaderID = @PriceBatchHeaderID

		SELECT @error_no = @@ERROR

		IF (@error_no = 0) -- AND (@ItemChgTypeID <> 3)
		BEGIN
			-- Set the PBD ItemChgTypdID values for the price change records that are in the PBD table
			-- NEW items (all price changes are also marked as new items)
			IF @ItemChgTypeID = 1
			BEGIN
				UPDATE PriceBatchDetail
				SET ItemChgTypeID = 1
				WHERE PriceBatchHeaderID = @PriceBatchHeaderID AND PriceChgTypeID IS NOT NULL
    
				SELECT @error_no = @@ERROR
            
				-- If the PriceBatchDetail record for the item change is marked as a re-auth, update the
				-- price change record accordingly so the data will still be captured if the PBD record is
				-- deleted.
				IF @error_no = 0
				BEGIN
					UPDATE PriceBatchDetail 
					SET ReAuthFlag = 1
					FROM PriceBatchDetail PBD (NOLOCK)
					WHERE PriceBatchHeaderID = @PriceBatchHeaderID AND EXISTS	(SELECT *
																				FROM 
																					PriceBatchDetail D (NOLOCK)
																				WHERE 
																					D.PriceBatchHeaderID	= @PriceBatchHeaderID
																					AND D.Item_Key			= PBD.Item_Key
																					AND ISNULL(D.ReAuthFlag,0) = 1)
									
					SELECT @error_no = @@ERROR
				END
			END

			--  ITEM changes (price changes are marked as item changes if both PBD records exist for the same item)
			IF @error_no = 0
			BEGIN
				UPDATE PriceBatchDetail
				SET ItemChgTypeID = 2
				FROM PriceBatchDetail PBD (NOLOCK)
				WHERE PriceBatchHeaderID = @PriceBatchHeaderID	AND PriceChgTypeID IS NOT NULL
					AND EXISTS	(SELECT * 
								FROM 
									PriceBatchDetail D (NOLOCK) 
								WHERE 
									D.PriceBatchHeaderID	= @PriceBatchHeaderID
									AND D.Item_Key			= PBD.Item_Key
									AND ItemChgTypeID		= 2)
    
				SELECT @error_no = @@ERROR
			END

			-- Expire the item change PBD records for items that had an item and price change assigned to the same batch.
			-- The price change PBD records now have the ItemChgTypeID set, as well.
			IF @error_no = 0
			BEGIN
				UPDATE PriceBatchDetail  
				SET 
					Expired				= 1, 
					PriceBatchHeaderID	= NULL
				
				FROM 
					PriceBatchDetail PBD (NOLOCK)
				
				WHERE 
					PBD.PriceBatchHeaderID = @PriceBatchHeaderID
					AND ( -- If this is not a re-auth, we can always safely expire the new item record because a 
						  -- price change is required to batch the new item.
						 (((PBD.ItemChgTypeID = 1) OR (PBD.ItemChgTypeID = 2 AND @ItemChgTypeID <> 2))
						 AND PBD.PriceChgTypeID IS NULL
						 AND PBD.ReAuthFlag = 0)
						 
						 OR
						 
						  -- If this is a re-auth, we only expire the new item record if there is also a 
						  -- price change record assigned to the same batch.
						 (PBD.ItemChgTypeID = 1
						  AND PBD.PriceChgTypeID IS NULL
						  AND PBD.ReAuthFlag = 1
						  AND EXISTS	(SELECT *
										FROM 
											PriceBatchDetail D (NOLOCK) 
										WHERE 
											D.PriceBatchHeaderID		= @PriceBatchHeaderID
											AND D.Item_Key				= PBD.Item_Key
											AND D.PriceBatchDetailID	<> PBD.PriceBatchDetailID))
						 )			               
    
				SELECT @error_no = @@ERROR
			END

			-- Fill in the item details for the PBD record, based on the current item data in IRMA.  
			-- This will ensure that the same item values are used consistently by shelf tag printing and POS Push,
			-- even if more item updates are made in IRMA.  
			-- Additionally, if the Store Jurisdiction flag is on, use values from the ItemOverride table where
			-- the default jurisdiction for an item does not match the jurisdiction for the given store.
			IF @error_no = 0
			BEGIN
				UPDATE PriceBatchDetail
				SET 
					Sign_Description		= ISNULL(ItemOverride.Sign_Description, Item.Sign_Description), 
					Ingredients				= Scale_ExtraText.ExtraText, 
					Identifier				= II.Identifier, 
					Sold_By_Weight			= CASE WHEN ISNULL(RU.Weight_Unit, 0) = 1 AND dbo.fn_IsScaleItem(II.Identifier) = 0 THEN 1 ELSE 0 END, 
					RetailUnit_WeightUnit	= CASE WHEN ISNULL(RU.Weight_Unit, 0) = 1 THEN 1 ELSE 0 END,
					SubTeam_No				= ISNULL(Price.ExceptionSubTeam_No, Item.SubTeam_No), 
					Origin_Name				= ItemOrigin.Origin_Name, 
					Brand_Name				= ItemBrand.Brand_Name, 
					Retail_Unit_Abbr		= RU.Unit_Abbreviation, 
					Retail_Unit_Full		= RU.Unit_Name, 
					Package_Unit			= PU.Unit_Abbreviation, 
					Package_Desc1			= ISNULL(ItemOverride.Package_Desc1, Item.Package_Desc1), 
					Package_Desc2			= ISNULL(ItemOverride.Package_Desc2, Item.Package_Desc2), 
					Organic					= Item.Organic, 
					Vendor_ID				= SIV.Vendor_ID, 
					ItemType_ID				= Item.ItemType_ID, 
					ScaleDesc1				= ISNULL(ISO.Scale_Description1, ItemScale.Scale_Description1), 
					ScaleDesc2				= ISNULL(ISO.Scale_Description2, ItemScale.Scale_Description2), 
					POS_Description			= ISNULL(ItemOverride.POS_Description, Item.POS_Description), 
					Restricted_Hours		= Price.Restricted_Hours, 
					LocalItem				= Price.LocalItem,
					Quantity_Required		= ISNULL(ItemOverride.Quantity_Required, Item.Quantity_Required), 
					Price_Required			= ISNULL(ItemOverride.Price_Required, Item.Price_Required), 
					Retail_Sale				= Item.Retail_Sale, 
					Discountable			= Price.Discountable, 
					Food_Stamps				= ISNULL(ItemOverride.Food_Stamps, Item.Food_Stamps), 
					IBM_Discount			= Price.IBM_Discount,
					NotAuthorizedForSale	= Price.NotAuthorizedForSale,
					PosTare					= Price.PosTare,
					LinkedItem				= Price.LinkedItem,
					POSLinkCode				= Price.POSLinkCode,
					GrillPrint				= Price.GrillPrint,
					AgeCode					= Price.AgeCode,
					VisualVerify			= Price.VisualVerify,
					SrCitizenDiscount		= Price.SrCitizenDiscount,
					QtyProhibit				= ISNULL(ItemOverride.QtyProhibit, Item.QtyProhibit), 
					GroupList				= ISNULL(ItemOverride.GroupList, Item.GroupList),
					MixMatch				= ISNULL(ISNULL(PBD.MixMatch, Price.MixMatch),0),
					PurchaseThresholdCouponAmount	=	case 
															when Item.ItemType_ID in (6,7) then isnull(Item.PurchaseThresholdCouponAmount, 0.00)
															else 0.00
														end,
					PurchaseThresholdCouponSubTeam	=	case 
															when ST.PurchaseThresholdCouponAvailable = 1 
																and Item.PurchaseThresholdCouponSubTeam = 1 
																and Item.PurchaseThresholdCouponAmount > 0
																and Item.ItemType_ID in (6,7)
																then isnull(Item.PurchaseThresholdCouponSubTeam , 0) 
															else 0 
														end,
					LabelType_ID			= ISNULL(ItemOverride.LabelType_ID, Item.LabelType_ID),
					StartDate				= ISNULL(PBD.StartDate, @StartDate),
					Item_Description		= ISNULL(ItemOverride.Item_Description, Item.Item_Description),
					Case_Discount			= ISNULL(ItemOverride.Case_Discount, Item.Case_Discount),
					Coupon_Multiplier		= ISNULL(ItemOverride.Coupon_Multiplier, Item.Coupon_Multiplier),
					FSA_Eligible			= ISNULL(ItemOverride.FSA_Eligible, ISNULL(Item.Coupon_Multiplier, 0)),
					Misc_Transaction_Sale	= ISNULL(ItemOverride.Misc_Transaction_Sale, Item.Misc_Transaction_Sale),
					Misc_Transaction_Refund = ISNULL(ItemOverride.Misc_Transaction_Refund, Item.Misc_Transaction_Refund),
					Recall_Flag				= ISNULL(ItemOverride.Recall_Flag, Item.Recall_Flag),
					Product_Code			= ISNULL(ItemOverride.Product_Code, Item.Product_Code),
					Unit_Price_Category		= ISNULL(ItemOverride.Unit_Price_Category, Item.Unit_Price_Category),
					Ice_Tare				= ISNULL(ItemOverride.Ice_Tare, Item.Ice_Tare),
					Age_Restrict			= Price.Age_Restrict,
					Routing_Priority		= Price.Routing_Priority,
					Consolidate_Price_To_Prev_Item	= Price.Consolidate_Price_To_Prev_Item,
					Print_Condiment_On_Receipt		= Price.Print_Condiment_On_Receipt,
					KitchenRoute_ID			= Price.KitchenRoute_ID,
					ItemSurcharge			= Price.ItemSurcharge
				
				FROM 
					PriceBatchDetail PBD				(NOLOCK)
					INNER JOIN	Price					(NOLOCK)	ON	Price.Item_Key			= PBD.Item_Key 
																	AND Price.Store_No			= PBD.Store_No
					INNER JOIN	Item					(NOLOCK)	ON	Item.Item_Key			= PBD.Item_Key
					INNER JOIN	Store S								ON	S.Store_No				= PBD.Store_No
					INNER JOIN	StoreSubTeam SST		(NOLOCK)	ON	SST.Store_No			= Price.Store_No 				
																	AND SST.SubTeam_No			= Item.SubTeam_No
					LEFT JOIN	SubTeam ST				(NOLOCK)	ON	SST.SubTeam_No			= ST.SubTeam_No  
					INNER JOIN	ItemIdentifier II		(NOLOCK)	ON	II.Item_Key				= PBD.Item_Key 
																	AND Default_Identifier		= 1
					LEFT JOIN	ItemOverride						ON	ItemOverride.Item_Key	= Item.Item_Key 
																	AND ItemOverride.StoreJurisdictionID = S.StoreJurisdictionID
					LEFT JOIN	ItemUomOverride IUO		(NOLOCK)	ON	PBD.Item_Key			= IUO.Item_Key
																	AND S.Store_No			= IUO.Store_No
					LEFT JOIN	ItemOrigin				(NOLOCK)	ON	ItemOrigin.Origin_ID	= ISNULL(ItemOverride.Origin_ID, Item.Origin_ID)
					LEFT JOIN	ItemBrand				(NOLOCK)	ON	ItemBrand.Brand_ID		= ISNULL(ItemOverride.Brand_ID, Item.Brand_ID)
					LEFT JOIN	ItemUnit RU				(NOLOCK)	ON	RU.Unit_ID				= COALESCE(IUO.Retail_Unit_ID, ItemOverride.Retail_Unit_ID, Item.Retail_Unit_ID)
					LEFT JOIN	ItemUnit PU				(NOLOCK)	ON	PU.Unit_ID				= ISNULL(ItemOverride.Package_Unit_ID, Item.Package_Unit_ID)
					LEFT JOIN	StoreItemVendor SIV		(NOLOCK)	ON	SIV.Store_No			= PBD.Store_No 
																	AND SIV.Item_Key			= PBD.Item_Key 
																	AND PrimaryVendor			= 1
					LEFT JOIN	ItemScale				(NOLOCK)	ON	ItemScale.Item_Key		= PBD.Item_Key
					LEFT JOIN	ItemScaleOverride ISO	(NOLOCK)	ON	ISO.Item_Key			= PBD.Item_Key
																	AND ISO.StoreJurisdictionID = S.StoreJurisdictionID
																	AND @UseRegionalScaleFile	= 0
																	AND @UseStoreJurisdictions	= 1
					LEFT JOIN	Scale_ExtraText			(NOLOCK)	ON ISNULL(ISO.Scale_ExtraText_ID, ItemScale.Scale_ExtraText_ID) = Scale_ExtraText.Scale_ExtraText_ID
				
				WHERE 
					PriceBatchHeaderID = @PriceBatchHeaderID
					AND	(@UseStoreJurisdictions = 0 OR dbo.fn_IsItemInStoreJurisdiction(PBD.Item_Key, II.Identifier, PBD.Store_No) = 1)
    
				SELECT @error_no = @@ERROR
			END

			-- For item changes that are not accompanied by a price change, fill in the pricing details for the PBD
			-- record from the SignQueue table.  This will ensure that the match the values last communicated to 
			-- the POS.
		
			-- Fill in the sale pricing details for items that are currently on sale.  If the item is not on sale,
			-- these values will remain NULL.  The SignQueue table will contain values from the last sale definition,
			-- even if the sale is over.  That's why the Sale and Regular prices are updated separately.
			IF @error_no = 0
			BEGIN
				UPDATE PriceBatchDetail
				SET 
					MSRPPrice			= SQ.MSRPPrice,
					MSRPMultiple		= SQ.MSRPMultiple,
					PricingMethod_ID	= SQ.PricingMethod_ID,
					Sale_Multiple		= SQ.Sale_Multiple,
					Sale_Price			= SQ.Sale_Price,
					Sale_End_Date		= SQ.Sale_End_Date,
					Sale_Earned_Disc1	= SQ.Sale_Earned_Disc1,
					Sale_Earned_Disc2	= SQ.Sale_Earned_Disc2,
					Sale_Earned_Disc3	= SQ.Sale_Earned_Disc3
				
				FROM 
					PriceBatchDetail PBD	(NOLOCK)
					INNER JOIN SignQueue SQ (NOLOCK) ON SQ.Item_Key = PBD.Item_Key AND SQ.Store_No = PBD.Store_No
				
				WHERE 
					PriceBatchHeaderID = @PriceBatchHeaderID
					AND dbo.fn_OnSale(SQ.PriceChgTypeID) = 1
					AND (ISNULL(ItemChgTypeID, 0) = 2 OR (ISNULL(ItemChgTypeID, 0) = 1 AND ReAuthFlag = 1))
					AND PBD.PriceChgTypeID IS NULL
    
				SELECT @error_no = @@ERROR
			END

			-- Fill in the regular pricing details for all items.
			IF @error_no = 0
			BEGIN
				UPDATE PriceBatchDetail
				SET 
					Multiple =	case 
									when ISNULL(SQ.Multiple, 0) = 0	then 1
									else SQ.Multiple
								end,
					Price			= SQ.Price,
					POSPrice		= SQ.POSPrice,
					POSSale_Price	= SQ.POSSale_Price,
					PriceChgTypeID	= SQ.PriceChgTypeID
				
				FROM 
					PriceBatchDetail PBD		(NOLOCK)
					INNER JOIN	SignQueue SQ	(NOLOCK)	ON SQ.Item_Key = PBD.Item_Key AND SQ.Store_No = PBD.Store_No
				
				WHERE 
					PriceBatchHeaderID = @PriceBatchHeaderID
					AND (ISNULL(ItemChgTypeID, 0) = 2 OR (ISNULL(ItemChgTypeID, 0) = 1 AND ReAuthFlag = 1))
					AND PBD.PriceChgTypeID IS NULL
    
				SELECT @error_no = @@ERROR
			END

			-- If this is a re-authorization, it is possible the SignQueue table was not populated on data
			-- conversion.  Fill in the pricing details from the Price table for these items.

			-- Set sale prices for all re-authorized items that are not in SignQueue and are currently on sale.
			IF @error_no = 0
			BEGIN
				UPDATE PriceBatchDetail
				SET 
					MSRPPrice			= ISNULL(PBD.MSRPPrice, P.MSRPPrice),
					MSRPMultiple		= ISNULL(PBD.MSRPMultiple, P.MSRPMultiple),
					PricingMethod_ID	= ISNULL(PBD.PricingMethod_ID, P.PricingMethod_ID),
					Sale_Multiple		= ISNULL(PBD.Sale_Multiple, P.Sale_Multiple),
					Sale_Price			= ISNULL(PBD.Sale_Price, P.Sale_Price),
					Sale_End_Date		= ISNULL(PBD.Sale_End_Date, P.Sale_End_Date),
					Sale_Earned_Disc1	= ISNULL(PBD.Sale_Earned_Disc1, P.Sale_Earned_Disc1),
					Sale_Earned_Disc2	= ISNULL(PBD.Sale_Earned_Disc2, P.Sale_Earned_Disc2),
					Sale_Earned_Disc3	= ISNULL(PBD.Sale_Earned_Disc3, P.Sale_Earned_Disc3)
				
				FROM 
					PriceBatchDetail PBD	(NOLOCK)
					INNER JOIN	Price P		(NOLOCK)	ON P.Item_Key = PBD.Item_Key AND P.Store_No = PBD.Store_No
				
				WHERE 
					PriceBatchHeaderID = @PriceBatchHeaderID
					AND dbo.fn_OnSale(P.PriceChgTypeID) = 1
					AND ISNULL(ItemChgTypeID, 0) = 1 
					AND ReAuthFlag = 1
					AND PBD.PriceChgTypeID IS NULL
    
				SELECT @error_no = @@ERROR
			END

			-- Set regular prices for all re-authorized items that are not in SignQueue.
			IF @error_no = 0
			BEGIN
				UPDATE PriceBatchDetail
				SET 
					Multiple		= ISNULL(PBD.Multiple, P.Multiple),
					Price			= ISNULL(PBD.Price, P.Price),
					POSPrice		= ISNULL(PBD.POSPrice, P.POSPrice),
					POSSale_Price	= ISNULL(PBD.POSSale_Price, P.POSSale_Price),
					PriceChgTypeID	= ISNULL(PBD.PriceChgTypeID, P.PriceChgTypeID)
				
				FROM 
					PriceBatchDetail PBD	(NOLOCK)
					INNER JOIN	Price P		(NOLOCK)	ON P.Item_Key = PBD.Item_Key AND P.Store_No = PBD.Store_No
				
				WHERE 
					PriceBatchHeaderID = @PriceBatchHeaderID
					AND ISNULL(ItemChgTypeID, 0) = 1 
					AND ReAuthFlag = 1
					AND PBD.PriceChgTypeID IS NULL
    
				SELECT @error_no = @@ERROR
			END

			-- Set the case price for the PBD records.
			IF @error_no = 0
			BEGIN
				UPDATE PriceBatchDetail
				SET Case_Price = ROUND(dbo.fn_Price(@PriceChgTypeID, Multiple, Price, PricingMethod_ID, Sale_Multiple, Sale_Price) * CasePriceDiscount * Package_Desc1, 2)
				FROM 
					PriceBatchDetail PBD			(NOLOCK)
					INNER JOIN	StoreSubTeam SST	(NOLOCK)	ON SST.Store_No = PBD.Store_No AND SST.SubTeam_No = PBD.SubTeam_No
				WHERE 
					PriceBatchHeaderID = @PriceBatchHeaderID
    
				SELECT @error_no = @@ERROR
			END

			-- Mark the PBD records that require a new shelf tag.
			IF @error_no = 0
			BEGIN
				UPDATE PriceBatchDetail
				SET PrintSign = 1
				FROM PriceBatchDetail PBD (NOLOCK)
				WHERE PriceBatchHeaderID = @PriceBatchHeaderID
				
				SELECT @error_no = @@ERROR
			END
		END

		IF @error_no = 0
		BEGIN
			--
			-- This returns a recordset with a single record containing the new status description
			--
			EXEC UpdatePriceBatchStatus @PriceBatchHeaderID, 2
    
			SELECT @error_no = @@ERROR
		END

		SET NOCOUNT OFF

		IF @error_no = 0
			COMMIT TRAN
		ELSE
		BEGIN
			IF @@TRANCOUNT <> 0
				ROLLBACK TRAN
			DECLARE @Severity smallint
			SELECT @Severity = ISNULL((SELECT severity FROM master.dbo.sysmessages WHERE error = @error_no), 16)
			RAISERROR ('UpdatePriceBatchPackage failed with @@ERROR: %d', @Severity, 1, @error_no)
		END
END