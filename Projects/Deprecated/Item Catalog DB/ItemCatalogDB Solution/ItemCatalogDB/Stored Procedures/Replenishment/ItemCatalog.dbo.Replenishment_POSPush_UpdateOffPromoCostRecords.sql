SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

if exists (select * from dbo.sysobjects where id = object_id(N'dbo.Replenishment_POSPush_UpdateOffPromoCostRecords') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure dbo.Replenishment_POSPush_UpdateOffPromoCostRecords
GO

CREATE PROCEDURE [dbo].[Replenishment_POSPush_UpdateOffPromoCostRecords]
    @Date datetime

AS

-- The logic in this stored procedure needs to be kept up to date with any changes that are made 
-- to the UpdatePriceBatchPackage procedure.

-- ****************************************************************************************************************
-- Procedure: Replenishment_POSPush_UpdateOffPromoCostRecords()
--    Author: unknown
--      Date: unknown
--
-- Description:
-- Something to do with Price Batches.
--
-- Modification History:
-- Date       	Init  	TFS   	Comment
-- 2013-01-04	KM		9251	Add update history template; Check ItemOverride for new 4.8 override values (Origin, Brand,
--								LabelType, Recall_Flag, Product_Code, Unit_Price_Category);
-- ****************************************************************************************************************

BEGIN
    SET NOCOUNT ON

    DECLARE @CurrDay smalldatetime
    SELECT @CurrDay = CONVERT(smalldatetime, CONVERT(varchar(255), @Date, 101))

    DECLARE @error_no int
    SELECT @error_no = 0

 	-- The user is allowed to make Item changes after Off Promo Cost batches are placed in the 
	-- Sent state.
	-- Update the PriceBatchDetails that are set when the batch is Packaged to ensure that
	-- the Off Promo Cost batch records have the latest Item information.
	
	BEGIN TRAN
        IF @error_no = 0
        BEGIN
        
			-- Check the Regional Scale Flag
			DECLARE @UseRegionalScaleFile bit
			
			SELECT @UseRegionalScaleFile = FlagValue 
			FROM InstanceDataFlags (NOLOCK) 
			WHERE FlagKey='UseRegionalScaleFile'
			
				
        	-- Check the Store Jurisdiction Flag
			DECLARE @UseStoreJurisdictions int
			
			SELECT @UseStoreJurisdictions = FlagValue 
			FROM InstanceDataFlags
			WHERE FlagKey = 'UseStoreJurisdictions'

            UPDATE PriceBatchDetail
            SET 
				Sign_Description				= ISNULL(ItemOverride.Sign_Description, Item.Sign_Description), 
                Ingredients						= Scale_ExtraText.ExtraText, 
                Identifier						= II.Identifier, 
                Sold_By_Weight					= CASE 
														WHEN COALESCE(IUORU.Weight_Unit, IORU.Weight_Unit, RU.Weight_Unit, 0) = 1 AND dbo.fn_IsScaleItem(II.Identifier) = 0 THEN 1 
														ELSE 0 
												  END, 
				SubTeam_No						= ISNULL(Price.ExceptionSubTeam_No, Item.SubTeam_No), 
                Origin_Name						= ItemOrigin.Origin_Name, 
                Brand_Name						= ItemBrand.Brand_Name, 
                Retail_Unit_Abbr				= COALESCE(IUORU.Unit_Abbreviation, IORU.Unit_Abbreviation, RU.Unit_Abbreviation),
                Retail_Unit_Full				= COALESCE(IUORU.Unit_Name, IORU.Unit_Name, RU.Unit_Name), 
                Package_Unit					= COALESCE(IOPU.Unit_Abbreviation, PU.Unit_Abbreviation), 
                Package_Desc1					= ISNULL(ItemOverride.Package_Desc1, Item.Package_Desc1), 
                Package_Desc2					= ISNULL(ItemOverride.Package_Desc2, Item.Package_Desc2), 
                Organic							= Item.Organic, 
                Vendor_ID						= SIV.Vendor_ID, 
                ItemType_ID						= Item.ItemType_ID, 
                ScaleDesc1						= ISNULL(ISO.Scale_Description1, ItemScale.Scale_Description1), 
                ScaleDesc2						= ISNULL(ISO.Scale_Description2, ItemScale.Scale_Description2), 
                POS_Description					= ISNULL(ItemOverride.POS_Description, Item.POS_Description), 
                Restricted_Hours				= Price.Restricted_Hours, 
                LocalItem						= Price.LocalItem,
                Quantity_Required				= ISNULL(ItemOverride.Quantity_Required, Item.Quantity_Required), 
                Price_Required					= ISNULL(ItemOverride.Price_Required, Item.Price_Required), 
                Retail_Sale						= Item.Retail_Sale, 
                Discountable					= Price.Discountable, 
                Food_Stamps						= ISNULL(ItemOverride.Food_Stamps, Item.Food_Stamps), 
                IBM_Discount					= Price.IBM_Discount,
                NotAuthorizedForSale			= Price.NotAuthorizedForSale,
                PosTare							= Price.PosTare,
                LinkedItem						= Price.LinkedItem,
                POSLinkCode						= Price.POSLinkCode,
                GrillPrint						= Price.GrillPrint,
                AgeCode							= Price.AgeCode,
                VisualVerify					= Price.VisualVerify,
                SrCitizenDiscount				= Price.SrCitizenDiscount,
                QtyProhibit						= ISNULL(ItemOverride.QtyProhibit, Item.QtyProhibit), 
                GroupList						= ISNULL(ItemOverride.GroupList, Item.GroupList),
                MixMatch						= CASE WHEN Price.MixMatch IS NOT NULL THEN Price.MixMatch ELSE 0 END,
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
				LabelType_ID					= ISNULL(ItemOverride.LabelType_ID, Item.LabelType_ID),
				StartDate						= ISNULL(PBD.StartDate, PBH.StartDate),
				Item_Description				= ISNULL(ItemOverride.Item_Description, Item.Item_Description),
				Case_Discount					= ISNULL(ItemOverride.Case_Discount, Item.Case_Discount),
				Coupon_Multiplier				= ISNULL(ItemOverride.Coupon_Multiplier, Item.Coupon_Multiplier),
				Misc_Transaction_Sale			= ISNULL(ItemOverride.Misc_Transaction_Sale, Item.Misc_Transaction_Sale),
				Misc_Transaction_Refund			= ISNULL(ItemOverride.Misc_Transaction_Refund, Item.Misc_Transaction_Refund),
				Recall_Flag						= ISNULL(ItemOverride.Recall_Flag, Item.Recall_Flag),
				Product_Code					= ISNULL(ItemOverride.Product_Code, Item.Product_Code),
				Unit_Price_Category				= ISNULL(ItemOverride.Unit_Price_Category, Item.Unit_Price_Category),
				Ice_Tare						= ISNULL(ItemOverride.Ice_Tare, Item.Ice_Tare),
				Age_Restrict					= Price.Age_Restrict,
				Routing_Priority				= Price.Routing_Priority,
				Consolidate_Price_To_Prev_Item	= Price.Consolidate_Price_To_Prev_Item,
				Print_Condiment_On_Receipt		= Price.Print_Condiment_On_Receipt,
				KitchenRoute_ID					= Price.KitchenRoute_ID,
				ItemSurcharge					= Price.ItemSurcharge            
			FROM 
				PriceBatchDetail PBD				
				INNER JOIN	PriceBatchHeader PBH				ON	PBD.PriceBatchHeaderID		= PBH.PriceBatchHeaderID
				INNER JOIN  Store					(NOLOCK)    ON	Store.Store_No				= PBD.Store_No
				INNER JOIN	Price							    ON	Price.Item_Key				= PBD.Item_Key 
																AND Price.Store_No				= PBD.Store_No
				INNER JOIN	Item							    ON	Item.Item_Key				= PBD.Item_Key
				INNER JOIN	StoreSubTeam SST		(NOLOCK)	ON	SST.Store_No				= Price.Store_No 				
																AND SST.SubTeam_No				= Item.SubTeam_No
				LEFT JOIN	SubTeam ST				(NOLOCK)	ON	SST.SubTeam_No				= ST.SubTeam_No  
				INNER JOIN	ItemIdentifier II					ON	II.Item_Key					= PBD.Item_Key 
																AND Default_Identifier			= 1
				LEFT JOIN	StoreItemVendor SIV					ON	SIV.Store_No				= PBD.Store_No 
																AND SIV.Item_Key				= PBD.Item_Key 
																AND PrimaryVendor				= 1
				LEFT JOIN	ItemOverride						ON	ItemOverride.Item_Key		= Item.Item_Key 
																AND ItemOverride.StoreJurisdictionID = Store.StoreJurisdictionID
				LEFT JOIN	ItemUomOverride IUO					ON PBD.Item_Key					= IUO.Item_Key
																AND Store.Store_No				= IUO.Store_No
				LEFT JOIN	ItemUnit RU						    ON	RU.Unit_ID					= Item.Retail_Unit_ID
				LEFT JOIN	ItemUnit PU						    ON	PU.Unit_ID					= Item.Package_Unit_ID
				LEFT JOIN   ItemUnit IORU						ON	IORU.Unit_ID				= ItemOverride.Retail_Unit_ID
				LEFT JOIN   ItemUnit IOPU						ON  IOPU.Unit_ID				= ItemOverride.Package_Unit_ID
				LEFT JOIN   ItemUnit IUORU						ON  IUORU.Unit_ID				= IUO.Retail_Unit_ID
				LEFT JOIN	ItemOrigin							ON	ItemOrigin.Origin_ID		= ISNULL(ItemOverride.Origin_ID, Item.Origin_ID)
				LEFT JOIN	ItemBrand						    ON	ItemBrand.Brand_ID			= ISNULL(ItemOverride.Brand_ID, Item.Brand_ID)
				LEFT JOIN	ItemScale				(NOLOCK)	ON	ItemScale.Item_Key			= PBD.Item_Key
				LEFT JOIN	ItemScaleOverride ISO	(NOLOCK)	ON	ISO.Item_Key				= PBD.Item_Key
																AND ISO.StoreJurisdictionID		= Store.StoreJurisdictionID
																AND @UseRegionalScaleFile		= 0
																AND @UseStoreJurisdictions		= 1
				LEFT JOIN	Scale_ExtraText			(NOLOCK)	ON ISNULL(ISO.Scale_ExtraText_ID, ItemScale.Scale_ExtraText_ID) = Scale_ExtraText.Scale_ExtraText_ID
            
			WHERE 
				ISNULL(PBD.ItemChgTypeID, 0) = 6		-- Only update Off Promo Cost details
				AND (Mega_Store = 1 OR WFM_Store = 1)	-- Use the same criteria that GetPriceBatchSent uses
				AND PriceBatchStatusID = 5
                AND PBH.StartDate <= @CurrDay
                AND PBD.Offer_ID IS NULL -- EXCLUDE OFFER PBD RECORDS    
				AND (@UseStoreJurisdictions = 0 OR dbo.fn_IsItemInStoreJurisdiction(PBD.Item_Key, II.Identifier, PBD.Store_No) = 1)
                
            SELECT @error_no = @@ERROR
        END

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
				PriceBatchDetail PBD
				INNER JOIN	SignQueue SQ						ON	SQ.Item_Key				= PBD.Item_Key 
																AND SQ.Store_No				= PBD.Store_No
				INNER JOIN  PriceBatchHeader PBH                ON	PBD.PriceBatchHeaderID	= PBH.PriceBatchHeaderID
				INNER JOIN  Store					(NOLOCK)    ON	Store.Store_No			= PBD.Store_No
				INNER JOIN	PriceChgType PCT					ON	PCT.PriceChgTypeID		= SQ.PriceChgTypeID
            
			WHERE 
                PCT.On_Sale = 1
				AND ISNULL(PBD.ItemChgTypeID, 0) = 6  AND PBD.PriceChgTypeID IS NULL -- Only update Off Promo Cost details
				AND (Mega_Store = 1 OR WFM_Store = 1)	-- Use the same criteria that GetPriceBatchSent uses
				AND PriceBatchStatusID = 5
                AND PBH.StartDate <= @CurrDay
                AND PBD.Offer_ID IS NULL -- EXCLUDE OFFER PBD RECORDS    

    
            SELECT @error_no = @@ERROR
        END

        IF @error_no = 0
        BEGIN
            UPDATE PriceBatchDetail
            SET 
				Multiple		= SQ.Multiple,
                Price			= SQ.Price,
                POSPrice		= SQ.POSPrice,
                POSSale_Price	= SQ.POSSale_Price
            
			FROM 
				PriceBatchDetail PBD
				INNER JOIN	SignQueue SQ            ON	SQ.Item_Key				= PBD.Item_Key 
													AND SQ.Store_No				= PBD.Store_No
				INNER JOIN	PriceBatchHeader PBH	ON	PBD.PriceBatchHeaderID	= PBH.PriceBatchHeaderID
 				INNER JOIN  Store	(NOLOCK)        ON	Store.Store_No			= PBD.Store_No
           
		   WHERE 
  				ISNULL(PBD.ItemChgTypeID, 0) = 6  AND PBD.PriceChgTypeID IS NULL -- Only update Off Promo Cost details
				AND (Mega_Store = 1 OR WFM_Store = 1)	-- Use the same criteria that GetPriceBatchSent uses
				AND PriceBatchStatusID = 5
                AND PBH.StartDate <= @CurrDay
                AND PBD.Offer_ID IS NULL -- EXCLUDE OFFER PBD RECORDS    
  
            SELECT @error_no = @@ERROR
        END

        IF @error_no = 0
        BEGIN
            UPDATE PriceBatchDetail
            SET Case_Price = ROUND(dbo.fn_Price(PBD.PriceChgTypeID, Multiple, Price, PricingMethod_ID, Sale_Multiple, Sale_Price) * CasePriceDiscount * Package_Desc1, 2)
            FROM PriceBatchDetail PBD
				INNER JOIN	StoreSubTeam SST (NOLOCK)	ON	SST.Store_No			= PBD.Store_No 
														AND SST.SubTeam_No			= PBD.SubTeam_No
				INNER JOIN  PriceBatchHeader PBH        ON	PBD.PriceBatchHeaderID	= PBH.PriceBatchHeaderID
 				INNER JOIN  Store (NOLOCK)              ON	Store.Store_No			= PBD.Store_No
            WHERE 
    			ISNULL(PBD.ItemChgTypeID, 0) = 6  AND PBD.PriceChgTypeID IS NULL -- Only update Off Promo Cost details
				AND (Mega_Store = 1 OR WFM_Store = 1)	-- Use the same criteria that GetPriceBatchSent uses
				AND PriceBatchStatusID = 5
                AND PBH.StartDate <= @CurrDay
                AND PBD.Offer_ID IS NULL -- EXCLUDE OFFER PBD RECORDS    
  
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
        RAISERROR ('Replenishment_POSPush_UpdateOffPromoCostRecords failed with @@ERROR: %d', @Severity, 1, @error_no)
    END
END