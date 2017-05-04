SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

if exists (select * from dbo.sysobjects where id = object_id(N'dbo.UpdatePriceBatchUnpackage') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure dbo.UpdatePriceBatchUnpackage
GO


CREATE PROCEDURE dbo.UpdatePriceBatchUnpackage
    @PriceBatchHeaderID int,
    @PriceBatchDetailID int
AS

BEGIN


/*********************************************************************************************
CHANGE LOG
DEV		DATE		TASK	Description
----------------------------------------------------------------------------------------------
DBS		20110125	1241	Merge Up FSA Changes
MU		20110126	 744	add ItemSurcharge

***********************************************************************************************/ 
   SET NOCOUNT ON

    DECLARE @error_no int, @Severity smallint
    SELECT @error_no = 0

    DECLARE @PriceBatchStatusID tinyint
    SELECT @PriceBatchStatusID = PriceBatchStatusID FROM PriceBatchHeader WHERE PriceBatchHeaderID = @PriceBatchHeaderID

    SELECT @error_no = @@ERROR

    IF @error_no = 0
    BEGIN
        IF @PriceBatchStatusID <> 2
            RETURN
    END
    ELSE
    BEGIN
        IF @@TRANCOUNT <> 0
            ROLLBACK TRAN
        SELECT @Severity = ISNULL((SELECT severity FROM master.dbo.sysmessages WHERE error = @error_no), 16)
        RAISERROR ('UpdatePriceBatchUnpackage failed with @@ERROR: %d', @Severity, 1, @error_no)
    END

    BEGIN TRAN

	-- If only a batch header value was passed in, move the batch from the Packaged to the Building state.
    IF @PriceBatchDetailID IS NULL
    BEGIN
        UPDATE PriceBatchHeader
        SET PriceBatchStatusID = 1
        WHERE PriceBatchHeaderID = @PriceBatchHeaderID
    
        SELECT @error_no = @@ERROR
    END

	-- Clear all of the details that are added to the PBD record at the time of packaging.  This does
	-- still leave the PBD records assigned to the batch, but it puts the data back to the way it looked
	-- when the batch was in the building state.
	-- If the @PriceBatchDetailID is NULL, these steps are performed for all details assigned to the batch
	-- header.  Otherwise, they are only done for the single detail record.
    IF @error_no = 0
    BEGIN
		-- For ITEM changes that are not accompanied by a PRICE change, 
		-- clear out the REG pricing details that were added when the batch was packaged.
        UPDATE PriceBatchDetail
        SET Multiple = NULL,
            Price = NULL
        WHERE PriceBatchHeaderID = @PriceBatchHeaderID
            AND PriceBatchDetailID = ISNULL(@PriceBatchDetailID, PriceBatchDetailID)
            AND ISNULL(ItemChgTypeID, 0) = 2 AND PriceChgTypeID IS NULL
    
        SELECT @error_no = @@ERROR
    END

    IF @error_no = 0
    BEGIN
		-- For ITEM changes that are not accompanied by a PRICE change OR non-sale PRICE changes,
		-- clear out the SALE pricing details that were added when the batch was packaged.
        UPDATE PriceBatchDetail
        SET MSRPPrice = NULL,
            MSRPMultiple = NULL,
            PricingMethod_ID = NULL,
            Sale_Multiple = NULL,
            Sale_Price = NULL,
            Sale_End_Date = NULL,
            Sale_Earned_Disc1 = NULL,
            Sale_Earned_Disc2 = NULL,
            Sale_Earned_Disc3 = NULL
        FROM PriceBatchDetail PBD
        WHERE PriceBatchHeaderID = @PriceBatchHeaderID
            AND PriceBatchDetailID = ISNULL(@PriceBatchDetailID, PriceBatchDetailID)
            AND dbo.fn_OnSale (PriceChgTypeID) = 0
    
        SELECT @error_no = @@ERROR
    END

    IF @error_no = 0
    BEGIN
		-- Clear out all of the item details that are set with the current date in IRMA when the batch
		-- is packaged.  This is done for ITEM and/or PRICE changes.
        UPDATE PriceBatchDetail
        SET PrintSign = 0,
            Case_Price = NULL,
            Sign_Description = NULL, 
            Ingredients = NULL, 
            Identifier = NULL, 
            Sold_By_Weight = NULL, 
            RetailUnit_WeightUnit = NULL,
            SubTeam_No = NULL, 
            Origin_Name = NULL, 
            Brand_Name = NULL, 
            Retail_Unit_Abbr = NULL, 
            Retail_Unit_Full = NULL, 
            Package_Unit = NULL, 
            Package_Desc1 = NULL, 
            Package_Desc2 = NULL, 
            Organic = NULL, 
            Vendor_ID = NULL, 
            ItemType_ID = NULL, 
            ScaleDesc1 = NULL, 
            ScaleDesc2 = NULL, 
            POS_Description = NULL, 
            Restricted_Hours = NULL, 
            LocalItem = NULL,
            Quantity_Required = NULL, 
            Price_Required = NULL, 
            Retail_Sale = NULL, 
            Discountable = NULL, 
            Food_Stamps = NULL, 
            IBM_Discount = NULL,
            NotAuthorizedForSale = NULL,
            PosTare = NULL,
            LinkedItem = NULL,
            POSLinkCode = NULL,
            GrillPrint = NULL,
            AgeCode = NULL,
            VisualVerify = NULL,
            SrCitizenDiscount = NULL,
            QtyProhibit = NULL, 
            GroupList = NULL,
            MixMatch = NULL,
            PurchaseThresholdCouponAmount = NULL,
			PurchaseThresholdCouponSubTeam = NULL,
			LabelType_ID = NULL,
			Item_Description = NULL,
			Case_Discount = NULL,
			Coupon_Multiplier = NULL,
			FSA_Eligible = NULL,
			Misc_Transaction_Sale = NULL,
			Misc_Transaction_Refund = NULL,
			Recall_Flag = NULL,
			Product_Code = NULL,
			Unit_Price_Category = NULL,
			Ice_Tare = NULL,
			Age_Restrict = NULL,
			Routing_Priority = NULL,
			Consolidate_Price_To_Prev_Item = NULL,
			Print_Condiment_On_Receipt = NULL,
			KitchenRoute_ID = NULL,
			ItemSurcharge = NULL
        WHERE PriceBatchHeaderID = @PriceBatchHeaderID
            AND PriceBatchDetailID = ISNULL(@PriceBatchDetailID, PriceBatchDetailID)
    
        SELECT @error_no = @@ERROR
    END

 	-- When the batch is packaged, if there is an ITEM and PRICE change for the same item-store 
	-- combination, these are combined into a single PBD record when the batch is packaged.  The
	-- PRICE record is used for this, and the ITEM record is expired.
	-- The following INSERTS, DELETES, and UPDATES are "uncombining" these records into their 
	-- individual ITEM and PRICE changes. 
   IF @error_no = 0
    BEGIN
		-- If this was a NEW item, we need to end up with the PBD record for the ITEM change and the 
		-- PBD record for the PRICE change.  This is re-creating the NEW item change PBD record that was
		-- expired by the packaging process, making sure to update the ReAuthFlag accordingly.
        INSERT INTO PriceBatchDetail (Item_Key, Store_No, PriceBatchHeaderID, ItemChgTypeID, ReAuthFlag)
        SELECT Item_Key, Store_No, @PriceBatchHeaderID, 1, ReAuthFlag
        FROM PriceBatchDetail
        WHERE PriceBatchHeaderID = @PriceBatchHeaderID
            AND PriceBatchDetailID = ISNULL(@PriceBatchDetailID, PriceBatchDetailID)
            AND PriceChgTypeID IS NOT NULL
            AND ItemChgTypeID = 1
    
        SELECT @error_no = @@ERROR
    END

    IF @error_no = 0
    BEGIN
		-- Delete any Item Change PBD records that were created since the New Item PBD was packaged.  These
		-- item changes will all wrapped up as part of the New item batch that is being restored.
        DELETE PriceBatchDetail
        FROM PriceBatchDetail PBD
        INNER JOIN
            (SELECT Item_Key, Store_No
             FROM PriceBatchDetail
             WHERE PriceBatchHeaderID = @PriceBatchHeaderID
                 AND PriceBatchDetailID = ISNULL(@PriceBatchDetailID, PriceBatchDetailID)
                 AND PriceChgTypeID IS NULL
                 AND ItemChgTypeID = 1) T
            ON T.Item_Key = PBD.Item_Key AND T.Store_No = PBD.Store_No
        WHERE PriceBatchHeaderID IS NULL
            AND ItemChgTypeID IS NOT NULL
    
        SELECT @error_no = @@ERROR
    END

    IF @error_no = 0
    BEGIN
		-- If this was an ITEM change that was also accompanied by a PRICE change, we need to end up 
		-- with a PBD record for the ITEM change and the PBD record for the PRICE change.  
		-- This is re-creating the ITEM change PBD record that was expired by the packaging process if
		-- another ITEM change does not exist.  If an ITEM change was added since this PBD record was
		-- packaged, we can just use that PBD record.
        INSERT INTO PriceBatchDetail (Item_Key, Store_No, PriceBatchHeaderID, ItemChgTypeID)
        SELECT Item_Key, Store_No, @PriceBatchHeaderID, 2
        FROM PriceBatchDetail PBD
        WHERE PriceBatchHeaderID = @PriceBatchHeaderID
            AND PriceBatchDetailID = ISNULL(@PriceBatchDetailID, PriceBatchDetailID)
            AND PriceChgTypeID IS NOT NULL
            AND ItemChgTypeID = 2
            AND NOT EXISTS (SELECT *
                            FROM PriceBatchDetail D
                            WHERE D.Item_Key = PBD.Item_Key AND D.Store_No = PBD.Store_No
                                AND D.PriceBatchHeaderID IS NULL
                                AND D.ItemChgTypeID IS NOT NULL)
    
        SELECT @error_no = @@ERROR
    END

    IF @error_no = 0
    BEGIN
		-- Set the ItemChgTypeID back to NULL for the PRICE change records.
        UPDATE PriceBatchDetail
        SET ItemChgTypeID = NULL
        WHERE PriceBatchHeaderID = @PriceBatchHeaderID
            AND PriceBatchDetailID = ISNULL(@PriceBatchDetailID, PriceBatchDetailID)
            AND PriceChgTypeID IS NOT NULL
    
        SELECT @error_no = @@ERROR
    END

    SET NOCOUNT OFF

    IF @error_no = 0
	    COMMIT TRAN -- only if BEGIN TRAN in this procedure
    ELSE
    BEGIN
        IF @@TRANCOUNT <> 0
            ROLLBACK TRAN
        SELECT @Severity = ISNULL((SELECT severity FROM master.dbo.sysmessages WHERE error = @error_no), 16)
        RAISERROR ('UpdatePriceBatchUnpackage failed with @@ERROR: %d', @Severity, 1, @error_no)
    END
END

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

